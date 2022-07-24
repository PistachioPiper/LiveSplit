using System;
using System.Globalization;

namespace LiveSplit.TimeFormatters
{
    public class ShortTimeFormatterMS : GeneralTimeFormatter
    {
        public ShortTimeFormatterMS()
        {
            Accuracy = TimeAccuracy.Milliseconds;
            NullFormat = NullFormat.ZeroWithAccuracy;
        }

        public string Format(TimeSpan? time, TimeFormat format)
        {
            var formatRequest = new GeneralTimeFormatter
            {
                Accuracy = TimeAccuracy.Milliseconds,
                NullFormat = NullFormat.ZeroWithAccuracy,
                TimeFormat = format
            };

            return formatRequest.Format(time);
        }
    }
}
