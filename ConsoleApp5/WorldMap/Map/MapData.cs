namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {
        public Chunk[,] MChunk { get; private set; }
        public Cell[,] MCell { get; private set; }

        private readonly int MapX;  ///размер карты
        private readonly int MapY;
        private readonly short ChMapX;
        private readonly short ChMapY;
        private const int waterhigh = 80;
        private const int StHiDif = 30;

    }
}