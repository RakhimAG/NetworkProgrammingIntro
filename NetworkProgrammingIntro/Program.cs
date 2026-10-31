using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

#region local network interfaces

string hostName = Dns.GetHostName();

Console.WriteLine($"Computer name: {hostName}");

Console.WriteLine(new string('=', 32));

var interfaces = NetworkInterface.GetAllNetworkInterfaces();

foreach (var networkInterface in interfaces)
{
    Console.WriteLine(new string('-', 32));

    Console.WriteLine($"\nInterface: {networkInterface.Name}");
    Console.WriteLine($"Description: {networkInterface.Description}");
    Console.WriteLine($"Type: {networkInterface.NetworkInterfaceType}");
    Console.WriteLine($"Status: {networkInterface.OperationalStatus}");

    var properties = networkInterface.GetIPProperties();

    foreach (var addressInfo in properties.UnicastAddresses)
    {
        IPAddress address = addressInfo.Address;

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            Console.WriteLine($"\nIPv4: {address}");
            Console.WriteLine($"Family: {address.AddressFamily}");
        }
        else if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            Console.WriteLine($"\nIPv6: {address}");
            Console.WriteLine($"Family: {address.AddressFamily}");
        }
    }

    Console.WriteLine(new string('-', 32));

}


#endregion

#region DNS habdling

Console.Write("Enter domain name: ");
string? domain = Console.ReadLine();

if (string.IsNullOrWhiteSpace(domain))
{
    Console.WriteLine("Domain name cannot be empty.");
    return;
}

try
{
    IPAddress[] addresses =
        await Dns.GetHostAddressesAsync(domain);

    Console.WriteLine($"\nResults for: {domain}");

    foreach (IPAddress address in addresses)
    {
        Console.WriteLine($"IP: {address}");
        Console.WriteLine($"Address Family: {address.AddressFamily}");
    }
}
catch (SocketException ex)
{
    Console.WriteLine($"DNS resolution failed: {ex.Message}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid domain name: {ex.Message}");
}

#endregion