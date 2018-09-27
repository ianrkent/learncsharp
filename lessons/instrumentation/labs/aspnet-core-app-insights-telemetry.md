# Lab - Application monitoring with Application Insights

## Objectives

By completing this lab, you will practice

- Measuring performance of your code
- Sending it to application insights

----------------------------------------------------------

## Measuring performance of your code

When instrumenting code for production systems you want to pick a library that was specifically designed to be:

- Light on resources
- Won't affect the performance of the code you are measuring
- Accurate enough to measure extremely fast executing code (nanoseconds)

For C# it is a common mistake to measure code using DateTime:

``` csharp
var start = DateTime.UtcNow

DoWork();

var duration = (DateTime.UtcNow - now).TotalMilliseconds;
```

Unfortunately for us, DateTime was designed for storing dates and times not calculating precise nano and micro second durations. For this, .NET gives us the Stopwatch object.

``` csharp
var sw = Stopwatch.StartNew();

DoWork();

var duration = sw.ElapsedMilliseconds;
```

## Sending it to application insights

Let's update either your solution or the lab to send these timings to application insights.

We can use the TelemetryClient again for this example and use the GetMetric function to aggregate individual calls to TrackValue. The TelemetryClient will automatically aggregate and calculate Minimum, Maximum and Average values and only send the aggregate calls over the network to the application insights container. This greatly improves performance and allows you to log more metrics without the additional cost in machine and network resources.

Add a new controller function to `ValuesController.cs`:

``` csharp
[HttpGet("duration")]
public async Task<string> GetDurationAsync(int id)
{
    var sw = Stopwatch.StartNew();
    try
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
    finally 
    {
        telemetryClient.GetMetric("GetDurationAsync").TrackValue(sw.ElapsedMilliseconds);
    }
    
    return "value";
}
```

We've added a delay here to simulate a slower piece of code we want to instrument. This could be a call to an external database or a calculatation that takes a lot of CPU time.

Navigate to [/api/values/duration]() in your browser and refresh a few times. You can now take a look at the Metrics blade in application insights to graph your new metric!