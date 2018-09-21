# Lab - Configuration in the .NET Framework

## Objectives

By completing this lab, you will practice

- Reading Application Settings and Connection Strings from an App.config file
- Reading custom configuration settings
- Creating your own structure of configuration, and reading it

## Reading simple config values

- Open the solution  in [code/DotnetFrameworkConfigurationLab](code/DotnetFrameworkConfigurationLab/DotnetFrameworkConfigurationLab.sln)
- Use `ConfigurationManager.AppSettings` and `ConfigurationManager.ConnectionStrings` to read the config and return the *minimum Order Delivery value* and the *connection string to MyDatabase*

## Reading key/value pairs from a named section

- Add the following section defintion to the `App.config` file, within the `<configuration>` element. It must be the **first** element within the `<configuration>` element.

``` xml
  <configSections>
    <sectionGroup name="RestaurantConfiguration">
      <section name="LoyaltyLevel" type="System.Configuration.NameValueSectionHandler"/>
    </sectionGroup>
  </configSections>
```

- Add the following section to the App.config. It can be anywhere as long is it is a child under the `<configuration>` element.

``` xml
  <RestaurantConfiguration>
    <LoyaltyLevel>
      <add key="Gold" value="250" />
      <add key="Silver" value="100"/>
      <add key="Bronze" value="10"/>
    </LoyaltyLevel>
  </RestaurantConfiguration>
```

So here we have defined a section group, called **RestaurantConfiguration** which will contain a section called **LoyaltyLevel** that will be handled within code by the `System.Configuration.NameValueSectionHandler` type.

- Complete the function that gets the minumum points for a loyalty level, by useing `ConfigurationManager.GetSection` to get an object representing the **LoyaltyLevel** section.  Use `"RestaurantConfiguration/LoyaltyLevel"` which is in the format `"<group name>/<section name>"`

> Hint: The retrieved section will be of type `NameValueCollection` and you will need to type cast it. You can use the `NameValueCollection` like a dictionary, accessing items using the index accessor (eg: `mySection["Gold"]`). This returns a string, which you will need to Parse to an integer eg: `int.TryParse("4556", out int myIntVariable)`

## Reading custom defined configuration structure, from a named section

### Create the custom elements in the config

Add the `<OpeningTimes>` element below as a sibling to the `<LoyaltyLevel>` added previously

``` xml
  <RestaurantConfiguration> <!-- don't duplicate the RestaurantConfguration element-->
    <OpeningTimes>
      <Days>
        <Day dayOfWeek="Monday" openFrom="08:00" openTo="13:00" />
        <Day dayOfWeek="Tuesday" openFrom="11:00" openTo="22:00" />
        <Day dayOfWeek="Wednesday" openFrom="11:00" openTo="22:00" />
        <Day dayOfWeek="Thursday" openFrom="11:00" openTo="22:00" />
        <Day dayOfWeek="Friday" openFrom="11:00" openTo="23:00" />
        <Day dayOfWeek="Saturday" openFrom="08:00" openTo="23:00" />
        <Day dayOfWeek="Sunday" openFrom="08:00" openTo="17:00" />
      </Days>
    </OpeningTimes>
  </RestaurantConfiguration>
```

### Create classes representing this configuration

We will now create a number of classes, that will allow the .NET Configuration system to deserialise this XML into a set of classs we can use in code.

1. Create a class to represent a single `<Day>` element.  It will inherit from  `ConfigurationElement`.  Create properies that map to the attributes in the element.

``` csharp
    public class DayOpeningTimesConfigElement: ConfigurationElement
    {
        [ConfigurationProperty("dayOfWeek")]
        public DayOfWeek? WeekDay => base["dayOfWeek"] as DayOfWeek?;

        [ConfigurationProperty("openFrom")]
        public string From => base["openFrom"] as string;

        [ConfigurationProperty("openTo")]
        public string To => base["openTo"] as string;
    }
```

> Note that we are using an `enum` called `DayOfWeek` to represent a fixed set of values for the days of the week.  Also note that the string values used in the attributes need to match to the names used in the config file (**XML is case-sensitive!**)

2. Create a class that represents a **collection of `<Day>` elements** which in our example maps to the `<Days>` element.  It will inherit from `ConfigurationElementCollection` and we will implement some typical collection functionality

``` csharp
    [ConfigurationCollection(typeof(DayOpeningTimesConfigElement), AddItemName = "Day", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class DaysConfigCollection : ConfigurationElementCollection
    {
        public DayOpeningTimesConfigElement this[DayOfWeek dayOfWeek] => (DayOpeningTimesConfigElement) BaseGet(dayOfWeek);

        protected override ConfigurationElement CreateNewElement()
        {
            return new DayOpeningTimesConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DayOpeningTimesConfigElement)element).WeekDay;
        }
    }
```

3. And finally (in terms of creating custom classes) we create a class that represents is a whole configuration section, which will contain one property for our `DaysConfigCollection`

``` csharp
    public class OpeningTimesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Days")]
        public DaysConfigCollection Days => base["Days"] as DaysConfigCollection;
    }
```

4 Update the `<configSections>` you put in earlier, to include a section in the `RestaurantConfiguration` group for our *OpeningTimes*

``` xml
  <configSections>
    <sectionGroup name="RestaurantConfiguration">
      <section name="LoyaltyLevel" type="System.Configuration.NameValueSectionHandler"/>
      <section name="OpeningTimes" type="DotnetFrameworkConfigurationLab.OpeningTimesConfigurationSection, DotnetFrameworkConfigurationLab"/>
    </sectionGroup>
  </configSections>
```

### Read the custom OpeningTimes section

- Complete the function that gets the opening times for a day. Use `ConfigurationManager.GetSection` with section name `"RestaurantConfiguration/OpeningTimes"` to get an object of type `OpeningTimesConfigurationSection` (which you will need to type cast)

> Hint: The retrieved section will be of type `NameValueCollection` and you will need to type cast it. You can use the `NameValueCollection` like a dictionary, accessing items using the index accessor (eg: `mySection["Gold"]`). This returns a string, which you will need to Parse to an integer eg: `int.TryParse("4556", out int myIntVariable)`

## If you are feeling confident

Add another collection of days to the config, that represents days of the year when the restaurant is closed!

Configuration to indicate the restaurant is closed on new years and christmas day would look like this

``` xml
  <RestaurantConfiguration>
    <OpeningTimes>
      <Days>
        <Day dayOfWeek="Monday" openFrom="08:00" openTo="13:00" />
        <Day dayOfWeek="Tuesday" openFrom="11:00" openTo="22:00" />
        <Day dayOfWeek="Wednesday" openFrom="11:00" openTo="22:00" />
        <Day dayOfWeek="Thursday" openFrom="11:00" openTo="22:00" />
        <Day dayOfWeek="Friday" openFrom="11:00" openTo="23:00" />
        <Day dayOfWeek="Saturday" openFrom="08:00" openTo="23:00" />
        <Day dayOfWeek="Sunday" openFrom="08:00" openTo="17:00" />
      </Days>
    </OpeningTimes>
    <ClosedDays>
      <Day month="01" day="01">
      <Day month="12" day="25">
    </ClosedDays>
  </RestaurantConfiguration>
```