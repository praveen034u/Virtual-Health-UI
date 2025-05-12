using Blazorise;
using Microsoft.JSInterop;
using VirtualHealth.UI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VirtualHealth.UI.Pages;

public partial class CreateProfile
{
    private PatientProfile patientProfile = new();
    private List<SocialHistoryStatusInput> socialHistoryStatus = new();
    private string selectedStatusCode = string.Empty;
    private string email = string.Empty;
    //private HttpClient Http = new HttpClient { BaseAddress = new Uri("https://localhost:7236") };

    protected override async Task OnInitializedAsync()
    {
        email = await SecureStorage.GetUserIdAsync();
        patientProfile.Email = email;
        await LoadInitialData();

        var profile = await MedplumService.GetPatientFullProfileAsync(email);
        if (string.IsNullOrEmpty(profile.PatientId))
        {
            patientProfile.PatientAddress.Country = "USA";
        }
        else
        {
            await LoadProfileData(profile);
        }
    }

    private async Task HandleValidSubmit()
    {
        patientProfile.PatientName = (patientProfile.FirstName + " " + patientProfile.LastName).Trim();
        patientProfile.Pcp.PractitionerName = (patientProfile.Pcp?.FirstName ?? string.Empty + " " + patientProfile.Pcp?.LastName ?? string.Empty).Trim();

        patientProfile.PastConditions.RemoveAll(c => !c.IsSelected);

        foreach (var history in patientProfile.SocialHistories.Where(sh => !string.IsNullOrEmpty(sh.StatusCode)))
        {
            var match = socialHistoryStatus
                .FirstOrDefault(s => s.BehaviorCode == history.BehaviorCode && s.StatusCode == history.StatusCode);

            if (match != null)
                history.StatusDisplay = match.StatusDisplay;
            else
                history.StatusCode = string.Empty;
        }
        patientProfile.SocialHistories
            .Where(sh => sh.StatusValue != null).ToList()
            .ForEach(sh => sh.StatusDisplay = sh.StatusValue?.ToString());

        foreach (var lifeStyle in patientProfile.LifestyleHistories.Where(sh => !string.IsNullOrEmpty(sh.StatusCode)))
        {
            var match = socialHistoryStatus
                .FirstOrDefault(s => s.BehaviorCode == lifeStyle.LifestyleCode && s.StatusCode == lifeStyle.StatusCode);

            if (match != null)
            {
                lifeStyle.StatusDisplay = match.StatusDisplay;
                //lifeStyle.Detail = match.StatusDisplay;
            }
            else
                lifeStyle.StatusCode = string.Empty;
        }
        patientProfile.LifestyleHistories
            .Where(sh => sh.StatusValue != null).ToList()
            .ForEach(sh =>
            {
                sh.StatusDisplay = sh.StatusValue?.ToString();
                //sh.Detail = sh.StatusValue?.ToString();
            });

        string patientId = string.Empty;
        if (string.IsNullOrEmpty(patientProfile.PatientId))
            patientId = await MedplumService.CreatePatientFullProfileAsync(patientProfile);
        else
            patientId = await MedplumService.UpdatePatientFullProfileAsync(patientProfile);

        await JS.InvokeVoidAsync("alert", "Profile submitted successfully!"); //"Profile submitted successfully!"

        StateHasChanged(); // 🔄 Force re-render
    }

    private async Task GenerateSummary()
    {
        //var summary = $"Patient Profile Summary\n\nName: {patientProfile.FirstName} {patientProfile.LastName}\nDOB: {patientProfile.BirthDate:d}\nAddress: {patientProfile.Address}\nPhone: {patientProfile.PhoneNumber}\nEmergency Contact: {patientProfile.EmergencyContact}\n\nProvider: {patientProfile.Pcp.FirstName} {patientProfile.Pcp.LastName}, {patientProfile.Pcp.Email}\nInsurance: {patientProfile.InsuranceProvider}, Policy #: {patientProfile.PolicyNumber}\n\nMedical History: {patientProfile.MedicalHistory}\nFamily History: {patientProfile.FamilyMedicalHistory}\nMedications: {patientProfile.CurrentMedications}\nReason for Visit: {patientProfile.ReasonForVisit}\nLifestyle: {patientProfile.Lifestyle}\n\nBP: {patientProfile.Vitals[0].Value}/{patientProfile.Vitals[1].Value}\nHR: {patientProfile.Vitals[2].Value} bpm\n\nConsents:\n- Treatment: {(patientProfile.ConsentTreatment ? "Yes" : "No")}\n- Privacy: {(patientProfile.ConsentPrivacy ? "Yes" : "No")}\n- Billing: {(patientProfile.ConsentBilling ? "Yes" : "No")}";
        string summary = string.Empty;
        await JS.InvokeVoidAsync("navigator.clipboard.writeText", summary);
        await JS.InvokeVoidAsync("alert", "Summary copied to clipboard. Paste in Word or save as PDF.");
    }

