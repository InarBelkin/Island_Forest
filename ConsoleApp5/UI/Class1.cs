using Forest_Game.Additional;
using Forest_Game.WorldMap;
using SFML.Graphics;
using SFML.Window;

namespace Forest_Game.UI
{
    partial class IngameUI
    {
        readonly InterCell[,] MasUI = new InterCell[4, 5];
        readonly Texture EnvText = new Texture("../Pictures/UI/Envirs.png");
        readonly Texture AnmText = new Texture("../Pictures/UI/Animals.png");
        readonly Texture Back = new Texture("../Pictures/UI/UIbackground.png");
        readonly Sprite BackSprite;
        private void InitialUICell()
        {
            for (byte i = 0; i < 4; i++)
            {
                for (byte j = 0; j < 5; j++)
                {
                    MasUI[i, j] = new InterCell(ActorID.Zero, null);
                }
            }
            MasUI[0, 0] = new InterCell(ActorID.Rabbit, new Sprite(AnmText, new IntRect(0, 0, 90, 40)));
            MasUI[0, 1] = new InterCell(ActorID.Wolf, new Sprite(AnmText, new IntRect(90, 0, 90, 40)));
            MasUI[1, 0] = new InterCell(ActorID.Chestnut, new Sprite(EnvText, new IntRect(0, 0, 90, 40)));
            MasUI[2, 0] = new InterCell(ActorID.Carrot, new Sprite(EnvText, new IntRect(90, 0, 90, 40)));
        }
        public void Win_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.R && cract != ActorID.Zero)
            {
                Pos CrPos;
                if (GetCell(out CrPos, out Cell _))
                {
                    CrActor(CrPos, cract);
                }

            }

        }



    }
    struct InterCell
    {
        public Sprite UICSprite;
        public ActorID ID;
        public InterCell(ActorID MID, Sprite Spr)
        {
            ID = MID;
            UICSprite = Spr;
        }
    }
}
