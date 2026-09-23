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
    public class PedidoViewModel : BaseViewModel
    {
        private readonly NeptunoRepository _repository;
        private DataTable _pedidosDT;
        private DataRowView _selectedPedido;
        private int? _clienteID, _empleadoID;
        private DateTime _fechaPedido = DateTime.Now;

        public PedidoViewModel()
        {
            _repository = new NeptunoRepository();
            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync(), _ => ClienteID.HasValue);
            EliminarCommand = new RelayCommand(async _ => await EliminarAsync(), _ => SelectedPedido != null);
            _ = CargarAsync();
        }

        public DataTable PedidosDT { get => _pedidosDT; set { _pedidosDT = value; OnPropertyChanged(); } }
        public DataRowView SelectedPedido { get => _selectedPedido; set { _selectedPedido = value; OnPropertyChanged(); if (value != null) CargarFormulario(); } }
        public int? ClienteID { get => _clienteID; set { _clienteID = value; OnPropertyChanged(); } }
        public int? EmpleadoID { get => _empleadoID; set { _empleadoID = value; OnPropertyChanged(); } }
        public DateTime FechaPedido { get => _fechaPedido; set { _fechaPedido = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                DataSet ds = await _repository.GetPedidosAsync();
                if (ds.Tables.Count > 0) PedidosDT = ds.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private async Task GuardarAsync()
        {
            try
            {
                await _repository.SavePedidoAsync(new Pedido
                {
                    PedidoID = SelectedPedido != null ? Convert.ToInt32(SelectedPedido["PedidoID"]) : 0,
                    ClienteID = ClienteID,
                    EmpleadoID = EmpleadoID,
                    FechaPedido = FechaPedido
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
                    await _repository.DeletePedidoAsync(Convert.ToInt32(SelectedPedido["PedidoID"]));
                    await CargarAsync();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void CargarFormulario()
        {
            ClienteID = Convert.ToInt32(SelectedPedido["ClienteID"]);
            EmpleadoID = Convert.ToInt32(SelectedPedido["EmpleadoID"]);
            FechaPedido = Convert.ToDateTime(SelectedPedido["FechaPedido"]);
        }
    }
}