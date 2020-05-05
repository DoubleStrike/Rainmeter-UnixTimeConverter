/*
  Copyright (C) 2020 Nikhil S. Shringarpurey
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rainmeter;

/* Overview: This plugin converts a UNIX timestamp into human-readable date and
 *      time values.  It expects to be passed in a value via the "Source"
 *      parameter and can be passed a static string or the value of an existing
 *      Measure.  If you're passing in a measure, you must use 
 *      DynamicVariables=1 in your skin.  See the sample skin below for details.
*/

// Sample skin using this plugin:
/*
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
*/

namespace PluginUnixTimeConverter
{
    internal class Measure
    {
        // 
        enum MeasureType
        {
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year,
            DayOfWeek,
            FormattedDate
        }

        // Value of source data passed in from another meter
        string source = "";

        DateTime convertedTime;

        // Store a reference to the API object for logging - set on Reload()
        Rainmeter.API api_reference = null;

        private MeasureType Type = MeasureType.DayOfWeek;

        internal Measure() {
        }

        internal void Reload(Rainmeter.API api, ref double maxValue) {
            // Store API reference
            api_reference = api;

            string type = api.ReadString("Type", "");

            source = api.ReadString("Source", "");
            api.Log(API.LogType.Debug, $"Source: {source}");

            if (source != "") {
                convertedTime = UnixTimeToDateTime(source);
                api.Log(API.LogType.Debug, $"Converted Time: {convertedTime}");
            }

            switch (type.ToLowerInvariant()) {
                case "second":
                    Type = MeasureType.Second;
                    break;

                case "minute":
                    Type = MeasureType.Minute;
                    break;

                case "hour":
                    Type = MeasureType.Hour;
                    break;

                case "day":
                    Type = MeasureType.Day;
                    break;

                case "month":
                    Type = MeasureType.Month;
                    break;

                case "year":
                    Type = MeasureType.Year;
                    break;

                case "dayofweek":
                    Type = MeasureType.DayOfWeek;
                    break;

                case "formatteddate":
                    Type = MeasureType.FormattedDate;
                    break;

                default:
                    api.Log(API.LogType.Error, "UnixTimeConverter.dll: Type=" + type + " not valid");
                    break;
            }
        }

        internal double Update() {
            // Exit if we have no data
            if (source == "") {
                return 0.0;
            }

            switch (Type) {
                case MeasureType.Second:
                    return convertedTime.Second;

                case MeasureType.Minute:
                    return convertedTime.Minute;

                case MeasureType.Hour:
                    return convertedTime.Hour;

                case MeasureType.Day:
                    return convertedTime.Day;

                case MeasureType.Month:
                    return convertedTime.Month;

                case MeasureType.Year:
                    return convertedTime.Year;

                default:
                    // MeasureType.DayOfWeek and MeasureType.FormattedDate are not
                    //      numbers and and therefore will be returned in GetString.
                    return 0.0;
            }
        }

        internal string GetString() {
            // Exit if we have no data
            if (source == "") {
                return null;
            }

            switch (Type) {
                case MeasureType.DayOfWeek:
                    return convertedTime.DayOfWeek.ToString();

                case MeasureType.FormattedDate:
                    return convertedTime.ToShortDateString();

                default:
                    // All other MeasureType values are numbers. Therefore,
                    //      null is returned here for them. This is to inform
                    //      Rainmeter that it can treat those types as numbers.
                    return null;
            }
        }

        /// <summary>
        /// Convert Unix time value to a DateTime object.
        /// </summary>
        /// <param name="unixtime">The Unix time stamp you want to convert to DateTime as a string.</param>
        /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
        public DateTime UnixTimeToDateTime(string rawUnixTime) {
            try {
                long unixTime = long.Parse(rawUnixTime.Trim());

                return UnixTimeToDateTime(unixTime);
            } catch {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert Unix time value to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The Unix time stamp you want to convert to DateTime as a long.</param>
        /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
        public DateTime UnixTimeToDateTime(long unixTime) {
            try {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                //dtDateTime = dtDateTime.AddMilliseconds(unixTime).ToLocalTime();
                dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();

                return dtDateTime;
            } catch {
                if (api_reference != null) api_reference.Log(API.LogType.Error, "UnixTimeConverter.dll: Could not convert input to DataTime object.");

                // If an error happened, return the minimum value
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert a DateTime to a unix timestamp
        /// </summary>
        /// <param name="MyDateTime">The DateTime object to convert into a Unix Time</param>
        /// <returns></returns>
        public long DateTimeToUnix(DateTime MyDateTime) {
            TimeSpan timeSpan = MyDateTime - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)timeSpan.TotalSeconds;
        }
    }

    public static class Plugin
    {
        static IntPtr StringBuffer = IntPtr.Zero;

        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm) {
            data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        }

        [DllExport]
        public static void Finalize(IntPtr data) {
            GCHandle.FromIntPtr(data).Free();

            if (StringBuffer != IntPtr.Zero) {
                Marshal.FreeHGlobal(StringBuffer);
                StringBuffer = IntPtr.Zero;
            }
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue) {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Reload(new Rainmeter.API(rm), ref maxValue);
        }

        [DllExport]
        public static double Update(IntPtr data) {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            return measure.Update();
        }

        [DllExport]
        public static IntPtr GetString(IntPtr data) {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            if (StringBuffer != IntPtr.Zero) {
                Marshal.FreeHGlobal(StringBuffer);
                StringBuffer = IntPtr.Zero;
            }

            string stringValue = measure.GetString();
            if (stringValue != null) {
                StringBuffer = Marshal.StringToHGlobalUni(stringValue);
            }

            return StringBuffer;
        }
    }
}
