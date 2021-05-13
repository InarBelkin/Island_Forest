using System.IO;
using System.Windows.Forms;

namespace Forest_Game
{
    class Program
    {
        [System.STAThread]
        static void Main()
        {
            string DllCheck = Application.StartupPath + "/csfml-audio-2.dll";

            if (!File.Exists(DllCheck))
            {
                File.WriteAllBytes(DllCheck, Properties.Resources.csfml_audio_2);
            }

            DllCheck = Application.StartupPath + "/csfml-graphics-2.dll";

            if (!File.Exists(DllCheck))
            {
                File.WriteAllBytes(DllCheck, Properties.Resources.csfml_graphics_2);
            }

            DllCheck = Application.StartupPath + "/csfml-system-2.dll";

            if (!File.Exists(DllCheck))
            {
                File.WriteAllBytes(DllCheck, Properties.Resources.csfml_system_2);
            }

            DllCheck = Application.StartupPath + "/csfml-window-2.dll";

            if (!File.Exists(DllCheck))
            {
                File.WriteAllBytes(DllCheck, Properties.Resources.csfml_window_2);
            }

            DllCheck = Application.StartupPath + "/openal32.dll";

            if (!File.Exists(DllCheck))
            {
                File.WriteAllBytes(DllCheck, Properties.Resources.openal32);
            }

            DllCheck = Application.StartupPath + "/OpenTK.dll";

            if (!File.Exists(DllCheck))
            {
                File.WriteAllBytes(DllCheck, Properties.Resources.OpenTK);
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI.Main_Menu());
        }
    }
}
