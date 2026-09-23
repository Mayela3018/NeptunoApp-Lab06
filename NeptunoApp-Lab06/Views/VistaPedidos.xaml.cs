using System.Windows.Controls;
using NeptunoApp_Lab06.ViewModels;

namespace NeptunoApp_Lab06.Views
{
    public partial class VistaPedidos : UserControl
    {
        public VistaPedidos()
        {
            InitializeComponent();
            this.DataContext = new PedidoViewModel();
        }
    }
}