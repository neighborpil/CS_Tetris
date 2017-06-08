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
        #endregion

        public Tetris() 
            : this(10, 20)
        {

        }

        /// <summary>
        /// 새로운 테트리스 게임  초기화
        /// </summary>
        /// <param name="width">게임 영역의 가로(열) >= 10</param>
        /// <param name="height">게임 영역의 세로(행) >= 20</param>
        public Tetris(int width, int height)
        {
            if(width >= 10 && height >= 20)
            {
                container = new int[height, width]; //기본값은 20행 10열짜리 2차원 배열
            }
            else
            {
                throw new Exception("게임 영역은 반드시 10*20 이상이어야 합니다");
            }
        }



        /// <summary>
        /// 게임 시작하고 블록을 출력하기
        /// </summary>
        public void GameStart()
        {
            this.isInGame = true;

            posX = container.GetUpperBound(1) / 2; //10열 / 2 = 5
            posY = 0; // PosY = container.GetUpperBound(0); //20행 맨위
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
                        //Console.WriteLine("Tetris 클래스에서의 Up");
                        Console.CursorTop--;
                        break;
                    case Key.Down:
                        //Console.WriteLine("Tetris 클래스에서의 Down");
                        Console.CursorTop++;
                        break;
                    case Key.Left:
                        //Console.WriteLine("Tetris 클래스에서의 Left");
                        Console.CursorLeft--;
                        break;
                    case Key.Right:
                        //Console.WriteLine("Tetris 클래스에서의 Right");
                        Console.CursorLeft++;
                        break;
                    case Key.TurnRight:
                        //Console.WriteLine("Tetris 클래스에서의 TurnRight");
                        break;
                    case Key.TurnLeft:
                        //Console.WriteLine("Tetris 클래스에서의 TurnLeft");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
