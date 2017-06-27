using System;
using System.Threading;
/*
[Textris]
 - 영역 : 가로 10칸, 세로 20칸 점수는 (1, 13)

*/
namespace Textris
{
    class Program
    {
        /// <summary>
        /// 총 완성시킨 점수(행수;라인수) : 카운트
        /// </summary>
        private static int points;
        /// <summary>
        /// Tetris Game 클래스
        /// </summary>
        private static Tetris t;

        /// <summary>
        /// 이동 관련 스레드
        /// </summary>
        private static Thread mover;

        /// <summary>
        /// 그리기 함수 잠금 여부
        /// </summary>
        private static bool drawLock;

        /// <summary>
        /// 워커 스레드 개수
        /// </summary>
        private static int threadCounter = 0; // 0부터 1000까지 반복(STEP 만큼) : 기본 100번 반복
        /// <summary>
        /// 하나의 스레드에서 실행할 Step 단위
        /// </summary>
        private const int STEP = 10;

        #region 블록 그리기 관련 상수들
        private const string BLOCK = "B";
        private const string BOX = "|";
        private const string EMPTY = " ";
        private const string ACTIVE = "A";
        private const string SHADOW = "S";
        #endregion

        #region 다음 블록 보이기 관련 필드들
        /// <summary>
        /// "다음" 블록에 대한 영역 설정 배열 : 클리어
        /// </summary>
        private static readonly int[,] clearBlock = new int[,] {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
        };

        /// <summary>
        /// 다음 블록 보이기 여부[y/n]?
        /// </summary>
        private static bool showNext; 
        #endregion

        static void Main(string[] args)
        {
            #region 콘솔 초기화 및 시작 화면 구성 영역
            Console.Clear(); //콘솔 화면 초기화
            Console.Beep(600, 500); //600Hz의 비프음을 500ms만큼 실행
            Console.ForegroundColor = ConsoleColor.Gray; //글자색
            Console.BackgroundColor = ConsoleColor.Black; //배경색
            Console.CursorVisible = false; //커서 보이기 off

            // 시작 화면
            Console.WriteLine("C# 콘솔 테트리스");
            Console.Write(@"
====================================
조작법 :
[→]     블록을 오른쪽으로 이동
[←]     블록을 왼쪽으로 이동
[↑]     블록을 아래로 떨어뜨리기
[↓]     블록을 아래로 1칸 내리기
[S]     시계 방향으로 돌리기
[A]     시계 방향으로 3번 돌리기
[G]     완성된 블록 보기 켜고 끄기
[N]     다음 블록 미리보기 켜고 끄기
[ESC]   게임 종료

a키를 누르면 시작됩니다.
====================================
            ");

            while(Console.ReadKey(true).Key != ConsoleKey.A)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.A)
                    break;
            }

            Console.Clear();
            #endregion

            #region 게임 진행

            showNext = true; // 기본으로 다음 블록을 보이기
            drawLock = false; // 제일 처음에는 lock을 걸지 않는다
            // 게임 클래스 초기화 영역
            // t = new Tetris();   // 테트리스 클래스의 인스턴스 생성
            t = new Tetris(10, 20); // 테트리스 클래스의 인스턴스 생성

            // 이벤트 처리기 등록
            t.LinesDone += T_LinesDone;
            //t.GameOver += new Tetris.GameOverHandler(T_GameOver); // 공식 코드
            //t.GameOver += T_GameOver;
            //t.GameOver += delegate { Console.WriteLine("게임 종료"); }; // 무명(익명) 메소드
            t.GameOver += () => { /*Console.WriteLine("게임 종료");*/ }; //람다식
            // 람다식은 .net 3.5이상
            t.GameStart();

            // Mover 스레드 실행
            mover = new Thread(new ThreadStart(Stepper));
            mover.IsBackground = true; // 배경 스레드 활동
            mover.Start();

