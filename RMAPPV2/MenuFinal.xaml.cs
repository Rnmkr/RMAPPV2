using System;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for MenuFinal.xaml
    /// </summary>
    public partial class MenuFinal : Page
    {
        public MenuFinal()
        {
            InitializeComponent();
            ButtonDeNuevo.Content = "CAMBIAR OTRO " + Datos.Categoria + " " + Datos.NumeroArticulo + " DE " + Datos.Producto + " " + Datos.Modelo;
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
        }

        private void ButtonDeNuevo_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDenuevo();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Fallas());
        }

        private void ButtonOtroDistinto_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDistinto();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
        }

        private void ButtonDesdeCero_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDesdeCero();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
        }

        private void ButtonReimprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (new WaitCursor())
                {
                    PrintDialog dialog = new PrintDialog();
                    dialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
                    dialog.PrintVisual((Canvas)Datos.Lienzo, "UX CANVAS");
                }
            }
            catch (Exception)
            {
            }
        }

    }

}
