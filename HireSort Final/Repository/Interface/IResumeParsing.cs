using HireSort.Models;

namespace HireSort.Repository.Interface
{
    public interface IResumeParsing
    {
        public Task<ApiResponseMessage> resumeCheckCompatibility(int resumeId, int jobId, int? clientId);
        public Task<ApiResponseMessage> ResumeCalculateCompatibility(int resumeId, int jobId, int? clientId);
        public Task<ApiResponseMessage> ResumeUpload(IFormFile file, int jobId, int? clientId, string firstName = null, string lastName = null, string email = null, string coverLetter = null);
    }
}
