using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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


    #region Game Information
    public static class UserSettings {
        public static int DAS { get; set; } //Delayed Auto Shift: the time between the initial keypress and the start of its automatic repeat movement, measured in frames (or milliseconds)
        public static int ARR { get; set; } //Automatic Repeat Rate: the speed at which tetrominoes move when holding down movement keys, measured in frames per movement (or milliseconds)
        public static int DCD { get; set; } //DAS Cut Delay: if not 0, any ongoing DAS movement will pause for a set amount of time after dropping/rotating a piece, measured in frames (or milliseconds) (keep at zero usually)
        public static int SDC { get; set; } //Soft Drop Factor: the factor with which soft drop changes the gravity speed (measured in multiples, for example 10x)
        public static int boardWidth { get; set; } //The width of the board
        public static int boardHeight { get; set; } //The height of the board

        public static Keys leftKey { get; set;}
        public static Keys rightKey { get; set;}
        public static Keys dropKey { get; set;}
        public static Keys rotateLeftKey { get; set;}
        public static Keys rotateRightKey { get; set;}
        public static Keys hardDropKey { get; set;}
        public static Keys holdKey { get; set;}
        public static Keys flipKey { get; set;}


        static UserSettings() {
            boardWidth = 10;
            boardHeight = 20;

            DAS = 133; //ms
            ARR = 10; //ms
        }
    }

    public static class GameState {
        public const float gravity = 1000; //Every second
        public static float gravitySpeed { get; set; }

        public static int currentLevel { get; set; }
        public static float currentScore { get; set; }
        public static int linesCleared { get; set; }
        public static int tSpinsPerfomed { get; set; }

        static GameState() {
            gravitySpeed = 1;
            currentLevel = 1;
            currentScore = 0;
            linesCleared = 0;
            tSpinsPerfomed = 0;
        }

    }
    
    #endregion

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

            //Draw board active pieces
            for(int i = 0; i < Board.TextureData.Length; i++){
                
            }

            //Draw active piece


            _spriteBatch.End();
            base.Draw(gameTime);
        }


    }

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

    //CHANGE: Move classes to seperate file
    public abstract class Piece {
        public System.Numerics.Vector2[] Position = new System.Numerics.Vector2[4];

        private enum ControlState{
            Inactive, //Nothing being pressed
            Held,
            DASWaiting,
            ARRActive,
        }

        private ControlState currentState = ControlState.Inactive;

        public abstract void Spawn();

        private bool finished; //bool for use in wait function

        private bool arrWaiting; //bool to see if arr is currently waiting

        public void UpdatePiece(){
            Move();
            Rotate();
            Gravity();
        }
        private void Move(){

            KeyboardState kState = Keyboard.GetState();

            //CHANGE: This isn't great
            if(kState.IsKeyDown(UserSettings.leftKey)){

                if(!CheckSide(-1)) { return ;}
            
                switch(currentState){
                    case ControlState.Inactive:
                    //Key press
                    Shift(-1);
                    currentState = ControlState.Held;
                    break;

                    case ControlState.Held:
                    //Start timer
                    Thread t = new Thread(() => Timer.Wait(new TimeSpan(0, 0, 0, 0, UserSettings.DAS), ref finished));
                    t.Start();
                    currentState = ControlState.DASWaiting;
                    break;

                    case ControlState.DASWaiting:
                    //Check if das is done, if so move left one and start arr
                    if(!finished) { Console.WriteLine("Waiting for DAS Delay"); return; }
                    Console.WriteLine("DAS Finished");
                    currentState = ControlState.ARRActive;
                    arrWaiting = true;
                    break;

                    case ControlState.ARRActive:
                    //Check if arr timer is up, if so move and reset timer
                    if(arrWaiting){
                        Console.WriteLine("ARR Tick");
                        Shift(-1);
                        Thread ta = new Thread(() => Timer.Wait(new TimeSpan(0, 0, 0, 0, UserSettings.DAS), ref arrWaiting));
                        ta.Start();
                        break;
                    }

                    Console.WriteLine("Waiting for ARR to reset");
                    break;

                    
                }
                return;
                
            } 

            if(kState.IsKeyDown(UserSettings.rightKey)){
                //Check if moving right is possible

                if(!CheckSide(1)) { return ;}

                switch(currentState){
                    case ControlState.Inactive:
                    //Key press
                    Shift(1);
                    currentState = ControlState.Held;
                    break;

                    case ControlState.Held:
                    //Start timer
                    Thread t = new Thread(() => Timer.Wait(new TimeSpan(0, 0, 0, 0, UserSettings.DAS), ref finished));
                    t.Start();
                    currentState = ControlState.DASWaiting;
                    break;

                    case ControlState.DASWaiting:
                    //Check if das is done, if so move left one and start arr
                    if(!finished) { Console.WriteLine("Waiting for DAS Delay"); return; }
                    Console.WriteLine("DAS Finished");
                    currentState = ControlState.ARRActive;
                    arrWaiting = true;
                    break;

                    case ControlState.ARRActive:
                    //Check if arr timer is up, if so move and reset timer
                    if(arrWaiting){
                        Console.WriteLine("ARR Tick");
                        Shift(1);
                        Thread ta = new Thread(() => Timer.Wait(new TimeSpan(0, 0, 0, 0, UserSettings.DAS), ref arrWaiting));
                        ta.Start();
                        break;
                    }

                    Console.WriteLine("Waiting for ARR to reset");
                    break;

                    
                }
                return;
                //if the key has been held
            }


            //if no input was taken
            currentState = ControlState.Inactive;
            //Cancel all das and arr timers
        }

        private void Shift(int direction){
            for(int i = 0; i < Position.Length; i++){
                Position[i].X += direction;
            }
        }

        private bool CheckSide(int direction){

            //Add code to check for edge of wall
            
            for(int i = 0; i < Position.Length; i++){
                int nextXIndex = (int)Position[i].X + direction;
                int currentYPosition = (int)Position[i].Y;
                if(Board.BoardData[nextXIndex, currentYPosition] == true){
                    return false;
                }
            }



            return true;
        }
        //Work on these next
        private void Rotate(){
            throw new NotImplementedException();
        }
        private void Gravity(){
            throw new NotImplementedException();
        }

    }

    public static class Timer{
        public static void Wait(TimeSpan time, ref bool finished){
            finished = false;
                try {
                    Thread.Sleep(time.Milliseconds);

                }
                catch (ThreadInterruptedException) {
                    //ignore
                }


                finished = true;
        }
    }

    #region Pieces
    public class LinePiece : Piece{
        public override void Spawn()
        {
            Position[0] = new Vector2(3, 0);
            Position[1] = new Vector2(4, 0);
            Position[2] = new Vector2(5, 0);
            Position[3] = new Vector2(6, 0);
        }

    }    
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
