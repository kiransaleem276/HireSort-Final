using System;
using System.Collections.Generic;

namespace HireSort.Entity.DbModels
{
    public partial class ClientWisePercentMapping
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public double GpaPercentage { get; set; }
        public double InstitutePercentage { get; set; }
        public double SkillsPercentage { get; set; }
        public double ExperiencePercentage { get; set; }
        public double EducationPercentage { get; set; }
        public double? Percentage { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}
