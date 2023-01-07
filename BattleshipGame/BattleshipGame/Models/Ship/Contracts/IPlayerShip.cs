namespace BattleshipGame.Models.Ship.Contracts
{
    using BattleshipGame.Contracts;

    public interface IPlayerShip : IShip, IDrawable, IClearable, IMovable
    {
        bool IsPositioned { get; }

        void Lock();
    }
}
