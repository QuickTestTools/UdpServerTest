using UdpServerTest;
using Quick.Localize;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

var textManager = TextManager.DefaultInstance;
var title = textManager.GetText(Texts.Title);
var version = "1.0.0";
Console.Title = title;
Console.WriteLine(textManager.GetText(Texts.WelcomeText, title, version));
Console.WriteLine(textManager.GetText(Texts.RepoUrl, "https://github.com/QuickTestTools/UdpServerTest"));

string ipAddress = null;
int port = 0;

if (args.Length >= 2)
{
    ipAddress = args[0];
    port = int.Parse(args[1]);
}
else
{
    while (string.IsNullOrEmpty(ipAddress))
    {
        Console.Write(textManager.GetText(Texts.PleaseInputIpAddress));
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
        {
            ipAddress = IPAddress.Any.ToString();
            break;
        }
        if (IPAddress.TryParse(line, out _))
            ipAddress = line;
    }
    while (port <= 0 || port > 65535)
    {
        Console.Write(textManager.GetText(Texts.PleaseInputPort));
        var line = Console.ReadLine();
        int.TryParse(line, out port);
    }
}

var dateFormat = textManager.GetText(Texts.DateFormat);
Console.Title = $"{ipAddress}:{port} - {title}";
UdpClient udpListener = new UdpClient(new IPEndPoint(IPAddress.Parse(ipAddress), port));
Console.WriteLine(textManager.GetText(Texts.ExitProgramTip));
while (true)
{
    IPEndPoint remoteIPEndPoint = null;
    var data = udpListener.Receive(ref remoteIPEndPoint);
    var clientEndpoint = remoteIPEndPoint.ToString();
    var text = Encoding.Default.GetString(data, 0, data.Length);
    var hex = BitConverter.ToString(data, 0, data.Length);
    Console.WriteLine(textManager.GetText(Texts.ClientDataRecved, clientEndpoint, text, hex));
}
