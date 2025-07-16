namespace VirtualHealth.UI.Models
{
    public class PrescriptionItem
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; } // Example: "Twice a day"
        public string Duration { get; set; }  // Optional: e.g. "5 days"
    }

}
