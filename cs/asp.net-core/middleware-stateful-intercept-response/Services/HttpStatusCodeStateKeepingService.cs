namespace Services
{
    public class HttpStatusCodeStateKeepingService
    {
        public int StatusCode;

        public HttpStatusCodeStateKeepingService(int initialStatusCode = 200)
        {
            StatusCode = initialStatusCode;
        }
    }
}
