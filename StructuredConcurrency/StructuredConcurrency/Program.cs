using System.Diagnostics;

var sw = Stopwatch.StartNew();
var tasks = TaskScope.Create(group => 
{
    group.Run(async token => await Task.Delay(100, token));
    group.Run(async token => await Task.Delay(200, token));
});     

// This runs 200 ms
await tasks;
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

sw.Restart();
var failedTasks = TaskScope.Create(group =>
{
    group.Run(async token =>
    {
        await Task.Delay(100, token);
        throw new Exception("Boooommm!!!");
    });
    group.Run(async token => await Task.Delay(1000, token));
});

// Runs 100 ms as the first task fails and cancels the rest.
// Also bubbles up the exception.
try
{
    await failedTasks;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

// This runs 100 ms
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

// Prints True
Console.WriteLine(failedTasks.IsFaulted);