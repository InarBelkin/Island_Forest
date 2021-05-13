using Forest_Game.Additional;
using SFML.System;

namespace Forest_Game.Animals
{
    class Deer : Animal
    {
        public Deer(Pos Envpos, Vector2f GlCoordinate) : base(Envpos, GlCoordinate)
        {
            ID = ActorID.Deer;
            ASprite = WorldMap.SpriteCollection.SDeer;
            highdr = 5;
            Anim.with = 200; Anim.top = 0; Anim.heigh = 200; Anim.AnimTime = 0;
            Speed = 1;
            RadSee = (10, 10);
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
            if (Act == null)
            {
                return true;
            }
            else
            {
                switch (Act.ID)//не нужно проверять животных, на них и так проверят
                {
                    case ActorID.Chestnut:
                        return false;
                }
            }
            return true;
        }
    }
}
