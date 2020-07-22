using System;
using Xamarin.Nordic.DFU.iOS;

namespace BleDemo.Nordic
{
    public class DfuServiceDelegateImplementation : DFUProgressDelegate
    {
        private DfuLogger dfuLogger;

        public DfuServiceDelegateImplementation(DfuLogger dfuLogger)
        {
            this.dfuLogger = dfuLogger;
        }

        public override void OutOf(nint part, nint totalParts, nint progress, double currentSpeedBytesPerSecond, double avgSpeedBytesPerSecond)
        {
            dfuLogger.Message(LogLevel.Info, string.Format("{0} {1} {2} {3} {4}", part, totalParts, progress, currentSpeedBytesPerSecond, avgSpeedBytesPerSecond));
        }
    }
}
