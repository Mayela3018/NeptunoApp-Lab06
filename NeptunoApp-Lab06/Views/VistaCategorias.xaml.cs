using System.Windows.Controls;
using NeptunoApp_Lab06.ViewModels;

namespace NeptunoApp_Lab06.Views
{
    public partial class VistaCategorias : UserControl
    {
        public VistaCategorias()
        {
            InitializeComponent();
            this.DataContext = new CategoriaViewModel();
        }
    }
}