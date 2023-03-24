// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;

Console.WriteLine($"Build at: {BuildInformation.BuildAt}");
Console.WriteLine($"Platform: {BuildInformation.Platform}");
Console.WriteLine($"Warning level: {BuildInformation.WarningLevel}");
Console.WriteLine($"Configuration: {BuildInformation.Configuration}");