# Rainmeter-UnixTimeConverter
Unix Time Conversion Plugin for Rainmeter

This plugin converts a UNIX timestamp into human-readable date and time values.  It expects to be passed in a value via the "Source" parameter and can be passed a static string or the value of an existing Measure.  If you're passing in a measure, you must use  DynamicVariables=1 in your skin and enclose the measure name in square brackets.  See the sample skin below for details.

### Install
Download latest build https://github.com/DoubleStrike/Rainmeter-UnixTimeConverter/releases

Drop UnixTimeConverter.dll into local user's Rainmeter Plugins Folder (%appdata%\Rainmeter\Plugins).  Usually, this will be something like:
```
c:\users\UserNameHere\AppData\Roaming\Rainmeter\Plugins.
```

### Usage

The plugin requires a Source

**Source:** a valid string.  Optionally, the name of a measure to read from surrounded by brackets.  If using a measure name, *remember to set DynamicVariables=1* as well.


Example:
```
    [Rainmeter]
    Update=1000
    BackgroundMode=2
    SolidColor=000000

    [MeasureInput]
    # This just provides a test value input for the follwing meters.  Replace
    #       it with whatever meter you choose.
    Measure=String
    String=1588611600

    [mSecond]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=Second

    [mMinute]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=Minute

    [mHour]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=Hour

    [mDay]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=Day

    [mMonth]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=Month

    [mYear]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=Year

    [mDayOfWeek]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=DayOfWeek
    Source=[MeasureInput]
    DynamicVariables=1

    [mFormattedDate]
    Measure=Plugin
    Plugin=UnixTimeConverter.dll
    Type=FormattedDate
    Source=[MeasureInput]
    DynamicVariables=1

    [Text1]
    Meter=STRING
    MeasureName=mSecond
    MeasureName2=mMinute
    MeasureName3=mHour
    MeasureName4=mDay
    MeasureName5=mMonth
    MeasureName6=mYear
    MeasureName7=mDayOfWeek
    MeasureName8=mFormattedDate
    MeasureName9=MeasureInput
    NumOfDecimals=1
    X=5
    Y=5
    W=200
    H=150
    FontColor=99FFFF
    FontSize=10
    Text="Second: %1#CRLF#Minute: %2#CRLF#Hour: %3#CRLF#Day: %4#CRLF#Month: %5#CRLF#Year: %6#CRLF#DayOfWeek: %7#CRLF#FormattedDate: %8#CRLF#InputValue: %9#CRLF#"
```

### Options

**None at the moment** More to come


### Changelog
Please see commit history for detailed logs.  Major changes will be listed here in YYYY-MM-DD format.

| Version  | Date  | Comments  |
|---|---|---|
| 1.0.0  | 2020-05-04  | Initial code upload for public use.  Working but not a ton of error handling or debugging.  I intend to add that later.  |
| 1.1.0  | 2020-05-05  | Error handling is improved and better examples are now included in the project. |
