namespace Battleship
{
    using ConsoleInitialSetup;

    using System;
    using System.Text;

    internal class Program
    {
        private static int gridRows = 21;
        private static int gridCols = 31;

        private static void Main(string[] args)
        {
            ConsoleSetup.Configure();
            Console.InputEncoding = Encoding.Unicode;

            CreateGrid();
        }

        private static void CreateGrid()
        {
            var rows = gridRows;
            var cols = gridCols;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    string symbol = GetSymbol(row, col);
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }

        private static string GetSymbol(int row, int col)
        {
            var rows = gridRows - 1;
            var cols = gridCols - 1;

            if (row == 0 && col == 0) return "\u250C";  //top left corner
            if (row == 0 && col == cols) return "\u2510"; //top right corner
            if (row == rows && col == 0) return "\u2514"; //bottom right corner
            if (row == rows && col == cols) return "\u2518";    //bottom left corner
            if (col == 0 && row % 2 == 0) return "\u251C";      //all left middle cells
            if (col == cols && row % 2 == 0) return "\u2524";   //all right middle cells
            if (row == 0 && col % 3 == 0) return "\u252C";      //all top middle cells
            if (row == rows && col % 3 == 0) return "\u2534";   //all bottom middle cells
            if (col % 3 == 0 && row % 2 != 0) return "\u2502";
            if (row % 2 == 0 && col % 3 != 0) return "\u2500";
            if (col % 3 == 0 && row % 2 == 0) return "\u253C";  //all cells cross cells
            return " ";
        }
    }
}
