using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SDA__46501_MyProject
{
    public partial class Form1 : Form
    {

        public Dictionary<string, string> bgDict = new Dictionary<string, string>();
        public Dictionary<string, string> deDict = new Dictionary<string, string>();


        BulgarianForm bulgarianForm;
        GermanForm germanForm;

        
        string p = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString();
        string path;
    
        public Form1()
        {
            path = Path.Combine(p, @"res\dict.txt");
            bulgarianForm = new BulgarianForm(this);
            germanForm = new GermanForm(this);
            initDicts();
            bulgarianForm.init();
            germanForm.init();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bulgarianForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            germanForm.Show();
            this.Hide();
        }

        public void initDicts()
        {
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader(fileStream);

            bgDict = new Dictionary<string, string>();
            deDict = new Dictionary<string, string>();
            while (!streamReader.EndOfStream)
            {
                string s = streamReader.ReadLine();
                if (!s.Remove(s.IndexOf("::"), 2).Any(c => char.IsDigit(c)))
                {
                    string[] separator = new string[] { "::" };
                    string[] words = s.Split(separator, StringSplitOptions.None);

                    try
                    {
                        deDict.Add(words[0], words[1]);
                        bgDict.Add(words[1], words[0]);
                    }
                    catch (ArgumentException e)
                    {
                        throw e;
                    }
                }
            }

            streamReader.Close();
            fileStream.Close();
        }

        private void add_Click(object sender, EventArgs e) {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("Не може да се оставят празни полета");
            }
            if (!textBox1.Text.All(c => char.IsLetter(c)) || !textBox2.Text.All(c => char.IsLetter(c)))
            {
                MessageBox.Show("Не може да се въвеждат числа");
                return;
            }
            if (textBox1.Text.Length > 40)
            {
                MessageBox.Show("Твърде дълга българска дума");
                return;
            }if (textBox2.Text.Length > 40)
            {
                MessageBox.Show("Твърде дълга немска дума");
                return;
            }

            if (!bgDict.ContainsKey(textBox1.Text) && !deDict.ContainsKey(textBox2.Text))
            {
                FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                try
                {

                    streamWriter.Write(textBox1.Text + "::" + textBox2.Text + "\n");
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                catch (ArgumentException ex)
                {
                    throw ex;
                }

                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();

                initDicts();
                bulgarianForm.init();
                germanForm.init();
            }
            else
            {
                if (bgDict.ContainsKey(textBox1.Text))
                {
                    MessageBox.Show("Вече има тази дума на български");
                }
                if (deDict.ContainsKey(textBox2.Text))
                {
                    MessageBox.Show("Вече има тази дума на немски");
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            deleteEntry(textBox3.Text, textBox4.Text);
        }

        public void deleteEntry(string bg, string de)
        {
            int index = bgDict.ContainsKey(bg) ? bgDict.Keys.ToList().IndexOf(bg) : (deDict.ContainsKey(de) ? deDict.Keys.ToList().IndexOf(de) : -1);

            if (index == -1)
            {
                if (bg == null || de == null)
                {
                    MessageBox.Show("Грешка при намирането на дума");
                }
                else
                {
                    MessageBox.Show("Няма такава дума");
                }
            }
            else
            {
                List<string> linesList = File.ReadAllLines(path).ToList();

                linesList.RemoveAt(index);

                File.WriteAllLines(path, linesList);

                textBox3.Text = "";
                textBox4.Text = "";

                initDicts();
            }
        }
    }
}
