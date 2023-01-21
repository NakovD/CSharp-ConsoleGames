namespace BattleshipGame.Models
{
    using BattleshipGame.Contracts;

    public class Cell : IEquatable<Cell>, IDrawableWithColor, IClearable
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Symbol { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Symbol = " ";
        }

        public void Clear()
        {
            var usedColor = Console.BackgroundColor;
            Console.SetCursorPosition(X, Y);
            Console.ResetColor();
            Console.Write(Symbol);
            Console.BackgroundColor = usedColor;
        }

        private void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }

        public void Draw(ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Draw();
            Console.ResetColor();
        }

        public bool Equals(Cell other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var other = obj as Cell;
            if (other == null) return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }
    }
}
