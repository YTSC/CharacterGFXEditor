using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base64Converter
{
    public partial class FormCopyMultiple : Form
    {
        List<ItemGFX> gfxList;        
        public FormCopyMultiple()
        {
            InitializeComponent();
            gfxList = LuaBasics.GetGfxList();
            int count = 1;            
            foreach (ItemGFX itemGFX in gfxList)
            {
                dataGridView1.Rows.Add(count, itemGFX.GetID());
                count++;
            }
        }
        public bool sucess = false;
        List<string> selectedGFX = new List<string>();
        public List<string> GetSelectedItems()
        {
            return selectedGFX;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (ItemGFX item in gfxList)
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        if (item.GetID() == int.Parse(row.Cells[1].Value.ToString()))
                            selectedGFX.Add(row.Cells[1].Value.ToString());
                    }
                }
                sucess = true;
                Close();
            }
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
