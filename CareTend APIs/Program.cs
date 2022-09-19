using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace CareTend_APIs
{
    internal class Program
    {
        static string _clientId = ""; //Put your client Id here
        static string _secretKey = ""; //Put the provided secret key here
        static string _scope = ""; //Put the scope defined in the ReadMe documentation
        static string _grantType = ""; //Put the grant type defined in the ReadMe documentation

        /* For the URLs only put the base URL in do not use the full path i.e. /connet/token */
        static string _authUrl = ""; //Put the authorization URL for the environment you are testing: stage/prod
        static string _restUrl = ""; //Put the IntegrationHub URL for the environment you are testing: stage/prod

        static void Main(string[] args)
        {
            var token = GetToken();
            GetPatient(token);
        }

        static string GetToken()
        {
            var httpClient = new RestClient(_authUrl);

            var request = new RestRequest("connect/token", Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", _grantType);
            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _secretKey);
            request.AddParameter("scope", _scope);

            var response = httpClient.Execute(request).Content;
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["access_token"].ToString();
        }

        static void GetPatient(string token)
        {
            var client = new RestClient(_restUrl);
            var httpRequest = new RestRequest("api/v1/commands/patients?medicalRecordNumber=200220", Method.Get);
            httpRequest.AddHeader("Authorization", "Bearer " + token);

            var patientResponse = client.Execute(httpRequest);
        }
    }
}

public class CommandRequest
{
    public string verb { get; set; }
    public string uri { get; set; }
    public List<Parameter> parameters { get; set; }
}

public class Parameter
{    
    public string name { get; set; }
    public string value { get; set; }
}
