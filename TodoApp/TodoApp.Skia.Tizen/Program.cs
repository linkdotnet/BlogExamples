using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace TodoApp.Skia.Tizen
{
	class Program
{
	static void Main(string[] args)
	{
		var host = new TizenHost(() => new TodoApp.App(), args);
		host.Run();
	}
}
}