    private async Task LoadInitialData()
    {
        patientProfile.VitalSigns = await LoadVitals();
        patientProfile.PastConditions = await LoadConditions();
        //patientProfile.LabResults = await LoadLabResult();
        //patientProfile.ImagingResults = await LoadImagingResult();
        //patientProfile.MentalHealthAssessments = await LoadMentalHealthAssessments();
        patientProfile.SocialHistories = await LoadSocialHistory();
        patientProfile.LifestyleHistories = await LoadLifeStyle();
        socialHistoryStatus = await LoadSocialHistoryStatus();
    }

    private async Task LoadProfileData(PatientProfile profile)
    {
        patientProfile.PatientId = profile.PatientId;
        patientProfile.PatientName = profile.PatientName;
        patientProfile.FirstName = profile.FirstName;
        patientProfile.LastName = profile.LastName;
        patientProfile.Gender = profile.Gender;
        patientProfile.BirthDate = profile.BirthDate;
        patientProfile.PhoneNumber = profile.PhoneNumber;

        patientProfile.PatientAddress.AddressLine1 = profile.PatientAddress?.AddressLine1 ?? string.Empty;
        patientProfile.PatientAddress.Street = profile.PatientAddress?.Street ?? string.Empty;
        patientProfile.PatientAddress.City = profile.PatientAddress?.City ?? string.Empty;
        patientProfile.PatientAddress.State = profile.PatientAddress?.State ?? string.Empty;
        patientProfile.PatientAddress.ZipCode = profile.PatientAddress?.ZipCode ?? string.Empty;
        patientProfile.PatientAddress.Country = profile.PatientAddress?.Country ?? string.Empty;

        patientProfile.EmergencyContactFirstName = profile.EmergencyContactFirstName ?? string.Empty;
        patientProfile.EmergencyContactLastName = profile.EmergencyContactLastName ?? string.Empty;
        patientProfile.EmergencyContactPhone = profile.EmergencyContactPhone ?? string.Empty;
        patientProfile.InsuranceProvider = profile.InsuranceProvider ?? string.Empty;
        patientProfile.PolicyNumber = profile.PolicyNumber ?? string.Empty;

        patientProfile.Pcp.PractitionerId = profile.Pcp?.PractitionerId ?? string.Empty;
        patientProfile.Pcp.PractitionerName = profile.Pcp?.PractitionerName ?? string.Empty;
        patientProfile.Pcp.FirstName = profile.Pcp?.FirstName ?? string.Empty;
        patientProfile.Pcp.LastName = profile.Pcp?.LastName ?? string.Empty;
        patientProfile.Pcp.Gender = profile.Pcp?.Gender ?? string.Empty;
        patientProfile.Pcp.Email = profile.Pcp?.Email ?? string.Empty;

        patientProfile.ConsentTreatment = profile.ConsentTreatment;
        patientProfile.ConsentPrivacy = profile.ConsentPrivacy;
        patientProfile.ConsentBilling = profile.ConsentBilling;

        // Update matching PastConditions from Saved data
        foreach (var updated in profile.PastConditions)
        {
            var existing = patientProfile.PastConditions.FirstOrDefault(c => 
                (c.Code == updated.Code || c.Display == updated.Display));
            if (existing != null)
            {
                existing.Id = updated.Id;
                existing.IsSelected = true;
            }
        }

        // Update matching Vitals from Saved data
        foreach (var updated in profile.VitalSigns)
        {
            var existing = patientProfile.VitalSigns.FirstOrDefault(v => 
                (v.Code == updated.Code || v.Display == updated.Display));
            if (existing != null)
            {
                existing.Id = updated.Id;
                existing.Value = updated.Value;
                //existing.Unit = updated.Unit;
                //existing.Timestamp = updated.Timestamp;
            }
        }

        // Update matching Social Histories from Saved data
        foreach (var updated in profile.SocialHistories)
        {
            var existing = patientProfile.SocialHistories.FirstOrDefault(s =>
                (s.BehaviorCode == updated.BehaviorCode || s.BehaviorName == updated.BehaviorName));

            if (existing != null)
            {
                existing.Id = updated.Id;
                if (existing.InputType == "number")
                {
                    existing.StatusDisplay = updated.StatusDisplay ?? string.Empty;
                    existing.StatusValue = Convert.ToInt32(updated.StatusDisplay ?? string.Empty);
                }
                else
                {
                    existing.StatusCode = updated.StatusCode ?? string.Empty;
                    existing.StatusDisplay = updated.StatusDisplay ?? string.Empty;
                }

            }
        }

        // Update matching Lifestyle Histories from Saved data
        foreach (var updated in profile.LifestyleHistories)
        {
            var existing = patientProfile.LifestyleHistories.FirstOrDefault(s =>
                (s.LifestyleCode == updated.LifestyleCode || s.LifestyleName == updated.LifestyleName));

            if (existing != null)
            {
                existing.Id = updated.Id;
                //if (string.IsNullOrEmpty(existing.Detail))
                //{
                    if (existing.InputType == "number")
                    {
                        existing.StatusDisplay = updated.StatusDisplay ?? string.Empty;
                        existing.StatusValue = Convert.ToInt32(updated.StatusDisplay ?? string.Empty);
                    }
                    else
                    {
                        existing.StatusCode = updated.StatusCode ?? string.Empty;
                        existing.StatusDisplay = updated.StatusDisplay ?? string.Empty;
                    }
                //}
                //else
                //{
                //    if (existing.InputType == "number")
                //    {
                //        existing.StatusDisplay = updated.Detail;
                //        existing.StatusValue = Convert.ToInt32(updated.Detail);
                //    }
                //    else
                //    {
                //        existing.StatusCode = updated.Detail?.ToLower();
                //        existing.StatusDisplay = updated.Detail;
                //    }
                //}
            }
        }
    }

