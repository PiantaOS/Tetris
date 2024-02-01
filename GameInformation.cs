namespace Tetris{
    using Keys = Microsoft.Xna.Framework.Input.Keys;
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


            leftKey = Keys.A;
            rightKey = Keys.D;
            dropKey = Keys.S;
            rotateLeftKey = Keys.Q;
            rotateRightKey = Keys.E;
            hardDropKey = Keys.Space;
            holdKey = Keys.C;
            flipKey = Keys.R;
        }
    }

    public static class GameState {
        public const float gravity = 1000; //Every second
        public static int gravitySpeed { get; set; }

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
    
}