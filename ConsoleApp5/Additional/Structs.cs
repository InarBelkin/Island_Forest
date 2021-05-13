using SFML.System;
using System;
namespace Forest_Game
{
    sealed class Vector<Obj> where Obj : class
    {
        private const byte StartVolume = 255;

        private const byte Step = 8;

        private Obj[] Objects;

        public ushort Lenght { get; private set; }//Я переделал

        public Obj this[uint Index] => Objects[Index];

        ///<summary>Добавляет новый объект</summary>
        ///<param name="Item">Новый объект</param>
        public void Add(Obj Item)
        {
            Objects[Lenght] = Item;
            Lenght++;
        }

        public void Enlarge()
        {
            Obj[] Exit = new Obj[Lenght + Step];

            uint I;
            for (I = 0; I < Lenght; I++) Exit[I] = Objects[I];

            Objects = Exit;
        }

        public void Delete(ushort Index)
        {
            if (Index < Lenght)
            {
                Objects[Index] = Objects[Lenght - 1];
                Objects[Lenght - 1] = null;
                Lenght--;
            }
        }

        public bool Delete(Obj Ob)
        {
            for (ushort I = 0; I < Lenght; I++)
            {
                if (Objects[I] == Ob)
                {
                    Delete(I);
                    return true;
                }
            }
            return false;
        }

        public Vector()
        {
            Objects = new Obj[StartVolume];
            Lenght = 0;
        }

        public Vector(ushort StartVol)
        {
            Objects = new Obj[StartVol];
            Lenght = 0;
        }
        public override string ToString() => $"Lenght = {Lenght}";
    }

    public struct Pos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Pos operator +(Pos Rhs, Pos Lhs) => new Pos(Rhs.X + Lhs.X, Rhs.Y + Lhs.Y);
        public static Pos operator -(Pos Rhs, Pos Lhs) => new Pos(Rhs.X - Lhs.X, Rhs.Y - Lhs.Y);
        public static Pos operator /(Pos Rhs, int Lhs) => new Pos(Rhs.X / Lhs, Rhs.Y / Lhs);
        public static Pos operator *(Pos Rhs, int Lhs) => new Pos(Rhs.X * Lhs, Rhs.Y * Lhs);

        public static bool operator ==(Pos Rhs, Pos Lhs)
        {
            return (Rhs.X == Lhs.X && Rhs.Y == Lhs.Y);
        }
        public static bool operator !=(Pos Rhs, Pos Lhs)
        {
            return (!(Rhs.X == Lhs.X && Rhs.Y == Lhs.Y));

        }

        public Pos(int NewX, int NewY)
        {
            X = NewX;
            Y = NewY;
        }

        public Pos(Pos Obj)
        {
            X = Obj.X;
            Y = Obj.Y;
        }
        /// <summary> Расстояние между позициями</summary>
        public static float PosDist(Pos ap, Pos bp)
        {
            int kx = bp.X - ap.X;
            int ky = bp.Y - ap.Y;
            return ((float)Math.Sqrt(kx * kx + ky * ky));
        }
        /// <summary>Находятся ли они рядом</summary>
        public static bool PosNear(Pos p1, Pos p2)
        {
            return (Math.Abs(p1.X - p2.X) <= 1 && Math.Abs(p1.Y - p2.Y) <= 1);
        }
        /// <summary>Сделать одиночной длины</summary>
        public static Pos Normalize(Pos p)
        {
            if (p.X > 1) p.X = 1;
            else if (p.X < -1) p.X = -1;
            if (p.Y > 1) p.Y = 1;
            else if (p.Y < -1) p.Y = -1;
            return p;
        }
        /// <summary>Возвращает нормализованную позицию по направлению</summary>
        public static Pos GetDirPos(Direction d)
        {
            switch (d)
            {
                case Direction.UpRigh: return new Pos(1, 0);
                case Direction.Right: return new Pos(1, 1);
                case Direction.DownRight: return new Pos(0, 1);
                case Direction.Down: return new Pos(-1, 1);
                case Direction.DownLeft: return new Pos(-1, 0);
                case Direction.Left: return new Pos(-1, -1);
                case Direction.Upleft: return new Pos(0, -1);
                case Direction.Up: return new Pos(1, -1);
                default: return new Pos(0, 0);
            }
        }
        public override string ToString() => $"X = {X} и Y = {Y}";
    }

    public struct StrAnim
    {
        public Vector2f GlCoord;
        public ushort with, heigh, top;
        /// <summary>  Кадров в секунду</summary>
        public float AnimSpeed; public byte CountFrame;
        public bool Animinverse;
        public float AnimTime;
        public byte PlusLine;
        public Direction Rotation;
        public StrAnim(int _)
        {
            with = 0; heigh = 0; top = 0;

            AnimSpeed = 10f; CountFrame = 1;
            Animinverse = false;
            AnimTime = 0;
            GlCoord = new Vector2f();
            PlusLine = 0;
            Rotation = Direction.DownRight;
        }
        public void SetRot(Pos CurPos, Pos TargPos)
        {
            Pos P = TargPos - CurPos;
            if (P.X > 0)
            {
                if (P.Y > 0) { Rotation = Direction.Right; return; }
                else if (P.Y == 0) { Rotation = Direction.UpRigh; return; }
                else if (P.Y < 0) { Rotation = Direction.Up; return; }
            }
            else if (P.X == 0)
            {
                if (P.Y > 0) { Rotation = Direction.DownRight; return; }
                // else if (P.Y == 0) { Rot = DirAct.UpRigh; return; }//Когда оба равны 0, оставим как есть
                else if (P.Y < 0) { Rotation = Direction.Upleft; return; }
            }
            else
            {
                if (P.Y > 0) { Rotation = Direction.Down; return; }
                else if (P.Y == 0) { Rotation = Direction.DownLeft; return; }
                else if (P.Y < 0) { Rotation = Direction.Left; return; }
            }
        }
        /// <summary>1 -если стоять, 2 - если ходить</summary>
        public void SetPlLine(byte a)
        {
            if (a == 1)
            {
                switch (Rotation)
                {
                    case Direction.Up:
                    case Direction.UpRigh:
                    case Direction.Upleft:
                        PlusLine = 0;
                        break;
                    case Direction.Left:
                    case Direction.Right:
                    case Direction.DownLeft:
                    case Direction.DownRight:
                        PlusLine = 1;
                        break;
                    case Direction.Down:
                        PlusLine = 2;
                        break;
                }
            }
            else
            {
                switch (Rotation)
                {
                    case Direction.Up:
                        PlusLine = 2;
                        break;
                    case Direction.UpRigh:
                    case Direction.Upleft:
                    case Direction.Left:
                    case Direction.Right:
                        PlusLine = 1;
                        break;
                    default:
                        PlusLine = 2;
                        break;


                }
            }
        }
    }

    public enum Direction : byte
    {
        UpRigh = 0,
        Right = 1,
        DownRight = 2,
        Down = 3,
        DownLeft = 4,
        Left = 5,
        Upleft = 6,
        Up = 7,
    }
}

