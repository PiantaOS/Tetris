using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Vector2 = System.Numerics.Vector2;

namespace Tetris {


    //Ctrl F add or change to see what to add or change



    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D boardTex;
        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here


            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            boardTex = Content.Load<Texture2D>("Board");
            Board.redTex = Content.Load<Texture2D>("RedTile");
            Board.blueTex = Content.Load<Texture2D>("BlueTile");
            Board.greenTex = Content.Load<Texture2D>("GreenTile");
            Board.lightBlueTex = Content.Load<Texture2D>("LightBlueTile");
            Board.yellowTex = Content.Load<Texture2D>("YellowTile");
            Board.purpleTex = Content.Load<Texture2D>("PurpleTile");
            Board.orangeTex = Content.Load<Texture2D>("OrangeTile");

            LinePiece l = new LinePiece();
          //  Box b = new Box();
            l.Spawn();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            Board.activePiece.UpdatePiece();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            //Draw board
            Rectangle destRect = new Rectangle(300, 30, 200, 400);

            _spriteBatch.Draw(boardTex, destRect, Color.Gray);

            
            const int xOffset = 300;
            const int yOffset = 410;
            
            const int pieceSideLength = 20;
            //Draw board active pieces
            for(int i = 0; i < Board.TextureData.GetLength(1); i++){
                for(int j = 0; j < Board.TextureData.GetLength(0); j++){
                    if(Board.TextureData[j, i]==null) { continue;}

                    //Replace overloads
                    Rectangle endRect = new Rectangle(1, 1, pieceSideLength, pieceSideLength);

                    _spriteBatch.Draw(Board.TextureData[j, i], endRect, Color.White);

                }
            }

            //Draw active piece
            const int minoCount = 4;
            
            for(int i = 0; i < minoCount; i++){
                int xMinoOffset = (int)Board.activePiece.Position[i].X * pieceSideLength;
                int yMinoOffset = (int)Board.activePiece.Position[i].Y * pieceSideLength * -1;

                
                Rectangle finalRect = new Rectangle(xOffset + xMinoOffset, yOffset + yMinoOffset, pieceSideLength, pieceSideLength);
                _spriteBatch.Draw(Board.activePiece.MinoTexture, finalRect, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }


    

    #region Pieces
    //CHANGE: Move classes to seperate file
    
 /*
    public class Box : Piece {

        public override void Spawn() {
            minos[0].position = new Vector2(4, 0);
            minos[1].position = new Vector2(5, 0);
            minos[2].position = new Vector2(4, 1);
            minos[3].position = new Vector2(5, 1);
        }
        public override void Rotate() {
            return;
        }
    } 
    */

    #endregion
   
}
