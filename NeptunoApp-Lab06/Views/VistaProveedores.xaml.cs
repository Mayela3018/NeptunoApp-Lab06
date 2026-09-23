using System.Windows.Controls;
using NeptunoApp_Lab06.ViewModels;

namespace NeptunoApp_Lab06.Views
{
    public partial class VistaProveedores : UserControl
    {
        public VistaProveedores()
        {
            InitializeComponent();
            this.DataContext = new ProveedorViewModel();
        }
    }
}