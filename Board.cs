using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Tetris{
public static class Board {

        /* {
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {false, false, false, false, false, false, false, false, false, false },
            {true , true , true , true , true , true , true , true , false, true },
            {true , true , true , true , true , true , true , true , false, true },
            {true , true , true , true , true , true , true , true , false, true },
            {true , true , true , true , true , true , true , true , false, true },
            {true , true , true , true , false, false, false, false, false, true },
            {true , true , true , true , false, true , true , true , false, true },
            {true , true , true , true , false, true , true , true , false, true },
            {true , true , true , true , false, true , true , true , false, true },
            {true , false, false, false, false, true , true , true , false, true }};
        */
        public static Piece activePiece;
        
        public static bool[,] BoardData; //Whether or not a board tile is filled in
        public static Texture2D[,] TextureData;

        public static Texture2D redTex;
        public static Texture2D greenTex;
        public static Texture2D blueTex;
        public static Texture2D lightBlueTex;
        public static Texture2D purpleTex;
        public static Texture2D yellowTex;
        public static Texture2D orangeTex;
        static Board() {
            BoardData =  new bool[UserSettings.boardWidth, UserSettings.boardHeight + 1];

            TextureData = new Texture2D[UserSettings.boardWidth, UserSettings.boardHeight];

            for (int i = 0; i < UserSettings.boardWidth; i++) {
                for (int j = 0; j < UserSettings.boardHeight + 1; j++) {
                    BoardData[i, j] = false;
                    
                }
            }
            


        }
        public static void CheckLines() {
            throw new System.NotImplementedException();
        }
       
    }
}
    