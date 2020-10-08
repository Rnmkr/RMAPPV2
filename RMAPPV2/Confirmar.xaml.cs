using System.Windows;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for Confirmar.xaml
    /// </summary>
    public partial class Confirmar : Window
    {
        public Confirmar()
        {
            InitializeComponent();

            if (Datos.NumeroPedido == "00000")
            {
                Text0.Content = "Imprimir Ticket de Prueba?";
                Borrar.Content = "IMPRIMIR";
            }

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }


    }
}
