using Forest_Game.Additional;
using SFML.Graphics;
using SFML.System;

namespace Forest_Game.WorldMap
{
    class Cell
    {
        #region MyRegion


        // private const int waterhigh = 80;
        public Sprite LSpriteUp, LSpriteDown;//LWater;
        public Vector2f GlCoord;
        public CellID ID = CellID.Zero;
        public bool isWater = false;
        /// <summary>  Рендерить ли низ </summary>
        public bool RendDown = true;
        /// <summary>  Высота спрайта ландшафта </summary>
        public ushort high;
        /// <summary>  Высота спрайта для отрисовки </summary>
        public ushort highdr;
        //public ushort musthighdr;
        /// <summary>  Высота спрайта для отрисовки при обрезании сверху</summary>
        public ushort downhighdr = 0;


        public readonly Pos Coord;


        public byte ShadMult = 0;// обычная яркость спрайта
        public byte bright = 255;//яркость спрайта под водой
        public byte brihtWater = 255;// яркость спрайта воды
        public byte ShadMultActor = 255;

        public int FallShad = 0;    //падающая тень, в данный момент не используется

        public Animal LAnimal;
        public Envir LEnvir;
        //Vector2f vectP;

        public void DrawCell(RenderWindow win, float DrowX, float DrowY)
        {

        }
        public void CopySprite(Sprite SprUp, Sprite SprDown)
        {
            LSpriteUp = SprUp;
            LSpriteDown = SprDown;
        }

        public Cell(Pos NewPos)
        {
            Coord = NewPos;
        }
    }

    #endregion

}