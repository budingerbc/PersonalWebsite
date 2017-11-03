using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PersonalWebsite.Models
{
    public class Starred
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Html_url { get; set; }
        public int Stargazers_count { get; set; }

        public static List<Starred> GetStarredRepos()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.github.com/users/budingerbc/starred");
            client.AddDefaultHeader("User-Agent", "budingerbc");

            var request = new RestRequest();
            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);

            var starredRepos = JsonConvert.DeserializeObject<List<Starred>>(jsonResponse.ToString());
            return starredRepos;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
