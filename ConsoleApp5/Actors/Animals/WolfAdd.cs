using Forest_Game.Additional;
using Forest_Game.Additional.Interfaces;
namespace Forest_Game.Animals
{
    sealed partial class Wolf
    {
        public StrHunger Hunger { get; set; }

        private (float, bool) QuerEat(Actor Act, float l)
        {
            if (Act is IEatable && (Act as IEatable).EType == EatType.MeatSmall && (Act as IEatable).FoodRec > 0
                && Act is IAttackable && (Act as IAttackable).Health.alive == false)
                return (l, true);
            else return (0, false);
        }
        private (float, bool) QuerAttack(Actor Act, float l)
        {
            if (Act is IEatable && (Act as IEatable).EType == EatType.MeatSmall && (Act as IEatable).FoodRec > 0
                && Act is IAttackable && (Act as IAttackable).Health.alive == true)
                return (l, true);
            else return (0, false);
        }
        protected override void SetAnim(AnimState AS)
        {
            switch (AS)
            {
                case AnimState.Move:
                    if (Anim.top >= 1600) Anim.AnimTime = 0;
                    Anim.top = (ushort)(200 * (int)Anim.Rotation);
                    Anim.AnimSpeed = 20f;
                    Anim.CountFrame = 16;
                    break;
                case AnimState.Eat:
                    Anim.top = (ushort)(200 * (int)Anim.Rotation + 1600);
                    Anim.AnimSpeed = 20f;
                    Anim.CountFrame = 1;
                    Anim.AnimTime = 0;
                    break;
                case AnimState.Rest:
                    if (Anim.top < 3200) Anim.AnimTime = 0;
                    Anim.top = (ushort)(200 * (int)Anim.Rotation + 3200);
                    Anim.AnimSpeed = 6f;
                    Anim.CountFrame = 5;
                    break;
                case AnimState.OstAtk:
                    if (Anim.top < 4800) Anim.AnimTime = 0;
                    Anim.top = (ushort)(200 * (int)Anim.Rotation + 4800);
                    Anim.AnimSpeed = 6f;
                    Anim.CountFrame = 8;
                    break;
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
            string St = "Волк";
            St += "\n";
            switch (State)
            {
                case AnimState.Move:
                    St += "Движется";
                    break;
                case AnimState.Eat:
                    St += "Кушает зайца";
                    break;
                case AnimState.Rest:
                    St += "Адихает";
                    break;
                default:
                    St += "ХЗ";
                    break;
            }
            St += "\n";
            switch (AvState)
            {
                case WolfState.GoEat: St += "Идёт есть"; break;
                case WolfState.GoAttack: St += "Преследует добычу"; break;
                case WolfState.GoGulyat: St += "Гуляет по окрестностям"; break;
                case WolfState.GoAway: St += "Убегает от опастности"; break;
                case WolfState.Disarray: St += "Дизориентирован"; break;
                default: St += "Непонятно что делает"; break;

            }
            St += $"\nГолод {Hunger.Hung}/{Hunger.MaxHung}";
            //  St += $"\nОЗ {Health.Health}/{Health.MaxHealth}";

            return St;
        }
        enum WolfState : byte
        {
            Zero = 0,
            GoEat = 1,
            GoGulyat = 2,
            GoAway = 3,
            GoAttack = 100,
            Disarray = 255,
        }
    }
}
