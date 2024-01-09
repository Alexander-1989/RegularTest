using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegularTest.Service.Serializer;

namespace RegularTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.TextChanged += ExpressionTest;
            textBox1.TextChanged += ExpressionTest;
        }

        private readonly Config config = new Config();

        private static T[] OfType<T>(IEnumerable items)
        {
            List<T> resultList = new List<T>();
            foreach (object item in items)
            {
                if (item is T)
                {
                    resultList.Add((T)item);
                }
            }
            return resultList.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            config.ReadConfig();
            Location = config.Properties.Location;
            richTextBox1.AppendText(config.Properties.Text);
            comboBox1.Items.AddRange(config.Properties.Items);
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveItem();
            config.Properties.Location = Location;
            config.Properties.Text = richTextBox1.Text;
            config.Properties.Items = OfType<string>(comboBox1.Items);
            config.WriteConfig();
        }

        private void ExpressionTest(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            richTextBox3.ForeColor = Color.Blue;

            string sourceText = richTextBox1.Text.Trim();
            string regularExpression = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(sourceText) || string.IsNullOrEmpty(regularExpression))
            {
                return;
            }

            try
            {
                Regex reg = new Regex(regularExpression);
                MatchCollection mathces = reg.Matches(sourceText);

                if (mathces.Count == 0)
                {
                    richTextBox3.AppendText("Совпадений не найдено!");
                    richTextBox3.ForeColor = Color.Blue;
                }
                else
                {
                    richTextBox3.AppendText(mathces[0].Value);
                    for (int i = 1; i < mathces.Count; i++)
                    {
                        richTextBox3.AppendText($"\n{mathces[i].Value}");
                    }
                }
            }
            catch (Exception exc)
            {
                richTextBox3.ForeColor = Color.Red;
                richTextBox3.AppendText(exc.Message);
            }
        }

        private void AddItem()
        {
            if (!comboBox1.Items.Contains(textBox1.Text))
            {
                comboBox1.Items.Add(textBox1.Text);
                comboBox1.Text = textBox1.Text;
            }
        }

        private void RemoveItem()
        {
            int index = comboBox1.SelectedIndex;

            if (index >= 0)
            {
                comboBox1.Items.RemoveAt(index);
                comboBox1.SelectedIndex = index < comboBox1.Items.Count ? index : index - 1;
            }

            if (comboBox1.Items.Count == 0)
            {
                comboBox1.ResetText();
                textBox1.Clear();
            }
        }

        private void SaveItem()
        {
            int index = comboBox1.SelectedIndex;

            if (index > -1)
            {
                comboBox1.Items[index] = textBox1.Text;
            }
            else if (!string.IsNullOrEmpty(textBox1.Text))
            {
                comboBox1.Items.Add(textBox1.Text);
                comboBox1.Text = textBox1.Text;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.AppendText(comboBox1.Text);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            RemoveItem();
        }
    }
}