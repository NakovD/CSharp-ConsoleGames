namespace BattleshipGame.Core
{
    using Enums;
    using Models;
    using Models.Grid;
    using Models.Ship;

    using ConsoleInitialSetup;

    using System.Text;

    public class GameEngine
    {
        public void Run()
        {
            ConsoleSetup.Configure();
            Console.InputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;

            var gridCols = 21;
            var gridRows = 21;

            var spaceBetweenGrids = 30;
            var centerX = (Console.WindowWidth - (gridCols * 2) - spaceBetweenGrids / 2) / 2;
            var centerY = ((Console.WindowHeight - gridRows) / 2);

            var playerGrid = new Grid(centerX, centerY);
            var emptyCells = playerGrid.Draw().OrderBy(c => c.x);

            var opponentGrid = new Grid(centerX + spaceBetweenGrids, centerY);
            var opponentCells = opponentGrid.Draw();

            var ship = new Ship("Submarine", playerGrid.Boundaries);
            ship.SetCoordinates(emptyCells.Take(3).Select(c => new Cell(c.x, c.y)).ToList());
            ship.Draw();

            while (true)
            {
                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.UpArrow) ship.Move(Direction.Up);
                if (key == ConsoleKey.DownArrow) ship.Move(Direction.Down);
                if (key == ConsoleKey.LeftArrow) ship.Move(Direction.Left);
                if (key == ConsoleKey.RightArrow) ship.Move(Direction.Right);
                continue;
            }
        }
    }
}
