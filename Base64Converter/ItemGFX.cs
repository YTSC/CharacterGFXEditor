using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64Converter
{
    class ItemGFX
    {
        public int ID;
        List<Properties> properties = new List<Properties>();
        public struct Properties
        {
            int num;
            string hook;
            string gfx_path;
            int gfx_ver;
            double posX;
            double posY;
            double posZ;
            double rotX;
            double rotY;
            double rotZ;   
            
            public Properties(int num,string hook, string gfx_path, int gfx_ver, double posX, double posY,
                double posZ, double rotX, double rotY, double rotZ)
            {
                this.num = num;
                this.hook = hook;
                this.gfx_path = gfx_path;
                this.gfx_ver = gfx_ver;
                this.posX = posX;
                this.posY = posY;
                this.posZ = posZ;
                this.rotX = rotX;
                this.rotY = rotY;
                this.rotZ = rotZ;
            }
            public int getNum(){ return num; }
            public string getHook(){ return hook; }
            public string getGfxPath() { return gfx_path; }
            public int getGfxVer() { return gfx_ver; }
            public double getPosX() { return posX; }
            public double getPosY() { return posY; }
            public double getPosZ() { return posZ; }
            public double getRotX() { return rotX; }
            public double getRotY() { return rotY; }
            public double getRotZ() { return rotZ; }

            public void SetPropertyNum(int num)
            {
                this.num = num;
            }
            public void SetPropertyHook(string hook)
            {
                this.hook = hook;
            }
            public void SetPropertyGfxPath(string gfx_path)
            {
                this.gfx_path = gfx_path;
            }
            public void SetPropertyGfxVer(int gfx_ver)
            {
                this.gfx_ver = gfx_ver;
            }
            public void SetPropertyPosX(double posX)
            {
                this.posX = posX;
            }
            public void SetPropertyPosY(double posY)
            {
                this.posY = posY;
            }
            public void SetPropertyPosZ(double posZ)
            {
                this.posZ = posZ;
            }
            public void SetPropertyRotX(double rotX)
            {
                this.rotX = rotX;
            }
            public void SetPropertyRotY(double rotY)
            {
                this.rotY = rotY;
            }
            public void SetPropertyRotZ(double rotZ)
            {
                this.rotZ = rotZ;
            }
        }
        public ItemGFX Clone()
        {
            ItemGFX newGFX = new ItemGFX();
            newGFX.SetID(0);
            foreach(Properties property in this.properties)            
                newGFX.SetProperties(property);

            return newGFX;
        }
        public void SetID(int ID)
        {
            this.ID = ID;
        }
        public int GetID()
        {
            return ID;
        }

        public void SetProperties(int num, string hook, string gfx_path, int gfx_ver, double posX,
                                  double posY, double posZ, double rotX, double rotY, double rotZ)
        {
            properties.Add(new Properties(num,hook,gfx_path,gfx_ver,posX,posY,posZ,rotX,rotY,rotZ));
        }
        public void SetProperties(Properties property)
        {
            properties.Add(property);
        }
        public void ChangeProperty(Properties property, int index)
        {
            this.properties[index] = property;
        }
        public List<Properties> getProperties()
        {
            return properties;
        }
        
        public void RemoveProperty(Properties property)
        {
            int propertyNum = property.getNum();
            properties.Remove(property);
            foreach (Properties property2 in properties.ToList())
            {
                if(property2.getNum() > propertyNum)
                {
                    int index = properties.IndexOf(property2);
                    properties[index] = new Properties(property2.getNum() - 1, property2.getHook(), property2.getGfxPath(), property2.getGfxVer(), property2.getPosX(), property2.getPosY(), property2.getPosZ(), property2.getRotX(), property2.getRotY(), property2.getRotZ());
                }
            }
        }
        
        
    }
}
