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
    public partial class FormCopyTo : Form
    {
        public FormCopyTo()
        {
            InitializeComponent();
            LoadNames(this.Controls);
        }
        public bool sucess = false;
        List<string> selectedClasses = new List<string>();
        public List<string> GetSelectedClasses()
        {
            return selectedClasses;
        }

        private void LoadNames(Control.ControlCollection ctrlCollection)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is CheckBox)
                    checkBoxes.Add((CheckBox)ctrl);
            }
            List<string> males = new List<string>();
            List<string> females = new List<string>();
            for (int x = 0; x < checkBoxes.Count; x++)
            {
                if (x % 2 == 0)
                    females.Add(DirectoryBasics.classFolder[x]);
                else
                    males.Add(DirectoryBasics.classFolder[x]);
            }
            //Males
            for (int x = checkBoxes.Count / 2, y = 0; x < checkBoxes.Count; x++, y++)
            {
                checkBoxes[x].Text = males[y];
            }
            //Females
            for (int x = 0; x < checkBoxes.Count / 2; x++)
            {
                checkBoxes[x].Text = females[x];
            }
        }

        private void SelectAllMale(Control.ControlCollection ctrlCollection)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is CheckBox)
                    checkBoxes.Add((CheckBox)ctrl);
            }            
            for (int x = checkBoxes.Count / 2; x < checkBoxes.Count; x++)
            {
                checkBoxes[x].Checked = true;
            }
        }
        private void SelectAllFemale(Control.ControlCollection ctrlCollection)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is CheckBox)
                    checkBoxes.Add((CheckBox)ctrl);
            }
            for (int x = 0; x < checkBoxes.Count / 2; x++)
            {
                checkBoxes[x].Checked = true;
            }
        }
        private void SelectAll(Control.ControlCollection ctrlCollection)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is CheckBox)
                    checkBoxes.Add((CheckBox)ctrl);
            }
            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.Checked = true;
            }
        }
        private void UnselectAll(Control.ControlCollection ctrlCollection)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is CheckBox)
                    checkBoxes.Add((CheckBox)ctrl);
            }
            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.Checked = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SelectAll(this.Controls);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnselectAll(this.Controls);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            UnselectAll(this.Controls);
            SelectAllMale(this.Controls);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            UnselectAll(this.Controls);
            SelectAllFemale(this.Controls);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is CheckBox)
                    checkBoxes.Add((CheckBox)ctrl);
            }
            List<string> selectedClasses = new List<string>();
            foreach (CheckBox checkBox in checkBoxes)
            {
                if(checkBox.Checked == true)
                {
                    string checkBoxText = checkBox.Text;
                    selectedClasses.Add(checkBoxText);
                }
            }
            this.selectedClasses = selectedClasses;
            sucess = true;
            Close();
        }
    }
}
