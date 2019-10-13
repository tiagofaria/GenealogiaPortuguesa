using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;

namespace GenealogiaPortuguesa
{
    public partial class Form1 : Form
    {
       // private string site;
        private int pageIni = 0;
        private int pageFim = 100;
        private string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents")+"\\GenealogiaPortuguesa\\";

        WebBrowser webBrowser1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = linkBox.Text;

            if (url.Contains("digitarq.")&& url.Contains("/viewer?id="))
            {
                Digitarq dg = new Digitarq(path, 10);
                toolStripStatusLabel1.Text = "Download do livro em progresso. Aguarde por favor.";
                dg.LoadPage(url);
            }
            else
            {
                MessageBox.Show("Site não suportado.");
            }
        }

        private void acercaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }
    }
}
