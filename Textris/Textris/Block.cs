using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textris
{
    /// <summary>
    /// 테트리스 블록 생성 클래스
    /// </summary>
    public class Block
    {
        /// <summary>
        /// 테트리스 블록 6개를 담아 놓을 그릇(리스트)
        /// </summary>
        private List<int[,]> blocks;

        /// <summary>
        /// 랜덤하게 블록을 생성할 개체
        /// </summary>
        private Random r;

        public Block()
        {
            #region 총 6개의 블록 생성
            // 총 7개의 블록 생성
            // ####
            int[,] firstBlock = new int[1, 4] { { 1, 1, 1, 1 } }; //White
            // ##
            // ##
            int[,] secondBlock = new int[2, 2] { { 2, 2 }, { 2, 2 } }; //Magenta
            //  #
            // ###
            int[,] thirdBlock = new int[2, 3] { { 0, 3, 0 }, { 3, 3, 3 } }; //Blue
            // #
            // ###
            int[,] fourthBlock = new int[2, 3] { { 0, 0, 4 }, { 4, 4, 4 } }; //Green
            //   #
            // ###
            int[,] fifthBlock = new int[2, 3] { { 4, 0, 0 }, { 4, 4, 4 } }; //Green
            //  ##
            // ##
            int[,] sixthBlock = new int[2, 3] { { 0, 5, 5 }, { 5, 5, 0 } }; //Yellow
            // ##
            //  ##
            int[,] seventhBlock = new int[2, 3] { { 6, 6, 0 }, { 0, 6, 6 } }; //Red 
            #endregion

            //리스트에 7개 블록 담기
            blocks = new List<int[,]>();
            blocks.Add(firstBlock);
            blocks.Add(secondBlock);
            blocks.Add(thirdBlock);
            blocks.Add(fourthBlock);
            blocks.Add(fifthBlock);
            blocks.Add(sixthBlock);
            blocks.Add(secondBlock);

            //랜덤개체 초기화
            r = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// 특정 인덱스의 블록을 반환
        /// </summary>
        /// <param name="id">0부터 5까지의 인덱스</param>
        /// <returns>블록(2차운 정수 배열)</returns>
        public int[,] GetBlock(int id)
        {
            // id는 0부터 5까지 넘겨져 와야함
            if (blocks.Count() > id && id >= 0)
            {
                return blocks[id]; 
            }
            return null;
        }

        /// <summary>
        /// 랜덤하게 블록을 반환
        /// </summary>
        /// <returns>랜덤한 블록</returns>
        public int[,] GetRandomBlock()
        {
            return blocks[r.Next(blocks.Count())];
        }

        /// <summary>
        /// 블록을 시계 방향으로 1회전
        /// </summary>
        /// <param name="block">원본 블록</param>
        /// <returns>회전된 블록</returns>
        public static int[,] RotateRight(int[,] block)
        {
            // 행과 열의 반대인 배열 생성 : 
            int w = block.GetUpperBound(0) + 1; // 2행
            int h = block.GetUpperBound(1) + 1; // 3열
            int[,] rotated = new int[h, w]; // w,h => h,w
            //원본값을 채우기
            for (int i = 0; i < w; i++)         // 2번 반복
            {
                for (int j = 0; j < h; j++)     // 3번 반복
                {
                    // 앞에 있었던 것을 뒤에서 부터 채우기
                    rotated[j, (w - i - 1)] = block[i, j];
                }
            }
            //반환
            return rotated;
        }

        /// <summary>
        /// 블록을 시계 반대 방향으로 1회전 : 시계 방향으로 3회전
        /// </summary>
        /// <param name="block">원본 블록</param>
        /// <returns>회전된 블록</returns>
        public static int[,] RotateLeft(int[,] block)
        {
            return RotateRight(RotateRight(RotateRight(block)));
        }
    }
}
