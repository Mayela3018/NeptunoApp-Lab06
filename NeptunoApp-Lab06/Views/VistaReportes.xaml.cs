using System.Windows.Controls;
using NeptunoApp_Lab06.ViewModels;

namespace NeptunoApp_Lab06.Views
{
    public partial class VistaReportes : UserControl
    {
        public VistaReportes()
        {
            InitializeComponent();
            this.DataContext = new ReporteViewModel();
        }
    }
}