using Forest_Game.Additional;
using Forest_Game.WorldMap;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Forest_Game.UI
{
    partial class IngameUI
    {
        readonly Font MyFont;
        Text MyText, MyText2;
        Actor act = null;
        ActorID cract = ActorID.Zero;
        readonly RenderWindow win;
        const int btnh = 40, btnw = 90;
        public IngameUI(RenderWindow winn)
        {
            MyFont = new Font("../Fonts/arial.ttf");
            MyText = new Text("Createt and directed by \n Inar Belkin", MyFont);
            MyText2 = new Text("Координаты", MyFont);
            win = winn;
            BackSprite = new Sprite(Back) { Origin = new Vector2f(0, 160) };
            InitialUICell();
        }
        public event StaticClass.IGetCellMouse GetCell;
        public event StaticClass.IcrActor CrActor;
        public void Win_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Cell Ocell = null;

            switch (e.Button)
            {
                case Mouse.Button.Left:
                    Vector2i cdMse = Mouse.GetPosition(win);
                    if (cdMse.X > 0 && cdMse.X < btnw * 5 && cdMse.Y < Configuration.VideoHigh && cdMse.Y > Configuration.VideoHigh - btnh * 4)
                    {
                        int CurX = cdMse.X / btnw;
                        int CurY = (int)(cdMse.Y - Configuration.VideoHigh + btnh * 4) / btnh;
                        if (CurX >= 0 && CurX < 5 && CurY >= 0 && CurY < 4)
                        {
                            cract = MasUI[CurY, CurX].ID;
                            System.Console.WriteLine(cract);
                        }
                        Console.WriteLine($"CurX = {CurX} CurY = {CurY}");
                    }
                    else
                    {
                        if (GetCell(out Pos _, out Ocell))
                        {
                            act = Ocell.LAnimal;
                        }
                        else
                        {
                            //Console.WriteLine("Достало");
                            act = null;
                        }
                    }
                    break;
                case Mouse.Button.Right:
                    if (GetCell(out _, out Ocell))
                    {
                        act = Ocell.LEnvir;
                    }
                    break;
                case Mouse.Button.Middle:
                    act = null;
                    Pos A;
                    if (GetCell(out A, out Ocell))
                    {
                        MyText2 = new Text(A.ToString(), MyFont);
                    }
                    break;

            }

        }
        public void DelActor(Actor dact)
        {
            if (act == dact)
            {
                act = null;
            }
        }
        public void Draw(RenderWindow win, Camera Cam)
        {
            float Camtop = (Cam.ViewCam.Center.Y - (Cam.ViewCam.Size.Y) / 2);
            float Camdown = (Cam.ViewCam.Center.Y + (Cam.ViewCam.Size.Y) / 2);
            float Camleft = (Cam.ViewCam.Center.X - (Cam.ViewCam.Size.X) / 2);
            float Camright = (Cam.ViewCam.Center.X + (Cam.ViewCam.Size.X) / 2);

            if (act != null)
            {
                MyText = new Text(act.GetState(), MyFont);
            }
            else MyText = new Text("", MyFont);
            MyText.CharacterSize = 20;

            MyText.Position = new Vector2f(Camright - 200, Camdown - 200);
            win.Draw(MyText);
            MyText2.Position = new Vector2f(Camright - 400, Camdown - 200);
            win.Draw(MyText2);
            BackSprite.Position = new Vector2f(Camleft, Camdown);
            win.Draw(BackSprite);
            for (byte i = 0; i < 4; i++)
            {
                for (byte j = 0; j < 5; j++)
                {
                    if (MasUI[i, j].UICSprite != null)
                    {
                        MasUI[i, j].UICSprite.Position = new Vector2f(Camleft + 90 * j, Camdown - 160 + 40 * i);
                        win.Draw(MasUI[i, j].UICSprite);
                    }
                }
            }
        }
    }
}
