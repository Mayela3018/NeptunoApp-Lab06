using System.Windows;
using NeptunoApp_Lab06.Views;

namespace NeptunoApp_Lab06
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Cargar la vista de productos por defecto al iniciar
            MainContent.Content = new VistaProductos();
        }

        private void BtnProductos_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VistaProductos();
        }

        private void BtnCategorias_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VistaCategorias();
        }

        private void BtnProveedores_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VistaProveedores();
        }

        private void BtnPedidos_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VistaPedidos();
        }
    }
}