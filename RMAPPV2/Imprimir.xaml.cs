using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Printing;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for Imprimir.xaml
    /// </summary>
    public partial class Imprimir : Page
    {
        private Cambio nc;

        public Imprimir()
        {
            InitializeComponent();
            CargarUltimoIdCambio();
            CargarVistaPrevia();
            UXHora.DataContext = Clock.Instance;
            UXFecha.DataContext = Clock.Instance;
        }

        private static void CargarUltimoIdCambio()
        {
            try
            {
                using (new WaitCursor())
                {
                    if (Datos.ProximoIDCambio == -1)
                    {
                        PRDB database = new PRDB();
                        Cambio c = database.Cambio.OrderByDescending(p => p.IdCambio).FirstOrDefault();
                        Datos.ProximoIDCambio = c.IdCambio + 1;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("¡ERROR OBTENIENDO ULTIMO ID!");
            }
        }

        private void CargarVistaPrevia()
        {
            if (Datos.NumeroPedido == "00000")
            {
                ButtonAnterior.Visibility = Visibility.Hidden;
                ComboBoxSector.SelectedIndex = 1;
                ButtonBorrarObs.Visibility = Visibility.Hidden;
            }

            if (Datos.ObservacionPrevia == null)
            {
                ButtonPreviousObs.Visibility = Visibility.Hidden;
            }
            else
            {
                ButtonPreviousObs.Content = "''" + Datos.ObservacionPrevia.ToString() + "''";
                ButtonPreviousObs.Visibility = Visibility.Visible;
            }

            if (Datos.Observacion == null)
            {
                ButtonBorrarObs.Visibility = Visibility.Hidden;
                UXObservacion1.Text = null;
            }
            else
            {
                UXObservacion1.Text = Datos.Observacion;
            }

            if (Datos.NumeroArticulo == "00000")
            {
                Datos.ProximoIDCambio = 0;
            }

            UXIdCambio.Content = ("#" + Datos.ProximoIDCambio.ToString().PadLeft(7, '0'));
            UXArticulo.Content = Datos.NumeroArticulo;
            UXCategoria.Content = Datos.Categoria;
            UXDescripcion.Content = Datos.Descripcion;
            UXPedido.Content = Datos.NumeroPedido;
            UXProducto.Content = Datos.Producto + " " + Datos.Modelo;
            UXTecnico.Content = "(" + Datos.LegajoUsuario + ") " + Datos.Usuario;
            UXVersion.Content = Datos.Version;

            if (Datos.Falla.Length > 29)
            {
                UXFalla1.Content = (Datos.Falla.Substring(0, 28) + "-");
                UXFalla2.Content = Datos.Falla.Substring(28);
            }
            else
            {
                UXFalla1.Content = Datos.Falla;
            }

        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
        }

        private void ButtonAnterior_Click(object sender, RoutedEventArgs e)
        {
            Datos.Observacion = null;
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Fallas());
        }

        private void KeypadPress(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string content;
            if (Datos.Observacion == null)
            {
                Datos.Observacion = "";
            }

            if (button.Name == "ButtonBackspace")
            {
                content = "BACKSPACE";
            }
            else
            {
                content = button.Content.ToString();
            }

            switch (content)
            {
                case "BACKSPACE":
                    string s = Datos.Observacion;

                    if (s.Length > 1)
                    {
                        s = s.Substring(0, s.Length - 1);
                    }
                    else
                    {
                        s = "";
                    }

                    Datos.Observacion = s;
                    UXObservacion1.Text = Datos.Observacion;
                    break;


                case " ":
                    if (string.IsNullOrWhiteSpace(Datos.Observacion))
                    {
                        break;
                    }

                    if (Datos.Observacion.Length < 65)
                    {
                        Datos.Observacion += " ";
                        UXObservacion1.Text = Datos.Observacion;
                        break;
                    }
                    else
                    {
                        break;
                    }

                default:
                    if (Datos.Observacion.Length < 65)
                    {
                        Datos.Observacion += button.Content.ToString();
                        UXObservacion1.Text = Datos.Observacion;
                        break;
                    }
                    else
                    {
                        break;
                    }
            }
        }

        private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            //Debido al Style se complico usar "ComboBox.SelectedValue"

            if (ComboBoxSector.SelectedIndex == 0)
            {
                LabelMensaje.Content = "¡SELECCIONE UN SECTOR!";
                return;
            }
            if (ComboBoxSector.SelectedIndex == 1)
            {
                Datos.SectorCambio = "PRODUCCION";
            }
            if (ComboBoxSector.SelectedIndex == 2)
            {
                Datos.SectorCambio = "SERVICIO TECNICO";
            }

            Confirmar c = new Confirmar();
            c.ShowDialog();
            if (c.DialogResult.HasValue && c.DialogResult.Value)
            {
                GuardarEImprimir();
            }
        }

        private void GuardarEImprimir()
        {
            Datos.ObservacionPrevia = Datos.Observacion;

            if (GuardarDatos())
            {
                if (ImprimirTicket())
                {
                    if (Datos.NumeroPedido == "00000")
                    {
                        return;
                    }
                    else
                    {
                        ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new MenuFinal());
                    }
                }
                else
                {
                    using (new WaitCursor())
                    {
                        PRDB database = new PRDB();
                        if (database.Cambio.Where(w => w.IdCambio == nc.IdCambio).Any())
                        {
                            database.Cambio.Remove(nc);
                            database.SaveChanges();
                        }
                    }

                    MessageBox.Show("¡ERROR INTENTANDO IMPRIMIR! NO SE GUARDO EL REGISTRO");
                }
            }
            else
            {
                MessageBox.Show("¡ERROR GUARDANDO DATOS!");
            }
        }

        private bool GuardarDatos()
        {
            if (Datos.NumeroPedido == "00000")
            {
                return true;
            }

            if (ServerConnection.IsServerOnline())
            {
                try
                {
                    using (new WaitCursor())
                    {
                        PRDB database = new PRDB();
                        nc = new Cambio
                        {
                            IdCambio = Datos.ProximoIDCambio,
                            Tecnico = Datos.Usuario,
                            Legajo = Datos.LegajoUsuario,
                            FechaCambio = (DateTime.Now),
                            NumeroPedido = Datos.NumeroPedido,
                            Producto = Datos.Producto,
                            Modelo = Datos.Modelo,
                            ArticuloItem = Datos.NumeroArticulo,
                            CategoriaItem = Datos.Categoria,
                            VersionItem = Datos.Version,
                            SectorCambio = Datos.SectorCambio,
                            DescripcionItem = Datos.Descripcion,
                            CodigoFalla = Datos.CodigoFalla,
                            DescripcionFalla = Datos.Falla,
                            Observaciones = Datos.Observacion,
                            EstadoCambio = "APROBADO",
                            SupervisorModificacion = null,
                            FechaModificacion = null,
                        };
                        database.Cambio.Add(nc);
                        database.SaveChanges();
                        return true;

                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            Datos.ResetDatos();
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Login());
            return false;
        }

        private bool ImprimirTicket()
        {
            try
            {
                using (new WaitCursor())
                {
                    Datos.Lienzo = CanvasUX;
                    PrintDialog dialog = new PrintDialog();
                    dialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
                    dialog.PrintVisual(CanvasUX, "UX CANVAS");
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ButtonDelObs_Click(object sender, RoutedEventArgs e)
        {
            Datos.Observacion = null;
            UXObservacion1.Text = null;
            UXObservacion2.Content = null;
            CargarVistaPrevia();
        }

        private void UXObservacion1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                if (string.IsNullOrWhiteSpace(Datos.Observacion))
                {
                    ButtonBorrarObs.Visibility = Visibility.Hidden;
                }
                else
                {
                    ButtonBorrarObs.Visibility = Visibility.Visible;

                    if (Datos.Observacion.Length > 28)
                    {
                        UXObservacion1.Text = (Datos.Observacion.Substring(0, 27) + "-");
                        UXObservacion2.Content = Datos.Observacion.Substring(27);
                    }
                    else
                    {
                        UXObservacion1.Text = Datos.Observacion;
                        UXObservacion2.Content = null;
                    }
                }
            }

        }

        private void ButtonPreviousObs_Click(object sender, RoutedEventArgs e)
        {
            Datos.Observacion = Datos.ObservacionPrevia;
            UXObservacion1.Text = Datos.Observacion;
        }

    }
}