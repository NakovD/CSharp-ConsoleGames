﻿namespace BattleshipGame.Models.Ship
{
    using Enums;
    using Contracts;

    using System.Collections.Generic;
    using System;
    using BattleshipGame.Contracts;

    public class Ship : BaseShip, IPlayerShip
    {
        private IReadOnlyDictionary<string, int> boundaries;

        private const int step = 2;

        public bool IsPositioned { get; private set; }

        public Axis Axis { get; private set; }

        public Ship(string name, int length, IReadOnlyDictionary<string, int> boundaries) : base(name, length)
        {
            this.boundaries = boundaries;

            CreateShip();
            SetBackgroundColor();

            Axis = Axis.Y;
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

        public void Draw(ConsoleColor backgroundColor = ConsoleColor.Gray)
        {
            Console.BackgroundColor = backgroundColor;
            Coordinates.ForEach(cell => cell.Draw(backgroundColor));
            Console.ResetColor();
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

        public void Rotate()
        {
            Clear();
            Axis = Axis == Axis.X ? Axis.Y : Axis.X;
            var isHorizontal = Axis == Axis.X;

            var firstCell = Coordinates.First();
            var index = 0;
            var isOutSide = false;
            if (isHorizontal)
            {
                index = firstCell.X;
                isOutSide = index + Coordinates.Count * 2 >= boundaries["right"];
            }
            else
            {
                index = firstCell.Y;
                isOutSide = index + Coordinates.Count * 2 >= boundaries["bottom"];
            }
            Coordinates.Skip(1).ToList().ForEach(cell =>
            {
                index = isOutSide ? index - step : index + step;
                cell.Y = isHorizontal ? firstCell.Y : index;
                cell.X = isHorizontal ? index : firstCell.X;
            });
            Draw();
        }
    }
}
