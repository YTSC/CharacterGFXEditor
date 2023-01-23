using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64Converter
{
    class Folder
    {
        string name;
        string translatedName;
        string directory;
        List<string> filesList = new List<string>();

        public void SetDirectory(string directory)
        {
            this.directory = directory;
        }
        public string GetDirectory()
        {
            return directory;
        }
        public void SetFilesList(string[] files)
        {
            //filesList = new List<string>();
            foreach (string file in files)
                filesList.Add(file);
        }
        public List<string> GetFilesList()
        {
            return filesList;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return name;
        }
        public void SetTranslatedName(string translatedName)
        {
            this.translatedName = translatedName;
        }
        public string GetTranslatedName()
        {
            return translatedName;
        }
    }
}
