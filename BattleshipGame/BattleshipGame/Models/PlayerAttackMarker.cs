namespace BattleshipGame.Models
{
    public class PlayerAttackMarker
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public PlayerAttackMarker(int x, int y)
        {
            X = x;
            Y = y;
            InitialWrite();
        }

        public void UpdateCoordinates(int x, int y)
        {
            X = x; Y = y;
        }

        private void InitialWrite()
        {
            Console.SetCursorPosition(X, Y);
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write(" ");
            Console.ResetColor();
        }
    }
}
