namespace BattleshipGame.Models.Ship.Contracts
{
    using BattleshipGame.Contracts;

    public interface IPlayerShip : IShip, IDrawableWithColor, IClearable, IMovable, IRotatable
    {
        bool IsPositioned { get; }

        void Lock();
    }
}
