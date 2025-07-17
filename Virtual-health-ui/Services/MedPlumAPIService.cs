using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using VirtualHealth.UI.Models;
//using System.Net.Http.Json; // For GetFromJsonAsync

namespace VirtualHealth.UI.Services;

public class MedPlumAPIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly string _apiBaseUrl;

    public MedPlumAPIService(IConfiguration config, MedplumWrapperApiHttpClient httpClient)
    {
        _httpClient = httpClient.Client;
        _config = config;
        _apiBaseUrl = $"{_config["ApiBaseUrl"]}";
    }

    public async Task<PatientProfile> GetPatientFullProfileAsync(string email)
    {
        var patient = new PatientProfile();
        // URL encode the email because @ is special character
        var encodedEmail = Uri.EscapeDataString(email);

        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Medplum/patient-full-profile/{encodedEmail}");

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
    }

    public async Task<string> CreatePatientFullProfileAsync(PatientProfile profile)
    {
        var apiUrl = $"{_apiBaseUrl}/create-patient-profile";

        try
        {
            var jsonProfile = JsonSerializer.Serialize(profile);
            var response = await _httpClient.PostAsync(apiUrl, new StringContent(jsonProfile, Encoding.UTF8, "application/fhir+json"));

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
    }

    public async Task<string> UpdatePatientFullProfileAsync(PatientProfile profile)
    {
        var apiUrl = $"{_apiBaseUrl}/update-patient-profile";

        try
        {
            var jsonProfile = JsonSerializer.Serialize(profile);
            var response = await _httpClient.PostAsync(apiUrl, new StringContent(jsonProfile, Encoding.UTF8, "application/fhir+json"));
            
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
            var jsonProfile = JsonSerializer.Serialize(profile);
            var response = await _httpClient.PostAsync(apiUrl, new StringContent(jsonProfile, Encoding.UTF8, "application/fhir+json"));

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
        
    }
}