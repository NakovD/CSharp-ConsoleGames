namespace BattleshipGame.Models.Ship
{
    using Enums;
    using Contracts;

    using System.Collections.Generic;
    using System;

    public class Ship : IShip
    {
        private IReadOnlyDictionary<string, int> boundaries;

        private const int step = 2;

        public string Name { get; private set; }

        public List<Cell> Coordinates { get; private set; }

        public bool IsPositioned { get; private set; }

        public Ship(string name, Dictionary<string, int> boundaries)
        {
            Name = name;
            this.boundaries = boundaries;
        }

        public void Move(Direction direction)
        {
            if (IsPositioned) return;

            var isMoveOutsideTheGrid = ValidateMove(direction);

            if (isMoveOutsideTheGrid) return;

            Clear();

            if (direction == Direction.Right) Coordinates.ForEach(cell => cell.X += step);
            else if (direction == Direction.Left) Coordinates.ForEach(cell => cell.X -= step);
            else if (direction == Direction.Down) Coordinates.ForEach(cell => cell.Y -= step);
            else Coordinates.ForEach(cell => cell.Y += step);

            Draw();
            Console.ResetColor();
        }

        private bool ValidateMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Coordinates.Any(c => c.Y - step <= boundaries["top"]);
                case Direction.Right: return Coordinates.Any(c => c.X + step >= boundaries["right"]);
                case Direction.Down: return Coordinates.Any(c => c.Y + step >= boundaries["bottom"]);
                default: return Coordinates.Any(c => c.X - step <= boundaries["left"]);
            }
        }

        public void Draw()
        {
            Coordinates.ForEach(cell =>
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                cell.Draw();
            });
        }

        public void SetCoordinates(List<Cell> coordinates)
        {
            if (Coordinates != null) return;
            Coordinates = coordinates;
        }

        public void Clear()
        {
            Coordinates.ForEach(cell => cell.Clear());
        }
    }
}
