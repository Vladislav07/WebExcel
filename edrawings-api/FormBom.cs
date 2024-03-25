
using eDrawings.Interop.EModelViewControl;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace edrawings_api
{
    public partial class FormBom : Form
    {
        private string FILE_PATH = ""; // @"C:\CUBY_PDM\Work\Other\Без проекта\CUBY-V1.1\CAD\Завод контейнер\Участок сварочный\Кран балка\CUBY-00170130.sldasm";
        EModelViewControl m_Ctrl;
        List<string> listDrawing;
        public FormBom()
        {
            InitializeComponent();

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            FILE_PATH = openFileDialog1.FileName;

            System.Data.DataTable dt = new System.Data.DataTable();
            BooksRepo repo = new BooksRepo();
            repo.GetTableDt(FILE_PATH, ref dt);
            Form1 fb = new Form1(dt);
            fb.proccesedBom += Fb_proccesedBom;
            
            fb.Show();
            this.Hide();
        }

       

        private void Fb_proccesedBom(List<string> list)
        {
            var host = new eDrawingHost();
            host.ControlLoaded += OnControlLoaded;
            this.Controls.Add(host);
            host.Dock = DockStyle.Fill;
            listDrawing = list;
            


        }

        private void OnControlLoaded(EModelViewControl ctrl)
        {
            m_Ctrl = ctrl;
            ctrl.OnFinishedLoadingDocument += OnDocumentLoaded;
            ctrl.OnFailedLoadingDocument += OnDocumentLoadFailed;
            ctrl.OnFinishedPrintingDocument += OnDocumentPrinted;
            ctrl.OnFailedPrintingDocument += OnPrintFailed;
            PrintNext();
        }

        private void OnPrintFailed(string PrintJobName)
        {
            Trace.WriteLine($"Failed to export - {PrintJobName}");
            PrintNext();
        }

        private void OnDocumentPrinted(string PrintJobName)
        {
            Console.WriteLine($"export completed {PrintJobName}");
            PrintNext();
        }

        private void OnDocumentLoadFailed(string fileName, int errorCode, string errorString)
        {
            Trace.WriteLine($"{fileName} failed to loaded: {errorString}");
            PrintNext();
        }

        private void OnDocumentLoaded(string fileName)
        {
            Trace.WriteLine($"{fileName} loaded");
             string PRINTER_NAME = "Microsoft Print to PDF";
             int AUTO_SOURCE = 7;
             m_Ctrl.SetPageSetupOptions(EMVPrintOrientation.eLandscape, 7, 100, 100, 1, AUTO_SOURCE, PRINTER_NAME, 0, 0, 0, 0);
            string pdfFileName = Path.GetFileNameWithoutExtension(fileName) + ".pdf";
            string outDir = @"D:\macros";
            string pdfFilePath;
            pdfFilePath = Path.Combine(outDir, pdfFileName);

            m_Ctrl.Print5(false, fileName, false, false, true, EMVPrintType.eWYSIWYG, 1, 0, 0, true, 1, 1, pdfFilePath);
        }

        private void PrintNext()
        {
            
            if (listDrawing.Count > 0)
            {
              string filePath = listDrawing[0];
              listDrawing.RemoveAt(0);
              m_Ctrl.CloseActiveDoc("");
              m_Ctrl.OpenDoc(filePath, false, false, false, "");

            }
            else
            {
                this.Close();
               // Application.Exit();
            }
        }
     
    }
}
