using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NeptunoApp.Data.Repository;
using NeptunoApp_Lab06.Commands;

namespace NeptunoApp_Lab06.ViewModels
{
    public class ReporteViewModel : BaseViewModel
    {
        private readonly NeptunoRepository _repository;
        private DataTable _reporteDT;
        private DateTime _fechaInicio = DateTime.Now.AddDays(-30); // Por defecto último mes
        private DateTime _fechaFin = DateTime.Now;

        public ReporteViewModel()
        {
            _repository = new NeptunoRepository();
            GenerarReporteCommand = new RelayCommand(async _ => await GenerarReporteAsync());

            // Cargar reporte inicial al abrir la vista
            _ = GenerarReporteAsync();
        }

        public DataTable ReporteDT
        {
            get => _reporteDT;
            set { _reporteDT = value; OnPropertyChanged(); }
        }

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set { _fechaInicio = value; OnPropertyChanged(); }
        }

        public DateTime FechaFin
        {
            get => _fechaFin;
            set { _fechaFin = value; OnPropertyChanged(); }
        }

        public ICommand GenerarReporteCommand { get; }

        private async Task GenerarReporteAsync()
        {
            try
            {
                // Llamada asíncrona al Repository (Modo Desconectado con DataSet)
                DataSet ds = await _repository.GetReportePedidosAsync(FechaInicio, FechaFin);

                if (ds.Tables.Count > 0)
                {
                    ReporteDT = ds.Tables[0];
                }
                else
                {
                    ReporteDT = null;
                    MessageBox.Show("No se encontraron pedidos en el rango de fechas seleccionado.",
                                    "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}