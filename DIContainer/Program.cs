// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using DIContainer;

var container = new Container();
container.RegisterSingleton<ICalculator, Calculator>();
container.Register<IMultiplier, Multiplier>();

var calculator = container.Resolve<IMultiplier>();

Console.WriteLine(calculator.Multiply(2, 3));

Console.WriteLine($"First call to GetHashCode in ICalculator: {container.Resolve<ICalculator>().GetHashCode()}");
Console.WriteLine($"Second call to GetHashCode in ICalculator: {container.Resolve<ICalculator>().GetHashCode()}");

Console.WriteLine($"First call to GetHashCode in IMultiplier: {container.Resolve<IMultiplier>().GetHashCode()}");
Console.WriteLine($"Second call to GetHashCode in IMultiplier: {container.Resolve<IMultiplier>().GetHashCode()}");
