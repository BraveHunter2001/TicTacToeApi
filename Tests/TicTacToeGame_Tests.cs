using TicTacToe;

namespace Tests
{
    public class TicTacToeGame_Tests
    {

        [Fact]
        public void Draft_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.X);

            game.Move(0, 1); // x
            game.Move(0, 0); // 0
            game.Move(1, 1); // x
            game.Move(2, 1); // 0
            game.Move(2, 0); // x
            game.Move(0, 2); // o
            game.Move(2, 2); // x
            game.Move(1, 0); // o
            game.Move(1, 2); // x
            Assert.Equal(Status.Draft, game.Status);

        }

        [Fact]
        public void Column_WinO_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.X);

            game.Move(0, 0); // x
            game.Move(0, 1); // o
            game.Move(0, 2); // x
            game.Move(1, 1); // o
            game.Move(1, 2); // x
            game.Move(2, 1); // o

            Assert.Equal(Status.WinO, game.Status);

        }

        [Fact]
        public void Column_WinX_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.X);

            game.Move(0, 0); // x
            game.Move(0, 1); // o
            game.Move(0, 2); // x
            game.Move(1, 1); // o
            game.Move(1, 2); // x
            game.Move(1, 0); // o
            game.Move(2, 2); // X

            Assert.Equal(Status.WinX, game.Status);

        }

        [Fact]
        public void Row_WinX_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.X);

            game.Move(0, 0); // x
            game.Move(1, 0);
            game.Move(0, 1);
            game.Move(1, 1);
            game.Move(0, 2);
            Assert.Equal(Status.WinX, game.Status);
        }

        [Fact]
        public void Row_WinO_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.O);

            game.Move(0, 0); // o
            game.Move(1, 0);
            game.Move(0, 1);
            game.Move(1, 1);
            game.Move(0, 2);
            Assert.Equal(Status.WinO, game.Status);

        }

        [Fact]
        public void Diagonal_WinX_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.X);

            game.Move(0, 0); // X
            game.Move(2, 1);
            game.Move(1, 1); 
            game.Move(0, 1);
            game.Move(2, 2);
            Assert.Equal(Status.WinX, game.Status);

        }
        [Fact]
        public void Diagonal_WinO_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.O);

            game.Move(0, 0); // X
            game.Move(2, 1);
            game.Move(1, 1);
            game.Move(0, 1);
            game.Move(2, 2);
            Assert.Equal(Status.WinO, game.Status);

        }


        [Fact]
        public void SideDiagonal_WinX_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.X);

            game.Move(0, 2); // X
            game.Move(2, 1);
            game.Move(1, 1);
            game.Move(0, 1);
            game.Move(2, 0);
            Assert.Equal(Status.WinX, game.Status);
        }

        [Fact]
        public void SideDiagonal_WinO_Successful()
        {
            TicTacToeGame game = new TicTacToeGame(Cell.O);

            game.Move(0, 2); // X
            game.Move(2, 1);
            game.Move(1, 1);
            game.Move(0, 1);
            game.Move(2, 0);
            Assert.Equal(Status.WinO, game.Status);
        }
    }
}