using System;
using System.ComponentModel;
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

        private void Page_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
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
                if (ObtenerUsuario())
                {
                    ReadNewKeyInputTimer.Stop();
                    Datos.EstoyEnLoginPage = false;
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PedidoArticulo());
                }

                ReadNewKeyInputTimer.Start();
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Page_PreviewKeyDown;
        }
    }
}
