using Forest_Game.Additional;
using System.Collections.Generic;

namespace Forest_Game.WorldMap
{
    sealed partial class Map
    {
        public void SearchWayAnimal(object sender, SearchWayEventArgs e)
        {
            Pos minP = new Pos();
            uint min; uint minr;
            Animal send = (sender as Animal);
            ushort radsee = send.RadSee.Item2; if (radsee > 40) radsee = 40;
            int massize = (radsee * 2 + 1);
            byte[,] Mcan;
            uint[,] Mdt;    //если прибавлять 14, хватит и ushort
            e.MGoPos = new List<Pos>(); //обновляем
            //прибавлять к координатам массива, чтобы получить глобальные
            Pos plPos = new Pos(send.Location.X - radsee, send.Location.Y - radsee);
            Pos targ = new Pos(e.TPos.X - plPos.X, e.TPos.Y - plPos.Y);
            if (targ.X >= massize || targ.Y >= massize || targ.X < 0 || targ.Y < 0)
            {   //Если цель за границами видимости, выходим.
                e.CanGo = false;
                return;
            }
            if (targ.X == radsee && targ.Y == radsee)
            {
                e.CanGo = true;
                return;
            }
            if (!e.isCreate)    //если массивы не были созданы, создаём их
            {
                e.isCreate = true;
                Mcan = new byte[massize, massize];//0- нельзя пройти, 1- можно, 2- проверенная клетка 3- путь
                Mdt = new uint[massize, massize];//== может работать плохо с float, поэтому uint

                for (int i = 0; i < Mcan.GetLength(0); i++)
                {
                    for (int j = 0; j < Mcan.GetLength(1); j++)
                    {
                        if (i + plPos.X >= 0 && i + plPos.X < MapX && j + plPos.Y >= 0 && j + plPos.Y < MapY // что вне размера карты, будет непроходимым(по хорошему надо массив обрезать)
                            && send.GetCanPlace(MCell[i + plPos.X, j + plPos.Y].ID, MCell[i + plPos.X, j + plPos.Y].LEnvir))//чтобы пытался обходить остальных, нужно добавить && Mcell[,].animal==null
                        {
                            if (e.IsAnimPr && MCell[i + plPos.X, j + plPos.Y].LAnimal != null)  //если надо проверять животных, проверяем
                            {
                                Mcan[i, j] = 0;
                            }
                            else
                            {
                                Mcan[i, j] = 1;
                            }
                        }
                        else
                        {
                            Mcan[i, j] = 0;
                        }
                        Mdt[i, j] = uint.MaxValue;
                    }
                }
                Mcan[radsee, radsee] = 1;// первая клетка не проверена
                Mdt[radsee, radsee] = 0;
                e.Mcan = Mcan;
                e.Mdt = Mdt;

            }
            else
            {
                Mcan = e.Mcan;
                Mdt = e.Mdt;
            }
            int cnt = 0;
            if (Mcan[targ.X, targ.Y] == 0)
            {
                Mcan[targ.X, targ.Y] = 1;// клетка с целью будет считаться проходимой(могут быть проблемы)
            }
            void iM(int x, int y, uint mi, uint plus)
            {
                if (Mcan[x, y] == 1 && Mdt[x, y] > mi + plus) { Mdt[x, y] = mi + plus; }
            }

            while (true)
            {
                if (Mcan[targ.X, targ.Y] == 2)
                {
                    break;
                }
                minr = uint.MaxValue;
                min = uint.MaxValue;
                for (int i = 0; i < massize; i++)// ищем минимальный непроверенный элемент
                {
                    for (int j = 0; j < massize; j++)
                    {
                        if (Mcan[i, j] == 1 && Mdt[i, j] < uint.MaxValue && Matem.SQDist(targ, new Pos(i, j)) < minr)
                        {
                            minr = Matem.SQDist(targ, new Pos(i, j));
                            min = Mdt[i, j];
                            minP.X = i;
                            minP.Y = j;
                        }
                    }
                }
                if (minr < uint.MaxValue)  //если нашли соответствующий элемент, нужно обновить все соседние клетки
                {
                    cnt++;
                    if (minP.X + 1 < massize) iM(minP.X + 1, minP.Y, min, 1000);
                    if (minP.X + 1 < massize && minP.Y + 1 < massize) iM(minP.X + 1, minP.Y + 1, min, 1414);
                    if (minP.Y + 1 < massize) iM(minP.X, minP.Y + 1, min, 1000);
                    if (minP.X - 1 >= 0 && minP.Y + 1 < massize) iM(minP.X - 1, minP.Y + 1, min, 1414);
                    if (minP.X - 1 >= 0) iM(minP.X - 1, minP.Y, min, 1000);
                    if (minP.X - 1 >= 0 && minP.Y - 1 >= 0) iM(minP.X - 1, minP.Y - 1, min, 1414);
                    if (minP.Y - 1 >= 0) iM(minP.X, minP.Y - 1, min, 1000);
                    if (minP.X + 1 < massize && minP.Y - 1 >= 0) iM(minP.X + 1, minP.Y - 1, min, 1414);
                    Mcan[minP.X, minP.Y] = 2;
                }
                else
                {
                    break;
                }
            }

            //старый алгоритм(Дийкстра)
            #region

            //while (true)
            //{
            //    if (Mcan[targ.X, targ.Y] == 2)
            //    {
            //        break;
            //    }
            //    min = uint.MaxValue;
            //    for (int i = 0; i < massize; i++)// ищем минимальный непроверенный элемент
            //    {
            //        for (int j = 0; j < massize; j++)
            //        {
            //            if (Mcan[i, j] == 1 && Mdt[i, j] < min)//для жадного алгоритма изменить эту строку
            //            {
            //                min = Mdt[i, j];
            //                minP.X = i;
            //                minP.Y = j;

            //            }
            //        }
            //    }
            //    if (min < uint.MaxValue)  //если нашли соответствующий элемент, нужно обновить все соседние клетки
            //    {
            //        cnt++;
            //        if (minP.X + 1 < massize) iM(minP.X + 1, minP.Y, min, 1000);
            //        if (minP.X + 1 < massize && minP.Y + 1 < massize) iM(minP.X + 1, minP.Y + 1, min, 1414);
            //        if (minP.Y + 1 < massize) iM(minP.X, minP.Y + 1, min, 1000);
            //        if (minP.X - 1 >= 0 && minP.Y + 1 < massize) iM(minP.X - 1, minP.Y + 1, min, 1414);
            //        if (minP.X - 1 >= 0) iM(minP.X - 1, minP.Y, min, 1000);
            //        if (minP.X - 1 >= 0 && minP.Y - 1 >= 0) iM(minP.X - 1, minP.Y - 1, min, 1414);
            //        if (minP.Y - 1 >= 0) iM(minP.X, minP.Y - 1, min, 1000);
            //        if (minP.X + 1 < massize && minP.Y - 1 >= 0) iM(minP.X + 1, minP.Y - 1, min, 1414);
            //        Mcan[minP.X, minP.Y] = 2;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            #endregion
            System.Console.WriteLine(cnt);
            if (Mcan[targ.X, targ.Y] != 2)  //если целевая клетка не проверена, значит до неё нельзя добраться
            {
                e.CanGo = false;
                return;
            }
            minP.X = targ.X;  //повторное использование переменных, а что?
            minP.Y = targ.Y;  //позиция
            min = Mdt[minP.X, minP.Y];      //сколько идти до этой клетки
                                            // Mgo[minP.X, minP.Y] = 1;        // счётчик клеток(чтобы вернуться)
            byte lastrot = 0;   // предыдущее неправление(по умолчанию никуда,т.к. они начинаются с 1
                                // e.GoPos = minP;
            bool iG1(int x, int y, uint minus, byte dirc)
            {
                if (Mdt[x, y] == min - minus)
                {
                    e.MGoPos.Add(new Pos(minP.X + plPos.X, minP.Y + plPos.Y));
                    lastrot = 0;
                    minP.X = x; minP.Y = y;
                    //e.MGoPos.Add(new Pos(minP.X + plPos.X, minP.Y + plPos.Y));
                    min -= minus;
                    return true;
                }
                else return false;
            }

            if (e.StayNear) //нужно остановиться у края цели, двигаемся один раз
            {
                if (minP.X + 1 < massize && iG1(minP.X + 1, minP.Y, 1000, 1)) ;
                else if (minP.X + 1 < massize && minP.Y + 1 < massize && iG1(minP.X + 1, minP.Y + 1, 1414, 2)) ;
                else if (minP.Y + 1 < massize && iG1(minP.X, minP.Y + 1, 1000, 3)) ;
                else if (minP.X - 1 >= 0 && minP.Y + 1 < massize && iG1(minP.X - 1, minP.Y + 1, 1414, 4)) ;
                else if (minP.X - 1 >= 0 && iG1(minP.X - 1, minP.Y, 1000, 5)) ;
                else if (minP.X - 1 >= 0 && minP.Y - 1 >= 0 && iG1(minP.X - 1, minP.Y - 1, 1414, 6)) ;
                else if (minP.Y - 1 >= 0 && iG1(minP.X, minP.Y - 1, 1000, 7)) ;
                else if (minP.X + 1 < massize && minP.Y - 1 >= 0 && iG1(minP.X + 1, minP.Y - 1, 1414, 8)) ;
            }

            bool iG(int x, int y, uint minus, byte dirk)
            {
                if (Mdt[x, y] == min - minus)
                {
                    if (lastrot != dirk)    //Если предыдущее направление не совпадает с новым, ставится точка на старой клетке
                    {
                        // e.GoPos = minP; 
                        lastrot = dirk;
                        e.MGoPos.Add(new Pos(minP.X + plPos.X, minP.Y + plPos.Y));
                    }
                    minP.X = x; minP.Y = y;
                    min -= minus;
                    return true;
                }
                else return false;
            };

            while (!(minP.X == radsee && minP.Y == radsee))
            {
                if (minP.X + 1 < massize && iG(minP.X + 1, minP.Y, 1000, 1)) ;
                else if (minP.X + 1 < massize && minP.Y + 1 < massize && iG(minP.X + 1, minP.Y + 1, 1414, 2)) ;
                else if (minP.Y + 1 < massize && iG(minP.X, minP.Y + 1, 1000, 3)) ;
                else if (minP.X - 1 >= 0 && minP.Y + 1 < massize && iG(minP.X - 1, minP.Y + 1, 1414, 4)) ;
                else if (minP.X - 1 >= 0 && iG(minP.X - 1, minP.Y, 1000, 5)) ;
                else if (minP.X - 1 >= 0 && minP.Y - 1 >= 0 && iG(minP.X - 1, minP.Y - 1, 1414, 6)) ;
                else if (minP.Y - 1 >= 0 && iG(minP.X, minP.Y - 1, 1000, 7)) ;
                else if (minP.X + 1 < massize && minP.Y - 1 >= 0 && iG(minP.X + 1, minP.Y - 1, 1414, 8)) ;
            }
            e.CanGo = true;
            foreach (Pos n in e.MGoPos)
            {
                System.Console.WriteLine(n);
            }
            // e.GoPos.X += plPos.X; e.GoPos.Y += plPos.Y;//перевод ходьбы в мировые координаты.
            //System.Console.WriteLine("Можно!");

        }



    }
}