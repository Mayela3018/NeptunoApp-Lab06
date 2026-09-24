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
        private int? _clienteID, _empleadoID, _transportistaID;
        private DateTime _fechaPedido = DateTime.Now;
        private DateTime? _fechaRequerida;
        private DateTime? _fechaEnvio;
        private string _destinatario;
        private string _ciudadDestino;
        private string _paisDestino;

        private DataTable _clientesDT;
        private DataTable _empleadosDT;
        private DataTable _transportistasDT;

        public PedidoViewModel()
        {
            _repository = new NeptunoRepository();
            CargarCommand = new RelayCommand(async _ => await CargarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync(), _ => ClienteID.HasValue);
            EliminarCommand = new RelayCommand(async _ => await EliminarAsync(), _ => SelectedPedido != null);
            NuevoCommand = new RelayCommand(_ => Limpiar());

            // Cargar datos al iniciar
            _ = CargarAsync();
            _ = CargarListasDesplegablesAsync();
        }

        public DataTable PedidosDT { get => _pedidosDT; set { _pedidosDT = value; OnPropertyChanged(); } }
        public DataRowView SelectedPedido { get => _selectedPedido; set { _selectedPedido = value; OnPropertyChanged(); if (value != null) CargarFormulario(); } }
        public int? ClienteID { get => _clienteID; set { _clienteID = value; OnPropertyChanged(); } }
        public int? EmpleadoID { get => _empleadoID; set { _empleadoID = value; OnPropertyChanged(); } }
        public int? TransportistaID { get => _transportistaID; set { _transportistaID = value; OnPropertyChanged(); } }
        public DateTime FechaPedido { get => _fechaPedido; set { _fechaPedido = value; OnPropertyChanged(); } }
        public DateTime? FechaRequerida { get => _fechaRequerida; set { _fechaRequerida = value; OnPropertyChanged(); } }
        public DateTime? FechaEnvio { get => _fechaEnvio; set { _fechaEnvio = value; OnPropertyChanged(); } }
        public string Destinatario { get => _destinatario; set { _destinatario = value; OnPropertyChanged(); } }
        public string CiudadDestino { get => _ciudadDestino; set { _ciudadDestino = value; OnPropertyChanged(); } }
        public string PaisDestino { get => _paisDestino; set { _paisDestino = value; OnPropertyChanged(); } }

        // Propiedades para los ComboBox
        public DataTable ClientesDT { get => _clientesDT; set { _clientesDT = value; OnPropertyChanged(); } }
        public DataTable EmpleadosDT { get => _empleadosDT; set { _empleadosDT = value; OnPropertyChanged(); } }
        public DataTable TransportistasDT { get => _transportistasDT; set { _transportistasDT = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand NuevoCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                DataSet ds = await _repository.GetPedidosAsync();
                if (ds.Tables.Count > 0) PedidosDT = ds.Tables[0];
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private async Task CargarListasDesplegablesAsync()
        {
            try
            {
                // Cargar Clientes
                var dsClientes = await _repository.GetClientesAsync();
                if (dsClientes.Tables.Count > 0)
                {
                    ClientesDT = dsClientes.Tables[0];
                }

                // Cargar Empleados
                var dsEmpleados = await _repository.GetEmpleadosAsync();
                if (dsEmpleados.Tables.Count > 0)
                {
                    EmpleadosDT = dsEmpleados.Tables[0];
                }

                // Cargar Transportistas
                var dsTransportistas = await _repository.GetTransportistasAsync();
                if (dsTransportistas.Tables.Count > 0)
                {
                    TransportistasDT = dsTransportistas.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando listas: " + ex.Message);
            }
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
                    FechaPedido = FechaPedido,
                    FechaRequerida = FechaRequerida,
                    FechaEnvio = FechaEnvio,
                    TransportistaID = TransportistaID,
                    Destinatario = Destinatario ?? string.Empty,
                    CiudadDestino = CiudadDestino ?? string.Empty,
                    PaisDestino = PaisDestino ?? string.Empty
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
            ClienteID = SelectedPedido["ClienteID"] != DBNull.Value ? Convert.ToInt32(SelectedPedido["ClienteID"]) : (int?)null;
            EmpleadoID = SelectedPedido["EmpleadoID"] != DBNull.Value ? Convert.ToInt32(SelectedPedido["EmpleadoID"]) : (int?)null;
            TransportistaID = SelectedPedido["TransportistaID"] != DBNull.Value ? Convert.ToInt32(SelectedPedido["TransportistaID"]) : (int?)null;

            FechaPedido = Convert.ToDateTime(SelectedPedido["FechaPedido"]);
            FechaRequerida = SelectedPedido["FechaRequerida"] != DBNull.Value ? Convert.ToDateTime(SelectedPedido["FechaRequerida"]) : (DateTime?)null;
            FechaEnvio = SelectedPedido["FechaEnvio"] != DBNull.Value ? Convert.ToDateTime(SelectedPedido["FechaEnvio"]) : (DateTime?)null;

            Destinatario = SelectedPedido["Destinatario"]?.ToString() ?? string.Empty;
            CiudadDestino = SelectedPedido["CiudadDestino"]?.ToString() ?? string.Empty;
            PaisDestino = SelectedPedido["PaisDestino"]?.ToString() ?? string.Empty;
        }

        private void Limpiar()
        {
            SelectedPedido = null;
            ClienteID = null;
            EmpleadoID = null;
            TransportistaID = null;
            FechaPedido = DateTime.Now;
            FechaRequerida = null;
            FechaEnvio = null;
            Destinatario = string.Empty;
            CiudadDestino = string.Empty;
            PaisDestino = string.Empty;

            OnPropertyChanged(nameof(ClienteID));
            OnPropertyChanged(nameof(EmpleadoID));
            OnPropertyChanged(nameof(TransportistaID));
            OnPropertyChanged(nameof(FechaPedido));
            OnPropertyChanged(nameof(FechaRequerida));
            OnPropertyChanged(nameof(FechaEnvio));
            OnPropertyChanged(nameof(Destinatario));
            OnPropertyChanged(nameof(CiudadDestino));
            OnPropertyChanged(nameof(PaisDestino));
        }
    }
}