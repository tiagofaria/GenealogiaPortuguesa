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
        private int page;
        private string path;
        private Timer timer;

        public Digitarq(WebBrowser wbf)
        {
            wb = wbf;
        }


        public void Download (string url)
        {
            page = 1;
            path = @"D:\arvore_genealogica\livros\pagina";
            
            wb = new WebBrowser();
            wb.Navigate(url);
            wb.DocumentCompleted +=  new WebBrowserDocumentCompletedEventHandler(GetHtml);            
        }

        private void GetHtml(object sender, WebBrowserDocumentCompletedEventArgs e)
        {            
            timer = new Timer();
            timer.Interval =10000 ;
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();

        }

        private void TimerTick ( object sender, EventArgs e)
        {
            try
            {
                string src = wb.Document.GetElementById("ViewerControl1_FramedPanelViewer_imgViewer_img").GetAttribute("src");
                DownloadImage(src, path + page.ToString() + ".jpg");
                page++;
            }
            catch(Exception er)
            {
                MessageBox.Show("FIM");
                timer.Stop();
                wb.Dispose();
            }
        }

        private void DownloadImage(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), path);
            }
            try
            {
                string doc = "ViewerControl1_TreeViewFilesn" + page.ToString();
                wb.Document.GetElementById(doc).InvokeMember("click");
            }
            catch(Exception er)
            {
                MessageBox.Show("FIM");
                timer.Stop();
                wb.Dispose();
            }
        }
    }
}
