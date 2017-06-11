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
            // 총 6개의 블록 생성
            // ####
            int[,] firstBlock = new int[1, 4] { { 1, 1, 1, 1 } }; //White
            // ##
            // ##
            int[,] secondBlock = new int[2, 2] { { 2, 2 }, { 2, 2 } }; //Magenta
            // #
            // ###
            int[,] thirdBlock = new int[2, 3] { { 0, 0, 3 }, { 3, 3, 3 } }; //Blue
            //   #
            // ###
            int[,] fourthBlock = new int[2, 3] { { 4, 0, 0 }, { 4, 4, 4 } }; //Green
            //  ##
            // ##
            int[,] fifthBlock = new int[2, 3] { { 0, 5, 5 }, { 5, 5, 0 } }; //Yellow
            // ##
            //  ##
            int[,] sixthBlock = new int[2, 3] { { 6, 6, 0 }, { 0, 6, 6 } }; //Red 
            #endregion

            //리스트에 6개 블록 담기
            blocks = new List<int[,]>();
            blocks.Add(firstBlock);
            blocks.Add(secondBlock);
            blocks.Add(thirdBlock);
            blocks.Add(fourthBlock);
            blocks.Add(fifthBlock);
            blocks.Add(sixthBlock);

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
    }
}
