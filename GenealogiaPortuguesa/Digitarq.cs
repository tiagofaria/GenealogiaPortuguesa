using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace GenealogiaPortuguesa
{
    public class Digitarq
    {
        private WebBrowser wb;
        private int pageIni;
        private int pageFim;
        private string path;
        private Timer timer;

        public Digitarq(string tempFolder, int pages)
        {
            path = tempFolder;
            pageIni = 1;
            pageFim = pages;
        }

        public void LoadPage(string url)
        {
            wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            wb.Navigate(url);
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

        private async void DownloadLivro()
        {
            while (pageIni <= pageFim)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        string doc = "ViewerControl1_TreeViewFilesn" + pageIni.ToString();
                        if (pageIni > 1)
                            wb.Document.GetElementById(doc).InvokeMember("click");
                        await WaitForElement();
                        string src = wb.Document.GetElementById("ViewerControl1_FramedPanelViewer_imgViewer_img").GetAttribute("src");
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
            DownloadLivro();
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
