namespace BattleshipGame.Models.Ship.Contracts
{
    public interface IShip
    {
        string Name { get; }

        int Length { get; }

        int TimesHit { get; }

        List<Cell> Coordinates { get; }
    }
}
