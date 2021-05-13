using Forest_Game.Additional;
using Forest_Game.WorldMap;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
namespace Forest_Game
{
    class Actor : IDisposable
    {
        public ActorID ID { get; protected set; }
        public Pos Location;

        public Sprite ASprite;
        public StrAnim Anim = new StrAnim(1);
        protected List<Pos> Targ = new List<Pos>();

        public ushort highdr = 0;
        public event EventHandler<DeathEventArgs> IDeath;
        public event StaticClass.IGetCell IGetCell;
        public Actor(Pos Envpos, Vector2f GlCoordinate)
        {
            Anim.GlCoord = GlCoordinate;
            ID = ActorID.Zero;
            Location = Envpos;
            Game.EvTick += EvTick;
        }
        public void QActDelMe()
        {
            IDeath(this, new DeathEventArgs());
            System.Console.WriteLine("”дал€ю");
        }
        public Cell QActGetCell(Pos Cpos)
        {
            return IGetCell(Cpos);
        }
        public virtual void EvTick(float Dtime) { }

        public virtual bool GetCanPlace(CellID IDPlace, Actor Act)
        {
            return false;
        }

        protected virtual void SetAnim(AnimState AS) { }

        protected virtual void TickAnim(float DTime)
        {
            Anim.AnimTime += DTime * Anim.AnimSpeed;
            if (Anim.AnimTime >= Anim.CountFrame) Anim.AnimTime -= Anim.CountFrame;
            //Anim.AnimTime += DTime * 10;
            //       if (Anim.AnimTime > 6) Anim.AnimTime -= 6;
        }

        public virtual string GetState() { return "јктЄр.\nЁто всЄ, что € могу сказать"; }

        public virtual void Dispose()
        {
            Game.EvTick -= EvTick;
        }

    }
}