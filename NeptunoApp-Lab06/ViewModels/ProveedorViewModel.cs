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
    public class ProveedorViewModel : BaseViewModel
    {
        private readonly NeptunoRepository _repository;
        private DataTable _proveedoresDT;
        private DataRowView _selectedProveedor;
        private string _filtroNombre, _filtroCiudad, _companiaNombre, _nombreContacto, _cargoContacto, _direccion, _ciudad, _codigoPostal, _pais, _telefono, _fax;

        public ProveedorViewModel()
        {
            _repository = new NeptunoRepository();
            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync(), _ => !string.IsNullOrEmpty(CompaniaNombre));
            EliminarCommand = new RelayCommand(async _ => await EliminarAsync(), _ => SelectedProveedor != null);
            NuevoCommand = new RelayCommand(_ => Limpiar());
            _ = CargarAsync();
        }

        public DataTable ProveedoresDT { get => _proveedoresDT; set { _proveedoresDT = value; OnPropertyChanged(); } }
        public DataRowView SelectedProveedor { get => _selectedProveedor; set { _selectedProveedor = value; OnPropertyChanged(); if (value != null) CargarFormulario(); } }

        // Filtros
        public string FiltroNombre { get => _filtroNombre; set { _filtroNombre = value; OnPropertyChanged(); _ = CargarAsync(); } }
        public string FiltroCiudad { get => _filtroCiudad; set { _filtroCiudad = value; OnPropertyChanged(); _ = CargarAsync(); } }

        // Formulario
        public string CompaniaNombre { get => _companiaNombre; set { _companiaNombre = value; OnPropertyChanged(); } }
        public string NombreContacto { get => _nombreContacto; set { _nombreContacto = value; OnPropertyChanged(); } }
        public string CargoContacto { get => _cargoContacto; set { _cargoContacto = value; OnPropertyChanged(); } }
        public string Direccion { get => _direccion; set { _direccion = value; OnPropertyChanged(); } }
        public string Ciudad { get => _ciudad; set { _ciudad = value; OnPropertyChanged(); } }
        public string CodigoPostal { get => _codigoPostal; set { _codigoPostal = value; OnPropertyChanged(); } }
        public string Pais { get => _pais; set { _pais = value; OnPropertyChanged(); } }
        public string Telefono { get => _telefono; set { _telefono = value; OnPropertyChanged(); } }
        public string Fax { get => _fax; set { _fax = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand NuevoCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                DataSet ds = await _repository.GetProveedoresAsync(FiltroNombre, FiltroCiudad);
                if (ds.Tables.Count > 0) ProveedoresDT = ds.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private async Task GuardarAsync()
        {
            try
            {
                await _repository.SaveProveedorAsync(new Proveedor
                {
                    ProveedorID = SelectedProveedor != null ? Convert.ToInt32(SelectedProveedor["ProveedorID"]) : 0,
                    CompaniaNombre = CompaniaNombre,
                    NombreContacto = NombreContacto,
                    CargoContacto = CargoContacto ?? string.Empty,
                    Direccion = Direccion ?? string.Empty,
                    Ciudad = Ciudad ?? string.Empty,
                    CodigoPostal = CodigoPostal ?? string.Empty,
                    Pais = Pais ?? string.Empty,
                    Telefono = Telefono ?? string.Empty,
                    Fax = Fax ?? string.Empty
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
                    await _repository.DeleteProveedorAsync(Convert.ToInt32(SelectedProveedor["ProveedorID"]));
                    await CargarAsync();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void CargarFormulario()
        {
            CompaniaNombre = SelectedProveedor["CompaniaNombre"].ToString();
            NombreContacto = SelectedProveedor["NombreContacto"].ToString();
            CargoContacto = SelectedProveedor["CargoContacto"]?.ToString() ?? string.Empty;
            Direccion = SelectedProveedor["Direccion"]?.ToString() ?? string.Empty;
            Ciudad = SelectedProveedor["Ciudad"]?.ToString() ?? string.Empty;
            CodigoPostal = SelectedProveedor["CodigoPostal"]?.ToString() ?? string.Empty;
            Pais = SelectedProveedor["Pais"]?.ToString() ?? string.Empty;
            Telefono = SelectedProveedor["Telefono"]?.ToString() ?? string.Empty;
            Fax = SelectedProveedor["Fax"]?.ToString() ?? string.Empty;
        }

        private void Limpiar()
        {
            SelectedProveedor = null;
            CompaniaNombre = string.Empty;
            NombreContacto = string.Empty;
            CargoContacto = string.Empty;
            Direccion = string.Empty;
            Ciudad = string.Empty;
            CodigoPostal = string.Empty;
            Pais = string.Empty;
            Telefono = string.Empty;
            Fax = string.Empty;

            OnPropertyChanged(nameof(CompaniaNombre));
            OnPropertyChanged(nameof(NombreContacto));
            OnPropertyChanged(nameof(CargoContacto));
            OnPropertyChanged(nameof(Direccion));
            OnPropertyChanged(nameof(Ciudad));
            OnPropertyChanged(nameof(CodigoPostal));
            OnPropertyChanged(nameof(Pais));
            OnPropertyChanged(nameof(Telefono));
            OnPropertyChanged(nameof(Fax));
        }
    }
}