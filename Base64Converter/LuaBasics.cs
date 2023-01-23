using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
//using LuaInterface;
using NLua;

namespace Base64Converter
{
   
    static class LuaBasics
    {
        public static bool loaded = false;
        static List<ItemGFX> gfxList = new List<ItemGFX>();

        public static List<ItemGFX> getValues()
        {
            using (Lua lua = new Lua())
            {   
                List<ItemGFX> gfxList = new List<ItemGFX>();
                ItemGFX itemGFX = new ItemGFX();
                List<long> Keys = new List<long>();
                int position = 0;
                Lua state = new Lua();
                lua.State.Encoding = Encoding.GetEncoding("gb2312");
                //string directory = Directory.GetCurrentDirectory() + @"\temp.lua";
                //lua.DoFile(directory);
                lua.DoString(DirectoryBasics.fileString);                
                lua.DoString("ChangeEquipScript:Init()"); // Chamando o construtor
                LuaTable equip_id_table = lua.GetTable("ChangeEquipScript.equip_id_table");             
                
                foreach (LuaTable item in equip_id_table.Values)
                {                    
                    foreach(long key in equip_id_table.Keys)                    
                        Keys.Add(key);

                    itemGFX = new ItemGFX();
                    itemGFX.SetID((int)Keys[position]);
                    Dictionary<object, object> propertiesList = lua.GetTableDict(item);
                    List<string> keyList = new List<string>();  

                    foreach (KeyValuePair<object, object> property in propertiesList)
                    {
                        double posX = 0, posY = 0, posZ = 0, rotX = 0, rotY = 0, rotZ = 0;
                        string hook = null, gfx_path = null;
                        int gfx_ver = 0;
                        LuaTable tag = (LuaTable)property.Value;
                        int keyNum = int.Parse(property.Key.ToString());   
                        
                        if(tag["hook"] != null)                        
                            hook = tag["hook"].ToString();                       

                        if(tag["gfx_path"] != null)
                            gfx_path = tag["gfx_path"].ToString();                        

                        if (tag["gfx_ver"] != null)
                            gfx_ver = int.Parse(tag["gfx_ver"].ToString());

                        if (tag["pos"] != null)
                        {
                            posX = (double)tag["pos.x"];
                            posY = (double)tag["pos.y"];
                            posZ = (double)tag["pos.z"];                                                     
                        }
                        if (tag["rot"] != null)
                        {
                            rotX = (double)tag["rot.x"];
                            rotY = (double)tag["rot.y"];
                            rotZ = (double)tag["rot.z"];                                                  
                        }
                        itemGFX.SetProperties(keyNum,hook,gfx_path,gfx_ver,posX,posY,posZ,rotX,rotY,rotZ);                        
                    }
                    gfxList.Add(itemGFX);
                    position++;
                }
                loaded = true;
                return gfxList;
            }
        }

