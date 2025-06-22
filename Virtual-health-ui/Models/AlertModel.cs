namespace VirtualHealth.UI.Models
{
    // ✅ AlertModel.cs (in Models folder)
    public class AlertModel
    {
        public string AlertId { get; set; }
        public string PatientId { get; set; }
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public Dictionary<string, double?> TriggeredBy { get; set; }
        public Dictionary<string, ThresholdRange> Thresholds { get; set; }
        public NotifiedModel Notified { get; set; }
        public RawVitalsModel RawVitals { get; set; }
    }

    public class ThresholdRange
    {
        public double? Min { get; set; }
        public double? Max { get; set; }
    }

    public class NotifiedModel
    {
        public ContactInfo Pcp { get; set; }
        public ContactInfo EmergencyContact { get; set; }
    }

    public class ContactInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }  // optional for emergency
        public string Phone { get; set; }
        public string Status { get; set; }
    }

    public class RawVitalsModel
    {
        public double? HeartRate { get; set; }
        public double? Spo2 { get; set; }
        public double? RespiratoryRate { get; set; }
        public double? BloodGlucose { get; set; }
        public double? Temperature { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AlertResponseWrapper
    {
        public List<AlertModel> Message { get; set; }
    }

}
