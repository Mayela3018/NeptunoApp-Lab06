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
        private string _cantidadPorUnidad;
        private int _categoriaID;
        private int _proveedorID;

        private DataTable _categoriasDT;
        private DataTable _proveedoresDT;

        public ProductoViewModel()
        {
            _repository = new NeptunoRepository();
            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync(), _ => !string.IsNullOrEmpty(NombreProducto));
            EliminarCommand = new RelayCommand(async _ => await EliminarAsync(), _ => SelectedProducto != null);
            NuevoCommand = new RelayCommand(_ => LimpiarFormulario());

            
            _ = CargarAsync();
            _ = CargarListasDesplegablesAsync();
        }

        public DataTable ProductosDT { get => _productosDT; set { _productosDT = value; OnPropertyChanged(); } }
        public DataRowView SelectedProducto { get => _selectedProducto; set { _selectedProducto = value; OnPropertyChanged(); if (value != null) CargarFormulario(); } }
        public string NombreProducto { get => _nombreProducto; set { _nombreProducto = value; OnPropertyChanged(); } }
        public decimal PrecioUnidad { get => _precioUnidad; set { _precioUnidad = value; OnPropertyChanged(); } }
        public short UnidadesEnExistencia { get => _unidadesEnExistencia; set { _unidadesEnExistencia = value; OnPropertyChanged(); } }
        public string CantidadPorUnidad { get => _cantidadPorUnidad; set { _cantidadPorUnidad = value; OnPropertyChanged(); } }

        
        public int CategoriaID { get => _categoriaID; set { _categoriaID = value; OnPropertyChanged(); } }
        public int ProveedorID { get => _proveedorID; set { _proveedorID = value; OnPropertyChanged(); } }
        public DataTable CategoriasDT { get => _categoriasDT; set { _categoriasDT = value; OnPropertyChanged(); } }
        public DataTable ProveedoresDT { get => _proveedoresDT; set { _proveedoresDT = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand NuevoCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                DataSet ds = await _repository.GetProductosAsync();
                if (ds.Tables.Count > 0) ProductosDT = ds.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        
        private async Task CargarListasDesplegablesAsync()
        {
            try
            {
                var dsCat = await _repository.GetCategoriasAsync();
                if (dsCat.Tables.Count > 0) CategoriasDT = dsCat.Tables[0];

                var dsProv = await _repository.GetProveedoresAsync();
                if (dsProv.Tables.Count > 0) ProveedoresDT = dsProv.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error cargando listas: " + ex.Message); }
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
                    CantidadPorUnidad = CantidadPorUnidad ?? string.Empty,
                    CategoriaID = CategoriaID, 
                    ProveedorID = ProveedorID, 
                    UnidadesEnPedido = 0,
                    NivelDeReorden = 0,
                    Descontinuado = false
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
            CantidadPorUnidad = SelectedProducto["CantidadPorUnidad"].ToString();

            CategoriaID = Convert.ToInt32(SelectedProducto["CategoriaID"]);
            ProveedorID = Convert.ToInt32(SelectedProducto["ProveedorID"]);
        }

        private void LimpiarFormulario()
        {
            SelectedProducto = null;
            NombreProducto = string.Empty;
            PrecioUnidad = 0;
            UnidadesEnExistencia = 0;
            CantidadPorUnidad = string.Empty;
            CategoriaID = 0; 
            ProveedorID = 0; 

            OnPropertyChanged(nameof(NombreProducto));
            OnPropertyChanged(nameof(PrecioUnidad));
            OnPropertyChanged(nameof(UnidadesEnExistencia));
            OnPropertyChanged(nameof(CantidadPorUnidad));
            OnPropertyChanged(nameof(CategoriaID));
            OnPropertyChanged(nameof(ProveedorID));
        }
    }
}