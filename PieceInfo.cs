using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris {
    public class PieceInfo {
        public Vector2[] Info = new Vector2[4];


        public PieceInfo() { }
        public PieceInfo(Vector2 one, Vector2 two, Vector2 three, Vector2 four) {
            Info[0] = one;
            Info[1] = two;
            Info[2] = three;
            Info[3] = four;
        }

        public void Combine(PieceInfo other) {
            for(int i = 0; i < 4; i++) {
                Info[i] += other.Info[i];
            }
        }
    }
}
