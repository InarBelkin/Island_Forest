using System.Collections.Generic;
namespace Forest_Game.WorldMap

{
    sealed class Chunk
    {
        public int BiomID;
        public List<Animal> Animals;
        public List<Envir> Envirs;

        public Chunk()
        {
            Animals = new List<Animal>();
            Envirs = new List<Envir>();
            //  Envirs.Remove();


            //  Game.EvTick += new Additional.StaticClass.Self(EvTick);
        }

        //public void EvTick(float Dtime)
        //{
        //    //Логика растений
        //}

    }
}