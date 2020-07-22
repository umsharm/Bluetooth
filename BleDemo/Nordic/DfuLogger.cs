using System;
using Xamarin.Nordic.DFU.iOS;

namespace BleDemo.Nordic
{
    public class DfuLogger : LoggerDelegate
    {
        public DfuLogger()
        {
        }

        public override void Message(LogLevel level, string message)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Log level {0} :: {1}", level.ToString(), message));
        }
    }
}
