using SFML.System;

namespace Forest_Game.Actors
{   //Классы, которые хранятся на протяжении одного действия
    //и нужны, чтобы middle и end части действия получали информацию
    abstract class ActionArg
    {
    }
    class ClActEat : ActionArg
    {
        public Pos EatPos;
    }
    class ClActMove : ActionArg
    {
        public Vector2f CCoord, TCoord;
    }

}
