using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Tetris {


    /* 
    CURRENT PROBLEMS / THINGS TO WORK ON

    Soft dropping
    Hard Dropping
    Ghost display
    Lock Delay - https://tetris.wiki/Lock_delay
    Make pieces/board look nicer
    Rotation - https://tetris.wiki/Super_Rotation_System
    Holding
    Bag System

    Player Input

    REFACTOR
    */





    /*
    VERY VERY SPECIAL THANKS
    https://stackoverflow.com/questions/7448589/interrupt-a-sleeping-thread - MobDev
    https://stackoverflow.com/questions/14854878/creating-new-thread-with-method-with-parameter
    */

    public enum Orientation {
        ZERO,
        RIGHT,
        TWO,
        LEFT
    }
    public static class UserSettings {
        public static float DAS { get; set; } //Delayed Auto Shift: the time between the initial keypress and the start of its automatic repeat movement, measured in frames (or milliseconds)
        public static float ARR { get; set; } //Automatic Repeat Rate: the speed at which tetrominoes move when holding down movement keys, measured in frames per movement (or milliseconds)
        public static float DCD { get; set; } //DAS Cut Delay: if not 0, any ongoing DAS movement will pause for a set amount of time after dropping/rotating a piece, measured in frames (or milliseconds) (keep at zero usually)
        public static float SDC { get; set; } //Soft Drop Factor: the factor with which soft drop changes the gravity speed (measured in multiples, for example 10x)
        public static int boardWidth { get; set; } //The width of the board
        public static int boardHeight { get; set; } //The height of the board


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

            Line l = new Line();
            l.Spawn();


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameTimeWrapper.gameTime = gameTime;
            GameTimeWrapper.gameTime.TotalGameTime = gameTime.TotalGameTime;

            Board.activePiece.Rotate();
            Board.activePiece.Move();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            Rectangle destRect = new Rectangle(300, 30, 200, 400);

            _spriteBatch.Draw(boardTex, destRect, Color.Gray);

            foreach (Mino mino in Board.activePiece.minos) {
                mino.SetRectangle();
                _spriteBatch.Draw(mino.texture, mino.texRect, Color.White);
            }


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
        public static Rectangle[,] rBoard = new Rectangle[UserSettings.boardWidth, UserSettings.boardHeight + 1]; //Version of the board comprised of rectangles
        public static Texture2D[,]? tBoard = new Texture2D[UserSettings.boardWidth, UserSettings.boardHeight + 1]; //Version of the board comprised of texture2ds
        public static bool[,] bBoard = new bool[UserSettings.boardWidth, UserSettings.boardHeight + 1];  //Version of the board comprised of bools
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
                    rBoard[i, j] = new Rectangle();
                    tBoard[i, j] = null;
                    bBoard[i, j] = false;
                }
            }

            
        }
        public static void CheckLines() {
            throw new System.NotImplementedException();

        }

        public static void SetActive(Piece piece) { // Add piece here
            switch (piece) {
                case Line l:
                    foreach (Mino mino in l.minos) {
                        mino.texture = lightBlueTex;
                    }
                    break;
                default:
                    break;

            }
            activePiece = piece;
        }

    }

    public static class GameTimeWrapper { public static GameTime gameTime; }

    public abstract class Piece {

        //Most of these variables are boilerplate, refactor later
        Dictionary<string, Orientation> turnMap = new Dictionary<string, Orientation>();
        Dictionary<Orientation, Vector2[]> relativePosition = new Dictionary<Orientation, Vector2[]>();
        Vector2[,] wallKickData = new Vector2[8, 5] {{new Vector2(0, 0 ), new Vector2(-1, 0) , new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2)},
                                                     {new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2)},
                                                      {new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2)},
                                                      {new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2)},
                                                       {new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(-1, -2)},
                                                       {new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)},
                                                        {new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2)},
                                                        {new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1 ), new Vector2(0, -2), new Vector2(1, -2)}};

        public Mino[] minos = new Mino[4];

        bool dasActive;
        bool arrActive;
        Orientation orientation = Orientation.ZERO;

        bool gFinished = true;
        bool touchingGround = false;

        int downMultiplier = 1;

        Thread[]? dasThreads = new Thread[] { null, null, null, null }; //0, 1 for left das and arr, 2, 3, for right das and arr

        bool rightPerHeld;
        bool leftPerHeld;
        bool arrReady = true;

        public Piece() {

            turnMap.Add("ZERO.Right", Orientation.RIGHT);
            turnMap.Add("ZERO.Left", Orientation.LEFT);
            turnMap.Add("RIGHT.Right", Orientation.TWO);
            turnMap.Add("RIGHT.Left", Orientation.ZERO);
            turnMap.Add("TWO.Right", Orientation.LEFT);
            turnMap.Add("TWO.Left", Orientation.RIGHT);
            turnMap.Add("LEFT.Right", Orientation.ZERO);
            turnMap.Add("LEFT.Left", Orientation.TWO);
            relativePosition.Add(Orientation.ZERO, new Vector2[4] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 1) });
            relativePosition.Add(Orientation.RIGHT, new Vector2[4] { new Vector2(2, 0), new Vector2(2, 1), new Vector2(2, 2), new Vector2(2, 3) });
            relativePosition.Add(Orientation.TWO, new Vector2[4] { new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 2), new Vector2(1, 2) });
            relativePosition.Add(Orientation.LEFT, new Vector2[4] { new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, 2), new Vector2(1, 3) });
        }
        

        public void Move() {
            KeyboardState state = Keyboard.GetState();
            //Change to whatever users input keys are
            if (state.IsKeyDown(Keys.A)) {
                if (!dasActive) {
                    bool canMoveLeft = true;
                    foreach (Mino mino in minos) {
                        if (mino.position.X - 1 < 0) {
                            canMoveLeft = false;
                            break;
                        }

                        if (Board.bBoard[(int)(mino.position.X - 1), (int)mino.position.Y]) {
                            canMoveLeft = false;
                            break;
                        }
                    }
                    if (canMoveLeft) {
                        foreach (Mino mino in minos) {
                            mino.position.X--;
                        }
                        dasActive = true;
                    }
                }
                else {
                    //Das cooldown here
                    if (!arrActive) {
                        Thread t = new Thread(() => Wait(UserSettings.DAS, ref arrActive));
                        dasThreads[0] = t;
                        t.Start();
                    }
                    else {
                        if (arrReady) {
                            bool canMoveLeft = true;
                            foreach (Mino mino in minos) {
                                if (mino.position.X - 1 < 0) {
                                    canMoveLeft = false;
                                    break;
                                }

                                if (Board.bBoard[(int)(mino.position.X - 1), (int)mino.position.Y]) {
                                    canMoveLeft = false;
                                    break;
                                }
                            }
                            if (canMoveLeft) {
                                foreach (Mino mino in minos) {
                                    mino.position.X--;
                                }
                            }
                            arrReady = false;
                            Thread d = new Thread(() => Wait(UserSettings.ARR, ref arrReady));
                            dasThreads[1] = d;
                            d.Start();
                        }
                    }

                }
            }

            if (state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.D)) {
                if (dasThreads[0] != null && dasThreads[0].ThreadState == ThreadState.Running || dasThreads[0] != null && dasThreads[0].ThreadState == ThreadState.WaitSleepJoin) {
                    dasThreads[0].Interrupt();
                    dasThreads[0] = null;
                }
                if (dasThreads[1] != null && dasThreads[1].ThreadState == ThreadState.Running || dasThreads[1] != null && dasThreads[1].ThreadState == ThreadState.WaitSleepJoin) {
                    dasThreads[1].Interrupt();
                    dasThreads[1] = null;
                }
                if (dasThreads[2] != null && dasThreads[2].ThreadState == ThreadState.Running || dasThreads[2] != null && dasThreads[2].ThreadState == ThreadState.WaitSleepJoin) { 
                    dasThreads[2].Interrupt();
                    dasThreads[2] = null;
                }
                if (dasThreads[3] != null && dasThreads[3].ThreadState == ThreadState.Running || dasThreads[3] != null && dasThreads[3].ThreadState == ThreadState.WaitSleepJoin) {
                    dasThreads[3].Interrupt();
                    dasThreads[3] = null;
                }
                dasActive = false;
                arrActive = false;
            }

            if (state.IsKeyDown(Keys.D)) {
                if (!dasActive) {
                    bool canMoveRight = true;
                    foreach (Mino mino in minos) {
                        if (mino.position.X + 1 >= UserSettings.boardWidth) {
                            canMoveRight = false;
                            break;
                        }

                        if (Board.bBoard[(int)(mino.position.X + 1), (int)mino.position.Y]) {
                            canMoveRight = false;
                            break;
                        }
                    }
                    if (canMoveRight) {
                        foreach (Mino mino in minos) {
                            mino.position.X++;
                        }
                    }
                    dasActive = true;

                }
                else {
                    //Das cooldown here
                    if (!arrActive) {
                        Thread t = new Thread(() => Wait(UserSettings.DAS, ref arrActive));
                        dasThreads[2] = t;
                        t.Start();
                    }
                    else {
                        if (arrReady) {
                            bool canMoveRight = true;
                            foreach (Mino mino in minos) {
                                if (mino.position.X + 1 >= UserSettings.boardWidth) {
                                    canMoveRight = false;
                                    break;
                                }

                                if (Board.bBoard[(int)(mino.position.X + 1), (int)mino.position.Y]) {
                                    canMoveRight = false;
                                    break;
                                }
                            }
                            if (canMoveRight) {
                                foreach (Mino mino in minos) {
                                    mino.position.X++;
                                }
                            }
                            arrReady = false;
                            Thread d = new Thread(() => Wait(UserSettings.ARR, ref arrReady));
                            dasThreads[3] = d;
                            d.Start();
                        }
                    }
                }
            }
            if (state.IsKeyDown(Keys.S)) {
                bool canMoveDown = true;
                foreach (Mino mino in minos) {
                    if ((int)mino.position.Y + 1 > UserSettings.boardHeight) {
                        canMoveDown = false;
                        break;
                    }

                    if (Board.bBoard[(int)mino.position.X, (int)mino.position.Y + 1]) {
                        canMoveDown = false;
                        break;
                    }

                }

                if (canMoveDown) {
                    //Reset gravity timer
                    downMultiplier = 25;
                }
                else {
                    downMultiplier = 1;
                }
            }

            //Gravity timer stuff here

            foreach (Mino mino in minos) {
                if ((int)mino.position.Y + 1 > UserSettings.boardHeight) {
                    touchingGround = true;
                    break;
                }

                if (Board.bBoard[(int)mino.position.X, (int)mino.position.Y + 1]) {
                    touchingGround = true;
                    break;
                }
            }

            if (!touchingGround) {
                if (gFinished) {
                    foreach (Mino mino in minos) {
                        mino.position.Y++;
                    }
                    Console.WriteLine("Gravity Tick");

                    //https://stackoverflow.com/questions/14854878/creating-new-thread-with-method-with-parameter
                    Thread t = new Thread(() => Wait(GameState.gravity / GameState.gravitySpeed / downMultiplier, ref gFinished));
                    t.Start();
                }
            }



            void Wait(float milliseconds, ref bool done) {
                done = false;
                try {
                    Thread.Sleep((int)milliseconds);

                }
                catch (ThreadInterruptedException) {
                    //ignore
                }


                done = true;
            }

        }

        public virtual void Rotate() {
            KeyboardState state = Keyboard.GetState();

            

            if (state.IsKeyDown(Keys.E)&&rightPerHeld==false) {
                RightRotation();
            }
            void RightRotation() {
                Orientation targetOrientation;
                if(turnMap.TryGetValue(orientation.ToString() + ".Right", out targetOrientation)) {
                    
                    Console.WriteLine(targetOrientation);
                }
                else {
                    Console.WriteLine("Failed Getting Rotation");
                
                }

                Vector2[] targetTransformations = new Vector2[4];

                Vector2[] nextPos;
                relativePosition.TryGetValue(targetOrientation, out nextPos);
                Vector2[] relativePos;
                relativePosition.TryGetValue(orientation, out relativePos);

                int rotationNum = (orientation == Orientation.ZERO && targetOrientation == Orientation.RIGHT) ? 0 :
                    (orientation == Orientation.RIGHT && targetOrientation == Orientation.ZERO) ? 1 :
                    (orientation == Orientation.RIGHT && targetOrientation == Orientation.TWO) ? 2 :
                    (orientation == Orientation.TWO && targetOrientation == Orientation.RIGHT) ? 3 :
                    (orientation == Orientation.TWO && targetOrientation == Orientation.LEFT) ? 4 :
                    (orientation == Orientation.LEFT && targetOrientation == Orientation.TWO) ? 5 :
                    (orientation == Orientation.LEFT && targetOrientation == Orientation.ZERO) ? 6 :
                    (orientation == Orientation.ZERO && targetOrientation == Orientation.LEFT) ? 7 : 0;



                for (int i = 0; i < 4; i++) {
                    targetTransformations[i] = nextPos[i] - relativePos[i];
                }
                Vector2[] finalVectors = new Vector2[4];
                for(int i = 0; i < 5; i++) {
                    for(int j = 0; j < 4; j++) {
                        Vector2 finalVector = (minos[j].position + targetTransformations[j]) + wallKickData[rotationNum, i];
                        finalVectors[j] = finalVector;
                        if (finalVector.X >= UserSettings.boardWidth || finalVector.Y >= UserSettings.boardWidth || finalVector.X < 0 || finalVector.Y < 0) {
                            break;
                        }
                        if (Board.bBoard[(int)finalVector.X, (int)finalVector.Y]) {
                            break;
                        }
                        goto RotateCheck;
                    }
                }
                goto End;
                RotateCheck:;

                for(int i = 0; i < 4; i++) {
                    minos[i].position = finalVectors[i];
                }
                rightPerHeld = true;

                End:;
            }
            if (rightPerHeld) {
                if (state.IsKeyUp(Keys.E)) {
                    rightPerHeld = false;
                }
            }
        }
    }
        public class Line : Piece {
            public Line() {
                //Initalize all 4 minos
                for (int i = 0; i < 4; i++) {
                    Mino m = new Mino();
                    minos[i] = m;
                }
        }

            public void Spawn() {
                minos[0].position = new Vector2(3, 0);
                minos[1].position = new Vector2(4, 0);
                minos[2].position = new Vector2(5, 0);
                minos[3].position = new Vector2(6, 0);


                Board.SetActive(this);
            }
        }
        public class Mino {
            public Vector2 position;
            public Texture2D texture;
            public Rectangle texRect;

            public void SetRectangle() {
                Rectangle rect = new Rectangle((int)position.X * 20 + 300, (int)position.Y * 20 + 10, 20, 20);
                texRect = rect;
            }
        }
    }
