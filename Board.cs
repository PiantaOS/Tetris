using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        
        public static bool[,] BoardData = new bool[,]{
  {true , true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false, false},
  {false, true , true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false},
  {true , false, true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false},
  {true , false, true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false},
  {true , false, true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false},
  {false, true , true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false},
  {true , true , true , true , true , true , true , true , true , false, false, false, false, false, false, false, false, false, false, false, false},
  {true , true , true , true , true , true , true , true , false, true , false, false, false, false, false, false, false, false, false, false, false},
  {true , true , true , true , true , true , true , true , false, true , false, false, false, false, false, false, false, false, false, false, false},
  {true , true , true , true , true , true , true , true , false, true , false, false, false, false, false, false, false, false, false, false, false}
};
        //Whether or not a board tile is filled in
        public static Texture2D[,] TextureData = new Texture2D[UserSettings.boardWidth, UserSettings.boardHeight];

        public static Texture2D redTex;
        public static Texture2D greenTex;
        public static Texture2D blueTex;
        public static Texture2D lightBlueTex;
        public static Texture2D purpleTex;
        public static Texture2D yellowTex;
        public static Texture2D orangeTex;
        static Board() {

            //BoardData =  new bool[UserSettings.boardWidth, UserSettings.boardHeight + 1];

            //TextureData =

            
            


        }

        public static void LoadPreset() {
            for (int i = 0; i < UserSettings.boardWidth; i++) {
                for (int j = 0; j < UserSettings.boardHeight; j++) {
                    //Making preset board data rendervb   
                    //Console.WriteLine($"Expected X: {BoardData.GetLength(0)}, Real X: {UserSettings.boardWidth}, Expected Y: {BoardData.GetLength(1)}, Real Y: {UserSettings.boardHeight}");
                    if (BoardData[i, j]) {
                        TextureData[i, j] = lightBlueTex;
                        Console.WriteLine("SET");

                    }
                    //BoardData[i, j] = false;

                }
            }
        }
        public static void CheckLines() {
            throw new System.NotImplementedException();
        }
       
    }
}
    