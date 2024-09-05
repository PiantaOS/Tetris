using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public static class RotationData {
        public static PieceInfo[,] dataL = new PieceInfo[4, 4];
        public static PieceInfo[,] dataT = new PieceInfo[4, 4];
        public static PieceInfo[,] dataS = new PieceInfo[4, 4];
        public static PieceInfo[,] dataZ = new PieceInfo[4, 4];
        public static PieceInfo[,] dataI = new PieceInfo[4, 4];
        public static PieceInfo[,] dataJ = new PieceInfo[4, 4];
        public static PieceInfo[,] dataO = new PieceInfo[4, 4];

        //var 1 is rotation, var 2 is offset #
        public static Vector2[,] oDataJLSTZ;
        public static Vector2[,] oDataI;
        public static Vector2[,] oDataO;


        static RotationData() {
            SetRotationData();
        }

        static void SetRotationData() {
            //dataI
            dataI = new PieceInfo[4, 4];

            dataI[0, 1] = new PieceInfo(new Vector2(1, 1), new Vector2(0, 0), new Vector2(-1, -1), new Vector2(-2, -2));
            dataI[1, 0] = new PieceInfo(new Vector2(-1, -1), new Vector2(0, -0), new Vector2(1, 1), new Vector2(2, 2));
            dataI[1, 2] = new PieceInfo(new Vector2(1, -1), new Vector2(0, 0), new Vector2(-1, 1), new Vector2(-2, 2));
            dataI[2, 1] = new PieceInfo(new Vector2(-1, 1), new Vector2(-0, 0), new Vector2(1, -1), new Vector2(2, -2));
            dataI[2, 3] = new PieceInfo(new Vector2(-1, -1), new Vector2(0, 0), new Vector2(1, 1), new Vector2(2, 2));
            dataI[3, 2] = new PieceInfo(new Vector2(1, 1), new Vector2(0, 0), new Vector2(-1, -1), new Vector2(-2, -2));
            dataI[3, 0] = new PieceInfo(new Vector2(-1, 1), new Vector2(0, 0), new Vector2(1, -1), new Vector2(2, -2));
            dataI[0, 3] = new PieceInfo(new Vector2(1, -1), new Vector2(-0, -0), new Vector2(-1, 1), new Vector2(-2, 2));

            oDataI = new Vector2[4, 5];
            //oDataI
            oDataI[0, 0] = new Vector2(0, 0);
            oDataI[0, 1] = new Vector2(-1, 0);
            oDataI[0, 2] = new Vector2(2, 0);
            oDataI[0, 3] = new Vector2(-1, 0);
            oDataI[0, 4] = new Vector2(2, 0);
            oDataI[1, 0] = new Vector2(-1, 0);
            oDataI[1, 1] = new Vector2(0, 0);
            oDataI[1, 2] = new Vector2(0, 0);
            oDataI[1, 3] = new Vector2(0, 1);
            oDataI[1, 4] = new Vector2(0, -2);
            oDataI[2, 0] = new Vector2(-1, 1);
            oDataI[2, 1] = new Vector2(1, 1);
            oDataI[2, 2] = new Vector2(-2, 1);
            oDataI[2, 3] = new Vector2(1, 0);
            oDataI[2, 4] = new Vector2(-2, 0);
            oDataI[3, 0] = new Vector2(0, 1);
            oDataI[3, 1] = new Vector2(0, 1);
            oDataI[3, 2] = new Vector2(0, 1);
            oDataI[3, 3] = new Vector2(0, -1);
            oDataI[3, 4] = new Vector2(0, 2);
          
        }
    } 
}
