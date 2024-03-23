
using eDrawings.Interop.EModelViewControl;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace edrawings_api
{
    public partial class FormBom : Form
    {
        private string FILE_PATH = ""; // @"C:\CUBY_PDM\Work\Other\Без проекта\CUBY-V1.1\CAD\Завод контейнер\Участок сварочный\Кран балка\CUBY-00170130.sldasm";
        EModelViewControl m_Ctrl;
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
            fb.Show();
           // var host = new eDrawingHost();
           // host.ControlLoaded += OnControlLoaded;
          //  this.Controls.Add(host);
          //  host.Dock = DockStyle.Fill;
        }

        private void OnControlLoaded(EModelViewControl ctrl)
        {
            m_Ctrl = ctrl;
            ctrl.OnFinishedLoadingDocument += OnFinishedLoadingDocument;
            ctrl.OnFailedLoadingDocument += OnFailedLoadingDocument;
            ctrl.OpenDoc(FILE_PATH, false, false, false, "");
        }

        private void OnFailedLoadingDocument(string fileName, int errorCode, string errorString)
        {
            Trace.WriteLine($"{fileName} failed to loaded: {errorString}");
        }

        private void OnFinishedLoadingDocument(string fileName)
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
    }
}
