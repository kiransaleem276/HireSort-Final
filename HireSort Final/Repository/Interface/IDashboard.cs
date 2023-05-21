using HireSort.Models;
using System.Data;

namespace HireSort.Repository.Interface
{
    public interface IDashboard
    {
        Task<ApiResponseMessage> GetDepartment(int? clientID);
        Task<ApiResponseMessage> GetVacanciesDepartmentWise(int departId, int? clientId);
        Task<ApiResponseMessage> GetDepartAndVacacyDetails(int? clientId, int departId = 0, int vacancyId = 0);
        Task<ApiResponseMessage> GetAllResumes(int departId, int vacancyId, int? clientId, bool isShortListedResume = false);
        Task<ApiResponseMessage> GetDepartmentVacancyCount(int? clientId);
        Task<ApiResponseMessage> GetDepartmentJobs(int departId, int? clientId);
        Task<ApiResponseMessage> GetJobDetail(int departId, int jobId, int? clientId);
        Task<ApiResponseMessage> GetResumeCompatibiltiy(int resumeId, int jobId, int? clientId);
        Task<ApiResponseMessage> ResumeShorlisting(int resumeId, int? clientId);
        Task<ApiResponseMessage> Login(string email, string password);
    }
}
