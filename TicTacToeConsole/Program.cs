using TicTacToe;

TicTacToeGame game = new TicTacToeGame();
game.Move(0, 0);
Console.WriteLine(game.Status);
Console.WriteLine(game.Field); // x]

game.Move(1, 1);
Console.WriteLine(game.Status);
Console.WriteLine(game.Field); // 0

game.Move(1, 0);
Console.WriteLine(game.Status);
Console.WriteLine(game.Field); // x

game.Move(2, 0);
Console.WriteLine(game.Status);
Console.WriteLine(game.Field); // 0

game.Move(0, 1);
Console.WriteLine(game.Status);
Console.WriteLine(game.Field); // X

game.Move(0, 2);
Console.WriteLine(game.Status);
Console.WriteLine(game.Field); // 0

