<Query Kind="Statements">
  <NuGetReference>Ix-Async</NuGetReference>
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
</Query>

var sw = new Stopwatch();
sw.Start();

var query = Enumerable.Range(1,10)
	.Where(num => num % 2 == 0)
	.Do(num => Task.Delay((10 - num) * 200).Wait());

foreach (var num in query)
{
	Console.WriteLine(num);
}

Console.WriteLine("Elapsed Time: " + sw.ElapsedTicks);
Console.WriteLine("Done. You can continue working.");