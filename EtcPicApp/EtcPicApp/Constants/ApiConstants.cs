namespace EtcPicApp.Constants
{
    public static class ApiConstants
    {
#if DEBUG
        public const string BaseApiUrl = "https://surveyapi.conveyor.cloud/"; 
#else
        public const string BaseApiUrl = "http://labapi.2etc.com/";
#endif


        public const string AuthenticateEtcLogin = "api/authentication/etclogin";
        public const string AuthenticateClientLogin = "api/authentication/clientlogin";
        public const string EtcJobs = "api/asbestossurvey/etcjobs";
        public const string Jobs = "api/jobs/all";
        public const string Captions = "api/captions";
        public const string PostPhotoStreamCaptions = "api/photostreamcaptions/post";
        public const string PutPhotoStreamCaptions = "api/photostreamcaptions/put";
    }
}
