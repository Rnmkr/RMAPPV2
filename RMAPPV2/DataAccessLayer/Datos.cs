using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RMAPPV2
{
    public static class Datos
    {
        #region fields
        private static IQueryable<Personal> _IQUsuarios;
        private static IQueryable<Item> _IQItemsTotal;
        private static IQueryable<Item> _IQItemsFiltrados;
        private static IQueryable<Falla> _IQFallas;

        private static List<string> _listProductos;
        private static List<string> _listModelos;
        private static List<string> _listCategorias;
        private static List<string> _listVersiones;
        private static List<string> _listFallas;

        private static string _legajoUsuario;
        private static string _nombreUsuario;
        private static string _apellidoUsuario;
        private static string _numeroPedido;
        private static string _numeroArticulo;
        private static string _producto;
        private static string _modelo;
        private static string _categoria;
        private static string _sectorCambio;

        private static string _version;
        private static string _descripcion;

        private static string _usuario;

        private static string _connectionString;
        private static string _hostName;

        private static int _indexProducto = -1;
        private static int _indexModelo = -1;
        private static int _indexCategoria = -1;
        private static int _indexVersion = -1;
        private static int _indexFalla = -1;
        private static int _indexSector = 0;

        private static string _codigoFalla;
        private static string _falla;
        private static string _observacion;
        private static string _observacionPrevia;

        private static bool _otroArticulo;
        private static bool _estoyEnLoginPage;
        private static int _proximoIDCambio = -1;

        private static object _canvas;
        #endregion

        #region properties
        public static IQueryable<Personal> IQUsuarios { get => _IQUsuarios; set => _IQUsuarios = value; }
        public static IQueryable<Item> IQTotalItems { get => _IQItemsTotal; set => _IQItemsTotal = value; }
        public static IQueryable<Item> IQItemsFiltrados { get => _IQItemsFiltrados; set => _IQItemsFiltrados = value; }
        public static IQueryable<Falla> IQFallas { get => _IQFallas; set => _IQFallas = value; }

        public static List<string> ListProductos { get => _listProductos; set => _listProductos = value; }
        public static List<string> ListModelos { get => _listModelos; set => _listModelos = value; }
        public static List<string> ListCategorias { get => _listCategorias; set => _listCategorias = value; }
        public static List<string> ListVersiones { get => _listVersiones; set => _listVersiones = value; }
        public static List<string> ListFallas { get => _listFallas; set => _listFallas = value; }

        public static string LegajoUsuario { get => _legajoUsuario; set => _legajoUsuario = value; }
        public static string NombreUsuario { get => _nombreUsuario; set => _nombreUsuario = value; }
        public static string ApellidoUsuario { get => _apellidoUsuario; set => _apellidoUsuario = value; }
        public static string NumeroPedido { get => _numeroPedido; set => _numeroPedido = value; }
        public static string NumeroArticulo { get => _numeroArticulo; set => _numeroArticulo = value; }
        public static string Producto { get => _producto; set => _producto = value; }
        public static string Modelo { get => _modelo; set => _modelo = value; }
        public static string Categoria { get => _categoria; set => _categoria = value; }
        public static string Version { get => _version; set => _version = value; }
        public static string Descripcion { get => _descripcion; set => _descripcion = value; }
        public static string SectorCambio { get => _sectorCambio; set => _sectorCambio = value; }

        public static string Usuario { get => _usuario; set => _usuario = value; }

        public static string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public static string HostName { get => _hostName; set => _hostName = value; }

        public static int IndexProducto { get => _indexProducto; set => _indexProducto = value; }
        public static int IndexModelo { get => _indexModelo; set => _indexModelo = value; }
        public static int IndexCategoria { get => _indexCategoria; set => _indexCategoria = value; }
        public static int IndexVersion { get => _indexVersion; set => _indexVersion = value; }
        public static int IndexFalla { get => _indexFalla; set => _indexFalla = value; }
        public static int IndexSector { get => _indexSector; set => _indexSector = value; }

        public static string CodigoFalla { get => _codigoFalla; set => _codigoFalla = value; }
        public static string Falla { get => _falla; set => _falla = value; }
        public static string Observacion { get => _observacion; set => _observacion = value; }
        public static string ObservacionPrevia { get => _observacionPrevia; set => _observacionPrevia = value; }

        public static bool OtroArticulo { get => _otroArticulo; set => _otroArticulo = value; }
        public static bool EstoyEnLoginPage { get => _estoyEnLoginPage; set => _estoyEnLoginPage = value; }
        public static int ProximoIDCambio { get => _proximoIDCambio; set => _proximoIDCambio = value; }

        public static object Lienzo { get => _canvas; set => _canvas = value; }


        #endregion

        public static void ResetDatos()
        {
            //IQUsuarios = null;
            IQTotalItems = null;
            IQItemsFiltrados = null;
            IQFallas = null;

            ListProductos = null;
            ListModelos = null;
            ListCategorias = null;
            ListVersiones = null;
            ListFallas = null;

            LegajoUsuario = null;
            NombreUsuario = null;
            ApellidoUsuario = null;
            NumeroPedido = null;
            NumeroArticulo = null;
            Producto = null;
            Modelo = null;
            Categoria = null;
            Version = null;
            Descripcion = null;

            Usuario = null;

            //ConnectionString = null;
            //HostName = null;

            IndexProducto = -1;
            IndexModelo = -1;
            IndexCategoria = -1;
            IndexVersion = -1;
            IndexFalla = -1;


            CodigoFalla = null;
            Falla = null;
            Observacion = null;
            ObservacionPrevia = null;

            OtroArticulo = false;
            ProximoIDCambio = -1;

            Lienzo = null;
        }

        public static void ResetDenuevo()
        {
            ProximoIDCambio = -1;
            Observacion = null;
            OtroArticulo = false;
            IndexSector = 0;
        }

        public static void ResetDistinto()
        {
            IQTotalItems = null;
            IQItemsFiltrados = null;
            ListProductos = null;
            ListModelos = null;
            ListCategorias = null;
            ListVersiones = null;
            ListFallas = null;
            NumeroArticulo = null;
            Producto = null;
            Modelo = null;
            Categoria = null;
            Version = null;
            Descripcion = null;
            ConnectionString = null;
            HostName = null;
            IndexProducto = -1;
            IndexModelo = -1;
            IndexCategoria = -1;
            IndexVersion = -1;
            IndexFalla = -1;
            IndexSector = 0;
            Falla = null;
            Observacion = null;
            OtroArticulo = true;
            ProximoIDCambio = -1;
        }

        public static void ResetDesdeCero()
        {
            IQTotalItems = null;
            IQItemsFiltrados = null;
            ListProductos = null;
            ListModelos = null;
            ListCategorias = null;
            ListVersiones = null;
            ListFallas = null;
            NumeroPedido = null;
            NumeroArticulo = null;
            Producto = null;
            Modelo = null;
            Categoria = null;
            Version = null;
            Descripcion = null;
            IndexProducto = -1;
            IndexModelo = -1;
            IndexCategoria = -1;
            IndexVersion = -1;
            IndexFalla = -1;
            IndexSector = 0;
            Falla = null;
            ObservacionPrevia = null;
            Observacion = null;
            OtroArticulo = false;
            ProximoIDCambio = -1;
        }
    }
}