        public static void WriteValues(List<List<ItemGFX>> ListOfGfxLists)
        {           
            foreach (List<ItemGFX> list in ListOfGfxLists)
            {
                string fileToWrite = null;
                fileToWrite += @"--Init ChangeEquipScript Table Start(Generated with CharacterGFXEditor by Yuri)";
                fileToWrite += "\n\n";
                fileToWrite += @"self.equip_id_table = {}";
                fileToWrite += "\n";
                fileToWrite += @"self.path_id_table = {}";
                fileToWrite += "\n";
                int listIndex = ListOfGfxLists.IndexOf(list);
                foreach (ItemGFX itemGFX in list)
                {
                    int itemID = itemGFX.GetID();
                    fileToWrite += @"self.equip_id_table[" + itemID + "] = {}";
                    fileToWrite += "\n";
                    foreach (ItemGFX.Properties property in itemGFX.getProperties())
                    {
                        int propertyNum = property.getNum();
                        string hook = property.getHook(); ;
                        string gfx_path = property.getGfxPath();
                        if(gfx_path != null && gfx_path.Contains(@"\"))
                        {
                            string[] splitted = gfx_path.Split('\\');
                            gfx_path = splitted[0];
                            for(int x = 1; x< splitted.Length; x++)
                            {                                
                                gfx_path += @"\\" + splitted[x];
                            }
                        }                        
                       
                        int gfx_ver = property.getGfxVer();
                        double posX = property.getPosX();
                        double posY = property.getPosY();
                        double posZ = property.getPosZ();
                        double rotX = property.getRotX();
                        double rotY = property.getRotY();
                        double rotZ = property.getRotZ();

                        fileToWrite += StringToWrite(itemID.ToString(), propertyNum.ToString(), hook, gfx_path, gfx_ver.ToString(), posX.ToString("0.000000", CultureInfo.InvariantCulture), posY.ToString("0.000000", CultureInfo.InvariantCulture), posZ.ToString("0.000000", CultureInfo.InvariantCulture), rotX.ToString("0.000000", CultureInfo.InvariantCulture), rotY.ToString("0.000000", CultureInfo.InvariantCulture), rotZ.ToString("0.000000", CultureInfo.InvariantCulture)) + "\n";                        
                    }
                    fileToWrite += "\n";
                }
                fileToWrite += "--Init ChangeEquipScript Table End(Automatic script generation by ECMEditor)";
                string folderName = DirectoryBasics.chineseFolder[listIndex];
                WriteFile(fileToWrite, folderName);              
            }
           
        }       
        private static void WriteFile(string fileString, string folderName)
        {
            foreach(Folder folder in DirectoryBasics.GetFoldersList())
            {
                if (folder.GetName() == folderName)
                {
                    string directory = folder.GetDirectory() + @"\躯干\" + folderName + ".ecm";
                    List<string> fileLines = File.ReadAllLines(directory,Encoding.GetEncoding("gb2312")).ToList();
                    int indexStart = 0, indexEnd = 0;
                    foreach (string line in fileLines)
                    {
                        if (line.Contains("ScriptLines:"))
                        {
                            indexStart = fileLines.IndexOf(line);
                        }
                        else if (line.Contains("AddiSkin"))
                        {
                            indexEnd = fileLines.IndexOf(line);
                            break;
                        }
                    }
                    string test = fileLines[indexStart];
                    string teste2 = fileLines[indexEnd];
                    List<string> stringBase64 = Form1.EncodeBase64(fileString);
                    if (File.Exists(directory))
                    {
                        File.Delete(directory);
                    }
                    File.Create(directory).Close();
                    using (StreamWriter file = new StreamWriter(directory, true,Encoding.GetEncoding("gb2312")))
                    {
                        for (int x = 0; x < indexStart; x++)
                        {
                            file.WriteLine(fileLines[x]);
                        }
                        file.WriteLine("ScriptLines: " + stringBase64.Count);
                        foreach(string line in stringBase64)
                        {
                            file.WriteLine(line);
                        }                        
                        for (int x = indexEnd; x < fileLines.Count; x++)
                        {
                            file.WriteLine(fileLines[x]);
                        }
                    }
                }
            }
        }
        private static string StringToWrite(string itemID, string keyNum, string hook, string gfx_path, string gfx_ver, string posX, string posY, string posZ, string rotX, string rotY, string rotZ)
        {
            string fileString = null;
            int version = int.Parse(gfx_ver);
            if(version == 1)            
                fileString = "self.equip_id_table[" + itemID + "]" + "[" + keyNum + "] = { hook = \"" + hook + "\", gfx_path = \"" + gfx_path + "\", gfx_ver = " + gfx_ver + ", pos = {x = " + posX + ", y = " + posY + ", z = " + posZ + "}, rot = {x = " + rotX + ", y = " + rotY + ", z = " + rotZ + "} }";            
            else if(version == 0)            
                fileString = "self.equip_id_table[" + itemID + "]" + "[" + keyNum + "] = { hook = \"" + hook + "\", gfx_path = \"" + gfx_path + "\", gfx_ver = " + gfx_ver +" }";
            
            return fileString;
        }
        public static void setGfxList(List<ItemGFX> gfxList2)
        {
            gfxList = gfxList2;
        }

        public static List<ItemGFX> GetGfxList()
        {
            return gfxList;
        }

        public static void AddItemGFX(ItemGFX itemGFX)
        {
            gfxList.Add(itemGFX);
        }

    }
    
    
}