    private async Task<List<VitalSignsInput>> LoadVitals()
    {
        List<VitalSignsInput> Vitals = new List<VitalSignsInput>();
        try
        {
            var response = await Http.GetAsync("Data/Vitals.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Vitals = System.Text.Json.JsonSerializer.Deserialize<List<VitalSignsInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<VitalSignsInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Vitals;
    }

    private async Task<List<ConditionInput>> LoadConditions()
    {
        List<ConditionInput> conditions = new List<ConditionInput>();
        try
        {
            var response = await Http.GetAsync("Data/MedicalConditions.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                conditions = System.Text.Json.JsonSerializer.Deserialize<List<ConditionInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ConditionInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return conditions;
    }

    private async Task<List<LabResultInput>> LoadLabResult()
    {
        List<LabResultInput> labResults = new List<LabResultInput>();
        try
        {
            var response = await Http.GetAsync("Data/LabResults.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                labResults = System.Text.Json.JsonSerializer.Deserialize<List<LabResultInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<LabResultInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return labResults;
    }

    private async Task<List<ImagingResultInput>> LoadImagingResult()
    {
        List<ImagingResultInput> labResults = new List<ImagingResultInput>();
        try
        {
            var response = await Http.GetAsync("Data/ImagingStudies.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                labResults = System.Text.Json.JsonSerializer.Deserialize<List<ImagingResultInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ImagingResultInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return labResults;
    }

    private async Task<List<MentalHealthInput>> LoadMentalHealthAssessments()
    {
        List<MentalHealthInput> labResults = new List<MentalHealthInput>();
        try
        {
            var response = await Http.GetAsync("Data/MentalHealth.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                labResults = System.Text.Json.JsonSerializer.Deserialize<List<MentalHealthInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MentalHealthInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return labResults;
    }

    private async Task<List<SocialHistoryInput>> LoadSocialHistory()
    {
        List<SocialHistoryInput> socialHistory = new List<SocialHistoryInput>();
        try
        {
            var response = await Http.GetAsync("Data/SocialHistoryBehavior.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                socialHistory = System.Text.Json.JsonSerializer.Deserialize<List<SocialHistoryInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SocialHistoryInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return socialHistory;
    }

    private async Task<List<LifeStyleInput>> LoadLifeStyle()
    {
        List<LifeStyleInput> lifeStyle = new List<LifeStyleInput>();
        try
        {
            var response = await Http.GetAsync("Data/LifeStyle.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                lifeStyle = System.Text.Json.JsonSerializer.Deserialize<List<LifeStyleInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<LifeStyleInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return lifeStyle;
    }

    private async Task<List<SocialHistoryStatusInput>> LoadSocialHistoryStatus()
    {
        List<SocialHistoryStatusInput> socialHistory = new List<SocialHistoryStatusInput>();
        try
        {
            var response = await Http.GetAsync("Data/SocialHistoryStatus.json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                socialHistory = System.Text.Json.JsonSerializer.Deserialize<List<SocialHistoryStatusInput>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SocialHistoryStatusInput>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return socialHistory;
    }
}
