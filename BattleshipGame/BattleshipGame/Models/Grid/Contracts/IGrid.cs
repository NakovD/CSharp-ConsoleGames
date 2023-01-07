namespace BattleshipGame.Models.Grid.Contracts
{
    using BattleshipGame.Contracts;

    public interface IGrid : IDrawable
    {
        IReadOnlyDictionary<string, int> Boundaries { get; }
    }
}
