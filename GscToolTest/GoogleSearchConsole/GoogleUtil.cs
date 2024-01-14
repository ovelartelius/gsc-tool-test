using Google.Apis.Requests;
using System.Linq;
using System.Net;

namespace GscToolTest.GoogleSearchConsole
{
    public interface IGoogleUtil
    {
        //String GetUniqueId();
        //void PrintErrors(IList<SingleError> errors);
        TResponse ExecuteWithRetries<TResponse>(IClientServiceRequest<TResponse> request, IEnumerable<HttpStatusCode> statusCodes);

        Task<TResponse> ExecuteWithRetriesAsync<TResponse>(IClientServiceRequest<TResponse> request, IEnumerable<HttpStatusCode> statusCodes);
    }

    internal class GoogleUtil : IGoogleUtil
    {
        //private int unique_id_increment = 0;

        ///// <summary>
        ///// Generates a unique ID based on the UNIX timestamp and a runtime increment.
        ///// </summary>
        //public String GetUniqueId()
        //{
        //    unique_id_increment += 1;
        //    String unixTimestamp = ((Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds).ToString();
        //    return unixTimestamp + unique_id_increment.ToString();
        //}

        ///// <summary>
        ///// Prints out a list of error results from the Content API.
        ///// </summary>
        //public void PrintErrors(IList<SingleError> errors)
        //{
        //    Console.WriteLine("Received the following errors:");
        //    foreach (SingleError err in errors)
        //    {
        //        Console.WriteLine($" - [{err.Reason}] {err.Message}");
        //    }
        //}

        /// <summary>
        /// Given a service request, retries the request up to 10 times using an exponential
        /// backoff strategy. Also takes an enumeration of status codes for which requests should be
        /// retried (that is, HTTP errors that are expected to occur transiently).
        /// </summary>
        public TResponse ExecuteWithRetries<TResponse>(IClientServiceRequest<TResponse> request, IEnumerable<HttpStatusCode> statusCodes)
        {
            var backOff = new Google.Apis.Util.ExponentialBackOff();
            int numRetries = 0;
            while (true)
            {
                try
                {
                    return request.Execute();
                }
                catch (Google.GoogleApiException e)
                {
                    if (!statusCodes.Contains(e.HttpStatusCode))
                    {
                        throw e;
                    }
                    numRetries++;
                    var nextSpan = backOff.GetNextBackOff(numRetries);
                    if (nextSpan == TimeSpan.MinValue)
                    {
                        throw e;
                    }
                    Console.WriteLine("Attempt {0} failed, retrying in {1}.", numRetries, nextSpan);
                    System.Threading.Thread.Sleep(nextSpan);
                }
            }
        }

        public async Task<TResponse> ExecuteWithRetriesAsync<TResponse>(IClientServiceRequest<TResponse> request, IEnumerable<HttpStatusCode> statusCodes)
        {
            var backOff = new Google.Apis.Util.ExponentialBackOff();
            int numRetries = 0;
            while (true)
            {
                try
                {
                    return await request.ExecuteAsync();
                }
                catch (Google.GoogleApiException e)
                {
                    if (!statusCodes.Contains(e.HttpStatusCode))
                    {
                        throw e;
                    }
                    numRetries++;
                    var nextSpan = backOff.GetNextBackOff(numRetries);
                    if (nextSpan == TimeSpan.MinValue)
                    {
                        throw e;
                    }
                    Console.WriteLine("Attempt {0} failed, retrying in {1}.", numRetries, nextSpan);
                    System.Threading.Thread.Sleep(nextSpan);
                }
            }
        }
    }
}
