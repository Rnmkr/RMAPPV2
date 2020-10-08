using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for PedidoArticulo.xaml
    /// </summary>
    public partial class PedidoArticulo : Page
    {
        private bool isArticuloFocused;

        public PedidoArticulo()
        {
            InitializeComponent();
            TextBoxArticulo.ContextMenu = null;
            TextBoxPedido.ContextMenu = null;
            ButtonBorrarPedido.Visibility = Visibility.Hidden;
            ButtonBorrarArticulo.Visibility = Visibility.Hidden;
            TextBoxPedido.Text = Datos.NumeroPedido;
            TextBoxArticulo.Text = Datos.NumeroArticulo;
            if (Datos.OtroArticulo)
            {
                TextBoxArticulo.Focus();
            }
            else
            {
                TextBoxPedido.Focus();
            }
        }

        private void KeypadPress(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (isArticuloFocused)
            {
                EscribirArticulo(button);
            }
            else
            {
                EscribirPedido(button);
            }
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
        }

        private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxPedido.Text == "00000")
            {
                TestImpresion();
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Imprimir());
                return;
            }

            if (ComprobarTextoPedidoArticulo())
            {
                if (ObtenerListaItems())
                {
                    Datos.NumeroPedido = TextBoxPedido.Text;
                    Datos.NumeroArticulo = TextBoxArticulo.Text;
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ProductoModeloCategoria());
                }
                else
                {
                    LabelMensaje.Content = "¡NO SE ENCONTRO EL NUMERO DE ARTICULO!";
                }

            }
            else
            {
                LabelMensaje.Content = "¡DATOS INCORRECTOS!";
            }

        }

        private void ButtonBorrarArticulo_Click(object sender, RoutedEventArgs e)
        {
            LabelMensaje.Content = null;
            TextBoxArticulo.Text = null;
            TextBoxArticulo.Focus();
        }

        private void ButtonBorrarPedido_Click(object sender, RoutedEventArgs e)
        {
            LabelMensaje.Content = null;
            TextBoxPedido.Text = null;
            TextBoxPedido.Focus();
        }
        
        private bool ObtenerListaItems()
        {
            if (ServerConnection.IsServerOnline())
            {
                using (new WaitCursor())
                {
                    try
                    {
                        PRDB database = new PRDB();
                        Datos.IQTotalItems = database.Item.Where(w => w.ArticuloItem == TextBoxArticulo.Text).Select(s => s);

                        if (Datos.IQTotalItems.Any())
                        {
                            Datos.ListProductos = Datos.IQTotalItems.Select(s => s.TipoProducto).Distinct().ToList();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }

                }
            }
            else
            {
                Datos.ResetDatos();
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
                return false;
            }
        }

        private void TestImpresion()
        {
            Datos.ApellidoUsuario = "TEST DE IMPRESION";
            Datos.NombreUsuario = "TEST DE IMPRESION";
            Datos.NumeroPedido = "00000";
            Datos.NumeroArticulo = "00000";
            Datos.Producto = "TEST DE IMPRESION";
            Datos.Categoria = "TEST DE IMPRESION";
            Datos.Version = "TEST DE IMPRESION";
            Datos.Descripcion = "TEST DE IMPRESION";
            Datos.Falla = "TEST DE IMPRESION";
            Datos.Observacion = "TEST DE IMPRESION";
        }

        private void TextBoxPedido_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxPedido.Text.Length > 0)
            {
                ButtonBorrarPedido.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonBorrarPedido.Visibility = Visibility.Hidden;
            }
        }

        private void TextBoxArticulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxArticulo.Text.Length > 0)
            {
                ButtonBorrarArticulo.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonBorrarArticulo.Visibility = Visibility.Hidden;
            }
        }

        private bool ComprobarTextoPedidoArticulo()
        {
            Regex regex = new Regex(@"^\d{7}[a-cA-C]-\d{2}$");
            bool vp = regex.IsMatch(TextBoxPedido.Text);
            bool va = string.IsNullOrWhiteSpace(TextBoxArticulo.Text);
            if (vp && !va)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EscribirPedido(Button b)
        {
            switch (b.Content)
            {
                case "A":
                    if (TextBoxPedido.Text.Length == 7)
                    {
                        TextBoxPedido.Text += "A-";
                    }
                    break;

                case "B":
                    if (TextBoxPedido.Text.Length == 7)
                    {
                        TextBoxPedido.Text += "B-";
                    }
                    break;

                case "C":
                    if (TextBoxPedido.Text.Length == 7)
                    {
                        TextBoxPedido.Text += "C-";
                    }
                    break;

                default:
                    if (TextBoxPedido.Text.Length < 11)
                    {
                        TextBoxPedido.Text += b.Content.ToString();
                        break;
                    }
                    else
                    {
                        break;
                    }
            }
            if (TextBoxPedido.Text.Length == 11)
            {
                TextBoxArticulo.Focus();
            }
            else
            {
                TextBoxPedido.Focus();
                TextBoxPedido.CaretIndex = TextBoxPedido.Text.Length;
            }
        }

        private void EscribirArticulo(Button b)
        {
            switch (b.Content)
            {
                case "A":
                    break;

                case "B":
                    break;

                case "C":
                    break;


                default:
                    if (TextBoxArticulo.Text.Length < 5)
                    {
                        TextBoxArticulo.Text += b.Content.ToString();
                        break;
                    }
                    else
                    {
                        break;
                    }
            }
            TextBoxArticulo.Focus();
            TextBoxArticulo.CaretIndex = TextBoxArticulo.Text.Length;
        }

        private void TextBoxPedido_GotFocus(object sender, RoutedEventArgs e)
        {
            isArticuloFocused = false;
        }

        private void TextBoxArticulo_GotFocus(object sender, RoutedEventArgs e)
        {
            isArticuloFocused = true;
        }
    }
}
