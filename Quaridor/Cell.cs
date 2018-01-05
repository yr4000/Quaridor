using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quaridor
{
    interface Cell
    {
    }

    enum DFSColor {White, Grey, Black}

    class PlayerCell : Cell
    {
        public DFSColor color;
        public int distFromSource;

        public PlayerCell()
        {
            this.color = DFSColor.White;
            this.distFromSource = int.MaxValue;
        }
    }

    class WallCell: Cell
    {
        public bool Occupied;
        public Wall wall;

        public WallCell()
        {
            this.Occupied = false;
            this.wall = null;
        }

        public bool isOccupied()
        {
            return this.Occupied;
        }

        public WallCell copy()
        {
            WallCell wc = new WallCell();
            wc.Occupied = this.Occupied;
            wc.wall = this.wall;
            return wc;
        }
    }
}
