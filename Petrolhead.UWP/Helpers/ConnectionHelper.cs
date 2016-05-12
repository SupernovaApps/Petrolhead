using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Petrolhead.UWP.Helpers
{
    public class ConnectionHelper : IConnectionHelper
    {
        private static ConnectionHelper _current = new ConnectionHelper();
        public static ConnectionHelper Current
        {
            get
            {
                return _current;
            }
            private set
            {
                _current = value;
            }
        }

        private ConnectionHelper()
        {
            Current = this;
        }

        public bool IsConnected
        {
            get
            {
                var activeConnection = NetworkInformation.GetInternetConnectionProfile();
                if (activeConnection == null)
                    return false;
                var connectionStatus = activeConnection.GetNetworkConnectivityLevel();
                if (connectionStatus == NetworkConnectivityLevel.InternetAccess)
                    return true;
                return false;
            }
        }

    }
}
