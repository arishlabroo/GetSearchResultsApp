namespace GetSearchResultsApp.Model
{
    public static class SearchEvents
    {
        public const int RequestProcessing = 1;
        public const int RawResponse = 2;
        public const int ResponseMapping = 3;

        public const int HttpRequestFailed = 4;
    }
}