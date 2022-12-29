namespace Battleship
{
    using BattleshipGame.Core;

    public class Program
    {
        private static void Main(string[] args)
        {
            var gameEngine = new GameEngine();
            gameEngine.Run();
        }
    }
}
