﻿/*
  Copyright (C) 2020 Nikhil S. Shringarpurey and DoubleStrike Consulting, LLC
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
        Rainmeter.API api_ref = null;

        private MeasureType Type = MeasureType.DayOfWeek;

        internal Measure() {
        }

        internal void Reload(Rainmeter.API api, ref double maxValue) {
            // Store API reference
            api_ref = api;

            string type = api.ReadString("Type", "");

            source = api.ReadString("Source", "");
            api.Log(API.LogType.Debug, $"Source: {source}");

            // Check data validity
            if (source != "") {
                if (source.Trim().Length < 9 && source.Trim().Length > 11) {
                    api.Log(API.LogType.Warning, "Source: data length is not between 9 and 11 characters.  Bad data?  Parsing anyway...");
                }

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
                dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();

                return dtDateTime;
            } catch {
                if (api_ref != null) api_ref.Log(API.LogType.Error, "UnixTimeConverter.dll: Could not convert input to DataTime object.");

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
