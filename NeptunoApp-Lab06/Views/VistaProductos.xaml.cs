using System.Windows.Controls;
using NeptunoApp_Lab06.ViewModels;

namespace NeptunoApp_Lab06.Views
{
    public partial class VistaProductos : UserControl
    {
        public VistaProductos()
        {
            InitializeComponent();
            this.DataContext = new ProductoViewModel();
        }
    }
}