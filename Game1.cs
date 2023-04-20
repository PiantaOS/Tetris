using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tetris {
    public static class UserSettings {
        public static float DAS { get; set; } //Delayed Auto Shift: the time between the initial keypress and the start of its automatic repeat movement, measured in frames (or milliseconds)
        public static float ARR { get; set; } //Automatic Repeat Rate: the speed at which tetrominoes move when holding down movement keys, measured in frames per movement (or milliseconds)
        public static float DCD { get; set; } //DAS Cut Delay: if not 0, any ongoing DAS movement will pause for a set amount of time after dropping/rotating a piece, measured in frames (or milliseconds) (keep at zero usually)
        public static float SDC { get; set; } //Soft Drop Factor: the factor with which soft drop changes the gravity speed (measured in multiples, for example 10x)
        public static int boardWidth { get; set; } //The width of the board
        public static int boardHeight { get; set; } //The height of the board
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

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }

        
    }

    public static class Board {
        public static Piece activePiece;
        public static Rectangle[,] rBoard = new Rectangle[UserSettings.boardWidth, UserSettings.boardHeight + 1];
        public static Texture2D[,]? tBoard = new Texture2D[UserSettings.boardWidth, UserSettings.boardHeight + 1];
        public static bool[,] bBoard = new bool[UserSettings.boardWidth, UserSettings.boardHeight + 1]; //Version of the board comprised of bools

        static Board() {
            for(int i = 0; i < UserSettings.boardWidth; i++) {
                for(int j = 0; j < UserSettings.boardHeight + 1; j++) {
                    rBoard[i, j] = new Rectangle();
                    tBoard[i, j] = null;
                    bBoard[i, j] = false;
                }
            }
        }
        public static void CheckLines() {
            throw new System.NotImplementedException();
            
        }
    
    }

    
    public abstract class Piece {
        public Mino[] minos = new Mino[4];

        bool dasActive;
        Orientation orientation = Orientation.Up;

        bool gFinished;
        bool touchingGround = false;
        public enum Orientation {
            Up, 
            Down, 
            Left, 
            Right,
        }

        public void Move(, GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();
            
            //Change to whatever users input keys are

            if (state.IsKeyDown(Keys.A)) {
                if (!dasActive) {
                    bool canMoveLeft = true;
                    foreach(Mino mino in minos) {
                        if(mino.position.X - 1 < 0) {
                            canMoveLeft = false;
                            break;
                        }

                        if (Board.bBoard[(int)(mino.position.X - 1), (int)mino.position.Y]) {
                            canMoveLeft = false;
                            break;
                        }
                    }
                    if (canMoveLeft) {
                        foreach(Mino mino in minos) {
                            mino.position.X--;
                        }
                    }
                    dasActive = true;
                    //Start das cooldown
                }
                else {
                    //Das cooldown here
                }
            }

            if (state.IsKeyDown(Keys.D)) {
                if (!dasActive) {
                    bool canMoveRight = true;
                    foreach (Mino mino in minos) {
                        if (mino.position.X + 1 < UserSettings.boardWidth) {
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

                }
            }

            if (state.IsKeyDown(Keys.S)) {
                bool canMoveDown = true;
                foreach(Mino mino in minos) { 
                    if((int)mino.position.Y - 1 < 0) {
                        canMoveDown = false;
                        break;
                    }

                    if (Board.bBoard[(int)mino.position.X, (int)mino.position.Y - 1]) {
                        canMoveDown = false;
                        break;
                    }

                }

                if (canMoveDown) {
                    foreach (Mino mino in minos) {
                        mino.position.Y--;
                    }

                    //Reset gravity timer
                    runNextFrame = false;
                }
            }

            //Gravity timer stuff here

            foreach (Mino mino in minos) {
                if ((int)mino.position.Y - 1 < 0) {
                    touchingGround = true;
                    break;
                }

                if (Board.bBoard[(int)mino.position.X, (int)mino.position.Y - 1]) {
                    touchingGround = true;
                    break;
                }
            }
            if (!touchingGround) {
                if (gFinished) {
                    foreach (Mino mino in minos) {
                        mino.position.Y--;
                    }
                    runNextFrame = true;
                    Wait(GameState.gravity / GameState.gravitySpeed, gameTime);
                }
            }
        }
        bool startedWait = false;
        bool runNextFrame = true;
        TimeSpan startTime;
        public void Wait(float milliseconds, GameTime gameTime) {
            if (runNextFrame) {
                TimeSpan millisecondsTS = TimeSpan.FromMilliseconds(milliseconds);
                gFinished = false;
                if (!startedWait) {
                    startTime = TimeSpan.Zero;
                    startedWait = true;
                }
                else {
                    if (startTime + millisecondsTS < gameTime.TotalGameTime) {
                        gFinished = true;
                        startedWait = false;
                    }
                    else {
                        Wait(milliseconds, gameTime);
                    }
                }
            }
            
        }

    }

    public class Line : Piece { 
        public Line() {
            //Initalize all 4 minos without a for loop for some reason

            Mino mino1 = new Mino();
            minos[0] = mino1;

            Mino mino2 = new Mino();
            minos[1] = mino2;

            Mino mino3 = new Mino();
            minos[2] = mino3;

            Mino mino4 = new Mino();
            minos[3] = mino4;
        }

        void Spawn() {
            minos[0].position = new Vector2(3, 19);
            minos[1].position = new Vector2(4, 19);
            minos[2].position = new Vector2(5, 19);
            minos[3].position = new Vector2(6, 19);
        }
    }
    public class Mino {
        public Vector2 position;
        public Texture2D texture;
        public Rectangle texRect;

        public void SetRectangle() {
            throw new System.NotImplementedException();
        }
    }
}