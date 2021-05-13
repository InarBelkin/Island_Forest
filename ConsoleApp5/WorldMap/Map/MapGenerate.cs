using Forest_Game.Additional;
using SFML.System;

namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {
        public void MapGenerate()
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(@"../Pictures/Substance/New_Graph_height.png");
            System.Drawing.Bitmap GHeigh = new System.Drawing.Bitmap(img);
            System.Drawing.Image imgligt = System.Drawing.Image.FromFile(@"../Pictures/Substance/New_Graph_light.png");
            System.Drawing.Bitmap Glight = new System.Drawing.Bitmap(imgligt);
            System.Drawing.Image imgSand = System.Drawing.Image.FromFile(@"../Pictures/Substance/New_Graph_Sand.png");
            System.Drawing.Bitmap GSand = new System.Drawing.Bitmap(imgSand);
            System.Drawing.Image imgSandgrass = System.Drawing.Image.FromFile(@"../Pictures/Substance/New_Graph_SandGrass.png");
            System.Drawing.Bitmap GSandGrass = new System.Drawing.Bitmap(imgSandgrass);

            int I, J, cntr, cntr2;
            for (I = 0; I < MapX; I++)
            {
                for (J = 0; J < MapY; J++)
                {

                    MCell[I, J].high = (ushort)(GHeigh.GetPixel(I, J).B * 4);
                    MCell[I, J].highdr = MCell[I, J].high;
                    MCell[I, J].GlCoord = new Vector2f(I * 32 + J * 32, -I * 18 + J * 18 - MCell[I, J].high);

                    MCell[I, J].ShadMult = Glight.GetPixel(I, J).R;
                    MCell[I, J].brihtWater = Glight.GetPixel(I, J).B;
                    MCell[I, J].ShadMultActor = Glight.GetPixel(I, J).G;

                    if (MCell[I, J].high < waterhigh)
                    {
                        MCell[I, J].isWater = true;
                        MCell[I, J].highdr = waterhigh;

                        MCell[I, J].bright = (byte)((255 - (((waterhigh - MCell[I, J].high) / (float)waterhigh) * 255)) * ((float)MCell[I, J].ShadMult / 255));

                    }

                    if (GSand.GetPixel(I, J).B > 200)
                    {
                        MCell[I, J].CopySprite(SpriteCollection.SSand_1U, SpriteCollection.SSand_1D);
                        MCell[I, J].ID = CellID.Sand;
                    }
                    else
                    {
                        if (GSandGrass.GetPixel(I, J).B > 200)
                        {
                            MCell[I, J].CopySprite(SpriteCollection.SSandGrass_1U, SpriteCollection.SSandGrass_1D);
                            MCell[I, J].ID = CellID.GrassSand;
                        }
                        else
                        {
                            MCell[I, J].CopySprite(SpriteCollection.SGrass_1U, SpriteCollection.SGrass_1D);
                            MCell[I, J].ID = CellID.Grass;
                        }

                    }






                }
            }

            for (I = 1; I < MapX - 1; I++)
            {
                for (J = 1; J < MapY - 1; J++)
                {
                    cntr = 0;
                    if (MCell[I, J].high - MCell[I + 1, J + 1].high >= StHiDif)
                    {
                        cntr++;
                    }
                    if (MCell[I, J].high - MCell[I + 1, J - 1].high >= StHiDif)
                    {
                        cntr++;
                    }
                    if (MCell[I, J].high - MCell[I - 1, J + 1].high >= StHiDif)
                    {
                        cntr++;
                    }
                    if (MCell[I, J].high - MCell[I - 1, J - 1].high >= StHiDif)
                    {
                        cntr++;
                    }

                    if (cntr > 0)
                    {

                        MCell[I, J].CopySprite(SpriteCollection.SStone_01U, SpriteCollection.SStone_01D);
                        MCell[I, J].ID = CellID.Stone;
                    }
                }
            }

            for (I = 1; I < MapX - 1; I++)
            {
                for (J = 1; J < MapY - 1; J++)
                {
                    if (MCell[I, J].ID == CellID.Stone)
                    {
                        cntr = 0;
                        if (MCell[I + 1, J + 1].ID == 0 && MCell[I + 1, J + 1].high - MCell[I, J].high < 30 && MCell[I + 1, J + 1].high - MCell[I, J].high >= 0)
                        {
                            cntr++;
                        }
                        if (MCell[I + 1, J - 1].ID == 0 && MCell[I + 1, J - 1].high - MCell[I, J].high < 30 && MCell[I + 1, J - 1].high - MCell[I, J].high >= 0)
                        {
                            cntr++;
                        }
                        if (MCell[I - 1, J + 1].ID == 0 && MCell[I - 1, J + 1].high - MCell[I, J].high < 30 && MCell[I - 1, J + 1].high - MCell[I, J].high >= 0)
                        {
                            cntr++;
                        }
                        if (MCell[I - 1, J - 1].ID == 0 && MCell[I - 1, J - 1].high - MCell[I, J].high < 30 && MCell[I - 1, J - 1].high - MCell[I, J].high >= 0)
                        {
                            cntr++;
                        }

                        cntr2 = 0;
                        if (MCell[I + 1, J + 1].ID == 0)
                        {
                            cntr2++;
                        }
                        if (MCell[I + 1, J - 1].ID == 0)
                        {
                            cntr2++;
                        }
                        if (MCell[I - 1, J + 1].ID == 0)
                        {
                            cntr2++;
                        }
                        if (MCell[I - 1, J - 1].ID == 0)
                        {
                            cntr2++;
                        }


                        if (cntr == 2 && cntr2 == 2)
                        {
                            System.Console.WriteLine("а?");
                            if (GSand.GetPixel(I, J).B > 200)
                            {
                                MCell[I, J].CopySprite(SpriteCollection.SSandStoneU, SpriteCollection.SSandStoneD);
                                MCell[I, J].ID = CellID.SandStone;
                            }
                            else
                            {
                                MCell[I, J].CopySprite(SpriteCollection.SGrassStone_01U, SpriteCollection.SGrassStone_01D);
                                MCell[I, J].ID = CellID.GrassStone;
                            }

                        }
                    }



                }
            }

            for (I = 1; I < MapX - 1; I++)
            {
                for (J = 1; J < MapY - 1; J++)
                {
                    if (MCell[I, J].ID == CellID.Stone && GSand.GetPixel(I, J).B > 200)
                    {
                        MCell[I, J].CopySprite(SpriteCollection.SStoneSandU, SpriteCollection.SStoneSandD);
                        MCell[I, J].ID = CellID.StoneSand;
                    }
                }
            }

            for (I = 1; I < MapX - 1; I++)
            {
                for (J = 1; J < MapY - 1; J++)
                {

                    if (MCell[I, J].high - MCell[I - 1, J + 1].high > 0)
                    {
                        MCell[I, J].downhighdr = MCell[I - 1, J + 1].high;
                    }
                    else
                    {
                        MCell[I, J].downhighdr = MCell[I, J].high;
                    }

                    if (MCell[I - 1, J].high - MCell[I, J].high >= 0 && MCell[I, J + 1].high - MCell[I, J].high >= 0)
                    {
                        MCell[I, J].RendDown = false;
                    }

                }
            }


            img.Dispose();
            imgligt.Dispose();
            imgSand.Dispose();
            imgSandgrass.Dispose();

            GHeigh.Dispose();
            Glight.Dispose();
            GSand.Dispose();
            GSandGrass.Dispose();


            System.GC.Collect();
        }



    }
}
