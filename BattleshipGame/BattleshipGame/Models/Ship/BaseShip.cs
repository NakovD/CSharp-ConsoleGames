namespace BattleshipGame.Models.Ship
{
    using Contracts;
    using System.Collections.Generic;

    public abstract class BaseShip : IShip
    {
        public string Name { get; protected set; }

        public int Length { get; protected set; }

        public int TimesHit { get; protected set; }

        public List<Cell> Coordinates { get; protected set; }

        public BaseShip(string name, int length)
        {
            Name = name;
            Length = length;
            TimesHit = 0;
            Coordinates = new List<Cell>();
        }

        public void Hit()
        {
            TimesHit++;
        }
    }
}
