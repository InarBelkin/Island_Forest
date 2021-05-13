using Forest_Game.Additional;
using Forest_Game.Additional.Interfaces;
using SFML.System;

namespace Forest_Game.Envirs
{
    class Carrot : Envir, IEatable
    {
        public ushort FoodRec { get; private set; }
        public EatType EType { get; private set; }

        public Carrot(Pos Envpos, Vector2f GlCoordinate) : base(Envpos, GlCoordinate)
        {
            FoodRec = 2;
            EType = EatType.SmallGround;
            ID = ActorID.Carrot;
            ASprite = WorldMap.SpriteCollection.SCarrot;

            highdr = 0;
            Anim.with = 64; Anim.top = 41; Anim.heigh = 41; Anim.AnimTime = 0;
        }
        public bool TakeEat(ICanEat ace)
        {
            if (FoodRec > 0)
            {
                ace.Hunger.Hung += FoodRec;
                FoodRec = 0;
            }
            return true;
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
            return true;
        }
        public override string GetState()
        {
            string St = "Морковка";
            St += $"\nТип:Корнеплод {FoodRec}";
            return St;
        }
    }
}
