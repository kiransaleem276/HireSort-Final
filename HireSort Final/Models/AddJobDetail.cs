namespace HireSort_Final.Models
{
    public class AddJobDetail
    {
        public int? ClientId { get; set; }
        public int DepartId { get; set; }
        public string? JobTitle { get; set; }
        public string? Salary { get; set; }
        public string? Responsibility { get; set; }
        public string? Requirement { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? JobType { get; set; }
        public string? JobShift { get; set; }
        public string? AdditionalDesc { get; set; }
        public int? ExperienceFrom { get; set; }
        public int? ExperienceTo { get; set; }
        public string? Educations { get; set; }
        public List<string>? Skills { get; set; }
        //public List<string>? Experiences { get; set; }
    }
}
