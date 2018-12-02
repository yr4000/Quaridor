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
        public Wall wall;

        public WallCell()
        {
            this.wall = null;
        }
        /*
        public bool isOccupied()
        {
            return this.Occupied;
        }
        */
        public WallCell copy()
        {
            WallCell wc = new WallCell();
            wc.wall = this.wall;
            return wc;
        }
    }
}
