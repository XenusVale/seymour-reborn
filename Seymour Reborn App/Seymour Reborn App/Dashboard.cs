/*
 * "To the dreams of my childhood ~
 *                                  Farewell"
 *                                  
 * XenusVale presents:
 * 
 *      Final Fantasy X: Sin Reborn
 */

using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Seymour_Reborn_App
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        #region Global Variable Declaration

        public static int ffxEntryPointAddress = 0x00;
        public static bool gameHasBeenOn = false;
        public static bool sinRebornOpenAttempt = false;

        #endregion

        private void Dashboard_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.dashboardLocation;
            sinRebornCheckBox.Checked = Properties.Settings.Default.openSinReborn;
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) //Minimize to system tray
            {
                this.Hide();
            }
        }

        private void TrayNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Dashboard_FormClosing(object sender, EventArgs e)
        {
            if (gameHasBeenOn)
            {
                Seymour_Reborn seyClose = new Seymour_Reborn();
                seyClose.RemoveSeymour();
            }

            Properties.Settings.Default.dashboardLocation = this.Location;
            Properties.Settings.Default.Save();
        }

        private void SinRebornCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.openSinReborn = sinRebornCheckBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void RunMods_Tick(object sender, EventArgs e)
        {
            if ((Process.GetProcessesByName("FFX").Length > 0) && (ffxEntryPointAddress == 0x00))
            {
                gameHasBeenOn = true;
                Process[] ffxProcess = Process.GetProcessesByName("FFX");

                try
                {
                    foreach (ProcessModule ffxProcessModule in ffxProcess[0].Modules)
                    {
                        if (ffxProcessModule.ModuleName.Equals("FFX.exe"))
                        {
                            ffxEntryPointAddress = (int)(ffxProcessModule.BaseAddress);
                        }
                    }
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            else if ((Process.GetProcessesByName("FFX").Length <= 0) && (gameHasBeenOn))
            {
                Environment.Exit(0);
            }

            if (sinRebornCheckBox.Checked && (!sinRebornOpenAttempt))
            {
                string sinRebornPath = System.Reflection.Assembly.GetEntryAssembly().Location; //If Sin Reborn is in the same folder, try to open it
                sinRebornPath = sinRebornPath.Remove(sinRebornPath.Length - 18);
                sinRebornPath += "Sin Reborn.exe";
                try
                {
                    Process.Start(sinRebornPath);
                }
                catch
                {

                }

                sinRebornOpenAttempt = true;
            }

            if (gameHasBeenOn)
            {
                Seymour_Reborn sey = new Seymour_Reborn();
                sey.AddSeymour();
            }
        }
    }

    class Memory
    {
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hProcess);

        public static Process ffx = Process.GetProcessesByName("FFX").FirstOrDefault();

        public static uint Initialization()
        {
            uint delete = 0x00010000;
            uint read = 0x00020000;
            uint writeDAC = 0x00040000;
            uint writeOwner = 0x00080000;
            uint sync = 0x00100000;
            uint end = 0xFFF;
            uint fullControl = (delete | read | writeDAC | writeOwner | sync | end);

            return fullControl;
        }

        public static byte ReadByte(IntPtr address)
        {
            uint fullControl = Initialization();

            IntPtr authorization = (IntPtr)OpenProcess(fullControl, false, Memory.ffx.Id);

            byte[] bytesToBeReadBuffer = new byte[1];
            ReadProcessMemory(authorization, address, bytesToBeReadBuffer, 1, 0);
            byte byteToReturn = bytesToBeReadBuffer[0];

            CloseHandle(authorization);

            return byteToReturn;
        }

        public static int ReadInt32(IntPtr address)
        {
            uint fullControl = Initialization();

            IntPtr authorization = (IntPtr)OpenProcess(fullControl, false, Memory.ffx.Id);

            byte[] bytesToBeReadBuffer = new byte[4];
            ReadProcessMemory(authorization, address, bytesToBeReadBuffer, bytesToBeReadBuffer.Length, 0);

            int valueToBeReturned = BitConverter.ToInt32(bytesToBeReadBuffer, 0);

            CloseHandle(authorization);

            return valueToBeReturned;
        }

        public static void WriteByte(IntPtr address, byte byteToBeWritten)
        {
            uint fullControl = Initialization();

            IntPtr authorization = (IntPtr)OpenProcess(fullControl, false, Memory.ffx.Id);

            byte[] bytesToBeWritten = new byte[1];
            bytesToBeWritten[0] = byteToBeWritten;
            WriteProcessMemory(authorization, address, bytesToBeWritten, bytesToBeWritten.Length, 0);

            CloseHandle(authorization);
        }

        public static void WriteByteArray(IntPtr address, byte[] bytesToBeWritten)
        {
            uint fullControl = Initialization();

            IntPtr authorization = (IntPtr)OpenProcess(fullControl, false, Memory.ffx.Id);

            WriteProcessMemory(authorization, address, bytesToBeWritten, bytesToBeWritten.Length, 0);

            CloseHandle(authorization);
        }
    }

    public class Seymour_Reborn
    {
        public void AddSeymour()
        {
            int menuIsOpen = Memory.ReadByte((IntPtr)(Dashboard.ffxEntryPointAddress + 0x8CB9D0));
            int apMenuisOpen = Memory.ReadByte((IntPtr)(Dashboard.ffxEntryPointAddress + 0xF407E4));
            int combatIdentifier = Memory.ReadInt32((IntPtr)(Dashboard.ffxEntryPointAddress + 0xD34460));

            Memory.WriteByte((IntPtr)(Dashboard.ffxEntryPointAddress + 0x842394), 0x08); //Switches Yuna's model for Seymour's while in combat

            byte[] yunaName = { 0x68, 0x84, 0x7D, 0x70, 0x00 };
            byte[] seymourName = { 0x62, 0x74, 0x88, 0x7C, 0x7E, 0x84, 0x81, 0x00 };

            if ((menuIsOpen == 1) || (combatIdentifier > 0) || (apMenuisOpen == 1))
            {
                Memory.WriteByteArray((IntPtr)(Dashboard.ffxEntryPointAddress + 0xD32DF0), seymourName);
            }
            else
            {
                Memory.WriteByteArray((IntPtr)(Dashboard.ffxEntryPointAddress + 0xD32DF0), yunaName);
            }
        }

        public void RemoveSeymour()
        {
            /*If a user abruptly closes the app while Seymour's data has been written, revert it to Yuna's data
             *The data is reset with every relaunch of FFX, but this is a fix for the current instance
             */
            byte[] yunaName = { 0x68, 0x84, 0x7D, 0x70, 0x00 };

            Memory.WriteByte((IntPtr)(Dashboard.ffxEntryPointAddress + 0x842394), 0x02);
            Memory.WriteByteArray((IntPtr)(Dashboard.ffxEntryPointAddress + 0xD32DF0), yunaName);
        }
    }
}

/*
 * The Reborn Project
 */
