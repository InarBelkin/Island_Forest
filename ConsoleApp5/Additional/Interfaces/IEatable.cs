namespace Forest_Game.Additional.Interfaces
{
    interface IEatable
    {
        ushort FoodRec { get; }
        EatType EType { get; }
        bool TakeEat(ICanEat anm);
    }
    interface ICanEat
    {
        StrHunger Hunger { get; }
    }
    class StrHunger
    {
        public float MaxHung;
        protected float hung;
        public virtual float Hung
        {
            get { return hung; }
            set { hung = value; if (hung > MaxHung) hung = MaxHung; }
        }
        public float addSecHung;
        public StrHunger(float maxhung, float addH = -0.1f)
        {
            MaxHung = maxhung;
            hung = maxhung;
            addSecHung = addH;
        }
    }

    enum EatType : byte
    {
        NotEat = 0,
        SmallGround = 1,
        Medicinal = 10,

        MeatSmall = 101,
    }
}
