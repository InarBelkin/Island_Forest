using Forest_Game.Additional;
using System;

namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {
        private Chunk this[Pos Index] => MChunk[Index.X, Index.Y];

        public void GoAnimal(object sender, GoEventArgs e)
        {
            Animal send = (sender as Animal);//если получится актор, делай его
            e.IsAnimPr = false;
            if (MCell[e.TargPos.X, e.TargPos.Y].LAnimal == null)
            {
                if (send.GetCanPlace(MCell[e.TargPos.X, e.TargPos.Y].ID, MCell[e.TargPos.X, e.TargPos.Y].LEnvir))
                {
                    if (MCell[e.CurPos.X, e.CurPos.Y].LAnimal == send)
                    {
                        if (e.CurPos.X / 16 != e.TargPos.X / 16 || e.CurPos.Y / 16 != e.TargPos.Y / 16)
                        {
                            this[e.TargPos / 16].Animals.Add(MCell[e.CurPos.X, e.CurPos.Y].LAnimal);//может добавляться null, надо удалять именно Sender
                            this[e.CurPos / 16].Animals.Remove(MCell[e.CurPos.X, e.CurPos.Y].LAnimal);
                        }

                        MCell[e.TargPos.X, e.TargPos.Y].LAnimal = MCell[e.CurPos.X, e.CurPos.Y].LAnimal;
                        MCell[e.CurPos.X, e.CurPos.Y].LAnimal = null;
                        e.CanGo = true;
                    }
                    else
                    {
                        Console.WriteLine("Координаты животного и клетки не совпадают");
                    }
                }
                else e.CanGo = false;
            }
            else
            {
                e.IsAnimPr = true;
                e.CanGo = false;
            }
        }

        public void LookAnimal(object sender, LookEventArgs e)
        {
            Animal send = sender as Animal;
            const ushort massize = 30;
            //  e.Sender.Location.
            Pos ChCoord = new Pos(send.Location.X / 16, send.Location.Y / 16);
            int ChRad = e.radius / 16 + 1;
            Pos Loc = send.Location;
            float lenght;

            if (e.MustLkAnm)    // если он нужен
            {
                e.Animals = new Vector<Animal>(massize);
                e.Asort = new (float, bool)[massize];
                for (int i = ChCoord.X - ChRad; i <= ChCoord.X + ChRad; i++)
                {
                    if (i >= 0 && i < ChMapX)
                    {
                        for (int j = ChCoord.Y - ChRad; j <= ChCoord.Y + ChRad; j++)
                        {
                            if (j >= 0 && j < ChMapY)
                            {
                                foreach (Animal k in MChunk[i, j].Animals)
                                {
                                    lenght = (float)Math.Sqrt((k.Location.X - Loc.X) * (k.Location.X - Loc.X) + (k.Location.Y - Loc.Y) * (k.Location.Y - Loc.Y));
                                    if (lenght <= e.radius)
                                    {
                                        (float, bool) a = e.Queier(k, lenght);
                                        if (a.Item2)
                                        {
                                            e.Animals.Add(k);
                                            e.Asort[e.Animals.Lenght - 1] = (a.Item1, false);
                                            if (e.Animals.Lenght >= massize) goto label1;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        label1:
            if (e.MustLkEnv)
            {
                e.Envirs = new Vector<Envir>(massize);
                e.Esort = new (float, bool)[massize];
                for (int i = ChCoord.X - ChRad; i <= ChCoord.X + ChRad; i++)
                {
                    if (i >= 0 && i < ChMapX)
                    {
                        for (int j = ChCoord.Y - ChRad; j <= ChCoord.Y + ChRad; j++)
                        {
                            if (j >= 0 && j < ChMapY)
                            {
                                foreach (Envir k in MChunk[i, j].Envirs)
                                {
                                    lenght = (float)Math.Sqrt((k.Location.X - Loc.X) * (k.Location.X - Loc.X) + (k.Location.Y - Loc.Y) * (k.Location.Y - Loc.Y));
                                    if (lenght <= e.radius)
                                    {
                                        (float, bool) a = e.Queier(k, lenght);
                                        if (a.Item2)
                                        {
                                            e.Envirs.Add(k);
                                            e.Esort[e.Envirs.Lenght - 1] = (a.Item1, false);
                                            if (e.Envirs.Lenght >= massize) return;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            //  System.Console.WriteLine(e.radius);
        }
    }
}
