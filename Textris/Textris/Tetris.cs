using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Textris
{
    public class Tetris
    {
        #region 완성된 코드 영역
        /// <summary>
        /// 현재 게임이 실행중이면 true값을 보관,
        /// 게임 종료이면 false값을 보관
        /// </summary>
        private bool isInGame;

        /// <summary>
        /// 블록의 X 좌표
        /// </summary>
        private int posX;
        /// <summary>
        /// 블록의 Y 좌표
        /// </summary>
        private int posY;

        /// <summary>
        /// 게임 영역 컨테이너
        /// </summary>
        private int[,] container;

        /// <summary>
        /// 현재 움직이고 있는(떨어지고 있는) 블록
        /// </summary>
        private int[,] currentBlock = null;

        /// <summary>
        /// 다음에 나타날 블록
        /// </summary>
        private int[,] nextBlock = null;

        /// <summary>
        /// 블록을 생성하고 관리 : 현재 블록, 다음 블록
        /// </summary>
        private Block generatedBlock = new Block();

        /// <summary>
        /// 미리보기 블록(고스트 블록, 새도우 블록) 보이기/숨기기 (y/n)
        /// </summary>
        private bool shadow;

        /// <summary>
        /// 고스트 블록을 보일건지 말건지 외부에서 결정시킬 때 사용하는 속성
        /// </summary>
        public bool ShadowBlock
        {
            get { return shadow; }
            set { shadow = value; }
        }
        /// <summary>
        /// 게임 실행 상태 표시(읽기전용)
        /// </summary>
        public bool isRunning
        {
            get { return isInGame; }
        }
                
        /// <summary>
        /// Tetris 클래스에서 사용 가능한 키값 열거형
        /// </summary>
        public enum Key
        {
            Up,
            Down,
            Left,
            Right,
            TurnRight,
            TurnLeft
        }

        public Tetris()
            : this(10, 20)
        {
            // Empty
        }

        /// <summary>
        /// 새로운 테트리스 게임  초기화
        /// </summary>
        /// <param name="width">게임 영역의 가로(열) >= 10</param>
        /// <param name="height">게임 영역의 세로(행) >= 20</param>
        public Tetris(int width, int height)
        {
            ShadowBlock = true; //고스트 블록 기본으로 보이기
            if (width >= 10 && height >= 20)
            {
                container = new int[height, width]; //기본값은 20행 10열짜리 2차원 배열
            }
            else
            {
                throw new Exception("게임 영역은 반드시 10*20 이상이어야 합니다");
            }
        }

        /// <summary>
        /// 다음에 나타날 다음 블록(읽기 전용)
        /// </summary>
        public int[,] Next
        {
            get
            {
                return nextBlock;
            }
        }

        #endregion

        #region GameStart : 게임 시작하고 블록을 출력하기
        /// <summary>
        /// 게임 시작하고 블록을 출력하기
        /// </summary>
        public void GameStart()
        {
            this.isInGame = true;

            posX = container.GetUpperBound(1) / 2; //10열 / 2 = 5
            posY = 0; // PosY = container.GetUpperBound(0); //20행 맨위

            // 현재 블럭과 다음 블록을 생성
            currentBlock = nextBlock != null ? nextBlock : generatedBlock.GetRandomBlock();
            nextBlock = generatedBlock.GetRandomBlock();

            // 예외 처리 : 더이상 블록을 쓸 수 없으면 게임을 종료
            if (!CanPositionedAt(currentBlock, posX, posY))
            {
                GameEnd();
            }

        } 

        #endregion

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void GameEnd()
        {
            this.isInGame = false;

            //이벤트 발생
            if (GameOver != null)
                GameOver();
        }

        #region KeyInput : 메인에서 특정 키값을 눌렀을 때 처리해야할 로직 구현
        /// <summary>
        /// 메인에서 특정 키값을 눌렀을 때 처리해야할 로직 구현
        /// </summary>
        /// <param name="key"></param>
        public void KeyInput(Key key)
        {
            int[,] temp; //회전된 블록을 임시로 담아놓을 그릇
            if (isInGame)
            {
                switch (key)
                {
                    case Key.Up:
                        while (CanPositionedAt(currentBlock, posX, posY + 1))
                        {
                            Step(); // 쓸 수 있는 만큼 아래로 떨어짐
                        }
                        Step(); //완료
                        break;
                    case Key.Down:
                        Step();
                        break;
                    case Key.Left:
                        if (posX > 0)
                        {
                            if (CanPositionedAt(currentBlock, posX - 1, posY))
                            {
                                posX--;
                            }
                        }
                        break;
                    case Key.Right:
                        // 컨테이너 개체의 가로 바운더리 안에서 X 좌표를 이동(???열 길이만큼이기 때문에 (.GetUpperBound(1))
                        if (posX < container.GetUpperBound(1) - currentBlock.GetUpperBound(1))
                        {
                            if (CanPositionedAt(currentBlock, posX + 1, posY))
                            {
                                posX++;
                            }
                        }
                        break;
                    case Key.TurnRight:
                        // 오른쪽으로 회전 할 수 있으면 회전
                        temp = Block.RotateRight(currentBlock);
                        if (CanPositionedAt(temp, posX, posY))
                        {
                            currentBlock = Block.RotateRight(currentBlock);
                        }
                        break;
                    case Key.TurnLeft:
                        // 왼쪽으로 회전 할 수 있으면 회전
                        temp = Block.RotateLeft(currentBlock);
                        if (CanPositionedAt(temp, posX, posY))
                        {
                            currentBlock = Block.RotateLeft(currentBlock);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        
        #endregion
        #region Step
        /// <summary>
        /// 아래로 한칸 이동할 수 있으면 이동하고, 그렇지 않으면 현재 블록 완료 처리
        /// </summary>
        public void Step()
        {
            if (isInGame)
            {
                if (CanPositionedAt(currentBlock, posX, posY + 1))
                {
                    posY++;
                }
                else //해당 블록이 완료(아래로 떨어짐)
                {
                    //컨테이너를 업데이트
                    container = FixBlock(container, currentBlock, posX, posY);
                    GameStart();
                }

                //한줄이 꽉 찬 행이 있는지 확인
                int lines = CheckLines();

                //알림을 위한 이벤트 발생
                if(LinesDone != null)
                {
                    LinesDone(lines);
                }
            }
        }

        /// <summary>
        /// 한줄이 완성된(꽉찬 라인의 수를 반환하고 해당 라인 삭제)
        /// </summary>
        /// <returns></returns>
        private int CheckLines()
        {
            int count = 0;

            // 컨테이너의 행 반복 : 0 ~19
            for (int i = 0; i <= container.GetUpperBound(0); i++)
            {
                bool isFullLIne = true; //꽉 찼다고 가정
                //컨테이너의 열 반복 : 0 ~ 9
                for (int j = 0; j <= container.GetUpperBound(1); j++)
                {
                    // 열반복하면서 한번이라도 0이 나오면 false
                    isFullLIne = isFullLIne && container[i, j] != 0; 
                }
                // 꽉찬 행이면 해당 라인을 삭제
                if (isFullLIne)
                {
                    //RemoveLine(i);
                    RemoveLine(i--); // 바로 위에서 떨어진 블록을 한번 더 확인
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 특정 인덱스에 해당하는 라인을 삭제
        /// </summary>
        /// <param name="i">Y축 (0 기준)</param>
        private void RemoveLine(int index)
        {
            // 라인을 한칸 아래로 이동
            for (int i = index; i > 0; i--) //19번째 인덱스(맨 아래)가 꽉 찼다면
            {
                for (int j = 0; j <= container.GetUpperBound(1); j++)
                {
                    container[i, j] = container[i - 1, j]; //19번째 = 18번째, 18 = 17, ... 1 = 0
                }
            }

            // 최 상단(0번째 인덱스) 비우기
            for (int j = 0; j < container.GetUpperBound(1); j++)
            {
                container[0, j] = 0;
            }
        }
        #endregion

        #region 게임이 실행될 영역에 대한 2차원 배열(완성된 블록과 현재 떨어지고 있는 블록)
        /// <summary>
        /// 게임이 실행될 영역에 대한 2차원 배열(완성된 블록과 현재 떨어지고 있는 블록)
        /// </summary>
        public int[,] GameFieldData
        {
            get
            {
                // 컨테이너 개체와 현재 블록 개체를 임시로 복사
                //int[,] arrCurrentBlock = new int[20, 10];
                int[,] arrContainer = (int[,])container.Clone(); //매번 클릭할때마다 새로운 컨테이너를 복사해서 써서 잔상 X
                int[,] arrCurrentBlock = (int[,])currentBlock.Clone();
                int x = posX; // 블록의 현재 X 좌표
                int y = posY; // 블록의 현재 Y 좌표

                // 현재 실행중인 블록의 색상을 변경
                for (int i = 0; i <= arrCurrentBlock.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= arrCurrentBlock.GetUpperBound(1); j++)
                    {
                        if (arrCurrentBlock[i, j] != 0)
                        {
                            arrCurrentBlock[i, j] = 7; //움직이고 있는 동안에는 레드 배경색
                        }
                    }
                }

                // 컨테이너에 현재 위치값에 해당하는 현재 블록을 덮어 쓰기
                arrContainer = FixBlock(arrContainer, arrCurrentBlock, x, y);

                #region 쉐도우 블록 출력하는 영역
                // 쉐도우 블록/고스트 블록 출력
                if (shadow)
                {
                    // 몇칸 아래에서 고스트 블록을 출력할건지
                    int add = 0;

                    #region 고스트 블록의 색상을 변경
                    // 고스트 블록의 색상을 변경

                    for (int i = 0; i <= arrCurrentBlock.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= arrCurrentBlock.GetUpperBound(1); j++)
                        {
                            if (arrCurrentBlock[i, j] != 0)
                            {
                                arrCurrentBlock[i, j] = 8; // 고스트 블록은 회색
                            }
                        }
                    }
                    #endregion

                    // 어디까지 블록을 쓸 수 있는지 확인
                    while (CanPositionedAt(arrCurrentBlock, posX, posY + add))
                    {
                        add++;
                    }

                    if (posY + add - 1 > 0) // 최소 아래로 두칸 이상 쓸 수 있을 때만 고스트 블록 출력
                    {
                        //return (int[,])FixBlock(arrContainer, arrCurrentBlock, posX, posY + add - 1).Clone();
                        arrContainer = FixBlock(arrContainer, arrCurrentBlock, posX, posY + add - 1);
                    }
                }
                #endregion

                return arrContainer;
            }
        } 
        #endregion

        #region 특정 위치에 블록이 들어갈 수 있는지 아닌지를 체크
        /// <summary>
        /// 특정 위치에 블록이 들어갈 수 있는지 아닌지를 체크
        /// </summary>
        /// <param name="arrBlock">체크할 블록 배열</param>
        /// <param name="x">X 좌표</param>
        /// <param name="y">Y 좌표</param>
        /// <returns>들어갈 수 있으면 참, 아니면 거짓</returns>
        private bool CanPositionedAt(int[,] arrBlock, int x, int y)
        {
            // 현재 컨테이너의 복사본 생성
            int[,] copy = (int[,])container.Clone();

            // 블록은 해당 컨테이너 안에서만 활동 가능
            if (x + arrBlock.GetUpperBound(1) <= copy.GetUpperBound(1) &&
                y + arrBlock.GetUpperBound(0) <= copy.GetUpperBound(0))
            {
                // 이미 다른 블록이 있는 위치라면?
                for (int i = 0; i <= arrBlock.GetUpperBound(1); i++) // 열반복
                {
                    for (int j = 0; j <= arrBlock.GetUpperBound(0); j++) // 행반복
                    {
                        if (arrBlock[j, i] != 0)
                        {
                            if (copy[y + j, x + i] != 0)
                            {
                                return false; // 들어갈 수 없다, 이미 블록이 있다
                            }
                        }
                    }
                }
                return true;
            }
            else
                return false;
            return false;
        } 
        #endregion

        #region FixBlock : 컨테이너에 현재 위치값에 해당하는 현재 블록을 덮어 쓰기
        /// <summary>
        /// 컨테이너에 현재 위치값에 해당하는 현재 블록을 덮어 쓰기
        ///  - 현재 X 좌표 + 현재 블록의 가로 크기 : 컨테이너의 X 크기
        ///  - 현재 Y 좌표 + 현재 블록의 세로 크기 : 컨테이너의 Y 크기
        /// </summary>
        /// <param name="arrContainer">게임 필드 영역</param>
        /// <param name="arrCurrentBlock">하나의 블록 개체</param>
        /// <param name="x">X 좌표(0 기준)</param>
        /// <param name="y">Y 좌표(0 기준)</param>
        /// <returns>컨테이너에 넘겨준 블록 개체를 포함한 2차원 배열</returns>
        private int[,] FixBlock(int[,] arrContainer, int[,] arrCurrentBlock, int x, int y)
        {
            // 컨테이너에 현재 위치값에 해당하는 현재 블록을 덮어 쓰기
            // 현재 X 좌표 + 현재 블록의 가로 크기 <= 컨테이너의 X 크기
            // 현재 Y 좌표 + 현재 블록의 세로 크기 <= 컨테이너의 Y 크기
            if ((x + currentBlock.GetUpperBound(1) <= arrContainer.GetUpperBound(1)))
            {
                if (y + currentBlock.GetUpperBound(0) <= arrContainer.GetUpperBound(0))
                {
                    //해당 컨테이너 내에서 블록을 덮어쓰기 : #### 0-3
                    for (int i = 0; i <= arrCurrentBlock.GetUpperBound(1); i++) // arrCurrentBlock.GetUpperBound(1) 열 수
                    {
                        //  ## : x(i) : 0~2 => 열 반복
                        // ##  : y(j) : 0~1 => 행 반복
                        for (int j = 0; j <= arrCurrentBlock.GetUpperBound(0); j++) // arrCurrentBlock.GetUpperBound(0) 행 수
                        {
                            if (arrCurrentBlock[j, i] != 0)
                            {
                                arrContainer[y + j, x + i] = arrCurrentBlock[j, i];
                            }
                        }
                    }
                }
            }
            return arrContainer; // 반환
        }
        #endregion

        // 대리자(delegate)와 이벤트 선언 부

        // 이벤트
        //  - 개체나 클래스가 알림을 제공 할 수 있게 하는 멤버
        //  - 클라이언트는 이벤트 처리기를 제공하여 이벤트에 대한 실행 코드를 추가

        /// <summary>
        /// [1] 대리자 : LinesDonHandler
        /// 몇개의 라인이 완성되었는지 확인 이벤트
        /// </summary>
        /// <param name="lines"></param>
        public delegate void LinesDoneHandler(int lines);

        /// <summary>
        /// 게임오버 이벤트를 위한 대리자
        /// </summary>
        public delegate void GameOverHandler();

        /// <summary>
        /// [2] 이벤트 : LinesDone
        /// 라인이 완성되었는지 알리는 역할
        /// </summary>
        public event LinesDoneHandler LinesDone;

        /// <summary>
        /// 게임오버 이벤트
        /// </summary>
        public event GameOverHandler GameOver;
    }
}
