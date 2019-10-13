using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.IO.Image;

namespace GenealogiaPortuguesa
{
    public class Digitarq
    {
        private WebBrowser wb;
        private int pageIni;
        private int pageFim;
        private string path;
        private Timer timer;
        private ToolStripStatusLabel status;
        List<Uri> images = new List<Uri>();
       

        public Digitarq(string tempFolder, int pages, ref ToolStripStatusLabel lbl)
        {
            status = lbl;
            path = tempFolder;
            pageIni = 1;
            pageFim = pages;
        }

        public void LoadPage(string url)
        {
            int index = url.IndexOf("=");
            path += url.Substring(index+1)+"\\";
            Directory.CreateDirectory(path);
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
                
                status.Text = Convert.ToString((pageIni * 100) / pageFim) + "%";
                pageIni++;

            }
            Createpdf();
        }

        private void Createpdf()
        {
            FileStream output = new FileStream(path + "livro.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(output);
            PdfDocument pdfdoc = new PdfDocument(writer);
            Document document = new Document(pdfdoc, PageSize.A3.Rotate());
            

            foreach (Uri ur in images)
            { 
                Image img = new Image(ImageDataFactory.CreateJpeg(ur));
                img.ScaleToFit(PageSize.A3.GetHeight(), PageSize.A3.GetWidth());
                document.Add(img);
            }
            document.Close();

            status.Text = "";
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
                Uri uri = new Uri(path);
                images.Add(uri);
            }
        }
    }
}