            #region 키보드 처리기
            //키보드 처리기
            while (t.isRunning)
            {
                if (Console.KeyAvailable)                                                                           //C에서는 kbhit()
                {
                    // 키보드 조작
                    switch (Console.ReadKey(true).Key) //Console.ReadKey(true)에서 true값을 주면 콘솔창에 누른키 표시 X //C : getch()
                    {
                        case ConsoleKey.A:
                            //Console.WriteLine("시계 방향으로 3번 돌리기");
                            t.KeyInput(Tetris.Key.TurnLeft);
                            break;
                        case ConsoleKey.S:
                            //Console.WriteLine("시계 방향으로 돌리기");
                            t.KeyInput(Tetris.Key.TurnRight);
                            break;
                        case ConsoleKey.G:
                            //Console.WriteLine("완성된 블록 보기 켜고 끄기");
                            t.ShadowBlock = !t.ShadowBlock; // 토글(true => false)
                            break;
                        case ConsoleKey.N:
                            showNext = !showNext; // true <-> false 토클
                            break;
                        case ConsoleKey.Escape:
                            //Console.WriteLine("게임 종료");
                            t.GameEnd();
                            break;
                        case ConsoleKey.UpArrow:
                            //Console.WriteLine("블록을 아래로 떨어뜨리기");
                            {
                                t.KeyInput(Tetris.Key.Up);
                                threadCounter = 0; // 다시 0부터 1000까지 반복
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            //Console.WriteLine("블록을 아래로 1칸 내리기");
                            {
                                t.KeyInput(Tetris.Key.Down);
                                threadCounter = 0; // 다시 0부터 1000까지 반복
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            //Console.WriteLine("블록을 왼쪽으로 이동");
                            t.KeyInput(Tetris.Key.Left);
                            break;
                        case ConsoleKey.RightArrow:
                            //Console.WriteLine("블록을 오른쪽으로 이동");
                            t.KeyInput(Tetris.Key.Right);
                            break;
                        default:
                            break;
                    }
                    //현재 키값에 해당하는 블록을 그리기
                    DrawField();
                }
            } 
            #endregion

            #endregion

            #region 게임 종료 화면
            Thread.Sleep(1000);
            Console.Clear();

            string msg = $@"

      ___           ___           ___           ___     
     /  /\         /  /\         /__/\         /  /\    
    /  /:/_       /  /::\       |  |::\       /  /:/_   
   /  /:/ /\     /  /:/\:\      |  |:|:\     /  /:/ /\  
  /  /:/_/::\   /  /:/~/::\   __|__|:|\:\   /  /:/ /:/_ 
 /__/:/__\/\:\ /__/:/ /:/\:\ /__/::::| \:\ /__/:/ /:/ /\
 \  \:\ /~~/:/ \  \:\/:/__\/ \  \:\~~\__\/ \  \:\/:/ /:/
  \  \:\  /:/   \  \::/       \  \:\        \  \::/ /:/ 
   \  \:\/:/     \  \:\        \  \:\        \  \:\/:/  
    \  \::/       \  \:\        \  \:\        \  \::/   
     \__\/         \__\/         \__\/         \__\/    
      ___                        ___           ___     
     /  /\          ___         /  /\         /  /\    
    /  /::\        /__/\       /  /:/_       /  /::\   
   /  /:/\:\       \  \:\     /  /:/ /\     /  /:/\:\  
  /  /:/  \:\       \  \:\   /  /:/ /:/_   /  /:/~/:/  
 /__/:/ \__\:\  ___  \__\:\ /__/:/ /:/ /\ /__/:/ /:/___
 \  \:\ /  /:/ /__/\ |  |:| \  \:\/:/ /:/ \  \:\/:::::/
  \  \:\  /:/  \  \:\|  |:|  \  \::/ /:/   \  \::/~~~~ 
   \  \:\/:/    \  \:\__|:|   \  \:\/:/     \  \:\     
    \  \::/      \__\::::/     \  \::/       \  \:\    
     \__\/           ~~~~       \__\/         \__\/    
최종 점수 : {points}행 완성.
ESC 키를 누르면 종료합니다.
";
            ConsoleColor color = ConsoleColor.Red;
            WriteColorMessage(msg, color);

            //ESC 키를 누를 동안 프로그램 종료 대기
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                ////무한 반복
                //Console.WriteLine($"{Console.ReadKey(true).Key}를"
                //    + "누르셨군요. ESC 키를 누르기 전까지 종료가 안됩니다");
                
            }
            Console.ResetColor();
            Console.CursorVisible = true;
            #endregion

            #region 프로그램 종료
            // 프로그램 종료
            return; 
            #endregion
        }

