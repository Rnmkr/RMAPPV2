using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for Version.xaml
    /// </summary>
    public partial class Version : Page
    {
        private string EstructuraDirectorios;
        private string DirectorioLocal;
        private string DirectorioRed;
        private string ArchivoImagenLocal;
        private string ArchivoImagenRed;

        public Version()
        {
            InitializeComponent();
            ListBoxVersiones.SelectedIndex = Datos.IndexVersion;
            ListBoxVersiones.ScrollIntoView(ListBoxVersiones.SelectedItem);
        }

        private void EstablecerRutasImagen()
        {
            string NombreArchivo = Datos.Version + ".JPG";
            EstructuraDirectorios = Path.Combine(Datos.Producto, Datos.Modelo, Datos.Categoria, Datos.NumeroArticulo);
            DirectorioLocal = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Items");
            DirectorioRed = Path.Combine(@"\\", Datos.HostName, "Items$");
            ArchivoImagenLocal = Path.Combine(DirectorioLocal, EstructuraDirectorios, NombreArchivo);
            ArchivoImagenRed = Path.Combine(DirectorioRed, EstructuraDirectorios, NombreArchivo);
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
        }

        private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            CargarFallas();

            if (ObtenerItem())
            {
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Fallas());
            }
        }

        private bool ObtenerItem()
        {
            if (ListBoxVersiones.SelectedIndex == -1)
            {
                LabelMensaje.Content = "¡SELECCIONE UNA VERSION PARA CONTINUAR!";
                return false;
            }

            try
            {
                using (new WaitCursor())
                {
                    Item i = Datos.IQItemsFiltrados.Where(w => w.VersionItem == ListBoxVersiones.SelectedValue.ToString()).Select(s => s).Single();
                    Datos.Version = i.VersionItem;
                    Datos.Descripcion = i.DescripcionItem;
                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: SE ESPERABA UN SOLO ITEM DE RETORNO." + Environment.NewLine + "AVISE AL ADMINISTRADOR.", "ITEM REPETIDO EN BASE DE DATOS", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void ButtonAnterior_Click(object sender, RoutedEventArgs e)
        {
            ResetVista();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ProductoModeloCategoria());
        }

        private void ListBoxVersiones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Datos.IndexVersion = ListBoxVersiones.SelectedIndex;
            Datos.Version = ListBoxVersiones.SelectedValue.ToString();
            EstablecerRutasImagen();
            CargarImagen();
        }

        private void ResetVista()
        {
            Datos.IndexVersion = -1;
            Datos.ListVersiones = null;
        }

        private void CargarImagen()
        {
            if (File.Exists(ArchivoImagenLocal))
            {
                ImageVistaPrevia.Source = new BitmapImage(new Uri(ArchivoImagenLocal, UriKind.Absolute));
                CargarDescripcion(true);
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(DirectorioLocal, EstructuraDirectorios));

                if (File.Exists(ArchivoImagenRed))
                {
                    File.Copy(ArchivoImagenRed, ArchivoImagenLocal, true); //comprobar server con try... usar using... etc
                    ImageVistaPrevia.Source = new BitmapImage(new Uri(ArchivoImagenLocal, UriKind.Absolute));
                    CargarDescripcion(true);
                }
                else
                {
                    ImageVistaPrevia.Source = new BitmapImage(new Uri("pack://application:,,,/RMAPPV2;component/Resources/notfound.png", UriKind.Absolute));
                    CargarDescripcion(false);
                }
            }
        }

        private void CargarDescripcion(bool b)
        {
            if (b)
            {
                LabelDescripcion.Content = Datos.NumeroArticulo + " : " + Datos.Categoria + " DE " + Datos.Producto + " " + Datos.Modelo + " - VERSION : " + Datos.Version;
            }
            else
            {
                LabelDescripcion.Content = "NO SE ENCONTRÓ LA IMAGEN PARA ESTE COMPONENTE. POR FAVOR AVISE AL ADMINISTRADOR.";
            }
        }

        private void CargarFallas()
        {
            if (ServerConnection.IsServerOnline())
            {
                using (new WaitCursor())
                {
                    try
                    {
                        PRDB database = new PRDB();
                        Datos.IQFallas = database.Falla.Where(w => w.CategoriaFalla == Datos.Categoria).Select(s => s);
                        Datos.ListFallas = Datos.IQFallas.Where(w => w.CategoriaFalla == Datos.Categoria).Select(s => s.DescripcionFalla).ToList();
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Error Obteniendo Lista de Fallas");
                    }

                }
            }
            else
            {
                Datos.ResetDatos();
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
            }

        }
    }
}

