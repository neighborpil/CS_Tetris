using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                // 컨테이너에 현재 위치값에 해당하는 현재 블록을 덮어 쓰기
                //현재 X 좌표 + 현재 블록의 가로 크기 <= 컨테이너의 X 크기
                //현재 Y 좌표 + 현재 블록의 세로 크기 <= 컨테이너의 Y 크기
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

                if (shadow)
                {
                    // 쉐도우 블록
                    arrContainer[18, 4] = 8;
                    arrContainer[19, 3] = 8;
                    arrContainer[19, 4] = 8;
                    arrContainer[19, 5] = 8; 
                }

                //####
                arrContainer[19, 6] = 1;
                arrContainer[19, 7] = 1;
                arrContainer[19, 8] = 1;
                arrContainer[19, 9] = 1;

                //##
                //##
                arrContainer[17, 8] = 2;
                arrContainer[17, 9] = 2;
                arrContainer[18, 8] = 2;
                arrContainer[18, 9] = 2;

                return arrContainer;
            }
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

        }

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void GameEnd()
        {
            this.isInGame = false;
        }

        /// <summary>
        /// 메인에서 특정 키값을 눌렀을 때 처리해야할 로직 구현
        /// </summary>
        /// <param name="key"></param>
        public void KeyInput(Key key)
        {
            if (isInGame)
            {
                switch (key)
                {
                    case Key.Up:
                        posY--; // 임시
                        break;
                    case Key.Down:
                        posY++;
                        break;
                    case Key.Left:
                        if (posX > 0)
                        {
                            posX--; 
                        }
                        break;
                    case Key.Right:
                        // 컨테이너 개체의 가로 바운더리 안에서 X 좌표를 이동
                        if (posX < container.GetUpperBound(0))
                        {
                            posX++;
                        }
                        break;
                    case Key.TurnRight:
                        break;
                    case Key.TurnLeft:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
