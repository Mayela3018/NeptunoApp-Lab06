using System;
using System.Data;
using System.Data.SqlClient;  // ← ESTE es el correcto (NO Microsoft)
using System.Threading.Tasks;
using NeptunoApp.Data.Helpers;
using NeptunoApp.Data.Models;

namespace NeptunoApp.Data.Repository
{
    public class NeptunoRepository
    {
        // ================= PRODUCTOS =================
        public async Task<DataSet> GetProductosAsync()
        {
            return await DatabaseHelper.ExecuteQueryAsync("SP_Productos_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "R")
            });
        }

        public async Task<int> SaveProductoAsync(Producto producto)
        {
            string opcion = producto.ProductoID == 0 ? "C" : "U";
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Productos_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", opcion),
                new SqlParameter("@ProductoID", producto.ProductoID),
                new SqlParameter("@NombreProducto", producto.NombreProducto ?? (object)DBNull.Value),
                new SqlParameter("@ProveedorID", producto.ProveedorID),
                new SqlParameter("@CategoriaID", producto.CategoriaID),
                new SqlParameter("@CantidadPorUnidad", producto.CantidadPorUnidad ?? (object)DBNull.Value),
                new SqlParameter("@PrecioUnidad", producto.PrecioUnidad),
                new SqlParameter("@UnidadesEnExistencia", producto.UnidadesEnExistencia),
                new SqlParameter("@UnidadesEnPedido", producto.UnidadesEnPedido),
                new SqlParameter("@NivelDeReorden", producto.NivelDeReorden),
                new SqlParameter("@Descontinuado", producto.Descontinuado)
            });
        }

        public async Task<int> DeleteProductoAsync(int id)
        {
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Productos_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "D"),
                new SqlParameter("@ProductoID", id)
            });
        }

        // ================= CATEGORÍAS =================
        public async Task<DataSet> GetCategoriasAsync()
        {
            return await DatabaseHelper.ExecuteQueryAsync("SP_Categorias_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "R")
            });
        }

        public async Task<int> SaveCategoriaAsync(Categoria categoria)
        {
            string opcion = categoria.CategoriaID == 0 ? "C" : "U";
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Categorias_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", opcion),
                new SqlParameter("@CategoriaID", categoria.CategoriaID),
                new SqlParameter("@NombreCategoria", categoria.NombreCategoria ?? (object)DBNull.Value),
                new SqlParameter("@Descripcion", categoria.Descripcion ?? (object)DBNull.Value)
            });
        }

        public async Task<int> DeleteCategoriaAsync(int id)
        {
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Categorias_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "D"),
                new SqlParameter("@CategoriaID", id)
            });
        }

        // ================= PROVEEDORES =================
        public async Task<DataSet> GetProveedoresAsync(string nombre = null, string ciudad = null)
        {
            object nParam = string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre;
            object cParam = string.IsNullOrWhiteSpace(ciudad) ? (object)DBNull.Value : ciudad;

            return await DatabaseHelper.ExecuteQueryAsync("SP_Proveedores_Buscar", new SqlParameter[] {
                new SqlParameter("@NombreContacto", nParam),
                new SqlParameter("@Ciudad", cParam)
            });
        }

        public async Task<int> SaveProveedorAsync(Proveedor proveedor)
        {
            string opcion = proveedor.ProveedorID == 0 ? "C" : "U";
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Proveedores_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", opcion),
                new SqlParameter("@ProveedorID", proveedor.ProveedorID),
                new SqlParameter("@CompaniaNombre", proveedor.CompaniaNombre ?? (object)DBNull.Value),
                new SqlParameter("@NombreContacto", proveedor.NombreContacto ?? (object)DBNull.Value),
                new SqlParameter("@CargoContacto", proveedor.CargoContacto ?? (object)DBNull.Value),
                new SqlParameter("@Direccion", proveedor.Direccion ?? (object)DBNull.Value),
                new SqlParameter("@Ciudad", proveedor.Ciudad ?? (object)DBNull.Value),
                new SqlParameter("@CodigoPostal", proveedor.CodigoPostal ?? (object)DBNull.Value),
                new SqlParameter("@Pais", proveedor.Pais ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", proveedor.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@Fax", proveedor.Fax ?? (object)DBNull.Value)
            });
        }

        public async Task<int> DeleteProveedorAsync(int id)
        {
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Proveedores_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "D"),
                new SqlParameter("@ProveedorID", id)
            });
        }

        // ================= PEDIDOS =================
        public async Task<DataSet> GetPedidosAsync()
        {
            return await DatabaseHelper.ExecuteQueryAsync("SP_Pedidos_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "R")
            });
        }

        public async Task<int> SavePedidoAsync(Pedido pedido)
        {
            string opcion = pedido.PedidoID == 0 ? "C" : "U";
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Pedidos_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", opcion),
                new SqlParameter("@PedidoID", pedido.PedidoID),
                new SqlParameter("@ClienteID", (object)pedido.ClienteID ?? DBNull.Value),
                new SqlParameter("@EmpleadoID", (object)pedido.EmpleadoID ?? DBNull.Value),
                new SqlParameter("@FechaPedido", pedido.FechaPedido),
                new SqlParameter("@FechaRequerida", (object)pedido.FechaRequerida ?? DBNull.Value),
                new SqlParameter("@FechaEnvio", (object)pedido.FechaEnvio ?? DBNull.Value),
                new SqlParameter("@TransportistaID", (object)pedido.TransportistaID ?? DBNull.Value),
                new SqlParameter("@Destinatario", pedido.Destinatario ?? (object)DBNull.Value),
                new SqlParameter("@CiudadDestino", pedido.CiudadDestino ?? (object)DBNull.Value),
                new SqlParameter("@PaisDestino", pedido.PaisDestino ?? (object)DBNull.Value)
            });
        }

        public async Task<int> DeletePedidoAsync(int id)
        {
            return await DatabaseHelper.ExecuteNonQueryAsync("SP_Pedidos_Crud", new SqlParameter[] {
                new SqlParameter("@Opcion", "D"),
                new SqlParameter("@PedidoID", id)
            });
        }

        // ================= REPORTE POR FECHAS =================
        public async Task<DataSet> GetReportePedidosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await DatabaseHelper.ExecuteQueryAsync("SP_DetallePedidos_PorFechas", new SqlParameter[] {
                new SqlParameter("@FechaInicio", fechaInicio),
                new SqlParameter("@FechaFin", fechaFin)
            });
        }
    }
}