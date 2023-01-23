using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LuaInterface;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Base64Converter
{
    public partial class Form1 : Form
    {
        string label4Text;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = null;
            richTextBox2.Text = null;
            label15.Text = null;
            label4Text = label4.Text;
            textBox4.MaxLength = 1;
        }
        bool invert = false;     
        string labelText = "Number of Script Lines: ";        
        int actualProperty = 0;
        List<List<ItemGFX>> ListOfGfxLists = new List<List<ItemGFX>>();
        List<ItemGFX.Properties> properties = new List<ItemGFX.Properties>();        

        public static string DecodeBase64(string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            string decodedString = Encoding.GetEncoding("gb2312").GetString(data);
            return decodedString;
        }
        public static string DecodeBase64(List<string> base64String)
        {
            string concatenated = null;
            foreach (string base64 in base64String)
                concatenated += base64;

            byte[] data = Convert.FromBase64String(concatenated);
            string decodedString = Encoding.GetEncoding("gb2312").GetString(data);            
            decodedString = decodedString.Substring(0, decodedString.IndexOf(@"End(Automatic script generation by ECMEditor)"));
            return decodedString;
        }
        public static List<string> EncodeBase64(string decodedString)
        {
            List<string> Base64StringPW = new List<string>();           
            byte[] data = Encoding.GetEncoding("gb2312").GetBytes(decodedString);
            string Base64String = Convert.ToBase64String(data);
            for (int x = 0; x < Base64String.Length; x += 1500)
            {
                if (x + 1500 < Base64String.Length)
                    Base64StringPW.Add(Base64String.Substring(x, 1500));
                else
                    Base64StringPW.Add(Base64String.Substring(x));
            }
            return Base64StringPW;
        }

        private int getItemID()
        {
            int rowIndex = dataGridView3.CurrentCell.RowIndex;
            int columnIndex = dataGridView3.CurrentCell.ColumnIndex;
            int itemID = int.Parse(dataGridView3.Rows[rowIndex].Cells[columnIndex].Value.ToString());
            return itemID;
        }
        private int getPropertyID()
        {
            int rowIndex = dataGridView4.CurrentCell.RowIndex;
            int columnIndex = dataGridView4.CurrentCell.ColumnIndex;
            int propertyID = int.Parse(dataGridView4.Rows[rowIndex].Cells[columnIndex].Value.ToString());
            return propertyID;
        }

        public void setProperty(int itemID, int actualProperty)
        {                      
            List<ItemGFX> gfxList = LuaBasics.GetGfxList();
            foreach (ItemGFX itemGFX in gfxList)
            {
                if (itemGFX.GetID() == itemID)
                {
                    properties = itemGFX.getProperties();
                    textBox11.Text = properties[actualProperty].getNum().ToString();
                    textBox2.Text = properties[actualProperty].getHook();
                    textBox3.Text = properties[actualProperty].getGfxPath();
                    textBox4.Text = properties[actualProperty].getGfxVer().ToString();
                    textBox5.Text = properties[actualProperty].getPosX().ToString();
                    textBox6.Text = properties[actualProperty].getPosY().ToString();
                    textBox7.Text = properties[actualProperty].getPosZ().ToString();
                    textBox10.Text = properties[actualProperty].getRotX().ToString();
                    textBox9.Text = properties[actualProperty].getRotY().ToString();
                    textBox8.Text = properties[actualProperty].getRotZ().ToString();
                }
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            if (!invert)
            {
                invert = true;
                button1.Text = "Convert to Base64";
            }
            else
            {
                invert = false;
                button1.Text = "Convert from Base64";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!invert)
            {
                if (richTextBox1.Text != null)
                {
                    try
                    {
                        richTextBox2.Text = DecodeBase64(richTextBox1.Text);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("The Base64 text is not in the correct format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                if (richTextBox2.Text != null)
                {
                    List<string> Base64StringPW = EncodeBase64(richTextBox2.Text);
                    richTextBox1.Text = "";
                    foreach (var CodedStringPW in Base64StringPW)
                        richTextBox1.Text += CodedStringPW + "\n";

                    label1.Text += Base64StringPW.Count;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = null;
            richTextBox2.Text = null;
            label1.Text = labelText;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                textBox1.Text = @"\models\players\形象\";
                textBox1.ForeColor = SystemColors.GrayText;
            }

        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == @"\models\players\形象\")
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }
        public void ClearTextBoxes(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase && !ctrl.Name.Equals("textBox1"))
                    ctrl.Text = String.Empty;
                else
                    ClearTextBoxes(ctrl.Controls);
            }
        }
        private List<List<ItemGFX>> SortList(List<List<ItemGFX>> ListOfGfxLists)
        {
            List<List<ItemGFX>> SortedList = new List<List<ItemGFX>>();            
            foreach (List<ItemGFX> list in ListOfGfxLists)
            {
                List<ItemGFX> SortedList2 = new List<ItemGFX>();
                IEnumerable<ItemGFX> query = list.OrderBy(item => item.ID);
                foreach(ItemGFX item in query)
                {
                    SortedList2.Add(item);
                }
                SortedList.Add(SortedList2);
            }
            return SortedList;
        }
        private void LoadAllFiles()
        {
            if (LuaBasics.loaded == false)
            {
                ListOfGfxLists = new List<List<ItemGFX>>();
                List<Folder> folders = DirectoryBasics.GetFoldersList();
                string ecmPath = null;
                ECMFile ecm = null;
                List<ItemGFX> temporaryGfxList = null;
                foreach (Folder folder in folders)
                {
                    DirectoryBasics.fileString = null;
                    foreach (string file in folder.GetFilesList())
                    {
                        string cuttedFile = file.Remove(0, folder.GetDirectory().Length + 1);
                        if (Path.GetExtension(cuttedFile) == ".ecm")
                        {
                            ecm = null;
                            ecmPath = cuttedFile;
                            ecm = new ECMFile(folder.GetDirectory() + @"\" + ecmPath);
                            ecm.setDecodedLines();
                            DirectoryBasics.WriteTempLua(ecm.GetDecodedBase64(), Directory.GetCurrentDirectory());
                            temporaryGfxList = new List<ItemGFX>();
                            foreach (ItemGFX gfx in LuaBasics.getValues())
                            {
                                temporaryGfxList.Add(gfx);                                
                            }
                        }
                        progressBar1.Increment(4);
                    }
                    ListOfGfxLists.Add(temporaryGfxList);
                }
                this.progressBar1.Value = 100;
                ListOfGfxLists = SortList(ListOfGfxLists);
                MessageBox.Show("Loaded sucessfully", "Loaded!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.progressBar1.Value = 0;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!DirectoryBasics.directorySelected)
            {
                dataGridView1.Rows.Clear();
                bool directorySelected = DirectoryBasics.SetBaseDirectory();
                if (directorySelected)
                {
                    textBox1.Enabled = false;
                    textBox1.Text = DirectoryBasics.GetBaseDirectory();
                    button6.Enabled = false;
                    DirectoryBasics.directorySelected = true;
                    foreach (Folder folder in DirectoryBasics.GetFoldersList())
                    {
                        for (int x = 0; x < DirectoryBasics.chineseFolder.Length; x++)
                        {
                            if (folder.GetName() == DirectoryBasics.chineseFolder[x])
                            {
                                dataGridView1.Rows.Add(DirectoryBasics.classFolder[x]);
                                break;
                            }
                        }
                    }
                    LoadAllFiles();
                }
                //else MessageBox.Show("Failed to load the path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //else ClearAll();

        }
        
        private void LoadFile()
        {
                DirectoryBasics.fileString = null;
                int rowIndex = dataGridView1.CurrentCell.RowIndex;               
                LuaBasics.setGfxList(ListOfGfxLists[rowIndex]);
        }    

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                if (actualProperty != 0)
                {
                    actualProperty -= 1;
                    dataGridView4.Rows[actualProperty].Selected = true;
                    setProperty(getItemID(), actualProperty);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                int itemID = getItemID();
                List<ItemGFX.Properties> property;
                List<ItemGFX> gfxList = LuaBasics.GetGfxList();
                foreach (ItemGFX itemGFX in gfxList)
                {
                    if (itemGFX.GetID() == itemID)
                    {
                        property = itemGFX.getProperties();
                        if (actualProperty != property.Count - 1)
                        {
                            actualProperty += 1;
                            dataGridView4.Rows[actualProperty].Selected = true;
                            setProperty(getItemID(), actualProperty);
                            break;
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                actualProperty = 0;
                dataGridView4.Rows[actualProperty].Selected = true;
                setProperty(getItemID(), actualProperty);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                int itemID = getItemID();
                List<ItemGFX.Properties> property;
                List<ItemGFX> gfxList = LuaBasics.GetGfxList();
                foreach (ItemGFX itemGFX in gfxList)
                {
                    if (itemGFX.GetID() == itemID)
                    {
                        property = itemGFX.getProperties();
                        actualProperty = property.Count - 1;
                        dataGridView4.Rows[actualProperty].Selected = true;
                        setProperty(getItemID(), property.Count - 1);
                        break;
                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if(dataGridView3.SelectedCells.Count > 0)
            {
                int itemID = getItemID();
                List<ItemGFX> gfxList = LuaBasics.GetGfxList();
                foreach (ItemGFX itemGFX in gfxList)
                {
                    if (itemID == itemGFX.GetID())
                    {
                        int num = itemGFX.getProperties().Count;
                        itemGFX.SetProperties(num + 1, null, null, 0, 0, 0, 0, 0, 0, 0);
                        dataGridView4.Rows.Add((num + 1).ToString());
                        label4.Text = label4Text + properties.Count;
                        break;
                    }
                }
            }            
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            ClearTextBoxes(this.Controls);
            properties = null;            
            int itemID = getItemID();           
            List<ItemGFX> gfxList = LuaBasics.GetGfxList();
            dataGridView4.Rows.Clear();
            foreach (ItemGFX itemGFX in gfxList)
            {                
                if (itemGFX.GetID() == itemID)
                {                   
                    properties = itemGFX.getProperties();
                    foreach(ItemGFX.Properties property in properties)                    
                        dataGridView4.Rows.Add(property.getNum().ToString());
                    
                    label4.Text = label4Text + properties.Count;
                    break;
                }
            }
            if(dataGridView4.Rows.Count > 0)
            {
                actualProperty = 0;
                setProperty(getItemID(), actualProperty);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells.Count > 0)
            {
                FormAddItem frmAI = new FormAddItem();
                frmAI.ShowDialog();
                if (frmAI.sucess == true)
                {
                    bool match = false;
                    int ID = frmAI.ID;
                    List<ItemGFX> gfxList = LuaBasics.GetGfxList();
                    foreach (ItemGFX item in gfxList)
                    {
                        if (ID == item.GetID())
                        {
                            match = true;
                            break;
                        }
                    }
                    if (match)
                        MessageBox.Show("The ID is already in the list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        int count = gfxList.Count;
                        ItemGFX newItem = new ItemGFX();
                        newItem.SetID(ID);
                        newItem.SetProperties(1, null, null, 0, 0, 0, 0, 0, 0, 0);
                        LuaBasics.AddItemGFX(newItem);
                        dataGridView3.Rows.Add(count + 1, ID);
                        dataGridView3.CurrentCell = dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells[dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells.Count - 1];
                    }
                }
            }            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadFile();
            dataGridView3.Rows.Clear();
            int count = 1;
            List<ItemGFX> gfxList = LuaBasics.GetGfxList();
            foreach (ItemGFX itemGFX in gfxList)
            {
                dataGridView3.Rows.Add(count, itemGFX.GetID());
                count++;
            }
        }   

        private void button15_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedCells.Count > 0)
            {
                int itemID = getItemID();
                List<ItemGFX> gfxList = LuaBasics.GetGfxList();
                foreach (ItemGFX itemGFX in gfxList)
                {
                    if (itemID == itemGFX.GetID())
                    {                                 
                        int num = itemGFX.getProperties().Count;                       
                        itemGFX.SetProperties(num + 1, textBox2.Text, textBox3.Text, int.Parse(textBox4.Text), double.Parse(textBox5.Text), double.Parse(textBox6.Text), double.Parse(textBox7.Text), double.Parse(textBox10.Text), double.Parse(textBox9.Text), double.Parse(textBox8.Text));
                        dataGridView4.Rows.Add((num + 1).ToString());
                        label4.Text = label4Text + properties.Count;
                        break;
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int itemID = getItemID();
                List<ItemGFX> gfxList = LuaBasics.GetGfxList();               
                foreach (ItemGFX itemGFX in gfxList)
                {
                    if (itemGFX.GetID() == itemID)
                    {                        
                        gfxList.Remove(itemGFX);                        
                        break;
                    }
                }
                LuaBasics.setGfxList(gfxList);
                dataGridView3.Rows.Clear();
                int count = 1;
                foreach (ItemGFX itemGFX in gfxList)
                {
                    dataGridView3.Rows.Add(count, itemGFX.GetID());
                    count++;
                }
                dataGridView3.CurrentCell = dataGridView3.Rows[dataGridView3.Rows.Count-1].Cells[dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells.Count-1];
                
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedCells.Count > 0)
            {
                int itemID = getItemID();
                List<ItemGFX> gfxList = LuaBasics.GetGfxList();
                foreach (ItemGFX itemGFX in gfxList)
                {
                    if (itemID == itemGFX.GetID())
                    {
                        int propertyID = getPropertyID();
                        List<ItemGFX.Properties> properties = itemGFX.getProperties();
                        foreach(ItemGFX.Properties property in properties)
                        {
                            if(propertyID == property.getNum())
                            {
                                itemGFX.RemoveProperty(property);
                                break;
                            }
                        }                     
                        dataGridView4.Rows.Clear();
                        foreach (ItemGFX.Properties property in properties)
                            dataGridView4.Rows.Add(property.getNum().ToString());
                        label4.Text = label4Text + properties.Count;
                        break;
                    }
                }
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int propertyID = getPropertyID();
            setProperty(getItemID(), propertyID - 1);
        }
        
        private void button16_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                FormCopyTo frmCopy = new FormCopyTo();
                frmCopy.ShowDialog();
                if (frmCopy.sucess == true)
                {
                    bool match = false;                    
                    int itemID = getItemID();
                    ItemGFX actualItem = new ItemGFX();
                    foreach (ItemGFX itemGFX in LuaBasics.GetGfxList())
                    {
                        if (itemID == itemGFX.GetID())
                        {
                            actualItem = itemGFX;
                            break;
                        }
                    }
                    List<string> selectedClasses = frmCopy.GetSelectedClasses();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (selectedClasses.Contains(row.Cells[0].Value))
                        {                            
                            int rowIndex = row.Index;
                            foreach (ItemGFX item in ListOfGfxLists[rowIndex])
                            {
                                if (itemID == item.GetID() && item != actualItem)
                                {
                                    match = true;
                                    break;
                                }
                            }
                            if (match)
                            {
                                MessageBox.Show("The ID is already in at least one of the characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            else
                            {
                                if (!ListOfGfxLists[rowIndex].Contains(actualItem))
                                {
                                    int count = ListOfGfxLists[rowIndex].Count;
                                    ItemGFX newItem = new ItemGFX();                                   
                                    ListOfGfxLists[rowIndex].Add(actualItem);                                   
                                }
                                MessageBox.Show("Completed sucessfully!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }                       
                    }                    
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int itemID = getItemID();
            FormAddItem frmAI = new FormAddItem();
            frmAI.ShowDialog();
            if (frmAI.sucess == true)
            {
                bool match = false;
                int ID = frmAI.ID;
                string actualClass = dataGridView1.CurrentCell.Value.ToString();
                int classIndex = dataGridView1.CurrentCell.RowIndex;
                List<ItemGFX> gfxList = ListOfGfxLists[classIndex];
                foreach (ItemGFX item in gfxList)
                {
                    if (ID == item.GetID())
                    {
                        match = true;
                        break;
                    }
                }
                if (match)
                    MessageBox.Show("The ID is already in the list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    foreach (ItemGFX itemGFX in ListOfGfxLists[classIndex])
                    {
                        if (itemID == itemGFX.GetID())
                        {
                            ItemGFX temp = itemGFX.Clone();
                            temp.SetID(ID);                            
                            ListOfGfxLists[classIndex].Add(temp);

                            LoadFile();
                            dataGridView3.Rows.Clear();
                            int count = 1;
                            List<ItemGFX> listGFX = LuaBasics.GetGfxList();
                            foreach (ItemGFX item in listGFX)
                            {
                                dataGridView3.Rows.Add(count, item.GetID());
                                count++;
                            }
                            dataGridView3.CurrentCell = dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells[dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells.Count - 1];
                            actualProperty = 0;
                            setProperty(getItemID(), actualProperty);
                            break;
                        }
                    }                  
                }

            }   
        }
        private async void button18_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 200;
            timer1.Start();
            label15.Text = "SAVING, DO NOT CLOSE";
            await Task.Run(()=>LuaBasics.WriteValues(ListOfGfxLists));
            timer1.Stop();
            progressBar1.Maximum = 100;
            label15.Text = null;
            progressBar1.Value = 100;
            MessageBox.Show("Saved sucessfully", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            progressBar1.Value = 0;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {                      
            this.progressBar1.Increment(1);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            bool ok = false;
            foreach(List<ItemGFX> list in ListOfGfxLists)
            {
                int listIndex = ListOfGfxLists.IndexOf(list);
                foreach (ItemGFX item in list)
                {
                    int itemIndex = list.IndexOf(item);
                    if (item.GetID() == getItemID())
                    {
                        foreach (ItemGFX.Properties property in item.getProperties())
                        {
                            if (textBox11.Text == property.getNum().ToString())
                            {
                                int propertyIndex = item.getProperties().IndexOf(property);
                                property.SetPropertyGfxVer(int.Parse(textBox4.Text));                               
                                ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property,propertyIndex);
                                ok = true;
                                break;                                
                            }
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
                if (ok) break;
            }            
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            bool ok = false;
            foreach (List<ItemGFX> list in ListOfGfxLists)
            {
                int listIndex = ListOfGfxLists.IndexOf(list);
                foreach (ItemGFX item in list)
                {
                    int itemIndex = list.IndexOf(item);
                    if (item.GetID() == getItemID())
                    {
                        foreach (ItemGFX.Properties property in item.getProperties())
                        {
                            if (textBox11.Text == property.getNum().ToString())
                            {
                                int propertyIndex = item.getProperties().IndexOf(property);
                                property.SetPropertyHook(textBox2.Text);
                                ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                ok = true;
                                break;
                            }
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
                if (ok) break;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            bool ok = false;
            foreach (List<ItemGFX> list in ListOfGfxLists)
            {
                int listIndex = ListOfGfxLists.IndexOf(list);
                foreach (ItemGFX item in list)
                {
                    int itemIndex = list.IndexOf(item);
                    if (item.GetID() == getItemID())
                    {
                        foreach (ItemGFX.Properties property in item.getProperties())
                        {
                            if (textBox11.Text == property.getNum().ToString())
                            {
                                int propertyIndex = item.getProperties().IndexOf(property);
                                property.SetPropertyGfxPath(textBox3.Text);
                                ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                ok = true;
                                break;
                            }
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
                if (ok) break;
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            bool isDigit = double.TryParse(textBox5.Text, out double result);
            if (isDigit)
            {
                bool ok = false;
                foreach (List<ItemGFX> list in ListOfGfxLists)
                {
                    int listIndex = ListOfGfxLists.IndexOf(list);
                    foreach (ItemGFX item in list)
                    {
                        int itemIndex = list.IndexOf(item);
                        if (item.GetID() == getItemID())
                        {
                            foreach (ItemGFX.Properties property in item.getProperties())
                            {
                                if (textBox11.Text == property.getNum().ToString())
                                {
                                    int propertyIndex = item.getProperties().IndexOf(property);
                                    property.SetPropertyPosX(double.Parse(textBox5.Text));
                                    ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                    ok = true;
                                    break;
                                }
                            }
                            if (ok) break;
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
            }
            else MessageBox.Show("Invalid Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

           
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            bool isDigit = double.TryParse(textBox6.Text, out double result);
            if (isDigit)
            {
                bool ok = false;
                foreach (List<ItemGFX> list in ListOfGfxLists)
                {
                    int listIndex = ListOfGfxLists.IndexOf(list);
                    foreach (ItemGFX item in list)
                    {
                        int itemIndex = list.IndexOf(item);
                        if (item.GetID() == getItemID())
                        {
                            foreach (ItemGFX.Properties property in item.getProperties())
                            {
                                if (textBox11.Text == property.getNum().ToString())
                                {
                                    int propertyIndex = item.getProperties().IndexOf(property);
                                    property.SetPropertyPosY(double.Parse(textBox6.Text));
                                    ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                    ok = true;
                                    break;
                                }
                            }
                            if (ok) break;
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
            }
            else MessageBox.Show("Invalid Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            bool isDigit = double.TryParse(textBox7.Text, out double result);
            if (isDigit)
            {
                bool ok = false;
                foreach (List<ItemGFX> list in ListOfGfxLists)
                {
                    int listIndex = ListOfGfxLists.IndexOf(list);
                    foreach (ItemGFX item in list)
                    {
                        int itemIndex = list.IndexOf(item);
                        if (item.GetID() == getItemID())
                        {
                            foreach (ItemGFX.Properties property in item.getProperties())
                            {
                                if (textBox11.Text == property.getNum().ToString())
                                {
                                    int propertyIndex = item.getProperties().IndexOf(property);
                                    property.SetPropertyPosZ(double.Parse(textBox7.Text));
                                    ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                    ok = true;
                                    break;
                                }
                            }
                            if (ok) break;
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
            }
            else MessageBox.Show("Invalid Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {          
            bool isDigit = double.TryParse(textBox10.Text, out double result);
            if (isDigit)
            {
                bool ok = false;
                foreach (List<ItemGFX> list in ListOfGfxLists)
                {
                    int listIndex = ListOfGfxLists.IndexOf(list);
                    foreach (ItemGFX item in list)
                    {
                        int itemIndex = list.IndexOf(item);
                        if (item.GetID() == getItemID())
                        {
                            foreach (ItemGFX.Properties property in item.getProperties())
                            {
                                if (textBox11.Text == property.getNum().ToString())
                                {
                                    int propertyIndex = item.getProperties().IndexOf(property);
                                    property.SetPropertyRotX(double.Parse(textBox10.Text));
                                    ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                    ok = true;
                                    break;
                                }
                            }
                            if (ok) break;
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
            }
            else MessageBox.Show("Invalid Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            bool isDigit = double.TryParse(textBox9.Text, out double result);
            if (isDigit)
            {
                bool ok = false;
                foreach (List<ItemGFX> list in ListOfGfxLists)
                {
                    int listIndex = ListOfGfxLists.IndexOf(list);
                    foreach (ItemGFX item in list)
                    {
                        int itemIndex = list.IndexOf(item);
                        if (item.GetID() == getItemID())
                        {
                            foreach (ItemGFX.Properties property in item.getProperties())
                            {
                                if (textBox11.Text == property.getNum().ToString())
                                {
                                    int propertyIndex = item.getProperties().IndexOf(property);
                                    property.SetPropertyRotY(double.Parse(textBox9.Text));
                                    ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                    ok = true;
                                    break;
                                }
                            }
                            if (ok) break;
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
            }
            else MessageBox.Show("Invalid Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            bool isDigit = double.TryParse(textBox8.Text, out double result);
            if (isDigit)
            {
                bool ok = false;
                foreach (List<ItemGFX> list in ListOfGfxLists)
                {
                    int listIndex = ListOfGfxLists.IndexOf(list);
                    foreach (ItemGFX item in list)
                    {
                        int itemIndex = list.IndexOf(item);
                        if (item.GetID() == getItemID())
                        {
                            foreach (ItemGFX.Properties property in item.getProperties())
                            {
                                if (textBox11.Text == property.getNum().ToString())
                                {
                                    int propertyIndex = item.getProperties().IndexOf(property);
                                    property.SetPropertyRotZ(double.Parse(textBox8.Text));
                                    ListOfGfxLists[listIndex][itemIndex].ChangeProperty(property, propertyIndex);
                                    ok = true;
                                    break;
                                }
                            }
                            if (ok) break;
                        }
                        if (ok) break;
                    }
                    if (ok) break;
                }
            }
            else MessageBox.Show("Invalid Value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }      

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == ',') && !(e.KeyChar == '-'))
            {
                e.Handled = true;                
            }           
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == ',') && !(e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == ',') && !(e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == ',') && !(e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == ',') && !(e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == ',') && !(e.KeyChar == '-') && !(e.KeyChar == (char)Keys.ControlKey) && !(e.KeyChar == (char)Keys.V))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == '0') && !(e.KeyChar == '1'))
            {
                 e.Handled = true;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                bool error = false;
                FormCopyMultiple frmCopyMultiple = new FormCopyMultiple();
                frmCopyMultiple.ShowDialog();      
                if(frmCopyMultiple.sucess == true)
                {
                    List<string> selectedGFX = frmCopyMultiple.GetSelectedItems();

                    FormCopyTo frmCopy = new FormCopyTo();
                    frmCopy.ShowDialog();
                    if (frmCopy.sucess == true)
                    {
                        List<string> selectedClasses = frmCopy.GetSelectedClasses();
                        string actualClass = dataGridView1.CurrentCell.Value.ToString();

                        bool match = false;
                        int itemID = getItemID();
                        ItemGFX actualItem = new ItemGFX();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (selectedClasses.Contains(row.Cells[0].Value) && row.Cells[0].Value.ToString() != actualClass)
                            {                               
                                int rowIndex = row.Index;                               
                                foreach (ItemGFX item in ListOfGfxLists[rowIndex])
                                {
                                    foreach(string gfx in selectedGFX)
                                    {
                                        if (gfx == item.GetID().ToString())
                                        {
                                            match = true;
                                            break;
                                        }
                                    }
                                    if (match) break;                                       
                                }                          
                                if (match)
                                {
                                    MessageBox.Show("The ID is already in at least one of the selected characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    error = true;
                                    break;
                                }
                                else
                                {
                                    List<ItemGFX> selected = new List<ItemGFX>();
                                    ItemGFX tempGfx;
                                    foreach (string gfx in selectedGFX)
                                    {
                                        foreach(ItemGFX item in ListOfGfxLists[dataGridView1.CurrentCell.RowIndex])
                                        {
                                            if (gfx == item.GetID().ToString())
                                            {
                                                tempGfx = item.Clone();
                                                tempGfx.SetID(item.GetID());
                                                selected.Add(tempGfx);
                                            }                                                
                                        }
                                    }
                                    foreach(ItemGFX item in selected)
                                    {                                         
                                        ListOfGfxLists[rowIndex].Add(item);
                                    }
                                }
                            }
                        }
                        if(!error) MessageBox.Show("Completed sucessfully!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
     

       
    }
}
