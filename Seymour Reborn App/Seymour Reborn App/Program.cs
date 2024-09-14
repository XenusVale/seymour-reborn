using System;
using System.Threading;
using System.Windows.Forms;

namespace Seymour_Reborn_App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Mutex seymourRebornMutex = new Mutex(false, "seymourRebornMutex"))
            {
                if (!seymourRebornMutex.WaitOne(0, false))
                {
                    return;
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Dashboard());
                }
            }
        }
    }
}
