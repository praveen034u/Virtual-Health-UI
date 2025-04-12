namespace SkillBridge.UI
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public EmployeeProfile EmployeeProfile { get; set; } = new();
        public EmployerProfile EmployerProfile { get; set; } = new();
        public string Role { get; set; } = "Employee"; // Default to Employee
        public NameModel Name { get; set; } = new();
        public Address Address { get; set; } = new();
        public string Email { get; set; } = string.Empty;    
        public string PhoneNumber { get; set; }= string.Empty;
    }

    public class Address
    {
        public string? Street { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; } = string.Empty;
    }
    public class NameModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
    }
    public class Skill
    {
       public string Name { get; set; } = string.Empty;
       public int ProficiencyLevel { get; set; } = 1; // 1-5
    }

    public class EmployeeProfile
    {
        public int YearsOfExperience { get; set; } = 0;
        public List<Skill> Skills { get; set; } = new();
        public List<Skill> ExtractedSkills { get; set; } = new();
        public bool UserConsentForSkills { get; set; }
        public List<string> Certifications { get; set; } = new();
        public List<string> AppliedJobs { get; set; }= new();
        public string ResumeUrl { get; set; } = string.Empty;
        public string LinkedInProfileUrl { get; set; }= string.Empty;
    }

    public class EmployerProfile
    {
        public string CompanyName { get; set; }= string.Empty;
        public string CompanyWebsiteUrl { get; set; } = string.Empty;
        public IList<CompanyLocation> CompanyLocations { get; set; }= new List<CompanyLocation>();  
    }

    public class CompanyLocation
    {
        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; } = string.Empty;

    }

    public class ResumeAPIResponse
    {
        public string ResumeUrl { get; set; } = string.Empty;
        public List<string> ExtractedSkills { get; set; } = new();
    }

    public class UserSecureProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // Other properties...
    }
}
