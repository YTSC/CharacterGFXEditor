using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base64Converter
{
    class ECMFile
    {
        int scriptLines = 0;
        List<string> base64String;
        string decodedString = null;

        public ECMFile(string directory)
        {
            int lineCount = 0, lineFound = 0;
            string[] lines = File.ReadAllLines(directory); 
            string compare = "ScriptLines";          
        
            for(int x = 0; x< lines.Length; x++)
            {
                if (lines[x].Contains(compare))
                    lineCount++;
                if (lines[x].Contains(compare) && lineCount == 2)
                    lineFound = x;
            }        

            List<string> base64lines = new List<string>();
            string stop = "AddiSkin";

            for (int x = lineFound+1; x< lines.Length; x++)
            {
                if (!lines[x].Contains(stop))
                    base64lines.Add(lines[x]);
                else break;
            }
            base64String = base64lines;
            scriptLines = base64String.Count;
           
        }

        private string ReviseBackslash(string stringToRevise)
        {
            using (StringReader sr = new StringReader(stringToRevise))
            {
                string temp = "";
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(@"\"))
                    {
                        string[] splitted = line.Split('\\');
                        line = splitted[0];
                        for (int x = 1; x < splitted.Length; x++)
                        {
                            if(splitted[x] != "")
                            line += @"\\" + splitted[x];
                        }                        
                    }
                    temp += line + "\n";
                }
                return temp;
            } 
        }
        public void setDecodedLines()
        {
            decodedString = ReviseBackslash(Form1.DecodeBase64(base64String));
            //decodedString = Form1.DecodeBase64(base64String);
        }

        public string GetDecodedBase64()
        {
            return decodedString;
        }

        public List<string> GetBase64String()
        {
            return base64String;
        }

        public int GetScriptLines()
        {
            return scriptLines;
        }
    }
}
