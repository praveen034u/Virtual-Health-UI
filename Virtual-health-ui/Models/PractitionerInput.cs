using System.Text.Json.Serialization;

namespace VirtualHealth.UI.Models;
public class PractitionerInput
{
    public string? PractitionerId { get; set; }
    public string? PractitionerName { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Gender { get; set; } = default!;
}

