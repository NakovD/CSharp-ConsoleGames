﻿namespace BattleshipGame.Models.Ship.Contracts
{
    using Enums;

    public interface IShip
    {
        string Name { get; }

        List<Cell> Coordinates { get; }

        bool IsPositioned { get; }

        void Move(Direction direction);
    }
}