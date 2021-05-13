using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {
        public Map()  //
        {
            MapX = Additional.Configuration.MapX;
            MapY = Additional.Configuration.MapY;

            ChMapX = (short)(MapX / 16);
            ChMapY = (short)(MapY / 16);
            MCell = new Cell[MapX, MapY];
            MChunk = new Chunk[ChMapX, ChMapY];
            for (int i = 0; i < ChMapX; i++)
            {
                for (int j = 0; j < ChMapY; j++)
                {
                    MChunk[i, j] = new Chunk();
                }
            }
            int I, J;
            for (I = 0; I < MapX; I++) //инициализация клеток
            {
                for (J = 0; J < MapY; J++)
                {
                    MCell[I, J] = new Cell(new Pos(I, J));
                }
            }
            MapGenerate();
        }
        /// <summary>
        /// Вот тут
        /// </summary>
        /// <param name="win">Это</param>
        public void Draw(RenderWindow win, float CamPosX, float CamPosY) //Отрисовать
        {

            int mi, mj, StX, StY;
            int X, Y;
            float DrowX, DrowY;
            bool b;
            mj = 0;
            b = true;
            StX = MapX - 1; StY = 0;
            Vector2f vectP = new Vector2f();
            Color colorP = new Color(255, 255, 255, 255);
            //int i = 0;

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
                    //if(DrowX > -120 && DrowX < Additional.Configuration.VideoWith + 120 && DrowY  > 0 && DrowY - 512 < Additional.Configuration.VideoHigh)
                    //{ }
                    if (DrowX > -120 && DrowX < Additional.Configuration.VideoWith + 120 && DrowY - MCell[X, Y].downhighdr > 0 && DrowY - MCell[X, Y].highdr - 36 < Additional.Configuration.VideoHigh)
                    {

                        vectP.X = DrowX;
                        vectP.Y = DrowY - MCell[X, Y].high;

                        if (DrowX > -32 && DrowX < Additional.Configuration.VideoWith + 32)
                        {
                            if (MCell[X, Y].isWater)
                            {
                                if (DrowY - MCell[X, Y].high - 33 < Additional.Configuration.VideoHigh)
                                {
                                    colorP.R = MCell[X, Y].bright; colorP.G = colorP.R; colorP.B = colorP.R;
                                    if (MCell[X, Y].RendDown)
                                    {
                                        MCell[X, Y].LSpriteDown.Position = vectP;
                                        MCell[X, Y].LSpriteDown.Color = colorP;
                                        win.Draw(MCell[X, Y].LSpriteDown);
                                    }

                                    MCell[X, Y].LSpriteUp.Position = vectP;
                                    MCell[X, Y].LSpriteUp.Color = colorP;
                                    win.Draw(MCell[X, Y].LSpriteUp);
                                }

                                if (DrowY - waterhigh > 0)
                                {
                                    vectP.Y = DrowY - waterhigh;
                                    SpriteCollection.SWater_1.Position = vectP;
                                    SpriteCollection.SWater_1.Color = new Color(MCell[X, Y].brihtWater, MCell[X, Y].brihtWater, MCell[X, Y].brihtWater, 255);
                                    win.Draw(SpriteCollection.SWater_1);
                                }

                            }
                            else
                            {
                                if (DrowY - MCell[X, Y].high - 33 < Additional.Configuration.VideoHigh)
                                {
                                    if (MCell[X, Y].RendDown)
                                    {
                                        colorP.R = 255; colorP.G = 255; colorP.B = 255; colorP.A = 255;
                                        MCell[X, Y].LSpriteDown.Color = colorP;
                                        MCell[X, Y].LSpriteDown.Position = vectP;
                                        win.Draw(MCell[X, Y].LSpriteDown);
                                    }
                                    if (DrowY - MCell[X, Y].high > 0)
                                    {
                                        colorP.R = MCell[X, Y].ShadMult; colorP.G = colorP.R; colorP.B = colorP.R;
                                        MCell[X, Y].LSpriteUp.Position = vectP;
                                        MCell[X, Y].LSpriteUp.Color = colorP;
                                        win.Draw(MCell[X, Y].LSpriteUp);
                                    }
                                }


                            }
                        }

                        if (DrowY - MCell[X, Y].high > 0 && MCell[X, Y].LEnvir != null)
                        {
                            vectP.Y = DrowY - MCell[X, Y].high;
                            MCell[X, Y].LEnvir.ASprite.Position = vectP;
                            MCell[X, Y].LEnvir.ASprite.TextureRect = new IntRect((int)MCell[X, Y].LEnvir.Anim.AnimTime * MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.top, MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.heigh);
                            colorP.R = MCell[X, Y].ShadMultActor; colorP.G = colorP.R; colorP.B = colorP.R;
                            MCell[X, Y].LEnvir.ASprite.Color = colorP;
                            win.Draw(MCell[X, Y].LEnvir.ASprite);

                        }

                    }
                    X++; Y++;
                }

                if (StX == 0) StY++;
                if (StX > 0) StX--;
            }


        }

        public void Draw2(RenderWindow win, Camera Cam)
        {
            List<Actor> DNext1 = new List<Actor>(), DNext2 = new List<Actor>();

            int X, Y, DrowX, DrowY;

            int StX = MapX, StY = 0;
            int CR;
            int dCR, ldCR;

            int Camtop = (int)(Cam.ViewCam.Center.Y - (Cam.ViewCam.Size.Y) / 2);
            int Camdown = (int)(Cam.ViewCam.Center.Y + (Cam.ViewCam.Size.Y) / 2);
            int Camleft = (int)(Cam.ViewCam.Center.X - (Cam.ViewCam.Size.X) / 2);
            int Camright = (int)(Cam.ViewCam.Center.X + (Cam.ViewCam.Size.X) / 2);


            for (int iline = -Camtop / 18; iline > -Camdown / 18 - 57; iline--)// каждый круг по линии
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

                //X++;  //Надо будет наоборот уменьшить для рендера акторов
                //Y++;

                for (int i = 0; i < ldCR && (X < MapX && Y < MapY); i++) //рендер линии клеток
                {
                    DrowX = X * 32 + Y * 32;
                    DrowY = -X * 18 + Y * 18;

                    if (DrowX > Camleft - 32 && DrowX < Camright + 32 && DrowY - MCell[X, Y].downhighdr > Camtop && DrowY - MCell[X, Y].highdr - 36 < Camdown)
                    {
                        if (MCell[X, Y].isWater)
                        {
                            if (DrowY - MCell[X, Y].high - 36 < Camdown)
                            {
                                if (MCell[X, Y].RendDown)
                                {
                                    MCell[X, Y].LSpriteDown.Color = new Color(MCell[X, Y].bright, MCell[X, Y].bright, MCell[X, Y].bright, 255);
                                    MCell[X, Y].LSpriteDown.Position = MCell[X, Y].GlCoord;
                                    win.Draw(MCell[X, Y].LSpriteDown);
                                }
                                MCell[X, Y].LSpriteUp.Color = new Color(MCell[X, Y].bright, MCell[X, Y].bright, MCell[X, Y].bright, 255);
                                MCell[X, Y].LSpriteUp.Position = MCell[X, Y].GlCoord;
                                win.Draw(MCell[X, Y].LSpriteUp);


                            }
                            if (DrowY - MCell[X, Y].highdr > Camtop)
                            {
                                SpriteCollection.SWater_1.Position = new Vector2f(DrowX, DrowY - MCell[X, Y].highdr);
                                SpriteCollection.SWater_1.Color = new Color(MCell[X, Y].brihtWater, MCell[X, Y].brihtWater, MCell[X, Y].brihtWater, 255);
                                win.Draw(SpriteCollection.SWater_1);
                            }

                        }
                        else
                        {
                            if (MCell[X, Y].RendDown)
                            {
                                MCell[X, Y].LSpriteDown.Color = new Color(255, 255, 255, 255);
                                MCell[X, Y].LSpriteDown.Position = MCell[X, Y].GlCoord;
                                win.Draw(MCell[X, Y].LSpriteDown);
                            }
                            if (DrowY - MCell[X, Y].high > Camtop)// не особо полезный
                            {
                                MCell[X, Y].LSpriteUp.Color = new Color(MCell[X, Y].ShadMult, MCell[X, Y].ShadMult, MCell[X, Y].ShadMult, 255);
                                MCell[X, Y].LSpriteUp.Position = MCell[X, Y].GlCoord;
                                win.Draw(MCell[X, Y].LSpriteUp);
                            }


                        }


                    }
                    X++; Y++;
                }
                byte col;
                foreach (Actor n in DNext1) //Сперва надо отрендерить отложенных акторов
                {
                    n.ASprite.Position = n.Anim.GlCoord;
                    n.ASprite.TextureRect = new IntRect((int)n.Anim.AnimTime * n.Anim.with, n.Anim.top, n.Anim.with, n.Anim.heigh);
                    col = MCell[n.Location.X, n.Location.Y].ShadMultActor;
                    n.ASprite.Color = new Color(col, col, col, 255);
                    win.Draw(n.ASprite);
                }
                DNext1 = DNext2;
                DNext2 = new List<Actor>();

                dCR -= 2; if (dCR < 0) dCR = 0;
                ldCR += 4;
                X = StX; Y = StY;
                X += dCR; Y += dCR; // сдвигаем вправо
                for (int i = 0; i < ldCR && (X < MapX && Y < MapY); i++) //рендер линии клеток
                {
                    if (MCell[X, Y].LEnvir != null)
                    {
                        if (MCell[X, Y].LEnvir.Anim.GlCoord.Y > Camtop && MCell[X, Y].LEnvir.Anim.GlCoord.Y - MCell[X, Y].LEnvir.highdr - 36 < Camdown)
                        {

                            MCell[X, Y].LEnvir.ASprite.Position = MCell[X, Y].LEnvir.Anim.GlCoord;
                            MCell[X, Y].LEnvir.ASprite.TextureRect = new IntRect((int)MCell[X, Y].LEnvir.Anim.AnimTime * MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.top, MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.heigh);
                            MCell[X, Y].LEnvir.ASprite.Color = new Color(MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, 255);
                            win.Draw(MCell[X, Y].LEnvir.ASprite);


                        }
                    }
                    if (MCell[X, Y].LAnimal != null)
                    {
                        if (MCell[X, Y].LAnimal.Anim.GlCoord.Y > Camtop && MCell[X, Y].LAnimal.Anim.GlCoord.Y - MCell[X, Y].LAnimal.highdr - 36 < Camdown)
                        {
                            if (MCell[X, Y].LAnimal.Anim.PlusLine == 0)
                            {
                                MCell[X, Y].LAnimal.ASprite.Position = MCell[X, Y].LAnimal.Anim.GlCoord;
                                MCell[X, Y].LAnimal.ASprite.TextureRect = new IntRect((int)MCell[X, Y].LAnimal.Anim.AnimTime * MCell[X, Y].LAnimal.Anim.with, MCell[X, Y].LAnimal.Anim.top, MCell[X, Y].LAnimal.Anim.with, MCell[X, Y].LAnimal.Anim.heigh);
                                MCell[X, Y].LAnimal.ASprite.Color = new Color(MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, 255);
                                win.Draw(MCell[X, Y].LAnimal.ASprite);
                            }
                            else if (MCell[X, Y].LAnimal.Anim.PlusLine == 1)
                            {
                                DNext1.Add(MCell[X, Y].LAnimal);
                            }
                            else
                            {
                                DNext2.Add(MCell[X, Y].LAnimal);
                            }

                        }
                    }
                    X++; Y++;
                }
                //for (int i = 0; i < ldCR && (X < MapX && Y < MapY); i++) //рендер линии клеток
                //{

                //    if (MCell[X, Y].LEnvir != null)
                //    {
                //        DrowX = X * 32 + Y * 32;
                //        DrowY = -X * 18 + Y * 18;
                //        if (DrowX > Camleft - 150 && DrowX < Camright + 150 && DrowY - MCell[X, Y].high > Camtop && DrowY - MCell[X, Y].LEnvir.highdr - MCell[X, Y].high - 36 < Camdown)
                //        {
                //            MCell[X, Y].LEnvir.ASprite.Position = MCell[X, Y].LEnvir.Anim.GlCoord;//new Vector2f(DrowX, DrowY - MCell[X, Y].high);
                //            MCell[X, Y].LEnvir.ASprite.TextureRect = new IntRect((int)MCell[X, Y].LEnvir.Anim.AnimTime * MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.top, MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.heigh);
                //            MCell[X, Y].LEnvir.ASprite.Color = new Color(MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, 255);
                //            win.Draw(MCell[X, Y].LEnvir.ASprite);
                //        }
                //    }
                //    X++;Y++;
                //}
                //int aline = iline + 1;  //то же самое, но для эктора
                //if (aline < MapX)
                //{
                //    if (aline >= 0)
                //    {
                //        StX = aline;
                //        StY = 0;
                //    }
                //    else if (aline < 0)
                //    {
                //        StX = 0;
                //        StY = -aline;
                //    }

                //    CR = 512 - System.Math.Abs(aline);
                //    X = StX; Y = StY;
                //    dCR = Camleft / 64 - (MapX - CR) / 2;

                //    if (dCR < 0)
                //    {
                //        ldCR = (int)(dCR + Cam.ViewCam.Size.X / 64 + 2);
                //        dCR = 0;
                //    }
                //    else
                //    {
                //        ldCR = (int)(Cam.ViewCam.Size.X / 64 + 2);
                //    }

                //    X += dCR; Y += dCR; // сдвигаем вправо


                //    for (int i = 0; i < ldCR && (X < MapX && Y < MapY); i++) //рендер линии клеток
                //    {

                //        if (MCell[X, Y].LEnvir != null)
                //        {
                //            DrowX = X * 32 + Y * 32;
                //            DrowY = -X * 18 + Y * 18;
                //            if (DrowX > Camleft - 150 && DrowX < Camright + 150 && DrowY - MCell[X, Y].high > Camtop && DrowY - MCell[X, Y].LEnvir.highdr - MCell[X, Y].high - 36 < Camdown)
                //            {
                //                MCell[X, Y].LEnvir.ASprite.Position = MCell[X, Y].LEnvir.Anim.GlCoord;//new Vector2f(DrowX, DrowY - MCell[X, Y].high);
                //                MCell[X, Y].LEnvir.ASprite.TextureRect = new IntRect((int)MCell[X, Y].LEnvir.Anim.AnimTime * MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.top, MCell[X, Y].LEnvir.Anim.with, MCell[X, Y].LEnvir.Anim.heigh);
                //                MCell[X, Y].LEnvir.ASprite.Color = new Color(MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, 255);
                //               win.Draw(MCell[X, Y].LEnvir.ASprite);
                //            }
                //        }
                //        if (MCell[X, Y].LAnimal != null)
                //        {
                //            DrowX = X * 32 + Y * 32;
                //            DrowY = -X * 18 + Y * 18;
                //            if (DrowX > Camleft - 60 && DrowX < Camright + 60 && DrowY - MCell[X, Y].high > Camtop && DrowY - MCell[X, Y].LAnimal.highdr - MCell[X, Y].high - 36 < Camdown)
                //            {
                //                MCell[X, Y].LAnimal.ASprite.Position = MCell[X, Y].LAnimal.Anim.GlCoord;//new Vector2f(DrowX, DrowY - MCell[X, Y].high);
                //                MCell[X, Y].LAnimal.ASprite.TextureRect = new IntRect((int)MCell[X, Y].LAnimal.Anim.AnimTime * MCell[X, Y].LAnimal.Anim.with, MCell[X, Y].LAnimal.Anim.top, MCell[X, Y].LAnimal.Anim.with, MCell[X, Y].LAnimal.Anim.heigh);
                //                MCell[X, Y].LAnimal.ASprite.Color = new Color(MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, MCell[X, Y].ShadMultActor, 255);
                //                win.Draw(MCell[X, Y].LAnimal.ASprite);
                //            }
                //        }

                //        X++; Y++;


                //    }








                //}


            }




        }


    }

}


