using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private Regex OnlyDigitsRegex = new Regex(@"^[D]\d$");
        private string KeyInput;
        private DispatcherTimer ReadNewKeyInputTimer = new DispatcherTimer();
        private int SyncTimeCounter = 0;
        public Login()
        {
            InitializeComponent();
            ReadNewKeyInputTimer.Interval = TimeSpan.FromMilliseconds(700);
            ReadNewKeyInputTimer.Tick += ReadNewKeyInputTimer_Tick;
            LabelVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Datos.EstoyEnLoginPage = true;
            lblHora.DataContext = Clock.Instance;
            lblMinutos.DataContext = Clock.Instance;
            TextBoxEstadoConexion.DataContext = Clock.Instance;
            Clock.Instance.PropertyChanged += IsServerOnlineEventHandler; //event handler suscription
            //PasswordBoxLogin.ContextMenu = null;
            //TextBoxEstadoConexion.ContextMenu = null;
            //ButtonBorrarPassword.Visibility = Visibility.Hidden;
            //ButtonIngresar.Visibility = Visibility.Hidden;
            ComprobarUltimaConexion();
            CargarListaUsuarios();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += SecondTickevent;
            timer.Start();
        }

        private void SecondTickevent(object sender, EventArgs e)
        {
            if (lblDots.Visibility == Visibility.Visible)
            {
                lblDots.Visibility = Visibility.Hidden;
            }
            else
            {
                lblDots.Visibility = Visibility.Visible;
            }
            //actualiza el reloj cada 5 minutos
            SyncTimeCounter++;
            if (SyncTimeCounter == 300)
            {
                SyncTime();
                SyncTimeCounter = 0;
            }
        }

        private void SyncTime()
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "NET";
                p.StartInfo.Arguments = "TIME \\\\BUBBA /SET /YES";
                p.Start();
            }
            catch (Exception)
            {
            }
        }

        private void ReadNewKeyInputTimer_Tick(object sender, EventArgs e)
        {
            ReadNewKeyInputTimer.Stop();
            KeyInput = string.Empty;
            TextBoxEstadoConexion.Foreground = Brushes.Green;
            lblTitle.Foreground = Brushes.Green;
            TextBoxEstadoConexion.Content = "ESCANEE SU CODIGO";
        }

        private void IsServerOnlineEventHandler(object sender, PropertyChangedEventArgs e)
        {
            ComprobarUltimaConexion();
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


        private void ActivarIngreso()
        {
            //BorderOnPassword.Visibility = Visibility.Visible;
            //PasswordBoxLogin.Password = null;
            //PasswordBoxLogin.Focus();
            TextBoxEstadoConexion.Foreground = Brushes.Green;
            lblTitle.Foreground = Brushes.Green;
            TextBoxEstadoConexion.Content = "ESCANEE SU CODIGO";
        }

        private void DesactivarIngreso()
        {
            //ButtonBorrarPassword.Visibility = Visibility.Hidden;
            //ButtonIngresar.Visibility = Visibility.Hidden;
            //BorderOnPassword.Visibility = Visibility.Hidden;
            TextBoxEstadoConexion.Foreground = Brushes.Red;
            lblTitle.Foreground = Brushes.Red;
            TextBoxEstadoConexion.Content = "DESCONECTADO";
        }

        private void ButtonBorrarPassword_Click(object sender, RoutedEventArgs e)
        {
            ComprobarUltimaConexion();
            //PasswordBoxLogin.Password = null;
            //PasswordBoxLogin.Focus();
        }

        private void PasswordBoxLogin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //if (PasswordBoxLogin.Password.Length > 0)
            //{
            //    ButtonBorrarPassword.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ButtonBorrarPassword.Visibility = Visibility.Hidden;
            //}

            //if (PasswordBoxLogin.Password.Length == 5)
            //{
            //    ButtonIngresar.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ButtonIngresar.Visibility = Visibility.Hidden;
            //}
        }

        private void ButtonIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (KeyInput == "REBOO")
            {
                Reiniciar();
                return;
            }

            if (ObtenerUsuario())
            {
                Datos.EstoyEnLoginPage = false;
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
            }
        }

        private void CargarListaUsuarios()
        {
            if (Datos.IQUsuarios == null)
            {
                if (ComprobarConexion())
                {
                    using (new WaitCursor())
                    {
                        PRDB database = new PRDB();
                        Datos.IQUsuarios = database.Personal.Select(s => s);
                    }
                }
            }
        }

        private bool ObtenerUsuario()
        {
            try
            {
                CargarListaUsuarios();
                Personal p = new Personal();

                if (Datos.IQUsuarios.Any(w => w.NumeroAcceso == KeyInput))
                {
                    p = Datos.IQUsuarios.Where(w => w.NumeroAcceso == KeyInput).Single();
                    AsignarVariablesUsuario(p);
                    return true;
                }
                else
                {
                    Datos.IQUsuarios = null; //bajar de nuevo la lista por si el usuario se agrego recientemente
                    CargarListaUsuarios();
                    if (Datos.IQUsuarios.Any(w => w.NumeroAcceso == KeyInput))
                    {
                        p = Datos.IQUsuarios.Where(w => w.NumeroAcceso == KeyInput).Single();
                        AsignarVariablesUsuario(p);
                        return true;
                    }
                    else
                    {
                        TextBoxEstadoConexion.Foreground = Brushes.Yellow;
                        TextBoxEstadoConexion.Content = "¡NO SE ENCONTRO EL USUARIO!";
                        return false;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                TextBoxEstadoConexion.Foreground = Brushes.Red;
                lblTitle.Foreground = Brushes.Red;
                TextBoxEstadoConexion.Content = "¡ERROR BUSCANDO USUARIO!";
                return false;
            }
        }

        private void AsignarVariablesUsuario(Personal p)
        {
            Datos.LegajoUsuario = p.NumeroLegajo;
            Datos.ApellidoUsuario = p.Apellido;
            Datos.NombreUsuario = p.Nombre;
            Datos.Usuario = (Datos.ApellidoUsuario.TrimEnd() + " " + Datos.NombreUsuario.TrimEnd()).ToUpper();
        }

        private bool ComprobarConexion()
        {
            if (ServerConnection.IsServerOnline())
            {
                Clock.Instance.IsServerOnline = true;
                return true;
            }
            else
            {
                Clock.Instance.IsServerOnline = false;
                return false;
            }
        }

        private void ComprobarUltimaConexion()
        {
            if (Clock.Instance.IsServerOnline) //leo la variable recien seteada que lanzo el evento
            {
                ActivarIngreso();
            }
            else
            {
                DesactivarIngreso();
            }
        }

        private void Reiniciar()
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }

        private void Apagar()
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-s -t 0");
        }

        private void Cerrar()
        {
            Application.Current.Shutdown();
        }

        private void Page_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            /////
            //KeyInput = "23770";
            //if (KeyInput.Length == 5)
            //{
            //    if (ObtenerUsuario())
            //    {
            //        ReadNewKeyInputTimer.Stop();
            //        Datos.EstoyEnLoginPage = false;
            //        ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
            //    }

            //    ReadNewKeyInputTimer.Start();
            //}
            /////
            ReadNewKeyInputTimer.Start();
            TextBoxEstadoConexion.Content = "IDENTIFICANDO...";
            var key = e.Key.ToString();

            if (!OnlyDigitsRegex.IsMatch(key))
            {
                return;
            }

            if (string.IsNullOrEmpty(KeyInput))
            {
                ReadNewKeyInputTimer.Start();
            }

            KeyInput += key.Substring(1, 1);

            if (KeyInput.Length == 5)
            {
                if (KeyInput == "00000")
                {
                    Reiniciar();
                    return;
                }

                if (KeyInput == "00001")
                {
                    Apagar();
                    return;
                }

                if (KeyInput == "00002")
                {
                    Cerrar();
                    return;
                }

                if (KeyInput == "00003")
                {
                    TestImpresion();
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Imprimir());
                    return;
                }

                if (ObtenerUsuario())
                {
                    ReadNewKeyInputTimer.Stop();
                    Datos.EstoyEnLoginPage = false;
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
                }
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Page_PreviewKeyDown;
        }
    }
}
