using System.Text;

namespace TicTacToe
{

    public enum Status
    {
        WinX,
        WinO,
        Draft,
        Contain
    }
    public enum Cell
    {
        Empty = 0,
        X,
        O
    }
    public class TicTacToeGame
    {
        public Status Status { get; private set; }
        public Cell[,] Field { get; set; }

        int CountCell = 3;
        private int countMove = 0;
        private Cell player = Cell.X;
        public TicTacToeGame(int countCell = 3)
        {
            CountCell = countCell;
            Field = new Cell[CountCell, CountCell];
        }
        public void Move(int x, int y)
        {
           // check to valid input data
           // out of bounds field
           if (x < 0 || y < 0 || x >= CountCell || y >= CountCell) 
                throw new ArgumentOutOfRangeException("This input data out of bounds field"); ;

           // checking if the cell is not empty
           if (Field[x,y] != Cell.Empty) 
                throw new ArgumentException("This cell is not empty");

            Field[x,y] = player;

            // Algorithm of game
            if (isWin(x, y, player))
            {
                if (player == Cell.X)
                    Status =  Status.WinX;
                else
                    Status = Status.WinO;
            }


            // switch of player
            if (player == Cell.X)
                player = Cell.O; 
            else player = Cell.X;

            countMove++;

            if (countMove == CountCell * CountCell) { Status = Status.Draft; }

            Status = Status.Contain;  
        }
        private bool isWin(int x, int y, Cell player)
        {
            bool counterCell = true;
            // check row 
            for (int i = 0; i < CountCell; i++) 
            {
               if (i == x) continue;
               counterCell &= (Field[x, i] == player);
            }
            if (counterCell) { return true; }

            // check coll 
            counterCell = true;
            for (int i = 0; i < CountCell; i++)
            {
                if (i == y) continue;
                counterCell &= Field[i, y] == player;
            }
            if (counterCell) { return true; }

            // check main diagonal
            if (x == y)
            {
                counterCell = true;
                for (int i = 0; i < CountCell; i++)
                {
                    if (i == x) continue;
                    counterCell &= Field[i, i] == player;
                }
            }
            if (counterCell) { return true; }

            // check other diagonal
            if (x + y == CountCell - 1)
            {
                counterCell = true;
                for (int i = 0; i < CountCell; i++)
                {
                    if (i == x) continue;
                    counterCell &= Field[i, CountCell - 1 - i] == player;
                }
            }
            if (counterCell) { return true; }


            return false;
        }

    }
}