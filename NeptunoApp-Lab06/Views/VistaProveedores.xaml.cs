using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void Telefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Solo dígitos 0-9
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]*$");
        }
    }
}
