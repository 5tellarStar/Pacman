using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze2
{
    /// <summary>
    /// Innehåller all information om ett spöke
    /// </summary>
    public class Ghost
    {
        /// <summary>
        /// Spökets X-position
        /// </summary>
        public int x = 0;
        /// <summary>
        /// Spökets Y-position
        /// </summary>
        public int y = 0;

        /// <summary>
        /// Spökets nuvarande riktning
        /// </summary>
        public int direction = 0;

        /// <summary>
        /// Vad ska spöket lämna efter sig.
        /// </summary>
        public int leaveBehind = 2;

        public int targetX = 0;
        public int targetY = 0;

        public bool frightend = false;

        public bool alive = true;
    }
}
