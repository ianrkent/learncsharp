# Lab - Configuration in .NET Core

## Objectives

By completing this lab, you will practice

- Building up configuration in .NET core from muliple sources
- Understand the order of loading config from different sources is important
- Read the configuration that has been loaded

----------------------------------------------------------

## Read config from a Json file

- Open the lab project in your preferred IDE.

| IDE                | Task |
| ------------------ | ---- |
| Visual Studio 2017 | open [code/DotnetCoreConfigurationLab/DotnetCoreConfigurationsLab.sln](code/DotnetCoreConfigurationLab/DotnetCoreConfigurationsLab.sln) |
| VS Code            | open working folder [code/DotnetCoreConfigurationLab](code/DotnetCoreConfigurationLab) |

- Add some code to the `Main` method, to create a `ConfigurationBuilder` and use it to build up your configuration!  (You may need to add `using Microsoft.Extensions.Configuration;`)

``` csharp
static void Main(string[] args)
{
    IConfiguration config = new ConfigurationBuilder()
        .Build();

    Console.ReadLine();
}
```

> So we have built some configuration, but specified **absolutley NOTHING** on how that configuration should be built up!

- Lets specify that we want to read the configuration from the `mySettings.json` file that is in our project, which contains config on the *minimum order amount for delivery*, and the *database connection string*

``` json
{
    "MinOrderAmountForDelivery" : 15,
    "TheDatabaseConnectionString" : "defined in mySettings.json"
}
```

To do that, we update the configuration builder

``` csharp
IConfiguration config = new ConfigurationBuilder()
  .AddJsonFile("mySettings.json", false, true)
  .Build();
```

We have specified the path to the json file, that it is not optional, and that the configuration will be reloaded if the runtime detects that the file has changed.

Add a call to `DisplayConfig(config)` to get output some information in the configuration

### Run the application

From Visual Studio, just start debugging (typcally F5)

Using VS Code, using your terminal, ensure you are in the project directory and type `dotnet run`

 You should see the contents of the `mySettings.json` file being output to the console.

``` txt
Minimum order amount 15
DB connection string defined in mySettings.json
```

----------------------------------------------------------

## Override values with Environment Variables

- Update the setup of your configuration to add in the Environment Variables provider, by calling `AddEnvironmentVariables`

``` csharp
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("mySettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();
```

Now the configuration is setup to also load environment variables into the configuration.  If there are enviornment variables that have the same key as in `mySettings.json` file, then the value from the  environment variable should be used - based on the order in which we have set up the build of the config.

### Set the environment variables when the app runs

#### In Visual Studio 2017

- Open the properties of the `DotnetCoreConfigurationLab` project, by right clicking it in the solution explorer and selecting `Properties`
- In the `Debug` tab, add an environment variable with the key `theDatabaseConnectionString` and a value of `DB Connection String from Environment Variables`

![](visual-studio-set-env-variable.png)

This will create a `Properties/launchSettings.json` file and create profile a entry in it, which will list the environment variables to set when using that profile.

- Run the app as usual

#### In VS Code

- Create a `launchSettings.json` file yourself in a `Properties` folder underneath the project folder. Put the following json contents in the file.

``` json
{
  "profiles": {
    "DotnetCoreConfigurationLab": {
      "commandName": "Project",
      "environmentVariables": {
        "theDatabaseConnectionString": "DB Connection String from Environment Variables"
      }
    }
  }
}
```

- Run the application as usual using `dotnet run`. The dotnet CLI will find the first profile with `"commandName" : "Project"` and apply its environment variables.

#### The results

You should now see the following being output to the console.

``` txt
Minimum order amount 15
DB connection string DB Connection String from Environment Variables
```

> Note that the value of the `theDatabaseConnectionString` key is coming from the environment variables, but the value of the `minOrderAmountForDelivery` key is still coming from the json file.

----------------------------------------------------------

## Override values with Command Line arguments

- Update the setup of your configuration to add the command line arguments into the configuration, by calling `AddCommandLine(args)`. 

``` csharp
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("mySettings.json", false, true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();
```

Now the configuration will also be loaded with key/value pairs sent in as command line arguements when running the app. 



- Update the `launchSettings.json` file to specify a command line argument for the `theDatabaseConnectionString` key.

``` json
"DotnetCoreConfigurationLab": {
      "commandName": "Project",
      "commandLineArgs": "theDatabaseConnectionString=FromCommandLineArgs",
      "environmentVariables": {
        "theDatabaseConnectionString": "DB Connection String from Environment Variables"
      }
    }
```

- Run the app, you should see now that the value for the DB connection string is what was specified on the command line.

> The dotnet CLI does not honour all values in the `launchSettings.json`, which includes ignoring the command lines!  To pass command line arguments into the application, you need to... specify them on the command line!!  **Surprise Surprise **

``` txt
dotnet run theDatabaseConnectionString=FromCommandLineArgs
```

#### The results

You should see the following output to the console.

``` txt
Minimum order amount : 15
The DB connection string : FromCommandLineArgs
```


The command line argument for `theDatabaseConnectionString` has overridden the values specifed in both the environment variable, and the `mySettings.json` file - while the `minOrderAmountForDelivery` value is still coming from `mySettings.json`