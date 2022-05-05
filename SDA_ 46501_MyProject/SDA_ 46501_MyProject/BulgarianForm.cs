using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDA__46501_MyProject
{
    public partial class BulgarianForm : Form
    {
        Form1 form1 = null;

        public BulgarianForm(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
        }
        public BulgarianForm()
        {
            InitializeComponent();
        }

        public void init()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(form1.bgDict.Keys.ToArray());
        }

        private void back_Click(object sender, EventArgs e)
        {
            form1.Show();
            this.Hide();
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                label1.Text = form1.bgDict[listBox1.Items[listBox1.SelectedIndex].ToString()];
            }
            
        }

        private void BulgarianForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void search_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.All(c => char.IsLetter(c)))
            {
                MessageBox.Show("Моля въведете само букви");
                return;
            }
            listBox1.SelectedItem = textBox1.Text;
        }

        private void delete_Click(object sender, EventArgs e)
        {
            form1.deleteEntry(listBox1.SelectedItem.ToString(), null);
            init();
        }

        private void searchByLetter_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 1)
            {
                MessageBox.Show("Въведете само една буква");
                return;
            }
            else if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Въведете една буква");
                return;
            }
            else if (char.TryParse(textBox2.Text, out char c))
            {
                if (!char.IsLetter(c))
                {
                    MessageBox.Show("Напишете буква от азбуката");
                }
            }

            listBox1.Items.Clear();
            List<string> entry = form1.bgDict.Keys.ToList();
            for (int i = 0; i < entry.Count; i++)
            {
                if (entry[i].ToString().ToLower().StartsWith(textBox2.Text.ToLower()))
                {
                    listBox1.Items.Add(entry[i]);
                }
            }

            if (listBox1.Items.Count == 0)
            {
                listBox1.Items.Add("Няма думи започващи с тази буква");
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            init();
            textBox2.Text = "";
        }
    }
}
