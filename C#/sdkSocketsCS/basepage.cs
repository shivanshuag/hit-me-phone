using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace sdkSocketsCS
{
    public class basepage : PhoneApplicationPage
    {
        bool _isNewPageInstance = false;

        public basepage()
        {
            _isNewPageInstance = true;
           
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_isNewPageInstance)
            {

                (Application.Current as App).ApplicationDataObjectChanged += new EventHandler(basepage_ApplicationDataObjectChanged);

                if ((Application.Current as App).HostName != null)
                {
                    _updateDependencyProperties();
                }
                else
                {
                    (Application.Current as App).GetDataAsync();

                }
            }

            _isNewPageInstance = false;
        }

        void basepage_ApplicationDataObjectChanged(object sender, EventArgs e)
        {
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                _updateDependencyProperties();
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _updateDependencyProperties();

                });
            }
           
        }

        private void _updateDependencyProperties()
        {
            ServerName = (Application.Current as App).HostName;
            PortNumber = (Application.Current as App).PortNumber;
          }

        public static readonly DependencyProperty ServerNameProperty = DependencyProperty.RegisterAttached("ServerName", typeof(string), typeof(string), new PropertyMetadata(string.Empty));
        public string ServerName
        {
            get { return (string)GetValue(ServerNameProperty); }
            set { SetValue(ServerNameProperty, value); }
        }

        public static readonly DependencyProperty PortNumberProperty = DependencyProperty.RegisterAttached("PortNumber", typeof(int), typeof(int), new PropertyMetadata(0));
        public int PortNumber
        {
            get { return (int)GetValue(PortNumberProperty); }
            set { SetValue(PortNumberProperty, value); }
        }

    
    }
}
