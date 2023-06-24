namespace BattleshipGame.Models.Grid
{
    using Contracts;
    using Ship;

    using System.Collections.Generic;

    public class Grid : BaseGrid, IHasShips<Ship>
    {
        public List<Ship> Ships { get; private set; }

        public Grid(int left, int top) : base(left, top)
        {
            Ships = new List<Ship>();
        }
    }
}
