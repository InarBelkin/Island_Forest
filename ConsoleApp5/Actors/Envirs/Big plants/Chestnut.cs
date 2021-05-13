using Forest_Game.Additional;
using SFML.System;
using System;

namespace Forest_Game.Envirs
{
    class Chestnut : Envir
    {
        protected sbyte revAnim = 1;
        public Chestnut(Pos Envpos, Vector2f GlCoordinate) : base(Envpos, GlCoordinate)
        {
            ID = ActorID.Chestnut;
            ASprite = WorldMap.SpriteCollection.Snut;

            highdr = 190;
            Anim.with = 370; Anim.heigh = 370; Anim.top = 1110; Anim.AnimTime = 0;

        }


        public override void EvTick(float Dtime)
        {
            Anim.AnimTime += Configuration.Dtime * 10 * revAnim;
            if (Anim.AnimTime >= 13)
            {

                Anim.AnimTime = 12 - (Anim.AnimTime - 13);
                revAnim = -1;
            }
            else if (Anim.AnimTime < 0)
            {
                Anim.AnimTime = 1 + Math.Abs(Anim.AnimTime);
                revAnim = 1;
            }


            //ASprite.TextureRect = new SFML.Graphics.IntRect((int)AnimTime * 370, 1110, 370, 370);

            // Console.WriteLine("Доска");
        }
        static public bool canCreate(CellID ID)
        {
            switch (ID)
            {
                case CellID.Grass:

                case CellID.GrassStone:

                case CellID.GrassSand:
                    return true;
                default:
                    return false;
            }

        }

        public override bool GetCanPlace(CellID IDPlace, Actor Act)
        {
            switch (IDPlace)
            {

                case CellID.Stone:
                case CellID.StoneSand:
                case CellID.Zero:
                    return false;

            }
            if (Act == null) return true;
            else
            {
                switch (Act.ID)
                {
                    case ActorID.Rabbit:
                    case ActorID.Wolf:
                        return false;
                }
            }

            return true;
        }
    }
}
