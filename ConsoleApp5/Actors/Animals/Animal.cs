using Forest_Game.Actors;
using Forest_Game.Additional;
using Forest_Game.Additional.Interfaces;
using Forest_Game.WorldMap;
using SFML.System;
using System;

namespace Forest_Game
{
    class Animal : Actor
    {
        protected float Mass;
        public float Speed;//врем€, за которое переходит на другую клетку
        protected float Age;
        public (ushort, ushort) RadSee { get; protected set; } = (0, 0);

        protected float TimeAct = 0, TimeActFull = 0;
        public AnimState State = AnimState.Zero;

        protected ActionArg ClAct;
        protected Act ActEnd = null, ActMiddle = null;
        protected delegate bool Act(float Dtime);

        public event EventHandler<GoEventArgs> IGoAct;
        public event EventHandler<LookEventArgs> ILookAct;
        public event EventHandler<SearchWayEventArgs> ISearchWay;
        public event StaticClass.LookCell IlookGlCoord;

        public Animal(Pos Envpos, Vector2f GlCoordinate) : base(Envpos, GlCoordinate)
        {
        }
        ///<param name="canRest">ќтдыхать ли после неудачи?</param>
        protected bool ActGo(GoEventArgs GA, bool canRest = true)
        {   //перемещатьс€ можно только на 1 клетку
            GA.TargPos = GA.CurPos + Pos.Normalize(GA.TargPos - GA.CurPos);
            //ѕоправка на диагональ
            float mnoz = (GA.TargPos.X != GA.CurPos.X && GA.TargPos.Y != GA.CurPos.Y) ? 1.41421356237f : 1f;

            IGoAct(this, GA);
            if (GA.CanGo)   //не сможет идти на свою клетку, т.к. там уже есть animal(он сам)
            {
                ClAct = new ClActMove() { CCoord = IlookGlCoord(Location), TCoord = IlookGlCoord(GA.TargPos) };
                Location = GA.TargPos;
                State = AnimState.Move;
                Anim.SetRot(GA.CurPos, GA.TargPos);
                SetAnim(AnimState.Move);
                ActMiddle = ActGoM;
                ActEnd = ActGoE;
                Anim.SetPlLine(2);
                TimeAct = Speed * mnoz;
                TimeActFull = TimeAct;
                return true;
            }
            else
            {
                if (canRest)
                {
                    ActRest(Speed);
                }
                return false;
                //Console.WriteLine("не могу пройти");
            }
        }
        private bool ActGoM(float DTime)
        {
            if (ClAct is ClActMove c)
            {
                Anim.GlCoord = c.CCoord * (TimeAct / TimeActFull) + c.TCoord * (1 - TimeAct / TimeActFull);

            }
            return true;
        }
        private bool ActGoE(float _)
        {
            if (ClAct is ClActMove c)
            {
                Anim.GlCoord = c.TCoord;
            }
            return true;
        }

        protected void ActRest(float TimeRest)
        {
            Anim.SetPlLine(1);
            State = AnimState.Rest;
            SetAnim(AnimState.Rest);
            TimeAct = TimeRest;
            ActMiddle = null;
            ActEnd = null;
        }

        protected void ActEat(Pos EPos)
        {
            ClAct = new ClActEat() { EatPos = EPos };
            Anim.SetRot(Location, EPos);
            Anim.SetPlLine(1);
            SetAnim(AnimState.Eat);
            TimeAct = 2;
            State = AnimState.Eat;
            ActEnd = ActEatE;
        }
        private bool ActEatE(float _)
        {
            if (ClAct is ClActEat c)
            {
                Cell tc = QActGetCell(c.EatPos);
                if (tc.LAnimal != null && tc.LAnimal != this
                    && tc.LAnimal is IEatable)
                {
                    (tc.LAnimal as IEatable).TakeEat(this as ICanEat);
                }
                else if (tc.LEnvir != null && tc.LEnvir is IEatable)
                {
                    (tc.LEnvir as IEatable).TakeEat(this as ICanEat);
                }
            }
            return false;
        }

        protected void ProcessAct(float DTime)
        {
            TimeAct -= DTime;
            ActMiddle?.Invoke(DTime);
            if (TimeAct <= 0)
            {
                ActEnd?.Invoke(DTime);
                ClAct = null;
                ActEnd = null;
                ActMiddle = null;
                State = AnimState.Zero;
            }
        }

        protected void QActLook(LookEventArgs EA)
        {
            ILookAct(this, EA);
        }
        protected void QActSearchWay(SearchWayEventArgs Way)
        {
            ISearchWay(this, Way);
        }

        public override void EvTick(float Dtime)
        {
        }


    }
}