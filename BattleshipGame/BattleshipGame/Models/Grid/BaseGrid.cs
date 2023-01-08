namespace BattleshipGame.Models.Grid
{
    using Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseGrid: IGrid
    {
        public static readonly int GridRows = 21;

        public static readonly int GridCols = 21;

        public IReadOnlyDictionary<string, int> Boundaries { get; private set; }

        private List<Cell> freeCells;

        public IReadOnlyCollection<Cell> FreeCells => freeCells.AsReadOnly();

        public BaseGrid(int left, int top)
        {
            Boundaries = new Dictionary<string, int>() {
                { "top", top },
                { "left", left },
                { "right", left + GridCols },
                { "bottom", top + GridRows }
            };
            freeCells = new List<Cell>();
        }

        public void Draw()
        {
            var rows = GridRows;
            var cols = GridCols;
            var colTracker = Boundaries["left"];
            var rowTracker = Boundaries["top"];

            Console.SetCursorPosition(Boundaries["left"], Boundaries["top"]);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    string symbol = GetSymbol(row, col);
                    if (symbol == " ")
                    {
                        freeCells.Add(new Cell(Boundaries["left"] + col, Boundaries["top"] + row));
                    }
                    Console.Write(symbol);
                    colTracker += 1;
                }
                rowTracker += 1;
                colTracker = Boundaries["left"];
                Console.SetCursorPosition(Boundaries["left"], rowTracker);
            }
        }

        private static string GetSymbol(int row, int col)
        {
            var rows = GridRows - 1;
            var cols = GridCols - 1;

            if (row == 0 && col == 0) return "\u250C";  //top left corner
            if (row == 0 && col == cols) return "\u2510"; //top right corner
            if (row == rows && col == 0) return "\u2514"; //bottom right corner
            if (row == rows && col == cols) return "\u2518";    //bottom left corner
            if (col == 0 && row % 2 == 0) return "\u251C";      //all left middle cells
            if (col == cols && row % 2 == 0) return "\u2524";   //all right middle cells
            if (row == 0 && col % 2 == 0) return "\u252C";      //all top middle cells
            if (row == rows && col % 2 == 0) return "\u2534";   //all bottom middle cells
            if (col % 2 == 0 && row % 2 != 0) return "\u2502";
            if (row % 2 == 0 && col % 2 != 0) return "\u2500";
            if (col % 2 == 0 && row % 2 == 0) return "\u253C";  //all cells cross cells
            return " ";
        }
    }
}
