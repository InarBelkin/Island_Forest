using Forest_Game.Additional;
using Forest_Game.Additional.Interfaces;
using Forest_Game.WorldMap;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forest_Game.Animals
{
    sealed partial class Wolf : Animal, ICanEat, ICanAttack
    {
        WolfState AvState = WolfState.Zero;
        private bool Near = false;
        public float Damage => 6;
        private byte CntThink = 0;
        private const byte MaxCntTh = 3;
        public Wolf(Pos Envpos, Vector2f GlCoordinate) : base(Envpos, GlCoordinate)
        {
            ID = ActorID.Wolf;
            ASprite = SpriteCollection.SWolf;
            highdr = 5;
            Anim.with = 200; Anim.top = 0; Anim.heigh = 200; Anim.AnimTime = 0;
            Speed = 0.5f;
            RadSee = (10, 10);
            Hunger = new StrHunger(10);
            Hunger.Hung = 5;
        }

        public override void EvTick(float DTime)
        {
            TickAnim(DTime);
            ProcessAct(Configuration.Dtime);
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

        private void ThinkNew(bool IsAnimPr = false)
        {
            LookEventArgs Mlook = new LookEventArgs();
            SearchWayEventArgs Way = new SearchWayEventArgs() { IsAnimPr = IsAnimPr };
            if (PartEat(Mlook, Way)) return;
            if (PartAttack(Mlook, Way)) return;
            if (PartIdle()) return;
            if (State == AnimState.Zero)
            {
                DoAvAct(WolfState.Disarray, new List<Pos>());
                ActRest(2);
            }

        }
        private bool ThinkMed()
        {
            LookEventArgs Mlook = new LookEventArgs();
            SearchWayEventArgs Way = new SearchWayEventArgs();
            if (PartEat(Mlook, Way)) return false;

            return true;
        }
        private bool PartEat(LookEventArgs Mlook, SearchWayEventArgs Way)
        {
            if (Hunger.Hung > Hunger.MaxHung * 0.8) return false;
            Mlook.MustLkAnm = true;
            Mlook.radius = RadSee.Item2;
            Mlook.Queier = QuerEat;
            QActLook(Mlook);
            float min; uint index;
            for (int iter = 0; iter < 3; iter++)
            {
                index = 0;
                min = float.MaxValue; //Раскидываем по приоритету.
                for (ushort i = 0; i < Mlook.Animals.Lenght; i++)  //проверяем нужное окружение
                {
                    if (Mlook.Asort[i].Item1 < min && Mlook.Asort[i].Item2 == false)//добавлять доп.условие что это еда
                    {
                        index = i;
                        min = Mlook.Asort[i].Item1;
                    }
                }
                if (min != float.MaxValue)
                {
                    Mlook.Asort[index].Item2 = true;
                    if (Pos.PosNear(Location, Mlook.Animals[index].Location))
                    {
                        if ((Mlook.Animals[index] as IEatable).FoodRec > 0)
                        {
                            ActEat(Mlook.Animals[index].Location);
                            DoAvAct(WolfState.GoEat, new List<Pos>());
                            return true;
                        }
                    }
                    Way.TPos = (Mlook.Animals[index]).Location;
                    Way.StayNear = true;
                    QActSearchWay(Way);

                    if (Way.CanGo == true)
                    {
                        DoAvAct(WolfState.GoEat, Way.MGoPos, true);
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
                else return false;
            }
            return false;
        }
        private bool PartAttack(LookEventArgs Mlook, SearchWayEventArgs Way)
        {
            Mlook.MustLkAnm = true;
            Mlook.radius = RadSee.Item2;
            Mlook.Queier = QuerAttack;
            QActLook(Mlook);
            float min; uint index;
            for (int iter = 0; iter < 5; iter++)
            {
                index = 0;
                min = float.MaxValue; //Раскидываем по приоритету.
                for (ushort i = 0; i < Mlook.Animals.Lenght; i++)  //проверяем все окружающие Envir
                {
                    if (Mlook.Asort[i].Item1 < min && Mlook.Asort[i].Item2 == false)//добавлять доп.условие что это еда
                    {
                        index = i;
                        min = Mlook.Asort[i].Item1;
                    }
                }
                if (min != float.MaxValue)
                {
                    Mlook.Asort[index].Item2 = true;
                    if (Pos.PosNear(Location, Mlook.Animals[index].Location))
                    {
                        ActAttack(Mlook.Animals[index]);
                        DoAvAct(WolfState.GoAttack, new List<Pos>());
                        return true;
                    }
                    Way.TPos = (Mlook.Animals[index]).Location;
                    Way.StayNear = true;
                    QActSearchWay(Way);
                    if (Way.CanGo == true)
                    {
                        DoAvAct(WolfState.GoAttack, new List<Pos>());
                        ActGo(new GoEventArgs()
                        {
                            CurPos = Location,
                            TargPos = Way.MGoPos.Last(),  //не должно быть 0 элементов
                        });
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
                    DoAvAct(WolfState.GoGulyat, MTarg);
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

        private bool ActAttack(Actor TargAct)
        {
            Anim.SetRot(Location, TargAct.Location);
            Anim.SetPlLine(2);
            SetAnim(AnimState.OstAtk);
            TimeAct = 1;
            State = AnimState.OstAtk;
            if (TargAct is IAttackable t)
            {
                t.TakeAttack(Damage, this);
            }

            return false;
        }

        //кушатб хочешь?
        private void DoAvAct(WolfState avstate, List<Pos> targact, bool near = false)
        {
            AvState = avstate;
            Targ = targact;
            Near = near;
        }

    }
}