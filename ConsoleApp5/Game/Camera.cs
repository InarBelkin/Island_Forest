using Forest_Game.Additional;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Forest_Game
{
    class Camera
    {
        public Vector2f CamPos = new Vector2f(0, 0);
        private sbyte OneX, OneY;
        private bool PShift = false;

        public View ViewCam;
        private readonly RenderWindow win;

        public float Camtop;
        public float Camdown;
        public float Camleft;
        public float Camright;

        public Camera(RenderWindow wind)
        {
            win = wind;
            // System.Console.WriteLine(Configuration.VideoWith);
            ViewCam = new View(new FloatRect(0, 0, Configuration.VideoWith, Configuration.VideoHigh));
            ViewCam.Center = new Vector2f(10000, 0);
        }

        public void EventTick()
        {

            if (PShift)
            {
                ViewCam.Move(new Vector2f(OneX * 5000 * Configuration.Dtime, OneY * 5000 * Configuration.Dtime));
                //CamPos.X += OneX * 5000 * Additional.Configuration.Dtime;
                //CamPos.Y += OneY * 5000 * Additional.Configuration.Dtime;
            }
            else
            {
                ViewCam.Move(new Vector2f(OneX * 1000 * Configuration.Dtime, OneY * 1000 * Configuration.Dtime));

                //CamPos.X += OneX * 1000 * Additional.Configuration.Dtime;
                //CamPos.Y += OneY * 1000 * Additional.Configuration.Dtime;
            }

            win.SetView(ViewCam);

            Camtop = (ViewCam.Center.Y - (ViewCam.Size.Y) / 2);
            Camdown = (ViewCam.Center.Y + (ViewCam.Size.Y) / 2);
            Camleft = (ViewCam.Center.X - (ViewCam.Size.X) / 2);
            Camright = (ViewCam.Center.X + (ViewCam.Size.X) / 2);
        }
        public void KeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.W:
                    OneY--;
                    break;
                case Keyboard.Key.S:
                    OneY++;
                    break;
                case Keyboard.Key.A:
                    OneX--;
                    break;
                case Keyboard.Key.D:
                    OneX++;
                    break;
                case Keyboard.Key.LShift:
                    PShift = true;
                    break;
            }
        }
        public void KeyRelease(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.W:
                    OneY++;
                    break;
                case Keyboard.Key.S:
                    OneY--;
                    break;
                case Keyboard.Key.A:
                    OneX++;
                    break;
                case Keyboard.Key.D:
                    OneX--;
                    break;
                case Keyboard.Key.LShift:
                    PShift = false;
                    break;
            }

        }
        public void Resize(object sender, SizeEventArgs e)
        {
            Configuration.VideoHigh = e.Height;
            Configuration.VideoWith = e.Width;
            ViewCam.Size = new Vector2f(e.Width, e.Height);
            //Vector2f cent = ViewCam.Center;
            //ViewCam = new View(new FloatRect(0, 0, e.Width, e.Height));
            //ViewCam.Center = cent;
        }

    }
}
