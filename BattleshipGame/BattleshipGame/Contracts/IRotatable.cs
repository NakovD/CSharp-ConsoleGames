namespace BattleshipGame.Contracts
{
    using Enums;

    public interface IRotatable
    {
        Axis Axis { get; }

        void Rotate();
    }
}
