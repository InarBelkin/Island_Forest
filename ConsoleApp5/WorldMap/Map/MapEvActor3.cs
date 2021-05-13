using SFML.System;

namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {
        public bool CheckPlace(Actor a1)
        {
            return (a1 == MCell[a1.Location.X, a1.Location.Y].LAnimal
                || a1 == MCell[a1.Location.X, a1.Location.Y].LEnvir);
        }

        public Vector2f GetGlobalCoord(Pos Cpos)
        {
            return (MCell[Cpos.X, Cpos.Y].GlCoord);
        }

        public Cell GetCellP(Pos Cpos)
        {
            return MCell[Cpos.X, Cpos.Y];
        }

        //public Cell GetCell(Pos Cpos)
        //{

        //}


    }
}
