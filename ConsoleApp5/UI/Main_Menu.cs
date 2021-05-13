using System;
using System.Windows.Forms;

namespace Forest_Game.UI
{
    public partial class Main_Menu : Form
    {
        public Main_Menu()
        {
            InitializeComponent();
        }

        private void GameStart(object Sender, EventArgs Args)
        {
            Visible = false;
            Game MGame = new Game();
            MGame.Start();
        }

        private void CloseGame(object Sender, EventArgs Args) => Application.Exit();
    }
}