        /// <summary>
        /// Mover 스레드에서 사용하는 1초에 한번씩 아래로 떨어뜨리는 로직
        /// </summary>
        private static void Stepper()
        {
            while (t.isRunning)
            {
                t.Step();
                DrawField();

                //Thread.Sleep(1000); //1초 대기 
                // 사용자 입력(위쪽 또는 아래쪽 방향키) 후 1초간 시간차 부여
                threadCounter = 0;
                //[1] threadCounter에 의해서 무조건 1초에 한번씩 떨어짐
                //[2] 한 라인을 맞출수록(1포인트 상승) 5밀리초씩 빨라지는 로직 추가
                while (threadCounter < (1000 - points * 5) && t.isRunning) 
                {
                    Thread.Sleep(STEP); // 10 밀리초마다 대기
                    threadCounter += STEP;
                }


            }
        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        private static void T_GameOver()
        //        {
        //            Console.WriteLine(
        //@"
        //=======================
        //게임이 종료되었습니다.
        //=======================
        //"
        //);
        //            Thread.Sleep(3000);
        //        }

        /// <summary>
        /// 하나의 라인이 완성될때마다 라인수 증가
        /// </summary>
        /// <param name="lines">현재 완성된 라인수</param>
        private static void T_LinesDone(int lines)
        {
            points += lines;
        }

        /// <summary>
        /// 게임 영역에 대한 그리기 함수
        /// </summary>
        private static void DrawField()
        {
            //잠금 기능 구현
            while (drawLock)
            {
                // Empty
            }
            drawLock = true;

            // 현재 콘솔 내의 커서 위치
            int posX = Console.CursorLeft;
            int posY = Console.CursorTop;

            //점수 출력
            Console.CursorLeft = 13;
            Console.CursorTop = 1;
            Console.WriteLine($"점수 : {points}");

            //원래 위치로 돌아오기
            Console.SetCursorPosition(posX, posY);

            WriteArray(t.GameFieldData, true);  //랜덤하게 만들어진 블록 출력

            //다음 블록 미리보기
            Console.CursorLeft = 13;
            Console.CursorTop = 3;
            WriteArray(clearBlock, true); //다음 블록 출력할 부분 클리어
            if(showNext)
            {
                Console.CursorLeft = 14;
                Console.CursorTop = 4;
                
                WriteArray(t.Next, false);
            }

            //다시 처음으로
            Console.SetCursorPosition(posX, posY);

            // 잠금 해제
            drawLock = false;
        }

        

        #region WriteArray : 콘솔에 2D 배열을 출력
        /// <summary>
        /// 콘솔에 2D 배열을 출력
        /// </summary>
        /// <param name="arr">넘겨온 2차원 배열 : 블록이 존재함</param>
        /// <param name="writeBorder">테두리를 그릴건지 여부</param>
        private static void WriteArray(int[,] arr, bool writeBorder)
        {
            int x = Console.CursorLeft; // X좌표
            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                if (writeBorder)
                {
                    Console.Write(BOX);
                }
                for (int j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    // 2차원 배열의 값이 0이 아니면 해당 위치에 블록 출력

                    switch (arr[i, j])
                    {
                        case 1:
                            WriteColorMessage(BLOCK, ConsoleColor.White);       // ####
                            break;
                        case 2:
                            WriteColorMessage(BLOCK, ConsoleColor.Magenta);     // ##
                            break;                                              // ##
                        case 3:
                            WriteColorMessage(BLOCK, ConsoleColor.Blue);        //   #
                            break;                                              // ###
                        case 4:
                            WriteColorMessage(BLOCK, ConsoleColor.Green);       // #
                            break;                                              // ####
                        case 5:
                            WriteColorMessage(BLOCK, ConsoleColor.Yellow);      //  ##
                            break;                                              // ##
                        case 6:
                            WriteColorMessage(BLOCK, ConsoleColor.Red);         // ##
                            break; ;                                            //  ##
                        case 7:  //현재 실행중인 블록
                            Console.BackgroundColor = ConsoleColor.Red;
                            WriteColorMessage(ACTIVE, ConsoleColor.Cyan);   //##
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        case 8:  //현재 실행중인 블록에 대한 완성된 부분을 보여주는 그림자 블록
                            Console.BackgroundColor = ConsoleColor.Gray;
                            WriteColorMessage(EMPTY, ConsoleColor.Gray);   //##
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        default:
                            Console.Write(EMPTY);
                            break;
                    }
                }
                if (writeBorder)
                {
                    Console.WriteLine(BOX);
                }
                else
                {
                    Console.WriteLine();
                }

                Console.CursorLeft = x;
            }
            for (int k = 0; k <= arr.GetUpperBound(1) + 2; k++)
            {
                if (writeBorder)
                {
                    Console.Write(BOX);
                }
            }
            //WriteColorMessage(BLOCK, ConsoleColor.Blue);
        } 
        #endregion

        #region WriteColorMessage : 특정 전경색을 기반으로 텍스트/블록 출력
        /// <summary>
        /// WriteColorMessage : 특정 전경색을 기반으로 텍스트/블록 출력
        /// </summary>
        /// <param name="msg">텍스트(단순문자열, 출력할 블록)</param>
        /// <param name="color">색상</param>
        private static void WriteColorMessage(string msg, ConsoleColor color)
        {
            ConsoleColor cc = Console.ForegroundColor; //원래 전경색 기록
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = cc; //원래 전경색으로 돌아옴
        } 
        #endregion
    }
}
