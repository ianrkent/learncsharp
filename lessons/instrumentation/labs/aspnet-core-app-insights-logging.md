# Lab - Application monitoring with Application Insights

## Objectives

By completing this lab, you will practice

- Creating a new application insights resource
- Integrating application insights into your solution
- Writing logs

----------------------------------------------------------

## Creating a new application insights resource

- Open [https://portal.azure.com]()
- Click "All services" and search for "Application Insights"
- Click the "Add" button

| Parameter | Example Value | Description |
| - | - | - |
| Name | my-test-ai | This should be a unique name for your container |
| Application Type | "ASP.NET Web Application" | This will setup some default visualisations and configuration |
| Subscription | TestSubscription | The subscription to deploy the resource to |
| Resource Group | my-test-ai-rg | Resource groups are a way of grouping resources so they can be found, updated and deleted together. Make sure to put your name on temporary resources so they are easy to find and delete later |
| Location | West Europe | The regional datacenter to deploy to |

Once its created, you can navigate to the *Properties* blade to find the *Instrumentation Key*. This is the unique ID for the container which we will use when setting up our code to push data to this container.

## Integrating application insights into your solution

- Open your solution or the lab project in your preferred IDE.

- Install the application insights package

``` powershell
Install-Package Microsoft.ApplicationInsights.AspNetCore
```

There will be quite a few dependencies so it might take a while to download and configure them all. You should see a similar log to below in your nuget output window:

```
Successfully installed 'Microsoft.ApplicationInsights 2.7.2' to aspnet-core-ai-sample
Successfully installed 'Microsoft.ApplicationInsights.AspNetCore 2.4.1' to aspnet-core-ai-sample
Successfully installed 'Microsoft.ApplicationInsights.DependencyCollector 2.7.2' to aspnet-core-ai-sample
Successfully installed 'Microsoft.ApplicationInsights.PerfCounterCollector 2.7.2' to aspnet-core-ai-sample
Successfully installed 'Microsoft.ApplicationInsights.WindowsServer 2.7.2' to aspnet-core-ai-sample
Successfully installed 'Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel 2.7.2' to aspnet-core-ai-sample
Executing nuget actions took 1.03 sec
Time Elapsed: 00:00:28.2318815
```

Now that we have the packages we need to configure our instrumentation key. You can do this by adding the following to your `appsettings.json` file:

``` json
"ApplicationInsights": {
  "InstrumentationKey": "XXXXX-XXXXX-XXXXX-XXXXX-XXXXX"
}
```

Finally initialise the library by editing `Startup.cs`:

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    services.AddApplicationInsightsTelemetry();
}
```

That's it! Run your application by hitting `F5` then refresh the browser a few times. After a minute or so you should see traces and requests showing in your application insights container.

## Writing logs

If you want to add some custom logging to a piece of code you can do that using the TelemetryClient directly.

Update ValuesController to dependency inject the TelemetryClient that we configured earlier and assign the reference to a field.

``` csharp
private readonly TelemetryClient telemetryClient;

public ValuesController(TelemetryClient telemetryClient)
{
    this.telemetryClient = telemetryClient;
}
```

The TelemetryClient has a few methods on it but the one we are interested in is TrackEvent. Let's update the code so whenever we retrieve a value by ID we log which one it is, semantically, so we can use it within our queries.

``` csharp
[HttpGet("{id}")]
public string Get(int id)
{
    telemetryClient.TrackEvent("Getting value by id", new Dictionary<string, string>()
    {
        { "value_id", id.ToString()}
    });

    return "value";
}
```

Once again, run the application but this time hit the following link (using your port number) a few times: [http://localhost:X/api/values/1]()

Take a look at your application insights container and view your new custom traces.

