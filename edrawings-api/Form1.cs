using System;
using System.Data;
//using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
//using Excel = Microsoft.Office.Interop.Excel;

namespace edrawings_api
{
    public partial class Form1 : Form
    {

        System.Data.DataTable dt2;


        public Form1(System.Data.DataTable dt)
        {
            InitializeComponent();
            dt2 = dt;
            
            this.advancedDataGridView1.AutoGenerateColumns = true;
    
        }
          
        void Data_output(System.Data.DataTable dt)
        {
            this.Cursor = Cursors.WaitCursor;
            this.bindingSource1.DataSource = dt;
            this.Cursor = Cursors.Arrow;
        }
  


        public void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                Data_output(dt2);
            }

            catch (System.Runtime.InteropServices.COMException ex)
            { MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void btnToPDF_Click(object sender, EventArgs e)
        {
            Convert_to_PDF(advancedDataGridView1);
        }



   
        private void Convert_to_PDF(ADGV.AdvancedDataGridView DG)
        {
            List<string> listDrawingPath = new List<string>();
            try
            {
                foreach (DataGridViewRow i in DG.Rows)
                {
                    if (i.IsNewRow) continue;
                    DataGridViewCellCollection j = i.Cells;
                    if (j["Drawing"].Value.ToString() == "1")
                    {
                        listDrawingPath.Add(j["File_Name"].Value.ToString());
                    }

                }

            }

            catch
            {

                this.Cursor = Cursors.Arrow;
                MessageBox.Show(" No access to file " + "\n" + saveFileDialog1.FileName.ToString());

            }


        }
        

    }
}
