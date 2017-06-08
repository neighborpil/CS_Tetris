using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textris
{
    public class Tetris
    {
        /// <summary>
        /// 현재 게임이 실행중이면 true값을 보관,
        /// 게임 종료이면 false값을 보관
        /// </summary>
        private bool isInGame;
        /// <summary>
        /// 게임 실행 상태 표시(읽기전용)
        /// </summary>
        public bool isRunning
        {
            get
            {
                return isInGame;
            }
        }

        /// <summary>
        /// 게임 시작하고 블록을 출력하기
        /// </summary>
        public void GameStart()
        {
            this.isInGame = true;
        }

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void GameEnd()
        {
            this.isInGame = false;
        }
    }
}
