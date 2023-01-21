namespace BattleshipGame.Contracts
{
    using System;

    public interface IDrawableWithColor
    {
        void Draw(ConsoleColor backgroundColor = ConsoleColor.Gray);
    }
}
