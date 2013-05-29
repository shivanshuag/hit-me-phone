using System;
using System.Windows;

namespace sdkSocketsCS
{
    public partial class settings : basepage
    {
        public settings()
        {
            InitializeComponent();

        }

        private void appbarSave_Click(object sender, EventArgs e)
        {
            (Application.Current as App).HostName = txtServerName.Text;
            (Application.Current as App).PortNumber = Convert.ToInt32(txtPortNumber.Text);
           if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

       
    }
}
