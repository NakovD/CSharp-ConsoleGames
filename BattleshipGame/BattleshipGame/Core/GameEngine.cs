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
    using BattleshipGame.Models.Grid.Contracts;

    public class GameEngine
    {
        private bool shipOverlap;

        private bool gameOver;

        private int winner = 0;

        private Grid playerGrid;

        private AIGrid AIGrid;

        private List<Cell> AIAttackedCells;

        private PlayerAttackMarker attackMarker;

        private Random random;

        private static readonly IReadOnlyDictionary<string, int> shipTypes
            = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>()
        {
            { "Carrier", 5 },
            { "Battleship", 4 },
            { "Cruiser", 3 },
            { "Submarine", 3 },
            { "Destroyer", 2 },
        });

        public GameEngine()
        {
            AIAttackedCells = new List<Cell>();
            random = new Random();
        }

        public void Run()
        {
            InitialPreparation();

            DrawGrids();

            StartPhaseOne();

            AIGrid.GenerateShips(shipTypes);

            StartAttackingPhase();

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game ended!");
            var winnerString = winner == 0 ? "AI" : "You";
            Console.WriteLine($"Winner: {winnerString}");
            Console.ReadLine();
        }

        private void StartAttackingPhase()
        {
            var attackMarkerInitialX = AIGrid.FreeCells.First().X;
            var attackMarkerInitialY = AIGrid.FreeCells.First().Y;
            attackMarker = new PlayerAttackMarker(attackMarkerInitialX, attackMarkerInitialY);

            while (!gameOver)
            {
                var isPlayerTurnSuccessfull = ExecutePlayerTurn();
                if (!isPlayerTurnSuccessfull) continue;
                ExecuteAITurn();
                ValidateGameState();
            }
        }

        private void ValidateGameState()
        {
            var playerLost = playerGrid.Ships.All(s => s.TimesHit == s.Length);
            if (playerLost)
            {
                gameOver = true; winner = 0; return;
            }
            var AILost = AIGrid.Ships.All(s => s.TimesHit == s.Length);
            if (AILost)
            {
                gameOver = true; winner = 1; return;
            }
        }

        private void ExecuteAITurn()
        {
            var availableCells = playerGrid.FreeCells.Except(AIAttackedCells).ToList();
            var randomCellIndex = random.Next(0, availableCells.Count);
            var neededCell = availableCells[randomCellIndex];
            var shipStruck = playerGrid.Ships.SingleOrDefault(s => s.Coordinates.Any(c => c.Equals(neededCell)));
            var charToWrite = "O";
            var colorOfCell = Console.BackgroundColor;
            if (shipStruck != null)
            {
                charToWrite = "X";
                shipStruck.Hit();
                colorOfCell = ConsoleColor.Red;
            }
            neededCell.Symbol = charToWrite;
            neededCell.Draw(colorOfCell);
            AIAttackedCells.Add(neededCell);
        }

        private bool ExecutePlayerTurn()
        {
            var (direction, isEnterPressed, isSpacePressed) = ReadPressedKey();

            if (isSpacePressed) return false;

            var currentCell = AIGrid.FreeCells.Single(c => c.X == attackMarker.X && c.Y == attackMarker.Y);

            if (isEnterPressed)
            {
                AttackCell(currentCell); return true;
            }

            var (nextX, nextY) = GetNextMarkerCoordinates(direction);

            var neededCell = AIGrid.FreeCells.SingleOrDefault(c => c.X == nextX && c.Y == nextY);
            if (neededCell == null) return false;

            attackMarker.UpdateCoordinates(nextX, nextY);
            neededCell.Draw(ConsoleColor.Magenta);
            currentCell.Draw();

            return false;
        }

        private void AttackCell(Cell currentCell)
        {
            var shipStruck = AIGrid.Ships.SingleOrDefault(s => s.Coordinates.Any(c => c.Equals(currentCell)));
            var charToWrite = "O";
            if (shipStruck != null)
            {
                charToWrite = "X";
                shipStruck.Hit();
            }
            currentCell.Symbol = charToWrite;
            currentCell.Draw(ConsoleColor.Magenta);

            Console.ResetColor();
        }

        private (int nextX, int nextY) GetNextMarkerCoordinates(Direction? direction)
        {
            switch (direction)
            {
                case Direction.Up: return (attackMarker.X, attackMarker.Y - 2);
                case Direction.Right: return (attackMarker.X + 2, attackMarker.Y);
                case Direction.Down: return (attackMarker.X, attackMarker.Y + 2);
                case Direction.Left: return (attackMarker.X - 2, attackMarker.Y);
                default: return (attackMarker.X, attackMarker.Y);
            }
        }

        private void StartPhaseOne()
        {
            var playerShips = GetShips(playerGrid.Boundaries);

            foreach (var ship in playerShips)
            {
                PositionShip(ship, playerGrid);
                playerGrid.Ships.Add(ship);
            }
        }

        private void DrawGrids()
        {
            var gridRows = BaseGrid.GridRows;
            var gridCols = BaseGrid.GridCols;

            var spaceBetweenGrids = 30;
            var centerX = (Console.WindowWidth - (gridCols * 2) - spaceBetweenGrids / 2) / 2;
            var centerY = ((Console.WindowHeight - gridRows) / 2);

            var playerGrid = new Grid(centerX, centerY);
            playerGrid.Draw();

            var opponentGrid = new AIGrid(centerX + spaceBetweenGrids, centerY);
            opponentGrid.Draw();

            this.playerGrid = playerGrid;
            this.AIGrid = opponentGrid;

        }

        private void InitialPreparation()
        {
            ConsoleSetup.Configure();
            Console.InputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            Console.ResetColor();
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
            ship.Draw(ConsoleColor.Red);
            var shipOverlaps = !ValidateShipPosition(ship);
            if (!shipOverlaps) ship.Draw();

            while (true)
            {
                var (_direction, isEnterKey, isSpaceKey) = ReadPressedKey();

                if (isSpaceKey)
                {
                    ship.Rotate();
                    var isShipPositionValid = ValidateShipPosition(ship);
                    if (!isShipPositionValid) { shipOverlap = true; continue; }

                    shipOverlap = false;
                    playerGrid.Ships.ForEach(s => s.Draw());
                    continue;
                }

                if (isEnterKey)
                {
                    if (shipOverlap) continue;
                    ship.Lock(); break;
                }

                if (_direction == null) continue;

                var direction = (Direction)_direction;

                ship.Move(direction);

                var shipOverlapsAnother = !ValidateShipPosition(ship);

                if (shipOverlapsAnother)
                {
                    shipOverlap = true;
                    continue;
                }

                shipOverlap = false;
                playerGrid.Ships.ForEach(s => s.Draw());
            }
        }

        private bool ValidateShipPosition(Ship ship)
        {
            var overlappedShips = playerGrid.Ships.Where(s => s.Coordinates.Intersect(ship.Coordinates).Any()).ToList();

            if (!overlappedShips.Any()) return true;

            HandleShipOverlapping(ship, overlappedShips);

            return false;
        }

        private void HandleShipOverlapping(Ship ship, List<Ship> overlappingShips)
        {
            ship.Draw(ConsoleColor.Red);

            overlappingShips.ForEach(s =>
            {
                var overlappingCells = s.Coordinates.Except(ship.Coordinates).ToList();
                overlappingCells.ForEach(c => c.Draw(ConsoleColor.Gray));
            });

            var notOverlappingShips = playerGrid.Ships.Except(overlappingShips).ToList();
            notOverlappingShips.ForEach(s => s.Draw());
        }

        private (Direction? direction, bool isEnterKey, bool isSpaceKey) ReadPressedKey()
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow) return (Direction.Up, false, false);
            else if (key == ConsoleKey.DownArrow) return (Direction.Down, false, false);
            else if (key == ConsoleKey.LeftArrow) return (Direction.Left, false, false);
            else if (key == ConsoleKey.RightArrow) return (Direction.Right, false, false);

            else if (key == ConsoleKey.Enter) return (null, true, false);

            else if (key == ConsoleKey.Spacebar) return (null, false, true);

            return (null, false, false);

        }
    }
}
