using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Textris
{
    class Program
    {
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

            Console.ReadKey(true);

            Console.Clear(); 
            #endregion
#if DEBUG
            ReadKey();
#endif
        }
    }
}
