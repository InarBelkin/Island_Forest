using Forest_Game.Additional;
using Forest_Game.Additional.Interfaces;

namespace Forest_Game.Animals
{
    sealed partial class Rabbit
    {
        public StrHunger Hunger { get; private set; }
        public ushort FoodRec { get; set; }
        public EatType EType => EatType.MeatSmall;
        public bool TakeEat(ICanEat anm)
        {
            if (!Health.alive)
            {
                if (FoodRec > 2)
                {
                    anm.Hunger.Hung += 2;
                    FoodRec -= 2;
                }
                else
                {
                    anm.Hunger.Hung += FoodRec;
                    FoodRec = 0;
                }
                return true;
            }
            else return false;

        }
        public StrHealth Health { get; set; }
        public bool TakeAttack(float Attack, ICanAttack damager)
        {
            Health.Health -= damager.Damage;
            if (Health.alive && Health.Health <= 0)
            {
                ActDeath();
            }
            return true;
        }
        private void ActDeath()
        {
            Health.alive = false;
            State = AnimState.Dead;
            SetAnim(AnimState.Dead);
            TimeAct = 20;
            ClAct = null;
            ActEnd = delegate (float Dtime) { QActDelMe(); return true; };
            ActMiddle = null;

        }

        private (float, bool) QuerEat(Actor Act, float l)
        {
            if (Act is IEatable && (Act as IEatable).EType == EatType.SmallGround && (Act as IEatable).FoodRec > 0)
                return (l, true);
            else return (0, false);
        }

        protected override void SetAnim(AnimState AS)
        {
            switch (AS)
            {
                case AnimState.Move:
                    Anim.top = (ushort)(200 * (int)Anim.Rotation);
                    Anim.AnimSpeed = 12f;
                    Anim.CountFrame = 6;
                    if (Anim.top >= 1600) Anim.AnimTime = 0;
                    break;
                case AnimState.Eat:
                    Anim.top = (ushort)(200 * (int)Anim.Rotation + 1600);
                    Anim.AnimSpeed = 10f;
                    Anim.CountFrame = 8;
                    Anim.AnimTime = 0;
                    break;
                case AnimState.Rest:
                    if (!(Anim.top >= 3200)) Anim.AnimTime = 0;
                    Anim.top = (ushort)(200 * (int)Anim.Rotation + 3200);
                    Anim.AnimSpeed = 4f;
                    Anim.CountFrame = 4;
                    Anim.AnimTime = 0;
                    break;
                case AnimState.Dead:
                    Anim.AnimTime = 0;
                    Anim.top = 4800;
                    Anim.CountFrame = 1;
                    break;
            }
            //Нужно ли прибавлять DTime?
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
        public override string GetState()
        {
            string St = "Кролик";
            St += "\n";
            switch (State)
            {
                case AnimState.Move:
                    St += "Движется";
                    break;
                case AnimState.Eat:
                    St += "Кушает морковку";
                    break;
                case AnimState.Rest:
                    St += "Адихает";
                    break;
                case AnimState.Dead:
                    St += "Мёртв";
                    break;
                default:
                    St += "ХЗ";
                    break;
            }
            St += "\n";
            switch (AvState)
            {
                case Rstate.GoEat: St += "Идёт есть"; break;
                case Rstate.GoGulyat: St += "Гуляет по окрестностям"; break;
                case Rstate.GoAway: St += "Убегает от опастности"; break;
                case Rstate.Disarray: St += "Дизориентирован"; break;
                default: St += "Непонятно что делает"; break;

            }
            St += $"\nОЗ {Health.Health}/{Health.MaxHealth}";
            St += $"\nГолод {Hunger.Hung}/{Hunger.MaxHung}";


            return St;
            //return base.GetState();
        }
        enum Rstate : byte
        {
            Zero = 0,
            GoEat = 1,
            GoGulyat = 2,
            GoAway = 3,
            Disarray = 4,
        }
    }
}


