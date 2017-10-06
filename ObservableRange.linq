<Query Kind="Statements">
  <NuGetReference>Ix-Main</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <NuGetReference>Rx-WinForms</NuGetReference>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var sw = new Stopwatch();
sw.Start();

var query = Enumerable.Range(1, 10)
	.Where(num => num % 2 == 0)
	.Do(num => Task.Delay((10 - num) * 200).Wait());

foreach (var num in query)
{
	Console.WriteLine(num);
}

Console.WriteLine("Elapsed Time: " + sw.ElapsedTicks);
Console.WriteLine("Done. You can continue working.");







//var query = Observable.Range(1,10)
//	.Where(num => num % 2 == 0)
//	.Do(num => Task.Delay((10 - num) * 200).Wait());
//
//query.Subscribe(val => Console.WriteLine(val));

//var query = Observable.Range(1, 10)
//	.ObserveOn(TaskPoolScheduler.Default)
//	.Where(num => num % 2 == 0)
//	.Do(num => Task.Delay((10 - num) * 200).Wait())
//	.Subscribe(num => Console.WriteLine(num),
//		() =>
//		{
//			Console.WriteLine("Elapsed Time: " + sw.ElapsedTicks);
//		}
//	);