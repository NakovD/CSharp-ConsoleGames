namespace BattleshipGame.Models
{
    public class Cell
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Clear()
        {
            var usedColor = Console.BackgroundColor;
            Console.SetCursorPosition(X, Y);
            Console.ResetColor();
            Console.Write(" ");
            Console.BackgroundColor = usedColor;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(" ");
        }
    }
}
