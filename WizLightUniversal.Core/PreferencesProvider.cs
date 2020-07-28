using System;
using System.Net;
using System.Net.NetworkInformation;

namespace WizLightUniversal.Core
{
    public abstract class PreferencesProvider
    {
        public abstract int HomeID { get; set; }

        public static PreferencesProvider Default { get; set; }
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