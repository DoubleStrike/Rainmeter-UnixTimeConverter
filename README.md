# Rainmeter-UnixTimeConverter
Unix Time Conversion Plugin for Rainmeter

This plugin does blah, blah, blah.

### Install
Download latest build https://github.com/DoubleStrike/Rainmeter-UnixTimeConverter/releases

Drop UnixTimeConverter.dll into local user's Rainmeter Plugins Folder (%appdata%\Rainmeter\Plugins).  Usually, this will be something like c:\users\UserNameHere\AppData\Roaming\Rainmeter\Plugins.

### Usage

The plugin requires a Source

**Source:** a valid string


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
Please see commit history for detailed logs.  Major changes will be listed here.
