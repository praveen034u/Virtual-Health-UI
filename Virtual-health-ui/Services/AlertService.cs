using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using VirtualHealth.UI.Models;

namespace VirtualHealth.UI.Services
{
    public class AlertService
    {
        private readonly SecureStorageService _secureStorage;
        private readonly MedplumWrapperApiHttpClient _medplumWrapperApiHttpClient;

        public AlertService(SecureStorageService secureStorage, MedplumWrapperApiHttpClient medplumWrapperApiHttpClient)
        {
            _secureStorage = secureStorage;
            _medplumWrapperApiHttpClient = medplumWrapperApiHttpClient;
        }

        public async Task<List<AlertModel>> GetAlertsAsync()
        {
            var patientId = await _secureStorage.GetItemAsync("vh_patient_id");
            if (string.IsNullOrWhiteSpace(patientId))
                patientId = "01978609-4506-72a9-a00e-8083bbf66207";

            try
            {
                var response = await _medplumWrapperApiHttpClient.Client.GetAsync($"/api/Medplum/alarm-notification/{patientId}");

                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new List<AlertModel>();

                var wrapper = JsonSerializer.Deserialize<AlertResponseWrapper>(raw, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return wrapper?.Message ?? new List<AlertModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in GetAlertsAsync: " + ex.Message);
                return new List<AlertModel>();
            }
        }
    }

    public class AlertResponseWrapper
    {
        public List<AlertModel> Message { get; set; }
    }
}
