using System;
using System.Threading;

namespace Seymour_Reborn_Installer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Mutex seymourRebornChangeMutex = new Mutex(false, "seymourRebornChange")) //Mutex is used to generate a unique ID to prevent multiple instances
            {                                                                                //of the same application. This ID is intentionally identical to the
                if (!seymourRebornChangeMutex.WaitOne(0, false))                             //Uninstaller's
                {
                    return;
                }
                else
                {
                    Seymour_Reborn_Install sey = new Seymour_Reborn_Install();
                    sey.Install();
                }
            }
        }
    }
}
