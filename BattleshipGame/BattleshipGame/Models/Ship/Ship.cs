namespace BattleshipGame.Models.Ship
{
    using Enums;
    using Contracts;

    using System.Collections.Generic;
    using System;

    public class Ship : BaseShip, IPlayerShip
    {
        private IReadOnlyDictionary<string, int> boundaries;

        private const int step = 2;

        public bool IsPositioned { get; private set; }

        public Ship(string name, int length, IReadOnlyDictionary<string, int> boundaries) : base(name, length)
        {
            this.boundaries = boundaries;

            CreateShip();
            SetBackgroundColor();
        }

        private void CreateShip()
        {
            var startIndex = boundaries["top"] + 1;
            var endIndex = boundaries["top"] + 1 + Length * 2;
            var left = boundaries["left"] + 1;

            for (int i = startIndex; i < endIndex; i += step)
            {
                var cell = new Cell(left, i);
                Coordinates.Add(cell);
            }
        }

        public List<Cell> CalculateNextMoveCoordinates(Direction direction)
        {
            if (IsPositioned) return null;

            var nextMoveCoordinates = new List<Cell>();

            Coordinates.ForEach(c =>
            {
                var newCell = new Cell(c.X, c.Y);
                nextMoveCoordinates.Add(newCell);
            });

            var isMoveOutsideTheGrid = ValidateMove(direction);

            if (isMoveOutsideTheGrid) return nextMoveCoordinates;

            if (direction == Direction.Right) nextMoveCoordinates.ForEach(cell => cell.X += step);
            else if (direction == Direction.Left) nextMoveCoordinates.ForEach(cell => cell.X -= step);
            else if (direction == Direction.Up) nextMoveCoordinates.ForEach(cell => cell.Y -= step);
            else nextMoveCoordinates.ForEach(cell => cell.Y += step);

            return nextMoveCoordinates;
        }

        public void Move(Direction direction)
        {
            if (IsPositioned) return;

            var isMoveOutsideTheGrid = ValidateMove(direction);

            if (isMoveOutsideTheGrid) return;

            Clear();

            if (direction == Direction.Right) Coordinates.ForEach(cell => cell.X += step);
            else if (direction == Direction.Left) Coordinates.ForEach(cell => cell.X -= step);
            else if (direction == Direction.Up) Coordinates.ForEach(cell => cell.Y -= step);
            else Coordinates.ForEach(cell => cell.Y += step);

            Draw();
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
            Coordinates.ForEach(cell => cell.Draw());
        }

        public void Clear()
        {
            var previousColor = Console.BackgroundColor;
            Console.ResetColor();
            Coordinates.ForEach(cell => cell.Clear());
            Console.BackgroundColor = previousColor;
        }

        public void Lock()
        {
            IsPositioned = true;
        }

        public void SetBackgroundColor(ConsoleColor bgColor = ConsoleColor.Gray)
        {
            Console.BackgroundColor = bgColor;
        }
    }
}
