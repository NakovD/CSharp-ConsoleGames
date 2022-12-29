namespace BattleshipGame.Models.Grid.Contracts
{
    public interface IGrid
    {
        Dictionary<string, int> Boundaries { get; }


        List<(int x, int y)> Draw();
    }
}
