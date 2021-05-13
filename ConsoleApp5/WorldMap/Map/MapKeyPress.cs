using Forest_Game.Additional;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {

        public bool GetMouseCellPos(RenderWindow win, float CamPosX, float CamPosY, ref Pos Msel, ref CellID LandID, ref bool canEnv, ref bool canAnim, ref byte Shadmult)
        {

            Vector2f coord;
            Vector2f coordMouse;

            coordMouse = (Vector2f)SFML.Window.Mouse.GetPosition(win);


            bool b;


            int mi, mj, StX, StY;
            int X, Y;   //Pos
            float DrowX, DrowY;
            bool bc = false;
            mj = 0;
            b = true;
            StX = MapX - 1; StY = 0;

            //Кошмарный алгоритм для отрисовки в нужном порядке

            for (mi = MapX * 2 - 1; mi > 0; mi--)
            {
                if (b == true) mj++;
                else mj--;

                if (mj == MapX) b = false;

                X = StX; Y = StY;
                for (int tj = mj; tj > 0; tj--)
                {
                    DrowX = CamPosX + X * 32 + Y * 32;
                    DrowY = CamPosY - X * 18f + Y * 18f;

                    if (!(DrowX < -32 || DrowX > Additional.Configuration.VideoWith + 32 || DrowY < -18f || DrowY > Additional.Configuration.VideoHigh + MCell[X, Y].highdr + 37))
                    {
                        coord = new Vector2f(DrowX, DrowY - MCell[X, Y].high);

                        if ((coordMouse.Y - coord.Y) < 0.5773502692 * (coordMouse.X - coord.X) && (coordMouse.Y - coord.Y + 35) > 0.5773502692 * (coordMouse.X - coord.X) && (coordMouse.Y - coord.Y) < -0.5773502692 * (coordMouse.X - coord.X) && (coordMouse.Y - coord.Y + 35) > -0.5773502692 * (coordMouse.X - coord.X))
                        {
                            Msel.X = X;
                            Msel.Y = Y;
                            bc = true;
                        }

                    }
                    X++; Y++;
                }
                if (StX == 0) StY++;
                if (StX > 0) StX--;
            }

            if (bc)
            {
                LandID = MCell[Msel.X, Msel.Y].ID;
                if (MCell[Msel.X, Msel.Y].LEnvir == null) canEnv = true;
                else canEnv = false;
                if (MCell[Msel.X, Msel.Y].LAnimal == null) canAnim = true;
                else canAnim = false;
                Shadmult = MCell[Msel.X, Msel.Y].ShadMult;
            }
            return bc;
        }

        public bool GetMouseCelPos2(RenderWindow win, Camera Cam, out Pos Msel, out Cell OCell)
        {
            OCell = null;
            Msel = new Pos();
            Vector2f coord;
            Vector2f coordMouse = win.MapPixelToCoords(Mouse.GetPosition(win));
            bool bc = false;

            // System.Console.WriteLine(coordMouse);
            int X, Y, DrowX, DrowY;

            int StX = MapX, StY = 0;
            int CR;
            int dCR, ldCR;

            int Camtop = (int)(Cam.ViewCam.Center.Y - (Cam.ViewCam.Size.Y) / 2);
            int Camdown = (int)(Cam.ViewCam.Center.Y + (Cam.ViewCam.Size.Y) / 2);
            int Camleft = (int)(Cam.ViewCam.Center.X - (Cam.ViewCam.Size.X) / 2);
            int Camright = (int)(Cam.ViewCam.Center.X + (Cam.ViewCam.Size.X) / 2);

            //for (int iline = -Camtop / 18; iline > -Camdown / 18 - 57; iline--)
            for (int iline = -Camdown / 18 - 57; iline <= -Camtop / 18; iline++)
            {

                if (iline >= 0)
                {
                    StX = iline;
                    StY = 0;
                }
                else if (iline < 0)
                {
                    StX = 0;
                    StY = -iline;
                }

                CR = 512 - System.Math.Abs(iline);
                X = StX; Y = StY;
                dCR = Camleft / 64 - (MapX - CR) / 2;

                if (dCR < 0)
                {
                    ldCR = (int)(dCR + Cam.ViewCam.Size.X / 64 + 2);
                    dCR = 0;
                }
                else
                {
                    ldCR = (int)(Cam.ViewCam.Size.X / 64 + 2);
                }

                X += dCR; Y += dCR; // сдвигаем вправо


                for (int i = 0; i < ldCR && (X < MapX && Y < MapY); i++) //рендер линии клеток
                {
                    DrowX = X * 32 + Y * 32;
                    DrowY = -X * 18 + Y * 18;

                    if (DrowX > Camleft - 32 && DrowX < Camright + 32 && DrowY - MCell[X, Y].downhighdr > Camtop && DrowY - MCell[X, Y].highdr - 36 < Camdown)
                    {
                        coord = new Vector2f(DrowX, DrowY - MCell[X, Y].high);
                        //System.Console.WriteLine(coord);
                        if ((coordMouse.Y - coord.Y) < 0.5773502692 * (coordMouse.X - coord.X) && (coordMouse.Y - coord.Y + 35) > 0.5773502692 * (coordMouse.X - coord.X) && (coordMouse.Y - coord.Y) < -0.5773502692 * (coordMouse.X - coord.X) && (coordMouse.Y - coord.Y + 35) > -0.5773502692 * (coordMouse.X - coord.X))
                        {
                            Msel.X = X;
                            Msel.Y = Y;
                            OCell = MCell[X, Y];
                            bc = true;
                            goto exit_label;


                        }

                    }
                    X++; Y++;
                }


            }
        exit_label:

            return bc;


        }


        public void AddEnvir(Envir env, Pos EnvPos)
        {

            MCell[EnvPos.X, EnvPos.Y].LEnvir = env;
            MChunk[EnvPos.X / 16, EnvPos.Y / 16].Envirs.Add(env);


        }

        public void AddAnim(Animal anm, Pos EnvPos)
        {
            MCell[EnvPos.X, EnvPos.Y].LAnimal = anm;
            MChunk[EnvPos.X / 16, EnvPos.Y / 16].Animals.Add(anm);


        }
        public bool DeleteEnvir(Envir env)
        {
            if (MCell[env.Location.X, env.Location.Y].LEnvir != null)
            {
                MCell[env.Location.X, env.Location.Y].LEnvir = null;
                if (!MChunk[env.Location.X / 16, env.Location.Y / 16].Envirs.Remove(env))
                    System.Console.WriteLine("Не найден envir в чанке");
                return true;
            }
            else return false;

        }
        public bool DeleteAnim(Animal anm)
        {
            if (MCell[anm.Location.X, anm.Location.Y].LAnimal != null)
            {
                MCell[anm.Location.X, anm.Location.Y].LAnimal = null;
                if (!MChunk[anm.Location.X / 16, anm.Location.Y / 16].Animals.Remove(anm))
                {
                    System.Console.WriteLine("Не найден animal в чанке");
                }
                return true;
            }
            else return false;
        }

    }
}
