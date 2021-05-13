using Forest_Game.Additional;
using SFML.Window;

namespace Forest_Game
{
    partial class Game
    {
        private void EvCreateActor(object sender, KeyEventArgs e) //Заменяемая в будущем штука, вызывающая осздание
        {
            ActorID ID = 0;
            Pos MSelect;

            switch (e.Code)// свич надо менять на что-то другое, поэтому он отдельно от следующего
            {
                case Keyboard.Key.Num1:
                    ID = ActorID.Chestnut;
                    break;
                case Keyboard.Key.Num9:
                    ID = ActorID.Carrot;
                    break;
                case Keyboard.Key.Num5:
                    ID = ActorID.Rabbit;
                    break;
                case Keyboard.Key.Num6:
                    ID = ActorID.Wolf;
                    break;
                case Keyboard.Key.Num7:
                    ID = ActorID.Deer;
                    break;
                default:
                    return;
            }
            if (ID != 0 && MyMap.GetMouseCelPos2(win, MyCam, out MSelect, out _))
            {
                CreateActor(MSelect, ID);
            }

        }

        private void EvDelActor(object sender, KeyEventArgs e)//Заменяемая в будущем штука, вызывающая удаление
        {
            bool del = false, delAnim = false;
            switch (e.Code)
            {
                case Keyboard.Key.C:
                    del = true;
                    break;
                case Keyboard.Key.V:
                    del = true;
                    delAnim = true;
                    break;
            }
            if (del)
            {
                Pos Mselect;
                if (MyMap.GetMouseCelPos2(win, MyCam, out Mselect, out _))
                {
                    if (delAnim)
                    {

                        DeleteAnim(MyMap.MCell[Mselect.X, Mselect.Y].LAnimal);
                    }
                    else
                    {
                        DeleteEnvir(MyMap.MCell[Mselect.X, Mselect.Y].LEnvir);
                    }
                }
            }
        }

        private bool CreateActor(Pos EnvPos, ActorID ID)
        {
            if (MyMap.MCell[EnvPos.X, EnvPos.Y].LEnvir == null)
            {

                Envir env = null;
                switch (ID)
                {
                    case ActorID.Chestnut:
                        env = new Envirs.Chestnut(EnvPos, MyMap.MCell[EnvPos.X, EnvPos.Y].GlCoord);
                        break;
                    case ActorID.Carrot:
                        env = new Envirs.Carrot(EnvPos, MyMap.MCell[EnvPos.X, EnvPos.Y].GlCoord);
                        break;
                }
                if (env != null)
                {
                    if (env.GetCanPlace(MyMap.MCell[EnvPos.X, EnvPos.Y].ID, MyMap.MCell[EnvPos.X, EnvPos.Y].LAnimal))
                    {
                        Envirs.Add(env);
                        MyMap.AddEnvir(env, EnvPos);
                        env.IGetCell += MyMap.GetCellP;
                        env.IDeath += Delete;
                        return true;
                    }
                    else
                    {
                        env.Dispose();
                    }
                }

            }
            if (MyMap.MCell[EnvPos.X, EnvPos.Y].LAnimal == null)
            {
                Animal anim = null;
                switch (ID)
                {
                    case ActorID.Rabbit:
                        anim = new Animals.Rabbit(EnvPos, MyMap.MCell[EnvPos.X, EnvPos.Y].GlCoord);
                        break;
                    case ActorID.Wolf:
                        anim = new Animals.Wolf(EnvPos, MyMap.MCell[EnvPos.X, EnvPos.Y].GlCoord);
                        break;
                    case ActorID.Deer:
                        anim = new Animals.Deer(EnvPos, MyMap.MCell[EnvPos.X, EnvPos.Y].GlCoord);
                        break;
                }
                if (anim != null)
                {

                    if (anim.GetCanPlace(MyMap.MCell[EnvPos.X, EnvPos.Y].ID, MyMap.MCell[EnvPos.X, EnvPos.Y].LEnvir))
                    {
                        Animals.Add(anim);
                        MyMap.AddAnim(anim, EnvPos);
                        anim.IGoAct += MyMap.GoAnimal;
                        anim.ILookAct += MyMap.LookAnimal;
                        anim.ISearchWay += MyMap.SearchWayAnimal;
                        anim.IlookGlCoord += MyMap.GetGlobalCoord;
                        anim.IGetCell += MyMap.GetCellP;
                        anim.IDeath += Delete;
                        return true;
                    }
                    else
                    {
                        anim.Dispose();
                        return false;
                    }
                }

            }

            return false;


        }

        private void Delete(object sender, DeathEventArgs e)
        {
            if (sender is Animal) DeleteAnim(sender as Animal);
            else if (sender is Envir) DeleteEnvir(sender as Envir);
        }
        private bool DeleteEnvir(Envir env)
        {
            if (env != null)
            {
                if (!Envirs.Delete(env))
                {
                    System.Console.WriteLine("Не найден Envir в массиве");
                    return false;
                }
                if (!MyMap.DeleteEnvir(env))
                {
                    System.Console.WriteLine("Не найден Envir в карте!");
                    return false;
                }
                MyUI.DelActor(env);
                env.Dispose();
            }
            return true;
        }
        private bool DeleteAnim(Animal anm)
        {
            if (anm != null)
            {
                if (!Animals.Delete(anm))
                {
                    System.Console.WriteLine("Не найден anim в массиве");
                    return false;
                }
                if (!MyMap.DeleteAnim(anm))
                {
                    System.Console.WriteLine("Не найден Anim в карте");
                    return false;
                }

                MyUI.DelActor(anm);
                anm.Dispose();
            }
            return true;
        }

    }
}
