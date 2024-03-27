
using eDrawings.Interop.EModelViewControl;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace edrawings_api
{
    public partial class FormBom : Form
    {
      
        EModelViewControl m_Ctrl;
        public event Action EndProcessing;
        List<string> listDrawing;
    
        public FormBom(List<string> list)
        {
            listDrawing = list;
            InitializeComponent(); 
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ctrlEDrw.LoadEDrawings();
        }

        private void OnControlLoaded(EModelViewControl ctrl)
        {
            m_Ctrl = ctrl;
            m_Ctrl.OnFinishedLoadingDocument += OnDocumentLoaded;
            m_Ctrl.OnFailedLoadingDocument += OnDocumentLoadFailed;
            m_Ctrl.OnFinishedPrintingDocument += OnDocumentPrinted;
            m_Ctrl.OnFailedPrintingDocument += OnPrintFailed;
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
            Console.WriteLine($"{fileName} failed to loaded: {errorString}");
            PrintNext();
        }

        private void OnDocumentLoaded(string fileName)
        {
            Console.WriteLine($"{fileName} loaded");
             string PRINTER_NAME = "Microsoft Print to PDF";
             int AUTO_SOURCE = 7;
             m_Ctrl.SetPageSetupOptions(EMVPrintOrientation.eLandscape, 7, 100, 100, 1, AUTO_SOURCE, PRINTER_NAME, 0, 0, 0, 0);
            string pdfFileName = Path.GetFileNameWithoutExtension(fileName) + ".pdf";
            string outDir = @"D:\macros\TEMP";
            string pdfFilePath;
            pdfFilePath = Path.Combine(outDir, pdfFileName);
            int sheetHeigth = (int)m_Ctrl.SheetHeight;
            int sheetWidth =(int) m_Ctrl.SheetWidth;
            EMVPrintOrientation orient;
            if (sheetHeigth > sheetWidth)
            {
                orient = EMVPrintOrientation.ePortrait;
            }
            else
            {
               orient = EMVPrintOrientation.eLandscape;
            }


            m_Ctrl.SetPageSetupOptions(orient, (int)PaperKind.A4, sheetHeigth, sheetWidth, 1,
                (int)PaperSourceKind.Custom, PRINTER_NAME, 0, 0, 0,0);
            m_Ctrl.Print5(false, fileName, false, false, true, EMVPrintType.eScaleToFit, 1, 0, 0, true, 1, 1, pdfFilePath);
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
                var rs = MessageBox.Show("processing completed", "", MessageBoxButtons.OK);
                if (rs == DialogResult.OK)
                {
                    this.Dispose();
                }
                 EndProcessing?.Invoke();
              

            }
        }

        
    }
}
