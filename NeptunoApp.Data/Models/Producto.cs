namespace NeptunoApp.Data.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int ProveedorID { get; set; }
        public int CategoriaID { get; set; }
        public string CantidadPorUnidad { get; set; } = string.Empty;
        public decimal PrecioUnidad { get; set; }
        public short UnidadesEnExistencia { get; set; }
        public short UnidadesEnPedido { get; set; }
        public short NivelDeReorden { get; set; }
        public bool Descontinuado { get; set; }
        public bool Activo { get; set; } = true;

        // Propiedades de navegación (para mostrar en la UI)
        public string NombreCategoria { get; set; } = string.Empty;
        public string CompaniaNombre { get; set; } = string.Empty;
    }
}