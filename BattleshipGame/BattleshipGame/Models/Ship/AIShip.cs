namespace BattleshipGame.Models.Ship
{
    using Contracts;

    using System.Collections.Generic;

    public class AIShip : IShip
    {
        public string Name { get; private set; }

        public int Length { get; private set; }

        public List<Cell> Coordinates { get; private set; }

        public AIShip(string name, int length)
        {
            Name = name;
            Length = length;
            Coordinates = new List<Cell>();
        }
    }
}
