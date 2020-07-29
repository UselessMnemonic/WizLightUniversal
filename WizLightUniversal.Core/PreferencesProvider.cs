using System.Net;
using System.Net.NetworkInformation;

namespace WizLightUniversal.Core
{
    // The preference provider gives access to application preferences
    // Each OS has a different API for this, so I implement each property
    // in an OS-dependent implementation
    public abstract class PreferencesProvider
    {
        // Not-so-global access to the provider
        public static PreferencesProvider Default { get; set; }

        // Explicitly defined preferences
        public abstract int HomeID { get; set; }

        // Any extra parameters the app may need
        public IPAddress HostIP { get; }
        public NetworkInterface HostNIC { get; }

        // Any extra set-up info is calculated here
        public PreferencesProvider()
        {
            HostIP = null;
            HostNIC = null;

            // first find network interface
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ( (nic.OperationalStatus == OperationalStatus.Up) &&
                    ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet) || (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                   )
                {
                    // then find a valid IPv4 address
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            HostIP = ip.Address;
                            HostNIC = nic;
                        }
                    }
                }
            }
        }
    }
}