/*
 * "To the dreams of my childhood ~
 *                                  Farewell"
 *                                  
 * XenusVale presents:
 * 
 *      Final Fantasy X: Sin Reborn
 * 
 */

using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Seymour_Reborn_Installer
{
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

    class Seymour_Reborn_Install
    {
        #region Global Variable Declaration

        public static string ffxPath = @"C:\Program Files\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\data\FFX_Data.vbf";
        public static string modPath;
        public static byte[] originalHeaderLength = { 0xC8, 0x7A, 0x96, 0x00 }; //The 5-8 bytes of FFX_Data inclusive
        public static byte[] headerLength = { 0x08, 0x97, 0x96, 0x00 }; //The offset immediately following the last of the modded block data
        public static byte[] stringTableLength = { 0xC6, 0xF2, 0x51, 0x00 }; //The length of the combined string names plus the table bytes
        public static byte[] footer = //All header bytes converted to MD5 hash
            { 0xE4, 0x73, 0x62, 0x1D, 0xA4, 0x67, 0xA2, 0x92, 0x13, 0x6A, 0x42, 0x6B, 0xA5, 0x7A, 0x65, 0x4A };
        public static byte[] modFileNames = //The names I assigned to my dead end files, what these are have no significance in the code
            { 0x6D, 0x6F, 0x64, 0x76, 0x6F, 0x69, 0x63, 0x65, 0x62, 0x74, 0x6C, 0x2E, 0x66, 0x65, 0x76, 0x00,
              0x6D, 0x6F, 0x64, 0x76, 0x6F, 0x69, 0x63, 0x65, 0x62, 0x6E, 0x6B, 0x2E, 0x66, 0x73, 0x62, 0x00,
              0x6D, 0x6F, 0x64, 0x76, 0x6F, 0x69, 0x63, 0x65, 0x69, 0x6F, 0x70, 0x2E, 0x66, 0x65, 0x76, 0x00,
              0x6D, 0x6F, 0x64, 0x76, 0x6F, 0x69, 0x63, 0x65, 0x69, 0x62, 0x6B, 0x2E, 0x66, 0x73, 0x62, 0x00,
              0x6D, 0x6F, 0x64, 0x62, 0x61, 0x74, 0x74, 0x6C, 0x65, 0x73, 0x79, 0x2E, 0x64, 0x64, 0x73, 0x00,
              0x6D, 0x6F, 0x64, 0x66, 0x61, 0x63, 0x65, 0x5F, 0x70, 0x6C, 0x79, 0x2E, 0x64, 0x64, 0x73, 0x00,
              0x6D, 0x6F, 0x64, 0x73, 0x61, 0x76, 0x65, 0x6C, 0x6F, 0x61, 0x64, 0x2E, 0x73, 0x77, 0x66, 0x00,
              0x52, 0x65, 0x62, 0x6F, 0x72, 0x6E, 0x50, 0x72, 0x6A, 0x63, 0x74, 0x2E, 0x62, 0x69, 0x6E, 0x00 };
        public static byte[] deadEndFilePointers = 
            /*These bytes reference the original files and act as dead ends so that the original files may reference the Seymour Reborn Files
             *2x the first four bytes is the item's block data, the offset starts at the end of the string names
             *The next four bytes are irrelevant
             *The next eight bytes are the original size of the decompressed document
             *The next eight bytes are the file offset starting at the document's beginning
             *The final eight bytes are the string name offset, starting after the string table
            */
            { 0x14, 0x62, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xD6, 0x4B, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //ffx_us_voice_btl.fev
              0x6E, 0x3F, 0x85, 0x5C, 0x01, 0x00, 0x00, 0x00, 0x42, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in sound_pc > voice > us folder

              0x1B, 0x62, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xC0, 0xE1, 0xF4, 0x03, 0x00, 0x00, 0x00, 0x00, //ffx_us_voice_btl_bank00.fsb
              0x10, 0x16, 0x86, 0x5C, 0x01, 0x00, 0x00, 0x00, 0x52, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in sound_pc > voice > us folder

              0x10, 0x66, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xF0, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //ffx_us_voice_btl_iop.fev
              0xD3, 0x1E, 0xD1, 0x5F, 0x01, 0x00, 0x00, 0x00, 0x62, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in sound_pc > voice > us folder

              0x11, 0x66, 0x01, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x60, 0x30, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, //ffx_us_voice_btl_iop_bank00.fsb
              0x5D, 0x29, 0xD1, 0x5F, 0x01, 0x00, 0x00, 0x00, 0x72, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in sound_pc > voice > us folder

              0x62, 0xDA, 0x02, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x60, 0x0A, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, //battle.dds
              0xAF, 0x80, 0xC4, 0x3E, 0x02, 0x00, 0x00, 0x00, 0x82, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in menu_us folder

              0xE9, 0xE3, 0x02, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x60, 0x0A, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, //face_ply.dds
              0x3A, 0x03, 0x1E, 0x42, 0x02, 0x00, 0x00, 0x00, 0x92, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in menu > d3d11 folder

              0x2A, 0x60, 0x05, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0xDD, 0xCD, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00, //saveload_jp
              0x75, 0xB9, 0x78, 0x37, 0x03, 0x00, 0x00, 0x00, 0xA2, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, //Found in flash folder

              0x50, 0xF5, 0x07, 0x00, 0xF6, 0x13, 0xA8, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //RebornPrjct.bin
              0xC1, 0xE9, 0x04, 0xDF, 0x04, 0x00, 0x00, 0x00, 0xB2, 0xF2, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00  //The last file cannot function, this is a dud 
            };
        public static byte[] modOriginalFilePointers =
            /*These bytes reference the Seymour Reborn Files and are thus used to point original files to their modified counterpart
             *The string name offsets do not change
            */
            { 0x31, 0xE8, 0x07, 0x00, 0x31, 0xE8, 0x07, 0x00, 0xC8, 0xA5, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00,
              0xA0, 0xC0, 0xE9, 0xD1, 0x04, 0x00, 0x00, 0x00,

              0x38, 0xE8, 0x07, 0x00, 0x38, 0xE8, 0x07, 0x00, 0xE0, 0xFF, 0xAA, 0x0C, 0x00, 0x00, 0x00, 0x00,
              0x68, 0x66, 0xF0, 0xD1, 0x04, 0x00, 0x00, 0x00,

              0xE3, 0xF4, 0x07, 0x00, 0xE3, 0xF4, 0x07, 0x00, 0xFE, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x48, 0x66, 0x9B, 0xDE, 0x04, 0x00, 0x00, 0x00,

              0xE4, 0xF4, 0x07, 0x00, 0xE4, 0xF4, 0x07, 0x00, 0xC0, 0x52, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x46, 0xAC, 0x9B, 0xDE, 0x04, 0x00, 0x00, 0x00,

              0xFD, 0xF4, 0x07, 0x00, 0xFD, 0xF4, 0x07, 0x00, 0x5C, 0x0A, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x06, 0xFF, 0xB3, 0xDE, 0x04, 0x00, 0x00, 0x00,

              0x0E, 0xF5, 0x07, 0x00, 0x0E, 0xF5, 0x07, 0x00, 0x60, 0x0A, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
              0x62, 0x09, 0xC4, 0xDE, 0x04, 0x00, 0x00, 0x00,

              0x17, 0xF5, 0x07, 0x00, 0x17, 0xF5, 0x07, 0x00, 0xFF, 0xD5, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
              0xC2, 0x13, 0xCC, 0xDE, 0x04, 0x00, 0x00, 0x00 };

        #endregion

        public void Install()
        {
            if (Process.GetProcessesByName("FFX").Length > 0)
            {
                MessageBox.Show("Please close FFX, then retry."); //If FFX is open, Exit
                Environment.Exit(0);
            }

            CheckPath();

            var ffxStream = File.Open(ffxPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            ffxStream.Seek(4, SeekOrigin.Begin);
            byte[] protectVBF = new byte[4];
            ffxStream.Read(protectVBF, 0, 4);

            if (Enumerable.SequenceEqual(protectVBF, headerLength))
            {
                MessageBox.Show("Seymour Reborn is already installed.");
                FinishedInstallation();
            }
            else if (!Enumerable.SequenceEqual(protectVBF, originalHeaderLength))
            {
                MessageBox.Show("Foreign VBF injections detected, exiting.");
                Environment.Exit(0);
            }

            ffxStream.Seek(4, SeekOrigin.Begin);
            ffxStream.Write(headerLength, 0, 4); //Change the Header Length to include the new files
            ffxStream.WriteByte(0x33); //Change the number of files present in the document

            #region Modify Existing File Pointers

            byte[] offsetChanges = { 0xA6, 0x73, 0xE9, 0xD1, 0x04 };
            ffxStream.Seek(0x1192D0, SeekOrigin.Begin); //Change the File offsets for the first two File Pointers
            ffxStream.Write(offsetChanges, 0, offsetChanges.Length);

            offsetChanges[0] = 0xC6;
            ffxStream.Seek(27, SeekOrigin.Current);
            ffxStream.Write(offsetChanges, 0, offsetChanges.Length);

            ffxStream.Seek(0x158B, SeekOrigin.Current); //ffx_us_voice_btl.fev File Pointer offset is 0x11A880, Index (position in document) is 174
            ffxStream.Write(modOriginalFilePointers, 0, 24);

            ffxStream.Seek(8, SeekOrigin.Current); //ffx_us_voice_btl_bank00.fsb File Pointer offset is 0x11A8A0, Index is 175
            ffxStream.Write(modOriginalFilePointers, 24, 24);

            ffxStream.Seek(8, SeekOrigin.Current); //ffx_us_voice_btl_iop.fev File Pointer offset is 0x11A8C0, Index is 176
            ffxStream.Write(modOriginalFilePointers, 48, 24);

            ffxStream.Seek(8, SeekOrigin.Current); //ffx_us_voice_btl_iop_bank00.fsb File Pointer offset is 0x11A8E0, Index is 177
            ffxStream.Write(modOriginalFilePointers, 72, 24);

            ffxStream.Seek(0x27768, SeekOrigin.Current); //battle.dds File Pointer offset is 0x142060, Index is 5229
            ffxStream.Write(modOriginalFilePointers, 96, 24);

            ffxStream.Seek(0x1C68, SeekOrigin.Current); //face_ply.dds File Pointer offset is 0x143CE0, Index is 5457
            ffxStream.Write(modOriginalFilePointers, 120, 24);

            ffxStream.Seek(0x10E208, SeekOrigin.Current); //saveload_jp.swf File Pointer offset is 0x251F00, Index is 40034
            ffxStream.Write(modOriginalFilePointers, 144, 24);

            #endregion

            byte[] filePointers = new byte[2303328];
            ffxStream.Seek(0x1192C0, SeekOrigin.Begin);
            ffxStream.Read(filePointers, 0, 2303328); //Copy all the File Pointers

            ffxStream.Write(stringTableLength, 0, stringTableLength.Length);

            byte[] stringNames = new byte[5370438];
            ffxStream.Seek(-4, SeekOrigin.Current);
            ffxStream.Read(stringNames, 0, 5370438); //Copy all the String File Names

            byte[] blockData = new byte[1036386];
            ffxStream.Read(blockData, 0, 1036386); //Copy all the Block Data

            ffxStream.Seek(0x1192C0, SeekOrigin.Begin); //Write out the dead end file names in MD5 hash
            for (int i = 0; i < 8; i++)
            {
                byte[] individualName = new byte[15];
                Array.Copy(modFileNames, (16 * i), individualName, 0, 15);
                ffxStream.Write(MD5.Create().ComputeHash(individualName), 0, 16);
            }

            ffxStream.Write(filePointers, 0, filePointers.Length);
            ffxStream.Write(deadEndFilePointers, 0, deadEndFilePointers.Length);

            ffxStream.Write(stringNames, 0, stringNames.Length);
            ffxStream.Write(modFileNames, 0, modFileNames.Length);

            ffxStream.Write(blockData, 0, blockData.Length);

            var modStream = File.Open(modPath, FileMode.Open, FileAccess.Read, FileShare.None); //Read and write the 6720 bytes of mod block data
            byte[] modBlockData = new byte[6720];
            modStream.Read(modBlockData, 0, 6720);
            ffxStream.Write(modBlockData, 0, modBlockData.Length);

            ffxStream.Seek(-16, SeekOrigin.End); //Read and write the mod files
            byte[] modBytes = new byte[modStream.Length - 6720];
            modStream.Read(modBytes, 0, modBytes.Length);
            ffxStream.Write(modBytes, 0, modBytes.Length);

            ffxStream.Seek(0, SeekOrigin.End);
            ffxStream.Write(footer, 0, footer.Length);

            modStream.Close();
            ffxStream.Close();

            MessageBox.Show("Seymour Reborn was successfully installed.");
            FinishedInstallation();
        }

        public static void CheckPath()
        {
            if (!File.Exists(ffxPath)) //Check all drives for the normal FFX_Data path
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
            modPath = modPath.Remove(modPath.Length - 13);
            modPath += "Files.vale";

            if (!File.Exists(modPath)) //If the mod files are not in the same folder as the installer, prompt the user to find them
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

        public static void FinishedInstallation()
        {
            string seymourAppPath = System.Reflection.Assembly.GetEntryAssembly().Location; //If the app is in the same folder, start the app
            seymourAppPath = modPath.Remove(modPath.Length - 14);
            seymourAppPath += ".exe";

            try
            {
                Process.Start(seymourAppPath);
            }
            catch
            {
                Environment.Exit(0);
            }
        }
    }
}

/*
 * The Reborn Project
 */
