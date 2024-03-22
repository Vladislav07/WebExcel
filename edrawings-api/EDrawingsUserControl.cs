using System;
using System.Windows.Forms;
using eDrawings.Interop.EModelViewControl;

namespace edrawings_api
{
  
    public partial class EDrawingsUserControl : UserControl
    {
        public event Action<EModelViewControl> EDrawingsControlLoaded;

        public EDrawingsUserControl()
        {
            InitializeComponent();
        }

        public void LoadEDrawings()
        {
            var host = new eDrawingHost();
            host.ControlLoaded += OnControlLoaded;
            this.Controls.Add(host);
            host.Dock = DockStyle.Fill;
        }

        private void OnControlLoaded(EModelViewControl ctrl)
        {
            EDrawingsControlLoaded?.Invoke(ctrl);
        }
    }
    
}
