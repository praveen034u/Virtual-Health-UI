using System.Net.Http.Json;
using VirtualHealth.UI.Models;
//using System.Net.Http.Json; // For GetFromJsonAsync

namespace VirtualHealth.UI.Services;

public class MedPlumAPIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public MedPlumAPIService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _config = config;
    }

    public async Task<PatientProfile> GetPatientFullProfileAsync(string email)
    {
        var patient = new PatientProfile();
        // URL encode the email because @ is special character
        var encodedEmail = Uri.EscapeDataString(email);
        var baseUrl = _config["MedPlum:ApiBaseUrl"];
        var apiUrl = $"{baseUrl}/patient-full-profile/{encodedEmail}";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        //request.Headers.Add("Accept", "application/fhir+json");

        try
        {
            var response = await _httpClient.SendAsync(request);
            //var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                patient = await response.Content.ReadFromJsonAsync<PatientProfile>();
                return patient;
            }
            else
            {
                // Handle error case
                Console.WriteLine($"Error: {response.StatusCode}");
                return patient;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return patient;
        }
        finally { 
            request.Dispose();
            _httpClient.Dispose(); 
            patient = null;
        }
    }

    public async Task<string> CreatePatientFullProfileAsync(PatientProfile profile)
    {
        var baseUrl = _config["MedPlum:ApiBaseUrl"];
        var apiUrl = $"{baseUrl}/create-profile-with-pcp-and-vitals";

        try
        {
            var response = await _httpClient.PostAsJsonAsync(apiUrl, profile);

            //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7195/api/MedPlum/create-profile-with-pcp-and-vitals");
            //request.Headers.Add("Accept", "application/fhir+json");
            //var content = new StringContent("{\r\n    \"patientId\": \"string\",\r\n    \"patientName\": \"string\",\r\n    \"firstName\": \"string\",\r\n    \"lastName\": \"string\",\r\n    \"gender\": \"string\",\r\n    \"birthDate\": \"string\",\r\n    \"email\": \"string\",\r\n    \"phoneNumber\": \"string\",\r\n    \"pastConditions\": [\r\n        {\r\n            \"code\": \"string\",\r\n            \"display\": \"string\"\r\n        }\r\n    ],\r\n    \"pcp\": {\r\n        \"practitionerId\": \"string\",\r\n        \"practitionerName\": \"string\",\r\n        \"firstName\": \"string\",\r\n        \"lastName\": \"string\",\r\n        \"email\": \"string\",\r\n        \"gender\": \"string\"\r\n    },\r\n    \"emergencyContactFirstName\": \"string\",\r\n    \"emergencyContactLastName\": \"string\",\r\n    \"emergencyContactPhone\": \"string\",\r\n    \"immunizations\": [\r\n        {\r\n            \"vaccineCode\": \"string\",\r\n            \"display\": \"string\",\r\n            \"dateGiven\": \"2025-04-28T21:58:09.525Z\"\r\n        }\r\n    ],\r\n    \"mentalHealthAssessments\": [\r\n        {\r\n            \"surveyQuestionCode\": \"string\",\r\n            \"questionText\": \"string\",\r\n            \"score\": 0\r\n        }\r\n    ],\r\n    \"patientAddress\": {\r\n        \"addressLine1\": \"string\",\r\n        \"street\": \"string\",\r\n        \"city\": \"string\",\r\n        \"state\": \"string\",\r\n        \"zipCode\": \"string\",\r\n        \"country\": \"string\"\r\n    },\r\n    \"socialHistories\": [\r\n        {\r\n            \"behaviorCode\": \"string\",\r\n            \"behaviorName\": \"string\",\r\n            \"statusCode\": \"string\",\r\n            \"statusDisplay\": \"string\"\r\n        }\r\n    ],\r\n    \"lifestyleHistories\": [\r\n        {\r\n            \"lifestyleCode\": \"string\",\r\n            \"lifestyleName\": \"string\",\r\n            \"detail\": \"string\"\r\n        }\r\n    ],\r\n    \"vitalSigns\": [\r\n        {\r\n            \"type\": \"string\",\r\n            \"value\": 0,\r\n            \"unit\": \"string\",\r\n            \"timestamp\": \"2025-04-28T21:58:09.525Z\"\r\n        }\r\n    ]\r\n}", null, "application/json");
            //request.Content = content;
            //var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var patientId = await response.Content.ReadAsStringAsync();
                Console.WriteLine(patientId);
                return patientId;
            }
            else
            {
                // Handle error case
                Console.WriteLine($"Error: {response.StatusCode}");
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return string.Empty;
        }
        finally
        {
            _httpClient.Dispose();
        }
    }
}
