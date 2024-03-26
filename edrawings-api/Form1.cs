using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;


namespace edrawings_api
{
   
    public partial class Form1 : Form
    {
        private string FILE_PATH = "";
        DataTable dt;
     
   
        public Form1()
        {
            InitializeComponent();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            FILE_PATH = openFileDialog1.FileName;
            BooksRepo repo = new BooksRepo();
            dt = new DataTable();
            repo.GetTableDt(FILE_PATH, ref dt);       
            this.advancedDataGridView1.AutoGenerateColumns = true;
    
        }
          
        void Data_output(DataTable dt)
        {
            this.Cursor = Cursors.WaitCursor;
            this.bindingSource1.DataSource = dt;
            this.Cursor = Cursors.Arrow;
        }
  


        public void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                Data_output(dt);
            }

            catch (System.Runtime.InteropServices.COMException ex)
            { MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Show();

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
                        string Found_In = j["Found_In"].Value.ToString();
                        string File_Name = Path.GetFileNameWithoutExtension(j["File_Name"].Value.ToString()) + ".pdf"; 
                       
                        string fileDRW = Path.Combine(Found_In, File_Name);

                        listDrawingPath.Add(fileDRW);
                    }

                }

                if (listDrawingPath.Count > 0)
                {
                    FormBom fb = new FormBom(listDrawingPath);
                    fb.Show();
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
