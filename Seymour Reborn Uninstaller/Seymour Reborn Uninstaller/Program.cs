using System;
using System.Threading;

namespace Seymour_Reborn_Uninstaller
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Mutex seymourRebornChangeMutex = new Mutex(false, "seymourRebornChange"))
            {
                if (!seymourRebornChangeMutex.WaitOne(0, false))
                {
                    return;
                }
                else
                {
                    Seymour_Reborn_Uninstall seyu = new Seymour_Reborn_Uninstall();
                    seyu.Uninstall();
                }
            }
        }
    }
}
