using Forest_Game.WorldMap;
using SFML.System;
using System;

namespace Forest_Game.Additional
{
    static class StaticClass
    {
        public delegate void Self(float Dtime);
        //public delegate void ISee(Pos PosActor, float visActor);
        //public delegate void IGo(Pos CurPos, Pos TargPos);
        //public delegate void Spisok(List<int> Mas);
        public delegate Vector<Actor> ILook(Animal anim);

        public delegate bool ICheck(Actor act);
        public delegate bool IGetCellMouse(out Pos OPos, out Cell OCell);
        public delegate Vector2f LookCell(Pos Cpos);
        public delegate Cell IGetCell(Pos Cpos);
        public delegate bool IcrActor(Pos EnvPos, ActorID ID);

    }
    static class Configuration
    {
        static public uint VideoWith = 1000;
        static public uint VideoHigh = 800;
        static public float Dtime = 0;
        static public int MapX = 512;
        static public int MapY = 512;
        public const uint waterhigh = 80;
        static public Random random = new Random();
        static public Camera cam;
    }

    static class Matem
    {
        /// <summary> Возвращает квадрат расстояния между точками  </summary>
        public static uint SQDist(Pos ap, Pos bp)
        {
            int kx = bp.X - ap.X;
            int ky = bp.Y - ap.Y;
            return (uint)(kx * kx + ky * ky);
        }
    }

    enum EnvSizeID : byte
    {
        Stop = 0,
        Small = 1,
        Bush = 2,
        middle = 3,
        tree = 4,
    }
    enum CellID : byte
    {
        Zero = 0,
        Grass = 1,
        GrassSand = 2,
        GrassStone = 3,
        Sand = 10,
        SandStone = 13,
        Stone = 20,
        StoneSand = 22,
    }

    enum ActorID : byte
    {
        Zero = 0,
        Chestnut = 1,
        Carrot = 11,
        Rabbit = 100,
        Wolf = 101,
        Deer = 102,
    }

    enum AnimState : byte
    {
        Zero = 0,
        Rest = 1,
        Move = 2,
        Eat = 3,
        OstAtk = 100,
        Dead = 255,
    }


}
