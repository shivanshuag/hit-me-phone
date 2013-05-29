using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using Microsoft.Phone.Shell;
using System.Windows;

namespace sdkSocketsCS
{
    public partial class MainPage : basepage
    {
        private ProgressIndicator _progressIndicator;

        public MainPage()
        {
            InitializeComponent();

            _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = false;
            _progressIndicator.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, _progressIndicator);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        private void appbarNewGame_Click(object sender, EventArgs e)
        {
            if (checkServerName())
            {
                NewGame();
            }
        }

        private void NewGame()
        {
            InitializeBoard();   

        }

        #region Update Status
        private void UpdateStatus(string status)
        {

            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                _updateStatus(status);
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _updateStatus(status);

                });
            }

        }

        private void _updateStatus(string status)
        {
            SystemTray.ProgressIndicator.IsVisible = false;
            tbStatus.Text = status;
        }
        #endregion

        private void InitializeBoard()
        {
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                _initBoard();
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _initBoard();

                });
            }

        }

        private void _initBoard()
        {
          foreach (TextBlock textBlock in gBoard.Children.OfType<TextBlock>())
            {
              textBlock.Text = string.Empty;
              }

            gBoard.IsHitTestVisible = true;
        }

        private void Play(TextBlock tbTapped)
        {
            tbStatus.Text = String.Empty;

            InitializeBoard();

            if (checkServerName())
            {

                GetMoveFromServer();
                   
            }

        }

        private bool checkServerName()
        {
            bool result = true;
            if (String.IsNullOrWhiteSpace(this.ServerName))
            {
                MessageBox.Show("You can set the server name on the settings page", "Missing Server Name", MessageBoxButton.OK);
                result = false;
            }

            return result;
        }

        private bool PiecesAvailable()
        {
            bool piecesAvailable = false;
           return piecesAvailable;
        }

        bool _waitingOnServerMove = false;

        private void GetMoveFromServer()
        {
          SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
            SystemTray.ProgressIndicator.Text = "Getting move from server";

            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            client.ResponseReceived += new ResponseReceivedEventHandler(ac_GotMove);
           
        }

        void ac_GotMove(object sender, ResponseReceivedEventArgs e)
        {
         
        }

        private string DidSomeoneWin()
        {
            return string.Empty;

        }

        private string GetBoardState()
        {
            StringBuilder sb = new StringBuilder();

            
            return sb.ToString();
        }

        private void ReportMoveError(string error)
        {
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                _reportMoveError(error);
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _reportMoveError(error);

                });
            }
        }

        private void _reportMoveError(string error)
        {

            UnLockGameboard();
            ClearStatusText();

            error = String.Format("Error: {0}", error) + Environment.NewLine + "Press OK to try again or Cancel to canel game";
            if (MessageBox.Show(error, "Error getting move from server", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

                GetMoveFromServer();
            }
            else
            {
                NewGame();
            }
        }



        private void ClearStatusText()
        {
            UpdateStatus(String.Empty);
        }

        private void ShowWinningLine(TextBlock w1, TextBlock w2, TextBlock w3)
        {
          }

        private void GameOver()
        {
          }

        private void LockGameboard()
        {
            gBoard.IsHitTestVisible = false;
        }

        private void UnLockGameboard()
        {
            gBoard.IsHitTestVisible = true;
        }

        private void appbarHideDiag_Click(object sender, EventArgs e)
        {
            ApplicationBarMenuItem toggle = (ApplicationBarMenuItem)sender;

            spDiagnostics.Visibility = (spDiagnostics.Visibility == System.Windows.Visibility.Visible) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            toggle.Text = (spDiagnostics.Visibility == System.Windows.Visibility.Visible) ? "Hide Diagnostics" : "Show Diagnostics";
        }


        private void appbarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void tb_00_Tap(object sender, GestureEventArgs e)
        {
            TextBlock tapped = (TextBlock)sender;
            //Play(tapped);
            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            if (checkServerName())
            client.SendData("1");
        }

        private void tb_01_Tap(object sender, GestureEventArgs e)
        {
            TextBlock tapped = (TextBlock)sender;
            //Play(tapped);
            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            if (checkServerName())            
            client.SendData("2");
        }

        private void tb_02_Tap(object sender, GestureEventArgs e)
        {
            TextBlock tapped = (TextBlock)sender;
            //Play(tapped);
            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            if (checkServerName())            
            client.SendData("5");
        }
        private void tb_03_Tap(object sender, GestureEventArgs e)
        {
            TextBlock tapped = (TextBlock)sender;
            //Play(tapped);
            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            if (checkServerName())
                client.SendData("4");
        }
        private void tb_04_Tap(object sender, GestureEventArgs e)
        {
            TextBlock tapped = (TextBlock)sender;
            //Play(tapped);
            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            if (checkServerName())
                client.SendData("3");
        }
        private void tb_05_Tap(object sender, GestureEventArgs e)
        {
          /*  TextBlock tapped = (TextBlock)sender;
            //Play(tapped);
            AsynchronousClient client = new AsynchronousClient(this.ServerName, this.PortNumber);
            if (checkServerName())
                client.SendData("3");*/
        }

    }
}
