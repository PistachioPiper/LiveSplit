using System;
using UpdateManager;

namespace LiveSplit.Updates
{
    public class LiveSplitUpdateable : IUpdateable
    {
        public string UpdateName => "LiveSplit";

        public string XMLURL => "";

        public string UpdateURL => "http://livesplit.org/update/";

        public Version Version => UpdateHelper.Version;
    }
}
