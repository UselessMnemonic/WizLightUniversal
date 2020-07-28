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
        public abstract int HomeID { get; set; }

        // Find a NIC and an IPv4 address
        // TODO: Find a better way of doing this
        public (NetworkInterface, IPAddress) NetworkInformation
        {
            get
            {
                // first find network interface
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet) || (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)) && (nic.OperationalStatus == OperationalStatus.Up))
                    {
                        // then find a valid IPv4 address
                        foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                return (nic, ip.Address);
                            }
                        }
                    }
                }
                return (null, null);
            }
        }
    }
}