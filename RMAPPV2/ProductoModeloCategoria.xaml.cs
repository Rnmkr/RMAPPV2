using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for ProductoModelo.xaml
    /// </summary>
    public partial class ProductoModeloCategoria : Page
    {
        public ProductoModeloCategoria()
        {
            InitializeComponent();
            ListBoxProductos.SelectedIndex = Datos.IndexProducto;
            ListBoxModelos.SelectedIndex = Datos.IndexModelo;
            ListBoxCategorias.SelectedIndex = Datos.IndexCategoria;
            ListBoxProductos.ScrollIntoView(ListBoxProductos.SelectedItem);
            ListBoxModelos.ScrollIntoView(ListBoxModelos.SelectedItem);
            ListBoxCategorias.ScrollIntoView(ListBoxCategorias.SelectedItem);
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
        }

        private void ButtonAnterior_Click(object sender, RoutedEventArgs e)
        {
            ResetVista();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
        }

        private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxProductos.SelectedIndex == -1)
            {
                LabelMensaje.Content = "¡SELECCIONE UN PRODUCTO!";
                return;
            }


            if (ListBoxModelos.SelectedIndex == -1)
            {
                LabelMensaje.Content = "¡SELECCIONE UN MODELO!";
                return;
            }

            if (ListBoxCategorias.SelectedIndex == -1)
            {
                LabelMensaje.Content = "¡SELECCIONE UNA CATEGORIA!";
                return;
            }

            if (Datos.Categoria != null)
            {
                using (new WaitCursor())
                {
                Datos.IQItemsFiltrados = Datos.IQItemsFiltrados.Where(w => w.CategoriaItem == ListBoxCategorias.SelectedValue.ToString()).Select(s => s);
                Datos.ListVersiones = Datos.IQItemsFiltrados.Select(s => s.VersionItem).ToList();
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Version());
                }
            }
        }

        private void CargarModelos()
        {
            using (new WaitCursor())
            {
                Datos.ListModelos = Datos.IQTotalItems.Where(w => w.TipoProducto == ListBoxProductos.SelectedValue.ToString()).Select(s => s.ModeloProducto).Distinct().ToList();
                ListBoxModelos.ItemsSource = Datos.ListModelos; // reemplazar por binding
            }
        }

        private void CargarCategorias()
        {
            using (new WaitCursor())
            {
                Datos.IQItemsFiltrados = Datos.IQTotalItems.Where(w => w.ModeloProducto == ListBoxModelos.SelectedValue.ToString()).Select(s => s);
                Datos.ListCategorias = Datos.IQItemsFiltrados.Select(s => s.CategoriaItem).Distinct().ToList();
                ListBoxCategorias.ItemsSource = Datos.ListCategorias; // reemplazar por binding
            }
        }

        private void ListBoxProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Datos.IndexProducto = ListBoxProductos.SelectedIndex;
            Datos.Producto = ListBoxProductos.SelectedValue.ToString();
            CargarModelos();
        }

        private void ListBoxModelo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Datos.IndexModelo = ListBoxModelos.SelectedIndex;
            Datos.Modelo = ListBoxModelos.SelectedValue.ToString();
            CargarCategorias();
        }

        private void ListBoxCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Datos.IndexCategoria = ListBoxCategorias.SelectedIndex;
            Datos.Categoria = ListBoxCategorias.SelectedValue.ToString();
        }


        private void ResetVista()
        {
            Datos.IndexProducto = -1;
            Datos.IndexModelo = -1;
            Datos.IndexCategoria = -1;
            Datos.ListProductos = null;
            Datos.ListModelos = null;
            Datos.ListCategorias = null;
        }
    }
}
