namespace Tetris{
    using Vector2 = System.Numerics.Vector2;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Console = System.Console;
    using Thread = System.Threading.Thread;
    using TimeSpan = System.TimeSpan;
    using NotImplementedException = System.NotImplementedException;
    using Microsoft.Xna.Framework.Graphics;

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
        private bool gravityFinished; //bool to see if gravity timer has finished

        public void UpdatePiece()
        {
            //HardDrop();
            Move();
           // Rotate();
            //Gravity();
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

        private void Shift(int direction){
            for(int i = 0; i < Position.Length; i++){
                Position[i].X += direction;
            }
        }

        private bool CheckSide(int direction){

            for(int i = 0; i < Position.Length; i++){
                int nextXIndex = (int)Position[i].X + direction;
                int currentYPosition = (int)Position[i].Y;


                if(nextXIndex < 0) { return false;}
                if(nextXIndex >= UserSettings.boardWidth) { return false;}
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
            if(!gravityFinished) { return;}

            Thread T = new Thread(() => Timer.Wait(GameState.gravitySpeed, ref arrWaiting));

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