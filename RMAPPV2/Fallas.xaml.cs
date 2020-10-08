using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for Falla.xaml
    /// </summary>
    public partial class Fallas : Page
    {
        public Fallas()
        {
            InitializeComponent();
            ListBoxFallas.SelectedIndex = Datos.IndexFalla;
            ListBoxFallas.ScrollIntoView(ListBoxFallas.SelectedItem);
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
        }

        private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxFallas.SelectedIndex == -1)
            {
                LabelMensaje.Content = "¡SELECCIONE UNA FALLA PARA CONTINUAR!";
                return;
            }
            else
            {
                Datos.IndexFalla = ListBoxFallas.SelectedIndex;
                Datos.Falla = ListBoxFallas.SelectedValue.ToString();
                Datos.CodigoFalla = Datos.IQFallas.Where(w => w.DescripcionFalla == Datos.Falla).Select(s => s.CodigoFalla).SingleOrDefault();
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Imprimir());
            }
        }

        private void ButtonAnterior_Click(object sender, RoutedEventArgs e)
        {
            ResetVista();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Version());
        }

        private void ResetVista()
        {
            Datos.IndexFalla = -1;
            Datos.Falla = null;
        }
    }
}
