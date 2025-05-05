using System.ComponentModel.DataAnnotations;

namespace VirtualHealth.UI.Models;

public class PatientProfile
{
    public string? PatientId { get; set; }
    public string? PatientName { get; set; }
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    public string Gender { get; set; } = default!;
    [Required]
    public DateTime BirthDate { get; set; } = default!;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public PatientAddressInput PatientAddress { get; set; } = new();

    public string EmergencyContactFirstName { get; set; } = string.Empty;
    public string EmergencyContactLastName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;

    public PractitionerInput Pcp { get; set; } = new();
    
    public string InsuranceProvider { get; set; } = string.Empty;
    public string PolicyNumber { get; set; } = string.Empty;

    public bool ConsentTreatment { get; set; } = false;
    public bool ConsentPrivacy { get; set; } = false;
    public bool ConsentBilling { get; set; } = false;

    public List<VitalSignsInput> VitalSigns { get; set; } = new();
    public List<ConditionInput> PastConditions { get; set; } = new();

    // Lab Results (Category = laboratory)
    //public List<LabResultInput> LabResults { get; set; } = new();

    // Imaging Results (Category = imaging)
    //public List<ImagingResultInput> ImagingResults { get; set; } = new();

    // Social History (Category = social-history)
    public List<SocialHistoryInput> SocialHistories { get; set; } = new();

    // Mental Health (Category = survey or exam)
    //public List<MentalHealthInput> MentalHealthAssessments { get; set; } = new();

    public List<LifeStyleInput> LifestyleHistories { get; set; } = new();
}

public class PatientAddressInput
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class ConditionInput
{
    public string Code { get; set; } = default!;
    public string Display { get; set; } = default!;
    public bool IsSelected { get; set; } = false; // For checkbox binding
}

public class VitalSignsInput
{
    public string Code { get; set; } = default!;
    public string Display { get; set; } = default!;
    public double? Value { get; set; }
    public string Unit { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}

public class LabResultInput
{
    public string TestCode { get; set; }    // LOINC code (e.g., "718-7" for Hemoglobin)
    public string TestName { get; set; }     // Test name (e.g., "Hemoglobin")
    public decimal? Value { get; set; }
    public string Unit { get; set; }         // e.g., "g/dL", "mg/dL"
    public string NormalRange { get; set; }
}

public class ImagingResultInput
{
    public string ImagingTypeCode { get; set; } // e.g., "36626-5" (Chest X-ray)
    public string ImagingTypeName { get; set; }
    public string ReportSummary { get; set; }   // Simple text summary of imaging findings
}

public class SocialHistoryInput
{
    public string BehaviorCode { get; set; }  // SNOMED/LOINC code (e.g., Smoking, Alcohol Use)
    public string BehaviorName { get; set; }
    public string InputType { get; set; } //number, text, dropdown, check
    public string? StatusCode { get; set; }    // Coded value (e.g., "Current Smoker")
    public string? StatusDisplay { get; set; }
    public int? StatusValue { get; set; }
}

public class LifeStyleInput
{
    public string LifestyleCode { get; set; }  // SNOMED/LOINC code (e.g., Smoking, Alcohol Use)
    public string LifestyleName { get; set; }
    public string InputType { get; set; } //number, text, dropdown, check
    public string? StatusCode { get; set; }    // Coded value (e.g., "Current Smoker")
    public string? StatusDisplay { get; set; }
    public int? StatusValue { get; set; }

    public string? Detail { get; set; }            // e.g., Exercises 3 times/week
}

public class SocialHistoryStatusInput
{
    public string BehaviorCode { get; set; }  // SNOMED/LOINC code (e.g., Smoking, Alcohol Use)
    public string StatusCode { get; set; }    // Coded value (e.g., "Current Smoker")
    public string StatusDisplay { get; set; }
    public bool IsSelected { get; set; } = false;
}

public class MentalHealthInput
{
    public string Code { get; set; }  // PHQ-9 question code or similar
    public string Display { get; set; }
    public int? Score { get; set; }          // Score (e.g., 0-27 for PHQ-9 items)
    public string Unit { get; set; }         // e.g., "score"
    public string NormalRange { get; set; }
}

public class UserSecureProfile
{
    public string Id { get; set; }
    public string Name { get; set; }
    // Other properties...
}