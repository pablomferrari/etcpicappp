namespace EtcPicApp.Constants
{
    public static class ApiConstants
    {
#if DEBUG
        public const string BaseApiUrl = "https://surveyapi.conveyor.cloud/";
        //public const string BaseApiUrl = "http://labapi.2etc.com/";
#else
        public const string BaseApiUrl = "http://labapi.2etc.com/";

#endif


        public const string AuthenticateEtcLogin = "api/authentication/etclogin";
        public const string Jobs = "api/jobs/picapp";
        public const string Captions = "api/servicecaptions";
        public const string PostPhotoStreamCaptions = "api/photostreamcaptions/post";
        public const string PutPhotoStreamCaptions = "api/photostreamcaptions/put";
    }
}
