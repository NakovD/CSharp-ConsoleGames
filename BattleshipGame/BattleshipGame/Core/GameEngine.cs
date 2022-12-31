namespace BattleshipGame.Core
{
    using Enums;
    using Models;
    using Models.Grid;
    using Models.Ship;

    using ConsoleInitialSetup;

    using System.Text;
    using System;
    using BattleshipGame.Models.Ship.Contracts;
    using System.Collections.ObjectModel;

    public class GameEngine
    {
        private static readonly IReadOnlyDictionary<string, int> shipTypes
            = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>()
        {
            { "Carrier", 5 },
            { "Battleship", 4 },
            { "Cruiser", 3 },
            { "Submarine", 3 },
            { "Destroyer", 2 },
        });

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
            playerGrid.Draw();

            var opponentGrid = new Grid(centerX + spaceBetweenGrids, centerY);
            opponentGrid.Draw();

            var playerShips = GetShips(playerGrid.Boundaries);

            foreach (var ship in playerShips)
            {
                PositionShip(ship, playerGrid);
                playerGrid.Ships.Add(ship);
            }
        }

        private List<Ship> GetShips(IReadOnlyDictionary<string, int> boundaries)
        {
            var ships = new List<Ship>();

            foreach (var shipType in shipTypes)
            {
                var newShip = new Ship(shipType.Key, shipType.Value, boundaries);
                ships.Add(newShip);
            }

            return ships;
        }

        private void PositionShip(Ship ship, Grid playerGrid)
        {
            ship.Draw();
            while (true)
            {
                ship.SetBackgroundColor();
                var key = Console.ReadKey().Key;

                var direction = Direction.Up;

                if (key == ConsoleKey.Enter) { ship.Lock(); break; }
                if (key == ConsoleKey.UpArrow) direction = Direction.Up;
                if (key == ConsoleKey.DownArrow) direction = Direction.Down;
                if (key == ConsoleKey.LeftArrow) direction = Direction.Left;
                if (key == ConsoleKey.RightArrow) direction = Direction.Right;

                var nextShipCoordinates = GetNextShipCoordinates(ship.Coordinates, direction);

                var overlappingShips = playerGrid.Ships.Where(s => s.Coordinates.Intersect(nextShipCoordinates).Any()).ToList();
                var areThereAnyOverlappingShips = overlappingShips.Any();

                if (areThereAnyOverlappingShips)
                {
                    ship.SetBackgroundColor(ConsoleColor.Red);
                    Console.Beep();
                    ship.Move(direction);
                    ship.SetBackgroundColor();
                    continue;
                }

                ship.Move(direction);
                playerGrid.Ships.ForEach(s => (s as Ship)?.Draw());

                continue;
            }
        }

        private List<Cell> GetNextShipCoordinates(List<Cell> coordinates, Direction direction)
        {
            var nextCoordinates = new List<Cell>();
            coordinates.ForEach(c =>
            {
                var newCell = new Cell(c.X, c.Y);
                nextCoordinates.Add(newCell);
            });

            if (direction == Direction.Right) nextCoordinates.ForEach(cell => cell.X += 2);
            else if (direction == Direction.Left) nextCoordinates.ForEach(cell => cell.X -= 2);
            else if (direction == Direction.Up) nextCoordinates.ForEach(cell => cell.Y -= 2);
            else nextCoordinates.ForEach(cell => cell.Y += 2);

            return nextCoordinates;
        }
    }
}
