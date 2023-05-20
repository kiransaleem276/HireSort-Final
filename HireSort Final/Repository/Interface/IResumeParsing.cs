﻿using HireSort.Models;

namespace HireSort.Repository.Interface
{
    public interface IResumeParsing
    {
        public Task<ApiResponseMessage> resumeCheckCompatibility(int resumeId, int jobId);
        public Task<ApiResponseMessage> ResumeCalculateCompatibility(int resumeId, int jobId);
        public Task<ApiResponseMessage> ResumeUpload(IFormFile file, int jobId, string firstName = null, string lastName = null, string email = null, string coverLetter = null);
    }
}
