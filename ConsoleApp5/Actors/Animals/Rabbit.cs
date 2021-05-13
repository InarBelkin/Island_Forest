using Forest_Game.Additional;
using Forest_Game.Additional.Interfaces;
using Forest_Game.WorldMap;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Forest_Game.Animals
{
    partial class Rabbit : Animal, ICanEat, IEatable, IAttackable
    {
        Rstate AvState = Rstate.Zero;
        private bool Near = false;
        private byte CntThink = 0;
        private const byte MaxCntTh = 3;
        public Rabbit(Pos Envpos, Vector2f GlCoordinate) : base(Envpos, GlCoordinate)
        {
            ID = ActorID.Rabbit;
            ASprite = SpriteCollection.SRabbit;
            highdr = 5;
            Anim.with = 200; Anim.top = 400; Anim.heigh = 200; Anim.AnimTime = 0;
            Speed = 0.7f;
            RadSee = (10, 10);
            Hunger = new StrHunger(10, -0.1f);
            Hunger.Hung = 5;
            FoodRec = 10;
            Health = new StrHealth(10, 0);

        }
        public override void EvTick(float DTime)
        {
            TickAnim(DTime);
            ProcessAct(Configuration.Dtime);
            if (Health.alive == false) return;
            if (State == AnimState.Zero) //В этом случае надо думать, что делать
            {
                CntThink++;
                if (Targ.Count == 0)//Пришли, что дальше?
                {
                    ThinkNew(); CntThink = 0;
                }
                else if (Targ[Targ.Count - 1] == Location || CntThink >= MaxCntTh)
                {   //А правильно ли мы идём?
                    CntThink = 0;
                    Console.WriteLine("Думаютб");
                    if (Targ[Targ.Count - 1] == Location) Targ.RemoveAt(Targ.Count - 1);

                    if (Targ.Count == 0 || Near && Targ.Count == 1)
                    {
                        ThinkNew(); CntThink = 0;
                    }
                    else
                    {
                        if (ThinkMed())
                        {
                            GoEventArgs g = new GoEventArgs()
                            {
                                CurPos = Location,
                                TargPos = Targ[Targ.Count - 1],
                            };
                            if (!ActGo(g, false))//если не сможет пройти, думоем
                            {
                                ThinkNew(g.IsAnimPr); CntThink = 0;
                            }
                        }
                        else
                        {
                            if (State == AnimState.Zero)
                            {
                                ThinkNew(); CntThink = 0;
                            }
                        }
                    }
                }
                else
                {   //Просто идём
                    GoEventArgs g = new GoEventArgs()
                    {
                        CurPos = Location,
                        TargPos = Targ[Targ.Count - 1],
                    };
                    if (!ActGo(g, false))   //если не сможет пройти, думоем
                    {
                        ThinkNew(g.IsAnimPr); CntThink = 0;
                    }

                }
            }
        }
        ///<param name="IsAnimPr">Нужно ли искать путь вместе с животными</param>
        private void ThinkNew(bool IsAnimPr = false)
        {
            LookEventArgs Mlook = new LookEventArgs();
            if (PartEnem(Mlook)) return;
            SearchWayEventArgs Way = new SearchWayEventArgs() { IsAnimPr = IsAnimPr };
            if (PartEat(Mlook, Way)) return;
            if (PartIdle()) return;
            if (State == AnimState.Zero)
            {
                DoAvAct(Rstate.Disarray, new List<Pos>());
                ActRest(2);
            }

        }
        private bool ThinkMed()  //возвращает true,если надо дальше двигаться к цели
        {
            LookEventArgs Mlook = new LookEventArgs();
            if (PartEnem(Mlook)) return false;

            if (AvState == Rstate.GoEat)
            {
                Cell c = QActGetCell(Targ[0]);
                if (c.LEnvir != null && QuerEat(c.LEnvir, 1).Item2)
                {
                    Console.WriteLine("Иди дальше");
                    return true;// можешь идти дальше   
                }
                else
                {
                    DoAvAct(Rstate.Zero, new List<Pos>());
                    return false;
                }
            }
            else if (AvState == Rstate.GoGulyat)
            {
                SearchWayEventArgs Way = new SearchWayEventArgs() { IsAnimPr = false };
                if (PartEat(Mlook, Way)) return false;
            }
            return true;
        }
        private bool PartEnem(LookEventArgs Mlook)
        {
            if (Mlook.Animals == null)
            {
                Mlook.MustLkAnm = true; Mlook.MustLkEnv = false;
                Mlook.radius = RadSee.Item2;
                Mlook.Queier = delegate (Actor Act, float l) { if (Act is Wolf) return (l, true); else return (100, false); };
                QActLook(Mlook);
            }
            for (ushort i = 0; i < Mlook.Animals.Lenght; i++)
            {
                if (Mlook.Animals[i] is Wolf)    //go away!
                {

                    Pos GAtarg = Location - Mlook.Animals[i].Location;
                    Pos RTarg = GAtarg;
                    float minangle = float.MaxValue;
                    for (byte j = 0; j <= 7; j++)
                    {
                        Cell Mcell = QActGetCell(Location + Pos.GetDirPos((Direction)j));
                        float angle = (float)Math.Abs(Vector.AngleBetween(new Vector(GAtarg.X, GAtarg.Y), new Vector(Pos.GetDirPos((Direction)j).X, Pos.GetDirPos((Direction)j).Y)));
                        if (this.GetCanPlace(Mcell.ID, Mcell.LEnvir) && Mcell.LAnimal == null &&
                            angle < minangle)
                        {
                            RTarg = Pos.GetDirPos((Direction)j);
                            minangle = angle;
                        }
                    }

                    if (minangle < float.MaxValue)
                    {
                        DoAvAct(Rstate.GoAway, new List<Pos>());
                        ActGo(new GoEventArgs()
                        {
                            CurPos = Location,
                            TargPos = Location + RTarg,
                        });
                        return true;
                    }
                    else return false;


                    //Console.WriteLine("Go Away!");
                    ////  ActRest(2);
                    //return true;
                }
            }
            return false;
        }
        private bool PartEat(LookEventArgs Mlook, SearchWayEventArgs Way)
        {
            if (Hunger.Hung > Hunger.MaxHung * 0.8) return false;
            if (Mlook.Envirs == null)
            {
                Mlook.MustLkEnv = true; Mlook.MustLkAnm = false;
                Mlook.radius = RadSee.Item2;
                Mlook.Queier = QuerEat;
                QActLook(Mlook);
            }
            float min; uint index;
            for (int iter = 0; iter < 5; iter++)
            {
                index = 0;
                min = float.MaxValue; //Раскидываем по приоритету.
                for (ushort i = 0; i < Mlook.Envirs.Lenght; i++)  //проверяем все окружающие Envir
                {
                    if (Mlook.Esort[i].Item1 < min && Mlook.Esort[i].Item2 == false)
                    {
                        index = i;
                        min = Mlook.Esort[i].Item1;
                    }
                }
                if (min != float.MaxValue)
                {
                    Mlook.Esort[index].Item2 = true;
                    if (Pos.PosNear(Location, Mlook.Envirs[index].Location))
                    {
                        ActEat(Mlook.Envirs[index].Location);
                        DoAvAct(Rstate.GoEat, new List<Pos>());
                        return true;
                    }
                    Way.TPos = (Mlook.Envirs[index]).Location;
                    Way.StayNear = true;
                    QActSearchWay(Way);//Console.Write("1");
                    if (Way.CanGo == true)
                    {
                        DoAvAct(Rstate.GoEat, Way.MGoPos, true);
                        if (Targ.Count != 0)
                        {
                            ActGo(new GoEventArgs()
                            {
                                CurPos = Location,
                                TargPos = Targ[Targ.Count - 1],   //не должно быть 0 элементов
                            });
                        }
                        return true;//мы уже всё решили и выходим
                    }
                }
                else return false;// всё просмотрели
            }
            return false;
        }
        private bool PartIdle()
        {
            byte d = (byte)Configuration.random.Next(0, 7);
            for (int i = 0; i <= 7; i++)
            {
                if (d > 7) d -= 8;
                Cell c = QActGetCell(Location + Pos.GetDirPos((Direction)(d)));
                if (GetCanPlace(c.ID, c.LEnvir) && c.LAnimal == null)    //если можно пройти туда
                {
                    List<Pos> MTarg = new List<Pos>();
                    MTarg.Add(Location + Pos.GetDirPos((Direction)(d)) * 7);
                    DoAvAct(Rstate.GoGulyat, MTarg);
                    ActGo(new GoEventArgs()
                    {
                        CurPos = Location,
                        TargPos = Targ[0],   //не должно быть 0 элементов
                    });
                    return true;
                }
                d++;
            }
            return false;
        }
        /// <summary>
        /// Делать среднее по продолжительности действие
        /// </summary>
        private void DoAvAct(Rstate avstate, List<Pos> targact, bool near = false)
        {
            AvState = avstate;
            Targ = targact;
            Near = near;
        }
    }
}