using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base64Converter
{
    static class DirectoryBasics
    {       
        static string currentDirectory = null;        
        static List<Folder> folders;
        public static bool directorySelected = false;
        public static string fileString = null;

        public static string[] chineseFolder = new string[] { "刺客女", "刺客男", "剑灵女", "剑灵男", "夜影女", "夜影男", "妖精", "妖兽男", "巫师女", "巫师男", "月仙女", "月仙男", "武侠女", "武侠男", "法师女", "法师男", "羽灵女", "羽灵男", "羽芒女", "羽芒男", "魅灵女", "魅灵男" };
        public static string[] classFolder = new string[] { "Assassin [F]", "Assassin [M]","Seeker [F]", "Seeker [M]", "Duskblade [F]", "Duskblade [M]", "Venomancer", "Barbarian", "Psychic [F]", "Psychic [M]", "Stormbringer [F]", "Stormbringer [M]", "Warrior [F]", "Warrior [M]", "Mage [F]", "Mage [M]", "Cleric [F]", "Cleric [M]", "Archer [F]", "Archer [M]", "Mystic [F]", "Mystic [M]" };

        public static bool SetBaseDirectory()
        {
            using(var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    currentDirectory = dialog.SelectedPath;
                    SetFoldersList(currentDirectory);
                    return true;
                }
                else if(result == DialogResult.Cancel)
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("Failed to load the path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                     
            }
        }
        public static string GetBaseDirectory()
        {
            return currentDirectory;
        }

        public static void SetFoldersList(string currentDirectory)
        {            
            string[] directories = Directory.GetDirectories(currentDirectory);          
            folders = new List<Folder>();
            for (int x = 0; x < directories.Length; x++)
            {
                string folderName = directories[x].Remove(0, currentDirectory.Length + 1);
                if (DirectoryBasics.chineseFolder.Contains(folderName))
                {
                    Folder tempFolder = new Folder();
                    tempFolder.SetDirectory(directories[x]);
                    //folders[x] = new Folder();
                    //folders[x].SetDirectory(directories[x]);
                    for (int y = 0; y < chineseFolder.Length; y++)
                    {
                        string fName = chineseFolder[y];
                        if (folderName == fName)
                        {
                            tempFolder.SetName(folderName);
                            tempFolder.SetTranslatedName(classFolder[y]);
                            folders.Add(tempFolder);
                            break;
                        }
                    }
                }
              
            }
            for (int x = 0; x < folders.Count; x++)
            {
                string diretorio = folders[x].GetDirectory() + @"\躯干";
                if (Directory.Exists(diretorio))
                {
                    folders[x].SetFilesList(Directory.GetFiles(diretorio));
                }                    
            }          
                
        }        
        public static List<Folder> GetFoldersList()
        {
            return folders;
        }

        public static void WriteTempLua(string decodedBase64,string directory)
        {
            fileString = null;
            fileString += @"ChangeEquipScript = {}";
            fileString += "\n";
            fileString += @"function ChangeEquipScript:Init()";
            fileString += "\n";
            fileString += decodedBase64;
            fileString += "\n";           
            fileString += "end";         
            
            /*string luaDirectory = directory + @"\temp.lua";
            if (File.Exists(luaDirectory))
            {
                File.Delete(luaDirectory);
            }
            File.Create(luaDirectory).Close();
            using (StreamWriter file = new StreamWriter(luaDirectory, true, Encoding.GetEncoding("gbk")))
            {
                file.WriteLine(@"ChangeEquipScript = {}");
                file.WriteLine(@"function ChangeEquipScript:Init()");
                file.Write(decodedBase64);
                file.WriteLine("\n");
                file.WriteLine(@"end");
                Console.Write(file.ToString());        
            }*/
        }
    }
}
