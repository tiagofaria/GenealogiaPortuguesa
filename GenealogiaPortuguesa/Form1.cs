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

namespace GenealogiaPortuguesa
{
    public partial class Form1 : Form
    {
        private string site;
        private int pageIni = 0;
        private int pageFim = 100;
        private string path = @"D:\arvore_genealogica\livros\pagina";

        WebBrowser webBrowser1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = linkBox.Text;
            Digitarq dg = new Digitarq(webBrowser1);

            if (url.Contains("digitarq.adctb.arquivos.pt/viewer?id=")|| url.Contains("digitarq.arquivos.pt/viewer?id=")
                || url.Contains("digitarq.adbgc.arquivos.pt/viewer?id=")
                )
            {
                site = "DIGITARQ";
              //  url = "https://digitarq.adctb.arquivos.pt/Controls/vaultimage/?id=DISSEMINATION/26437E2EF12ECCC74C1B0F98C0E4D539&r=0&ww=1000&wh=600";
              //   dg.Download(url);
            }
            else
            {
                MessageBox.Show("Site não suportado.");
            }
            pageIni = (int)numericUpDown1.Value;
            pageFim = (int)numericUpDown2.Value;
            LoadPage(url);
        }

        private void LoadPage(string url)
        {
            webBrowser1 = new WebBrowser();
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            webBrowser1.Navigate(url);
        }

        static async Task<string> WaitForElement()
        {
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            int timeoutMS = 2500;

            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime < timeoutMS)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }
            return "S";
        }

        private async void Teste()
        {
            while (pageIni <= pageFim)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        string doc = "ViewerControl1_TreeViewFilesn" + pageIni.ToString();
                        if (pageIni > 1)
                            webBrowser1.Document.GetElementById(doc).InvokeMember("click");
                        await WaitForElement();
                        string src = webBrowser1.Document.GetElementById("ViewerControl1_FramedPanelViewer_imgViewer_img").GetAttribute("src");
                        DownloadImage(src, path + pageIni.ToString() + ".jpg");
                        
                    }
                    catch
                    {
                        int x = 0; 
                        //por vezes o site tem problemas com imagens que nao existem
                    }
                }
                pageIni++;
            }
                MessageBox.Show("Download Concluido");
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Teste();
        }

        private void DownloadImage(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), path);
            }
        }


    }
}
