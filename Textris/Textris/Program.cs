﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Textris
{
    class Program
    {
        /// <summary>
        /// 총 완성시킨 점수(행수;라인수) : 카운트
        /// </summary>
        private static int points;

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

            #region 게임 진행


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
