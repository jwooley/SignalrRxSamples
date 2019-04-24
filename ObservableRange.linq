<Query Kind="Statements">
  <NuGetReference>System.Interactive</NuGetReference>
  <NuGetReference>System.Reactive</NuGetReference>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var sw = new Stopwatch();
sw.Start();

var query = Observable.Range(1, 10)
	.Where(num => num % 2 == 0)
	.ObserveOn(TaskPoolScheduler.Default)
	.Do(num => Task.Delay((10 - num) * 200).Wait());

query.Subscribe(num => Console.WriteLine(num),
   () => Console.WriteLine("Elapsed Time: " + sw.ElapsedTicks)
);

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