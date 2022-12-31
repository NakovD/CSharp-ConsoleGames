namespace BattleshipGame.Models
{
    public class Cell : IEquatable<Cell>
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

        public bool Equals(Cell? other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
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
