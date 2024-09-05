namespace Tetris{
    using Vector2 = System.Numerics.Vector2;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Console = System.Console;
    using Thread = System.Threading.Thread;
    using TimeSpan = System.TimeSpan;
    using NotImplementedException = System.NotImplementedException;
    using Microsoft.Xna.Framework.Graphics;
    using System.Runtime.InteropServices;

    public abstract class Piece {
        //Find way to set this
        public Texture2D MinoTexture;
        public Vector2[] Position = new Vector2[4];
        

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
        private bool gravityFinished = true; //bool to see if gravity timer has finished

        private int lastRotInput = 0;

        private int currentRotation = 0;
        public void SubmitPosition() {
            Console.WriteLine($"1: {Position[0]}, 2: {Position[1]}, 3: {Position[2]}, 4: {Position[3]}");
        }
        public void UpdatePiece()
        {
            //HardDrop();
            Move();
            Rotate();
            Gravity();
            //SubmitPosition();
        }
        private void Move(){


            //CHANGE: This isn't great

            int moveDirection = CheckMoveInput();
            //if no input was taken
            if(moveDirection==0){
                currentState = ControlState.Inactive;
                //Reset das and arr timers
                return;
            }
            //If moving there is not possible
            if(!CheckSide(moveDirection)){ return; }

            //CHANGE: Make this better and more concsise
            switch(currentState){
                //* Determine the action the player is trying to execute based on the current state
                    case ControlState.Inactive:
                       //Key press
                       Shift(moveDirection);
                       currentState = ControlState.Held;
                       break;

                    case ControlState.Held:
                       //Start timer
                       Thread t = new Thread(() => Timer.Wait(UserSettings.DAS, ref finished));
                       t.Start();
                       currentState = ControlState.DASWaiting;
                       break;

                    case ControlState.DASWaiting:
                       //Check if das is done, if so move left one and start arr
                       if(!finished) { return; }
                       currentState = ControlState.ARRActive;
                       arrWaiting = true;
                       break;

                    case ControlState.ARRActive:
                       //Check if arr timer is up, if so move and reset timer
                       if(arrWaiting){
                           Shift(moveDirection);
                           Thread ta = new Thread(() => Timer.Wait(UserSettings.ARR, ref arrWaiting));
                           ta.Start();
                           break;
                       }
                       break;

                    
                }
        }

        private int CheckMoveInput()
        {
            //* Figure out the direction of the input
            KeyboardState kState = Keyboard.GetState();
            int dir = 0;
            if (kState.IsKeyDown(UserSettings.leftKey)) {
                dir = -1;
            }

            if (kState.IsKeyDown(UserSettings.rightKey)) {
                dir = 1;
            }

            return dir;
        }

        private int CheckRotateInput() {
            KeyboardState kState = Keyboard.GetState();


            int dir = 0;

            if (kState.IsKeyDown(UserSettings.rotateLeftKey)) {
                dir = -1;
            }

            if (kState.IsKeyDown(UserSettings.rotateRightKey)) {
                dir = 1;
            }



            if (lastRotInput == dir) { return 0; }

            lastRotInput = dir;
            return dir;

        }
        private void Shift(int direction){
            //* Update the position of each mino to move the piece right
            for(int i = 0; i < Position.Length; i++){
                Position[i].X += direction;
            }
        }

        private void ShiftDown() {
            for(int i = 0; i < Position.Length; i++) {
                Position[i].Y -= 1;
            }
        }
        private bool CheckDown(){
            //* Determine if a specified side is clear to prevent the pieces from moving through walls and other pieces
            for(int i = 0; i < Position.Length; i++){
                int nextYIndex = (int)Position[i].Y - 1;
                int currentXPosition = (int)Position[i].X;


                if(nextYIndex < 0) { return false;}
                if(Board.BoardData[currentXPosition, nextYIndex] == true){
                    return false;
                }
            }



            return true;
        }

        private bool CheckSide(int direction) {
            //* Determine if a specified side is clear to prevent the pieces from moving through walls and other pieces
            for (int i = 0; i < Position.Length; i++) {
                int nextXIndex = (int)Position[i].X + direction;
                int currentYPosition = (int)Position[i].Y;


                if (nextXIndex < 0) { return false; }
                if (nextXIndex >= UserSettings.boardWidth) { return false; }
                if (Board.BoardData[nextXIndex, currentYPosition] == true) {
                    return false;
                }
            }



            return true;
        }
        //Work on these next
        private void Rotate(){


            int rotDir = CheckRotateInput();

            if (rotDir == 0) return;

            int nextRotNum = currentRotation + rotDir;
            if (nextRotNum < 0) nextRotNum = 3;
            if (nextRotNum > 3) nextRotNum = 0;


            //-----------------------------------------------------------------------------------------------------------------------------------------
            //change this out to a switch statement based on what piece it is
            PieceInfo nextRot = new PieceInfo();

            nextRot.Info = Position;
            nextRot.Combine(RotationData.dataI[currentRotation, nextRotNum]);
            //---------------------------------------------------------------------------------------------------------------------------------------
            //Same with this
            for(int i = 0; i < RotationData.oDataI.Length; i++) {

                //
                Vector2 offset = RotationData.oDataI[currentRotation, i] - RotationData.oDataI[nextRotNum, i]; //wahh

                PieceInfo offsetInfo = new PieceInfo(offset, offset, offset, offset);

                PieceInfo rotationTest = nextRot;
                rotationTest.Combine(offsetInfo);

                if (CheckRotation(rotationTest)) {
                    currentRotation = nextRotNum;

                    Position = rotationTest.Info;
                    return;
                }


            }
        }
        private void Gravity(){
            if(!gravityFinished) { ; return;}

            int dropSpeed = 1000 / GameState.gravitySpeed; //Prob slow

            Thread T = new Thread(() => Timer.Wait(dropSpeed, ref gravityFinished));
            T.Start();

            if (CheckDown()) {
                ShiftDown();
                return;
            }

            LockPiece();
        }

        private bool CheckRotation(PieceInfo nextRot) {
            for(int i = 0; i < 4; i++) {

                if ((int)nextRot.Info[i].X < 0) return false;
                if ((int)nextRot.Info[i].Y < 0) return false;
                if ((int)nextRot.Info[i].X > UserSettings.boardWidth) return false;
                if ((int)nextRot.Info[i].Y > UserSettings.boardHeight) return false;
                Console.WriteLine($"X: {(int)nextRot.Info[i].X}, Y: {(int)nextRot.Info[i].Y}");
                //if (Board.BoardData[(int)nextRot.Info[i].X, (int)nextRot.Info[i].Y] == true) return false;
            }

            return true;
        }

        private void LockPiece() {
            //throw new NotImplementedException();
        }

        private void HardDrop()
        {
            throw new NotImplementedException();

        }

    }
     
    public class LinePiece : Piece{
        public override void Spawn() {
            MinoTexture = Board.lightBlueTex;
            
            Position[0] = new Vector2(3, UserSettings.boardHeight);
            Position[1] = new Vector2(4, UserSettings.boardHeight);
            Position[2] = new Vector2(5, UserSettings.boardHeight);
            Position[3] = new Vector2(6, UserSettings.boardHeight);

            Board.activePiece = this;
            
        }

    }   
}