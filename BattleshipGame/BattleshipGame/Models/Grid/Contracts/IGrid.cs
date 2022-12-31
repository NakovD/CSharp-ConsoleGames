namespace BattleshipGame.Models.Grid.Contracts
{
    using Ship.Contracts;

    public interface IGrid
    {
        IReadOnlyDictionary<string, int> Boundaries { get; }

        IReadOnlyCollection<Cell> FreeCells { get; }

        List<IShip> Ships { get; }

        void Draw();
    }
}
