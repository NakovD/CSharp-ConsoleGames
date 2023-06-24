namespace BattleshipGame.Models.Grid.Contracts
{
    using Ship.Contracts;

    public interface IHasShips<T> where T : IShip
    {
        List<T> Ships { get; }
    }
}
