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
    public class CategoriaViewModel : BaseViewModel
    {
        private readonly NeptunoRepository _repository;
        private DataTable _categoriasDT;
        private DataRowView _selectedCategoria;
        private string _nombreCategoria;
        private string _descripcion;

        public CategoriaViewModel()
        {
            _repository = new NeptunoRepository();
            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync(), _ => !string.IsNullOrEmpty(NombreCategoria));
            EliminarCommand = new RelayCommand(async _ => await EliminarAsync(), _ => SelectedCategoria != null);
            _ = CargarAsync();
        }

        public DataTable CategoriasDT { get => _categoriasDT; set { _categoriasDT = value; OnPropertyChanged(); } }
        public DataRowView SelectedCategoria { get => _selectedCategoria; set { _selectedCategoria = value; OnPropertyChanged(); if (value != null) CargarFormulario(); } }
        public string NombreCategoria { get => _nombreCategoria; set { _nombreCategoria = value; OnPropertyChanged(); } }
        public string Descripcion { get => _descripcion; set { _descripcion = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                DataSet ds = await _repository.GetCategoriasAsync();
                if (ds.Tables.Count > 0) CategoriasDT = ds.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private async Task GuardarAsync()
        {
            try
            {
                await _repository.SaveCategoriaAsync(new Categoria
                {
                    CategoriaID = SelectedCategoria != null ? Convert.ToInt32(SelectedCategoria["CategoriaID"]) : 0,
                    NombreCategoria = NombreCategoria,
                    Descripcion = Descripcion
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
                    await _repository.DeleteCategoriaAsync(Convert.ToInt32(SelectedCategoria["CategoriaID"]));
                    await CargarAsync();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void CargarFormulario()
        {
            NombreCategoria = SelectedCategoria["NombreCategoria"].ToString();
            Descripcion = SelectedCategoria["Descripcion"].ToString();
        }
    }
}