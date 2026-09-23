using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NeptunoApp.Data.Models;
using NeptunoApp.Data.Repository;
using NeptunoApp_Lab06.Commands;

namespace NeptunoApp_Lab06.ViewModels
{
    public class ProductoViewModel : BaseViewModel
    {
        private readonly NeptunoRepository _repository;
        private DataTable _productosDT;
        private DataRowView _selectedProducto;
        private string _nombreProducto;
        private decimal _precioUnidad;
        private short _unidadesEnExistencia;

        public ProductoViewModel()
        {
            _repository = new NeptunoRepository();
            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync(), _ => !string.IsNullOrEmpty(NombreProducto));
            EliminarCommand = new RelayCommand(async _ => await EliminarAsync(), _ => SelectedProducto != null);
            _ = CargarAsync();
        }

        public DataTable ProductosDT { get => _productosDT; set { _productosDT = value; OnPropertyChanged(); } }
        public DataRowView SelectedProducto { get => _selectedProducto; set { _selectedProducto = value; OnPropertyChanged(); if (value != null) CargarFormulario(); } }
        public string NombreProducto { get => _nombreProducto; set { _nombreProducto = value; OnPropertyChanged(); } }
        public decimal PrecioUnidad { get => _precioUnidad; set { _precioUnidad = value; OnPropertyChanged(); } }
        public short UnidadesEnExistencia { get => _unidadesEnExistencia; set { _unidadesEnExistencia = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                DataSet ds = await _repository.GetProductosAsync();
                if (ds.Tables.Count > 0) ProductosDT = ds.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private async Task GuardarAsync()
        {
            try
            {
                await _repository.SaveProductoAsync(new Producto
                {
                    ProductoID = SelectedProducto != null ? Convert.ToInt32(SelectedProducto["ProductoID"]) : 0,
                    NombreProducto = NombreProducto,
                    PrecioUnidad = PrecioUnidad,
                    UnidadesEnExistencia = UnidadesEnExistencia,
                    ProveedorID = 1,
                    CategoriaID = 1
                });
                MessageBox.Show("Guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                await CargarAsync();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private async Task EliminarAsync()
        {
            if (MessageBox.Show("¿Eliminar?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _repository.DeleteProductoAsync(Convert.ToInt32(SelectedProducto["ProductoID"]));
                    await CargarAsync();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void CargarFormulario()
        {
            NombreProducto = SelectedProducto["NombreProducto"].ToString();
            PrecioUnidad = Convert.ToDecimal(SelectedProducto["PrecioUnidad"]);
            UnidadesEnExistencia = Convert.ToInt16(SelectedProducto["UnidadesEnExistencia"]);
        }
    }
}