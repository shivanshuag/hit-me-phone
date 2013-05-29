using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace sdkSocketsCS
{
    public class AsynchronousClient
    {
        private int _port = 13001;

        private const int TIMEOUT_MILLISECONDS = 3000;

        internal event ResponseReceivedEventHandler ResponseReceived;

        static string dataIn = String.Empty;

        private string _serverName = string.Empty;

        public AsynchronousClient(string serverName, int portNumber)
        {
            if (String.IsNullOrWhiteSpace(serverName))
            {
                throw new ArgumentNullException("serverName");
            }

            if (portNumber < 0 || portNumber > 65535)
            {
                throw new ArgumentNullException("portNumber");
            }

            _serverName = serverName;
            _port = portNumber;
        }

        public void SendData(string data)
        {
            if (String.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("data");
            }

            dataIn = data;

            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

            DnsEndPoint hostEntry = new DnsEndPoint(_serverName, _port);
            
        
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArg_Completed);
            socketEventArg.RemoteEndPoint = hostEntry;

            socketEventArg.UserToken = sock;

            try
            {
                sock.ConnectAsync(socketEventArg);
            }
            catch (SocketException ex)
            {
                throw new SocketException((int)ex.ErrorCode);
            }

        }
        #region

        void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e) 
        { 
            switch (e.LastOperation) 
            { 
                case SocketAsyncOperation.Connect: 
                    ProcessConnect(e); 
                    break; 
                case SocketAsyncOperation.Receive: 
                    ProcessReceive(e); 
                    break; 
                case SocketAsyncOperation.Send: 
                    ProcessSend(e); 
                    break; 
                default: 
                    throw new Exception("Invalid operation completed"); 
            } 
        }

        private void ProcessReceive(SocketAsyncEventArgs e) 
        { 
            if (e.SocketError == SocketError.Success) 
            {
                string dataFromServer = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                
                Socket sock = e.UserToken as Socket; 
                sock.Shutdown(SocketShutdown.Send); 
                sock.Close(); 

                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ResponseReceivedEventArgs args = new ResponseReceivedEventArgs();
                    args.response = dataFromServer;
                    OnResponseReceived(args);
                });

            } 
            else 
            {
                throw new SocketException((int)e.SocketError); 
            } 
        }

        protected void OnResponseReceived(ResponseReceivedEventArgs e)
        {
            if (ResponseReceived != null)
                ResponseReceived(this,e);
        }

        private void ProcessSend(SocketAsyncEventArgs e) 
        { 
            if (e.SocketError == SocketError.Success) 
            { 
                Socket sock = e.UserToken as Socket; 

                sock.ReceiveAsync(e); 
            } 
            else 
            {
                ResponseReceivedEventArgs args = new ResponseReceivedEventArgs();
                args.response = e.SocketError.ToString();
                args.isError = true;
                OnResponseReceived(args);
            } 
        }

        private void ProcessConnect(SocketAsyncEventArgs e) 
        { 
            if (e.SocketError == SocketError.Success) 
            { 
                byte[] buffer = Encoding.UTF8.GetBytes(dataIn + "<EOF>"); 
                e.SetBuffer(buffer, 0, buffer.Length); 
                Socket sock = e.UserToken as Socket; 
                sock.SendAsync(e); 
            } 
            else 
            {
                ResponseReceivedEventArgs args = new ResponseReceivedEventArgs();
                args.response = e.SocketError.ToString();
                args.isError = true;
                OnResponseReceived(args);

            } 
        } 
        #endregion
    }

    public delegate void ResponseReceivedEventHandler(object sender, ResponseReceivedEventArgs e);

    public class ResponseReceivedEventArgs : EventArgs
    {
        public bool isError { get; set; }

        public string response { get; set; }
    }

}
