using System;

namespace EtcPicApp.Constants
{
    public class CacheNameConstants
    {
        public const string EtcJobs = "EtcJobs";
        public static string Captions = "EtcCaptions";
        public static string PhotoCaptions = "EtcPhotoCaptions";
        public static string PhotoStreamCaptions = "EtcPhotoCaptions";

        public static string NewMaterials = "NewPhotoStreamCaptions";
        public static string NewSamples = "NewSamples";

        public static string SyncData()
        {
            return "ETCSyncData_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        }
    }
}
