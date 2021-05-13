using System;
using System.Collections.Generic;

namespace Forest_Game.Additional
{
    class LookEventArgs : EventArgs
    {
        public Vector<Animal> Animals;
        public Vector<Envir> Envirs;
        public (float, bool)[] Esort;
        public (float, bool)[] Asort;
        public ushort radius;
        public bool MustLkAnm = false, MustLkEnv = false;
        public Queries Queier = null;   //Метод, по которому определяется, пихать ли это в массив
        public LookEventArgs()
        {
            Animals = null;
            Envirs = null;
        }
        public delegate (float, bool) Queries(Actor Act, float lenght);

    }
    class GoEventArgs : EventArgs
    {
        public Pos CurPos;
        public Pos TargPos;
        public bool CanGo;
        ///<summary>Мешает ли животное</summary>
        public bool IsAnimPr;
    }
    class SearchWayEventArgs : EventArgs
    {
        ///<summary>Нужно ли искать путь по животным</summary>
        public bool IsAnimPr = false;
        public Pos TPos;

        public List<Pos> MGoPos = new List<Pos>();
        public bool isCreate = false;
        public bool CanGo;

        public byte[,] Mcan;
        public uint[,] Mdt;

        /// <summary> Путь строится до соседней точки цели </summary>
        public bool StayNear;
        public SearchWayEventArgs()
        {
            StayNear = false;
        }
    }
    class DeathEventArgs : EventArgs
    {

    }


}
