/*
 * "To the dreams of my childhood ~
 *                                  Farewell"
 *                                  
 * XenusVale presents:
 * 
 *      Final Fantasy X: Seymour Reborn
 */

using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace Seymour_Reborn_Uninstaller
{
    //Much of the documentation for this program can be found in the installer, as this is simply the reverse of that more complicated program

    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
    }

    class Seymour_Reborn_Uninstall
    {
        #region Global Variable Declaration

        public static string ffxPath = @"C:\Program Files\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\data\FFX_Data.vbf";
        public static string modPath;
        public static byte[] originalHeaderLength = { 0xC8, 0x7A, 0x96, 0x00 };
        public static byte[] headerLength = { 0x08, 0x97, 0x96, 0x00 };
        public static byte[] stringTableLength = { 0x46, 0xF2, 0x51, 0x00 };
        public static byte[] footer =
            //The last 16 bytes of the original FFX_Data file
            { 0xAC, 0xD1, 0x8D, 0xEC, 0x9F, 0x39, 0x50, 0xA5, 0xC2, 0xA0, 0x26, 0x94, 0xAE, 0x63, 0xF8, 0xBD };
        public static byte[] revertOriginalFilePointers =
            { 0x14, 0x62, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xD6, 0x4B, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x6E, 0x3F, 0x85, 0x5C, 0x01, 0x00, 0x00, 0x00,

              0x1B, 0x62, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xC0, 0xE1, 0xF4, 0x03, 0x00, 0x00, 0x00, 0x00,
              0x10, 0x16, 0x86, 0x5C, 0x01, 0x00, 0x00, 0x00,

              0x10, 0x66, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xF0, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
              0xD3, 0x1E, 0xD1, 0x5F, 0x01, 0x00, 0x00, 0x00,

              0x11, 0x66, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x60, 0x30, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x5D, 0x29, 0xD1, 0x5F, 0x01, 0x00, 0x00, 0x00,

              0x62, 0xDA, 0x02, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x60, 0x0A, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
              0xAF, 0x80, 0xC4, 0x3E, 0x02, 0x00, 0x00, 0x00,

              0xE9, 0xE3, 0x02, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x60, 0x0A, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x3A, 0x03, 0x1E, 0x42, 0x02, 0x00, 0x00, 0x00,

              0x2A, 0x60, 0x05, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xDD, 0xCD, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x75, 0xB9, 0x78, 0x37, 0x03, 0x00, 0x00, 0x00 };

        #endregion

        public void Uninstall()
        {
            if (Process.GetProcessesByName("FFX").Length > 0)
            {
                MessageBox.Show("Please close FFX, then retry.");
                Environment.Exit(0);
            }

            CheckPath();

            var ffxStream = File.Open(ffxPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            ffxStream.Seek(4, SeekOrigin.Begin);
            byte[] protectVBF = new byte[4];
            ffxStream.Read(protectVBF, 0, 4);

            if (Enumerable.SequenceEqual(protectVBF, originalHeaderLength))
            {
                MessageBox.Show("Seymour Reborn is already unistalled.");
                Environment.Exit(0);
            }
            else if (!Enumerable.SequenceEqual(protectVBF, headerLength))
            {
                MessageBox.Show("Foreign VBF injections detected: to restore your game, preform a reinstallation via Steam.");
                Environment.Exit(0);
            }

            ffxStream.Seek(4, SeekOrigin.Begin);
            ffxStream.Write(originalHeaderLength, 0, 4); //Change the Header Length to remove the new files
            ffxStream.WriteByte(0x2B); //Revert the number of files present in the document

            #region Modify Existing File Pointers

            byte[] offsetChanges = { 0xC8, 0x7A, 0x96, 0x00, 0x00 };
            ffxStream.Seek(0x119350, SeekOrigin.Begin); //Revert the file offsets for the first two File Pointers
            ffxStream.Write(offsetChanges, 0, offsetChanges.Length);

            offsetChanges[0] = 0xE8;
            ffxStream.Seek(27, SeekOrigin.Current);
            ffxStream.Write(offsetChanges, 0, offsetChanges.Length);

            ffxStream.Seek(0x158B, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 0, 24);

            ffxStream.Seek(8, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 24, 24);

            ffxStream.Seek(8, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 48, 24);

            ffxStream.Seek(8, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 72, 24);

            ffxStream.Seek(0x27768, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 96, 24);

            ffxStream.Seek(0x1C68, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 120, 24);

            ffxStream.Seek(0x10E208, SeekOrigin.Current);
            ffxStream.Write(revertOriginalFilePointers, 144, 24);

            #endregion

            byte[] filePointers = new byte[2303328];
            ffxStream.Seek(0x119340, SeekOrigin.Begin);
            ffxStream.Read(filePointers, 0, 2303328); //Copy all the original File Pointers

            ffxStream.Seek(0x100, SeekOrigin.Current);
            ffxStream.Write(stringTableLength, 0, stringTableLength.Length);

            byte[] stringNames = new byte[5370438];
            ffxStream.Seek(-4, SeekOrigin.Current);
            ffxStream.Read(stringNames, 0, 5370438); //Copy all the original String File Names

            ffxStream.Seek(0x80, SeekOrigin.Current);
            byte[] blockData = new byte[1036386];
            ffxStream.Read(blockData, 0, 1036386); //Copy all the original Block Data

            ffxStream.Seek(0x1192C0, SeekOrigin.Begin);
            ffxStream.Write(filePointers, 0, filePointers.Length);

            ffxStream.Write(stringNames, 0, stringNames.Length);

            ffxStream.Write(blockData, 0, blockData.Length);

            var modStream = File.Open(modPath, FileMode.Open, FileAccess.Read, FileShare.None);
            modStream.Seek(0x1A40, SeekOrigin.Begin);
            byte[] firstTwoFiles = new byte[19706];
            modStream.Read(firstTwoFiles, 0, 19706);
            ffxStream.Write(firstTwoFiles, 0, firstTwoFiles.Length);

            ffxStream.SetLength(0x4D1E973B6); //The original FFX_Data document length

            ffxStream.Seek(-16, SeekOrigin.End);
            ffxStream.Write(footer, 0, footer.Length);

            modStream.Close();
            ffxStream.Close();

            MessageBox.Show("Seymour Reborn was successfully uninstalled.");
        }

        public static void CheckPath()
        {
            if (!File.Exists(ffxPath))
            {
                ffxPath = @"D:\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\data\FFX_Data.vbf";
            }
            if (!File.Exists(ffxPath))
            {
                ffxPath = @"E:\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\data\FFX_Data.vbf";
            }
            if (!File.Exists(ffxPath))
            {
                ffxPath = @"F:\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\data\FFX_Data.vbf";
            }
            if (!File.Exists(ffxPath))
            {
                MessageBox.Show("FFX VBF cannot be located, please select your FFX_Data.vbf file.\n" +
                                "The normal path is:\n" +
                               @"C:\Program Files\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\data\FFX_Data.vbf");

                ffxPath = null;
                while (ffxPath == null)
                {
                    using (OpenFileDialog openFFXVBF = new OpenFileDialog())
                    {
                        openFFXVBF.Title = "Locate: FFX_Data.vbf";
                        openFFXVBF.Filter = "vbf files (*.vbf)|*.vbf";
                        openFFXVBF.InitialDirectory = @"C:\";

                        if (openFFXVBF.ShowDialog() == DialogResult.Cancel)
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            if (openFFXVBF.FileName.EndsWith("Data.vbf"))
                            {
                                ffxPath = openFFXVBF.FileName;
                            }
                            else
                            {
                                MessageBox.Show("You have selected the wrong file.");
                            }
                        }
                    }
                }
            }

            modPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            modPath = modPath.Remove(modPath.Length - 15);
            modPath += "Files.vale";

            if (!File.Exists(modPath))
            {
                MessageBox.Show("Seymour Reborn Files cannot be located, please select:\nSeymour Reborn Files.vale");

                modPath = null;
                while (modPath == null)
                {
                    using (OpenFileDialog openSRF = new OpenFileDialog())
                    {
                        openSRF.Title = "Locate: Seymour Reborn Files.vale";
                        openSRF.Filter = "vale files (*.vale)|*.vale";
                        openSRF.InitialDirectory = @"Desktop";

                        if (openSRF.ShowDialog() == DialogResult.Cancel)
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            if (openSRF.FileName.EndsWith("Files.vale"))
                            {
                                modPath = openSRF.FileName;
                            }
                            else
                            {
                                MessageBox.Show("You have selected the wrong file.");
                            }
                        }
                    }
                }
            }
        }
    }
}

/*
 * The Reborn Project
 */
