using System.Collections.Generic;
using System;

namespace VirtualHealth.UI
{
    public class Job
    {
        public string Id { get; set; }

        public string EmployerId { get; set; }
        public string Title { get; set; }= string.Empty;
        public string Description { get; set; }=string.Empty;   
        public int MinExperience { get; set; } = 0;

        public List<string> Skills { get; set; } = new();

        public List<string> RequiredSkills { get; set; } = new();

        public string Location { get; set; }= string.Empty;

        public string Company { get; set; } = string.Empty; //employer name

        public string JobType { get; set; }= string.Empty; //full-time, part-time, contract, etc.
        public decimal Salary { get; set; }= 0; //annual salary
        public string PostedByUserId { get; set; }= string.Empty; //user who posted the job
        public DateTime PostedDate { get; set; } //created date
        public bool IsActive { get; set; } = true;

    }

 

    public class JobRecommendation
    {
        public Job Job { get; set; }
        public int MatchingScore { get; set; } // Score in percentage (e.g., 85 means 85%)
        public bool HasApplied { get; set; } // Indicates if the user has applied for the job
    }

   

}
