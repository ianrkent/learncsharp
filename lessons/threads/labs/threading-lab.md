# Lab - Threads

## Objectives

By completing this lab, you will practice

- Using differnt approaches to enable parallel processing
- Cancelling Tasks

----------------------------------------------------------

## Intro

During this lab, your goal will be to calculate the [Prime Fibonacci numbers](https://en.wikipedia.org/wiki/Fibonacci_prime) below 3 billion (that 3 000 million - the British interpretation of billion).  A *Prime Fibonacci* numer as one that is both a Prime number AND a Fibonacci number.

----------------------------------------------------------

## Single Threaded Solution

Open up the [ThreadingLab solution](code/ThreadingLab.sln) in `./code` folder - either Visual Studio 2017 or in VS Code

The solution has a naive implementation of finding and outputting all the Prime Fibonacci numbers upto 3 billion, by

- enumerating through a list of all integers upto 3 billion
- testing each one to see if it both Prime and Fibonacci
- letting you see the results, by waiting for your input with a `Console.Readln()`

Run the solution using dotnet run from the command prompt, or F5 from Visual Studio 2017. You should see the following output

``` txt
**** THREAD ID : 1 -  2  ****
**** THREAD ID : 1 -  3  ****
**** THREAD ID : 1 -  5  ****
**** THREAD ID : 1 -  13  ****
**** THREAD ID : 1 -  89  ****
**** THREAD ID : 1 -  233  ****
**** THREAD ID : 1 -  1597  ****
**** THREAD ID : 1 -  28657  ****
**** THREAD ID : 1 -  514229  ****
```

> Note that the program is still running, but it is going to take AGES to get to the next Prime Fibonacci number of *433494437*. We never got to the point where the summary was output

Stop the program running by pressing Ctrl+C (or the stop button in Visual Studio 2017)

> Also note that there is only 1 thread here, the main thread used to run the application.  And it has an ID of 1 (surpise!)

----------------------------------------------------------

## Calculate on a seperate thread

Lets seperate the thread waiting for user input, and the thread doing all the work for us. This way we can interact with our application.

### Start a seperate thread to wait for user input

- Add a `isCancelled` private field to the `Program` class

``` csharp
private static bool _isCancelled = false;
```

- and add the following code into the `Main` method at the top, prior to the call to `SingleThreadCalculation`

``` csharp
Task.Factory.StartNew(() =>
{
    Reporting.Report("Press any key to stop processing...");
    Console.ReadKey();
    _isCancelled = true;
});
```

This starts a new thread, which with the `Console.ReadKey()` is waiting for you to press a key to continue, and sets a method level variable that is keeping track if you have cancelled.

### Break out the loop if you have cancelled

- In the main loop, add a check to see if the cancellation flag has been set

``` csharp
if (_isCancelled) break;
```

- Run your application, and you should see something like

``` txt
**** THREAD ID : 1 -  2  ****
Thread Id: 3 -  Press any key to stop processing...
**** THREAD ID : 1 -  3  ****
**** THREAD ID : 1 -  5  ****
**** THREAD ID : 1 -  13  ****
**** THREAD ID : 1 -  89  ****
**** THREAD ID : 1 -  233  ****
**** THREAD ID : 1 -  1597  ****
**** THREAD ID : 1 -  28657  ****
**** THREAD ID : 1 -  514229  ****
 **** THREAD ID : 1 -    ****
**** THREAD ID : 1 -     Evaluated 4604870 numbers in 4585 milliseconds  ****
**** THREAD ID : 1 -     at an average rate of 1004.33369683751 per millisecond  ****
**** THREAD ID : 1 -    ****
```

### Evaluate if we did better

So GREAT!  We are **not blocking** the thread waiting for user input, so our application is responsive.  You can see that code waiting for user input is running on `THREAD ID : 3`, while all the work is still being done on the main thread `THREAD ID : 1`

> Still using only 1 thread to do all the work, I am managing to evaluate `1004` numbers per millisecond.  How many is your laptop doing per second?  

However it is still going to take **AGES** to evaluate all 3 billion numbers (around 3 million secconds will be needed, which is 34 days and 17 hours - **yaaaawn!**)

- Write down how many numbers per millisecond you are currently evaluating using a single thread to do all the work - we will compare this to better solutions.

----------------------------------------------------------

## Partition the work, and run in parallel on multiple threads

Lets see if we can partition the 3 billion numbers, and have different threads working on those partitions.

### Create partions

- Copy the following `PartionedLongRange` method into the `Algorithm` class in your solution.

``` csharp
public static IEnumerable<IEnumerable<long>> PartionedLongRange(long lowValueInclusive, long highValueInclusive)
{
    var numPartions = Environment.ProcessorCount;

    var stepSize = (highValueInclusive - lowValueInclusive) / (numPartions - 1);

    var partions = new List<IEnumerable<long>>();

    var counter = lowValueInclusive;
    while (counter < highValueInclusive)
    {
        partions.Add(LongRange(counter, Math.Max(counter + stepSize, highValueInclusive)));
        counter += stepSize + 1;
    }

    return partions;
}
```

Don't worry too much about this code.  It is just a helper method that will create a **list of partions** where each partition is a **list of numbers**.. so it returns an `IEnumerable<IEnumerable<long>>`

The one interesting bit of this method is the `var numPartions = Environment.ProcessorCount`.  We look at the Environment we are running on, and define the number of partitions to create as the number of processes the machine has.  

### Hand roll our parallel processing

Copy the below method into the `Program` class

``` csharp
private static void HandRolledParallelThreadsCalculation()
{
    var partionedWorkerTasks = new List<Thread>();

    // start timing the whole process
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    // variable to store the count of numbers that have been evaluated across the partitions
    var countOfNumbersEvaluated = 0L;

    // get our partitioned numbers
    var partitions = Algorithm.PartionedLongRange(0, HighValue);

    foreach (var partition in partitions)
    {
        // create a new thread to loop over the range of numbers in this partion
        var workerThread = new Thread(() =>
        {
            // start timing this partitions work
            var innerStopWatch = new Stopwatch();
            innerStopWatch.Start();

            var counter = 0;
            foreach (var testNumber in partition)
            {
                counter++;

                if (Algorithm.IsPrime(testNumber) && Algorithm.IsFibonacci(testNumber))
                {
                    Reporting.ReportInHighlight(testNumber.ToString());
                }

                if (_isCancelled)
                {
                    Reporting.Report("Cancelling my thread.");
                    break;
                }
            }

            innerStopWatch.Stop();

            // update the main counter, with the count of work we have done
            Interlocked.Add(ref countOfNumbersEvaluated, counter);

            OutputSummary(counter, innerStopWatch.ElapsedMilliseconds);

        });

        // start this workerThread, so it begins to process its partition
        workerThread.Start();

        partionedWorkerTasks.Add(workerThread);
    }

    // delay the main thread until the user has cancelled
    while (!_isCancelled)
    {
        Thread.Sleep(50);  // sleep the current thread for 50 milliseconds
    }

    // make sure all the threads have finished
    foreach (var thread in partionedWorkerTasks)
    {
        thread.Join();
    }

    stopwatch.Stop();

    OutputSummary(countOfNumbersEvaluated, stopwatch.ElapsedMilliseconds);
}
```

This method uses the `Algorithm.PartionedLongRange(0, HighValue)` method copied earlier, to get partition up the 3 billion numbers we are looking up. The number of partitions should be then number of cores on your machine, so likely 8 partitions.

It then creates and starts a worker thread for each partition.  The workers keep track of how many items they have processed, and in how long.  They also update the `countOfNumbersEvaluated` with what they have processed so we can get a total across all the threads.

- Ensure you update the `Main` method to call `HandRolledParallelThreadsCalculation` instead of  `SingleThreadCalculation`

``` csharp
static void Main(string[] args)
{
  ..
  // SingleThreadCalculation();
  HandRolledParallelThreadsCalculation();
  ..
}
```

### Evaluate if we did better

- Run your application, and you will see similar output to previously, where a single thread finds all the Fibonacci Primes 514229, as they will all be in the 1st partion.

``` txt
Thread Id: 3 -  Press any key to stop processing...
**** THREAD ID : 5 -  2  ****
**** THREAD ID : 5 -  3  ****
**** THREAD ID : 5 -  5  ****
**** THREAD ID : 5 -  13  ****
**** THREAD ID : 5 -  89  ****
**** THREAD ID : 5 -  233  ****
**** THREAD ID : 5 -  1597  ****
**** THREAD ID : 5 -  28657  ****
**** THREAD ID : 5 -  514229  ****
```

- Let the app run for a little while. Have a look at your CPU usage, and you will see that has shot up.  You will also notice your fan kick in!

- Press any key, and have a look at the summary information presented. The threads all cancel immediatly, and report on themselves.  And an overall summary is presented at the end.

> The summarys reported at the end may all look a little mixed up, as the threads are all running at the same time and executing the summary steps at the same time.

Question?  Were you able to process more numbers per millisecond?

----------------------------------------------------------

## Partioning data and processing - It is already solved

So there is a possibility that you didn't see any improvements in our hand-cranked parallel processing and partioning mechanism. This could be due to a number of factors (partly down to badly written code) that is much to indepth to go into here. For those keen to understand deeper details, [here is detailed description of how to perform parallel processing in .NET](file:///C:/Users/ian.kent/Downloads/Patterns_of_Parallel_Programming_CSharp.pdf)

Lets modify our algorithm to use the inbuilt functions for processing a set of data in parallel, using the `Parallel.ForEach` static method.  This is been built to automatically partion the data you send in - and also give you some capability to have some **partion-local** data which you can control as each partition is being process

- Add the following class into your project.  It will be used by this algorithm to track some state within a partion - in particular a `StopWatch` to time the partition, and a `Counter` that we will use to track how many items were processed in the partition.

``` csharp
public class ParallelLocalState
{
    public long Counter { get; set; }

    public Stopwatch Stopwatch { get; set; }
}
```

- Add the following method to your program, and call it from your `Main` method instead of the previous approaches

``` csharp
private static void ParallelCalculation()
{
    // start timing the whole process
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    // Get the plain old list of numbers to 3 billion
    var testNumbers = Algorithm.LongRange(HighValue);

    // variable to store the count of numbers that have been evaluated across the partitions
    var countOfNumbersEvaluated = 0L;

    // Function which will be called once to set up our local state object of type ParallelLocalState
    Func<ParallelLocalState> getInitialLocalStateFunc = () => {
        var localStopWatch = new Stopwatch();
        localStopWatch.Start();
        return new ParallelLocalState { Stopwatch = localStopWatch };
    };

    // Function which will called be for each item in the partitions data set
    Func<long, ParallelLoopState, ParallelLocalState, ParallelLocalState> theRealWork = (testNumber, loopState, partionLocalState) =>
    {
        if (Algorithm.IsPrime(testNumber) && Algorithm.IsFibonacci(testNumber))
        {
            Reporting.ReportInHighlight(testNumber.ToString());
        }

        if (_isCancelled) loopState.Break();

        partionLocalState.Counter++;
        return partionLocalState;
    };

    // Function which will be called when a partion has been totally enumerated over
    Action<ParallelLocalState> partionCompletedFunc = partionLocalState =>
    {
        stopwatch.Start();
        Reporting.Report($"Partition completed. Processed { partionLocalState.Counter } in  { partionLocalState.Stopwatch.ElapsedMilliseconds } milliseconds at an average rate of { partionLocalState.Counter * 1.0 / partionLocalState.Stopwatch.ElapsedMilliseconds } per millisecond");
        Interlocked.Add(ref countOfNumbersEvaluated, partionLocalState.Counter);
    };

    // Setup the call to Parallel.ForEach, sending in all the callbacks
    Parallel.ForEach(
        testNumbers,
        getInitialLocalStateFunc,
        theRealWork,
        partionCompletedFunc);

    OutputSummary(countOfNumbersEvaluated, stopwatch.ElapsedMilliseconds);
}
```

For more details, here is [a description on how to use Parallel.ForEach](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-parallel-for-loop-with-thread-local-variables)

- Don't forget to update the `Main` method to call `ParallelCalculation` instead of `HandRolledParallelThreadsCalculation` or `SingleThreadCalculation`

``` csharp
static void Main(string[] args)
{
  ..
  // SingleThreadCalculation();
  // HandRolledParallelThreadsCalculation();
  ParallelCalculation();
  ..
}
```

### Evaluate if we did better

- Run the application

You will see that **many much smaller partions are being** created - and completed very quickly.

Threads from a **Thread Pool** are also being used to process these partitions.  Using a Thread Pool means the overhead of setting up, starting and destroying threads is not repeated.

A number of complexities on how to partition the data, and how to manage the threads are all taken care of us. This SHOULD result in a much better solution than our `hand cranked` one.

**Question** What was the average number of items processed per millisecond with this approach? I got

``` txt
**** THREAD ID : 1 -    ****
**** THREAD ID : 1 -     Evaluated 15133848 numbers in 5130 milliseconds  ****
**** THREAD ID : 1 -     at an average rate of 2950.06783625731 per millisecond  ****
**** THREAD ID : 1 -    ****
```