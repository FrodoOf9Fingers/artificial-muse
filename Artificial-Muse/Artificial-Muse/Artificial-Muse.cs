using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artificial_Muse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            folderBrowser.ShowNewFolderButton = true;

            DialogResult dlgResult = folderBrowser.ShowDialog();

            if (dlgResult.Equals(DialogResult.OK))
            {
                textBox1.Text = folderBrowser.SelectedPath;

                Environment.SpecialFolder rootFolder = folderBrowser.RootFolder;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Clear listBox if it contains items
            if (listBox1.Items.Count <= 1)
            {
                listBox1.Items.Clear();
                listBox1.Update();
                listBox1.Refresh();
            }

            if (!textBox1.Text.Equals(String.Empty))
              {
                  if (System.IO.Directory.GetFiles(textBox1.Text).Length > 0)
                  {
                      foreach (string fileName in System.IO.Directory.GetFiles(textBox1.Text, "*", System.IO.SearchOption.AllDirectories))
                      {
                          //Add file in ListBox.
                          listBox1.Items.Add(System.IO.Path.GetFileName(fileName));
                      }
                  }
                  else
                  {
                      listBox1.Items.Add(String.Format("No files Found at location : {0}", textBox1.Text));
                  }
              }
        }
    }
}
