using Forest_Game.Additional;
using Forest_Game.UI;
using Forest_Game.WorldMap;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
namespace Forest_Game
{
    partial class Game
    {
        public RenderWindow win;

        private readonly Vector<Animal> Animals;
        private readonly Vector<Envir> Envirs;
        private WorldMap.Map MyMap;

        public static event StaticClass.Self EvTick = delegate { };

        readonly Clock Mclock;

        readonly Camera MyCam;
        readonly IngameUI MyUI;
        public void Start()
        {
            Configuration.MapX = 512;
            Configuration.MapY = 512;
            Configuration.cam = MyCam;

            MyMap = new Map();

            win.Resized += Win_Resized;
            win.Resized += MyCam.Resize;
            win.KeyPressed += Win_KeyPressed;
            win.KeyReleased += Win_KeyReleased;
            win.KeyPressed += MyCam.KeyPress;
            win.KeyReleased += MyCam.KeyRelease;
            win.KeyPressed += EvCreateActor;
            win.KeyPressed += EvDelActor;
            win.KeyPressed += MyUI.Win_KeyPressed;
            MyUI.GetCell += MyUI_GetCell;
            MyUI.CrActor += CreateActor;
            win.MouseButtonPressed += MyUI.Win_MouseButtonPressed;

            while (win.IsOpen)
            {

                // Console.WriteLine(1 / Mclock.ElapsedTime.AsSeconds());

                Configuration.Dtime = Mclock.ElapsedTime.AsSeconds();
                Mclock.Restart();
                win.DispatchEvents();
                win.Clear();


                MyCam.EventTick();

                EvTick(Configuration.Dtime);

                // MyMap.Draw(win, MyCam.CamPos.X, MyCam.CamPos.Y);
                MyMap.Draw2(win, MyCam);

                MyUI.Draw(win, MyCam);
                win.Display();
            }
        }

        private bool MyUI_GetCell(out Pos OPos, out Cell OCell)
        {
            MyMap.GetMouseCelPos2(win, MyCam, out OPos, out OCell);
            return (OCell == null) ? false : true;

        }

        private void Win_KeyReleased(object sender, KeyEventArgs e)
        {
        }

        private void Win_KeyPressed(object sender, KeyEventArgs e)
        {

            if (e.Code == Keyboard.Key.M)
            {
                Console.WriteLine(win.MapPixelToCoords(Mouse.GetPosition(win)));
            }
            else if (e.Code == Keyboard.Key.N)
            {

                //   Console.WriteLine(MyCam.ViewCam.Size = new );
            }
        }

        private void Win_Resized(object sender, SizeEventArgs e)
        {

            Configuration.VideoHigh = e.Height;
            Configuration.VideoWith = e.Width;
            win.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));

        }
        public Game()
        {

            win = new RenderWindow(new VideoMode(Configuration.VideoWith, Additional.Configuration.VideoHigh), "TupayaIgra");
            MyUI = new IngameUI(win);
            MyCam = new Camera(win);
            //MyCam.ViewCam = new View(new FloatRect(0, 0, Configuration.VideoWith, Configuration.VideoHigh));
            //win.SetFramerateLimit(300);
            win.SetKeyRepeatEnabled(false);
            //win.SetVerticalSyncEnabled(true);
            Mclock = new Clock();

            Animals = new Vector<Animal>();
            Envirs = new Vector<Envir>();
        }

    }
}
