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
                // 수작업으로 블록 만들기
                int[,] arr = new int[20, 10];

                arr[2, 4] = 7;
                arr[3, 3] = 7;
                arr[3, 4] = 7;
                arr[3, 5] = 7;

                if (shadow)
                {
                    // 쉐도우 블록
                    arr[18, 4] = 8;
                    arr[19, 3] = 8;
                    arr[19, 4] = 8;
                    arr[19, 5] = 8; 
                }

                //####
                arr[19, 6] = 1;
                arr[19, 7] = 1;
                arr[19, 8] = 1;
                arr[19, 9] = 1;

                //##
                //##
                arr[17, 8] = 2;
                arr[17, 9] = 2;
                arr[18, 8] = 2;
                arr[18, 9] = 2;

                return arr;
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
                        break;
                    case Key.Down:
                        break;
                    case Key.Left:
                        if (posX > 0)
                        {
                            posX--; 
                        }
                        break;
                    case Key.Right:
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
