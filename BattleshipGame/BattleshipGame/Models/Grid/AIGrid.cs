namespace BattleshipGame.Models.Grid
{
    using BattleshipGame.Models.Ship.Contracts;
    using Contracts;
    using Ship;

    using System.Collections.Generic;

    public class AIGrid : BaseGrid, IHasShips<AIShip>
    {
        private Random random;

        public List<AIShip> Ships { get; private set; }

        public AIGrid(int left, int top) : base(left, top)
        {
            Ships = new List<AIShip>();
            random = new Random();
        }

        public void GenerateShips(IReadOnlyDictionary<string, int> shipsModels)
        {
            foreach (var ship in shipsModels)
            {
                var AIship = new AIShip(ship.Key, ship.Value);
                var (firstShipCellX, firstShipCellY, axis) = GetValidShipStartingPoint(ship.Value);

                var endOfLoop = ship.Value * 2;

                for (int i = 0; i < endOfLoop; i+= 2)
                {
                    var increment = i % 2 == 0 ? i : i + 1;
                    var x = axis == 1 ? firstShipCellX + increment : firstShipCellX;
                    var y = axis == 2 ? firstShipCellY + increment : firstShipCellY;
                    var cell = new Cell(x, y);
                    AIship.Coordinates.Add(cell);
                    Console.BackgroundColor = ConsoleColor.Red;
                }

                Ships.Add(AIship);
            }
        }

        private (int x, int y, int axis) GetValidShipStartingPoint(int shipLength)
        {
            while (true)
            {
                var firstShipCellX = random.Next(Boundaries["left"], Boundaries["right"]);
                var firstShipCellY = random.Next(Boundaries["top"], Boundaries["bottom"]);
                firstShipCellX = firstShipCellX % 2 != 0 ? firstShipCellX + 1 : firstShipCellX;
                firstShipCellY = firstShipCellY % 2 != 0 ? firstShipCellY + 1 : firstShipCellY;
                var axis = random.Next(1, 3);
                var isShipGonnaBeValid = ValidateShipCoordinates(firstShipCellX, firstShipCellY, axis, shipLength);

                if (!isShipGonnaBeValid) continue;

                return (firstShipCellX, firstShipCellY, axis);
            }
        }

        private bool ValidateShipCoordinates(int firstShipCellX, int firstShipCellY, int axis, int shipLength)
        {
            var isGonnaBeValid = true;
            var horizontal = axis == 1;
            var lastShipCell = horizontal ? firstShipCellX + shipLength * 2 : firstShipCellY + shipLength * 2;

            var willShipEndOutsideGrid = horizontal ?
                lastShipCell >= Boundaries["right"]
                : lastShipCell >= Boundaries["bottom"];

            var isShipOutSideGrid = horizontal ? firstShipCellY >= Boundaries["bottom"] : firstShipCellX >= Boundaries["right"];

            if (willShipEndOutsideGrid || isShipOutSideGrid) return false;

            var end = shipLength * 2;

            for (int i = 0; i < end; i += 2)
            {
                var x = horizontal ? firstShipCellX + i : firstShipCellX;
                var y = !horizontal ? firstShipCellY + i : firstShipCellY;
                if (Ships.Any(s => s.Coordinates.Any(c => c.X == x && c.Y == y))) return false;
            }

            return isGonnaBeValid;
        }
    }
}
