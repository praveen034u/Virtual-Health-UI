namespace VirtualHealth.UI.Models
{
    public class PrescriptionItem
    {
        public string MedicationName { get; set; } = "";
        public string DosageText { get; set; } = "";          // e.g. "Take 1 tablet daily"
        public string Frequency { get; set; } = "";           // derived from dosage text
        public string Instructions { get; set; } = "";        // optional notes
        public DateTime StartDate { get; set; }               // from authoredOn
    }


}
