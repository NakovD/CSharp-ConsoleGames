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
        private bool shipOverlap;

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
            var initiallyOverlappingShips = playerGrid.Ships.Where(s => s.Coordinates.Intersect(ship.Coordinates).Any()).ToList();
            var areThereInitiallyAnyOverlapingships = initiallyOverlappingShips.Any();
            if (areThereInitiallyAnyOverlapingships)
            {
                shipOverlap = true;
                ship.SetBackgroundColor(ConsoleColor.Red);
                ship.Draw();
                ship.SetBackgroundColor();
                HandleOverlapingShipsDrawing(ship, initiallyOverlappingShips, playerGrid.Ships);
            }
            else ship.Draw();

            while (true)
            {
                ship.SetBackgroundColor();

                var (_direction, isEnterKey) = ReadPressedKey();

                if (isEnterKey)
                {
                    if (shipOverlap) continue;
                    ship.Lock(); break;
                }

                if (_direction == null) continue;

                var direction = (Direction)_direction;

                var nextShipCoordinates = ship.CalculateNextMoveCoordinates(direction);

                var overlappingShips = playerGrid.Ships.Where(s => s.Coordinates.Intersect(nextShipCoordinates).Any()).ToList();
                var areThereAnyOverlappingShips = overlappingShips.Any();

                if (areThereAnyOverlappingShips)
                {
                    shipOverlap = true;
                    ship.SetBackgroundColor(ConsoleColor.Red);
                    ship.Move(direction);
                    ship.SetBackgroundColor();
                    HandleOverlapingShipsDrawing(ship, overlappingShips, playerGrid.Ships);
                    continue;
                }

                shipOverlap = false;
                ship.Move(direction);
                playerGrid.Ships.ForEach(s => (s as Ship)?.Draw());
            }
        }

        private (Direction? direction, bool isEnterKey) ReadPressedKey()
        {
            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.UpArrow) return (Direction.Up, false);
            else if (key == ConsoleKey.DownArrow) return (Direction.Down, false);
            else if (key == ConsoleKey.LeftArrow) return (Direction.Left, false);
            else if (key == ConsoleKey.RightArrow) return (Direction.Right, false);

            else if (key == ConsoleKey.Enter) return (null, true);

            return (null, false);

        }

        private void HandleOverlapingShipsDrawing(Ship ship, List<IShip> overlappingShips, List<IShip> allShips)
        {

            overlappingShips.ForEach(s =>           //From the overlaping ships draw only the not overlaping cells
            {
                s.Coordinates.Except(ship.Coordinates).ToList().ForEach(c => c.Draw());
            });
            var notOverlapingShips = allShips.Except(overlappingShips).ToList();
            notOverlapingShips.ForEach(s => (s as Ship)?.Draw());
        }

    }
}
