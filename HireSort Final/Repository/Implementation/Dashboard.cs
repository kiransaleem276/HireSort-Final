using HireSort.Context;
using HireSort.Entity.DbModels;
using HireSort.Helpers;
using HireSort.Models;
using HireSort.Repository.Interface;
using HireSort_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace HireSort.Repository.Implementation
{
    public class Dashboard : IDashboard
    {
        //private int clientId = 1;
        private readonly HRContext _dbContext;
        private readonly string _dateTimeFormat, _dateFormat;

        public Dashboard(HRContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _dateTimeFormat = configuration["AppSettings:DateTimeFormat"];
            _dateFormat = configuration["AppSettings:DateFormat"];
        }
        public async Task<ApiResponseMessage> GetDepartment(int? clientId)
        {
            try
            {
                var departmentList = await _dbContext.Departments.Where(w => w.ClientId == clientId && w.IsActive == true).Select(s => new Models.Department()
                {
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.DepartmentName,
                }).ToListAsync();

                return CommonHelper.GetApiSuccessResponse(departmentList);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }
        public async Task<ApiResponseMessage> GetVacanciesDepartmentWise(int departId, int? clientId)
        {
            try
            {
                var vacancies = await _dbContext.Jobs.Where(w => w.ClientId == clientId && w.StartDate <= DateTime.Now && w.EndDate >= DateTime.Now && w.DepartmentId == departId && w.IsActive == true).Select(s => new VacanciesDepartmentWise()
                {
                    VacancyId = s.JobId,
                    VacancyName = s.JobName
                }).ToListAsync();
                return CommonHelper.GetApiSuccessResponse(vacancies);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> GetDepartAndVacacyDetails(int? clientId, int departId = 0, int vacancyId = 0)
        {
            try
            {
                var list = _dbContext.Jobs.Where(w => w.ClientId == clientId && w.IsActive == true && w.Department.IsActive == true).Select(s => new DepartmentAndVacancyList()
                {
                    DepertId = s.DepartmentId,
                    DepartmentName = s.Department.DepartmentName,
                    VacancyId = s.JobId,
                    VacancyName = s.JobName
                });
                if (departId > 0 && vacancyId > 0)
                {
                    list = list.Where(w => w.DepertId == departId && w.VacancyId == vacancyId);
                }
                else if (departId > 0)
                {
                    list = list.Where(w => w.DepertId == departId);
                }
                else if (vacancyId > 0)
                {
                    list = list.Where(w => w.VacancyId == vacancyId);
                }
                return CommonHelper.GetApiSuccessResponse(list);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> GetAllResumes(int departId, int vacancyId, int? clientId, bool isShortListedResume = false)
        {
            try
            {
                var resumeList = new ResumeDetails() { Resumes = new() };
                resumeList.clientId = clientId;
                resumeList.DepartId = departId;
                resumeList.VacancyId = vacancyId;
                if (departId > 0 && vacancyId > 0)
                {
                    var resumes = _dbContext.Resumes.Where(w => w.ClientId == clientId && w.JobId == vacancyId && w.Job.DepartmentId == departId).Select(s => new Resumes()
                    {
                        ResumeID = s.Id,
                        JobId = s.JobId,
                        CandidateName = (s.IsFileParsed == true) ? s.FirstName + " " + s.LastName : s.FileName,
                        MobileNo = s.MobileNo,
                        EmailAddress = s.Email,
                        IsShortListed = s.IsShortlisted,
                        ShortListedDate = (s.ShortlistDate != null) ? Convert.ToDateTime(s.ShortlistDate).ToString(_dateFormat) : null,
                        IsFileParsed = s.IsFileParsed,
                        IsCompatibilityCheck = s.IsCompatibility,
                        Compatibility = s.Compatibility
                    });

                    if (isShortListedResume)
                        resumes = resumes.Where(w => w.IsShortListed == true);

                    resumeList.Resumes = resumes.OrderByDescending(o => o.ResumeID).ToList();
                }
                return CommonHelper.GetApiSuccessResponse(resumeList);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> GetDepartmentVacancyCount(int? clientId)
        {
            try
            {
                var response = _dbContext.Departments.Where(w => w.ClientId == clientId && w.IsActive == true).Select(s => new DepartJobCount()
                {
                    DepatId = s.DepartmentId,
                    DepartmentName = s.DepartmentName,
                    VacancyCounts = s.Jobs.Where(a => a.IsActive == true).Count()
                });
                return CommonHelper.GetApiSuccessResponse(response);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> GetDepartmentJobs(int departId, int? clientId)
        {
            try
            {
                var response = _dbContext.Jobs.Where(w => w.ClientId == clientId && w.DepartmentId == departId && w.IsActive == true).Select(s => new DepartmentJobList()
                {
                    DepartId = s.DepartmentId,
                    JobId = s.JobId,
                    JobName = s.JobName,
                    JobStartDate = s.StartDate.ToString(_dateFormat),
                    JobEndDate = (s.EndDate != null) ? Convert.ToDateTime(s.EndDate).ToString(_dateFormat) : "Not Available"
                });
                return CommonHelper.GetApiSuccessResponse(response);

            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> GetJobDetail(int departId, int jobId, int? clientId)
        {
            try
            {
                var response = _dbContext.Jobs.Where(w => w.ClientId == clientId && w.DepartmentId == departId && w.JobId == jobId).Select(s => new JobDetails
                {
                    JobId = s.JobId,
                    JobName = s.JobName,
                    JobStartDate = s.StartDate.ToString(_dateFormat),
                    JobEndDate = (s.EndDate != null) ? Convert.ToDateTime(s.EndDate).ToString(_dateFormat) : null,
                    JobShift = s.JobDetails.FirstOrDefault(a => a.JobCode.CodeName == "Job Shift").Description ?? null,
                    JobType = s.JobDetails.FirstOrDefault(b => b.JobCode.CodeName == "Job Type").Description ?? null,
                    Experience = s.ExperienceFrom + " - " + s.ExperienceTo + " years of experience",
                    //JobDesc = s.JobDetails.Where(w => w.JobCode.CodeName != "Job Shift" && w.JobCode.CodeName != "Job Type" && w.JobCode.CodeName != "Experience").Select(s => new JobDescription
                    JobDesc = s.JobDetails.Where(w => w.JobCode.CodeName == "Additional Description" || w.JobCode.CodeName == "Requirements" || w.JobCode.CodeName == "Responsibility" || w.JobCode.CodeName == "Salary Package").Select(s => new JobDescription
                    {
                        JobDetailId = s.Id,
                        JobCode = s.JobCode.CodeName,
                        Description = s.Description,
                    }).ToList(),
                    JobSkills = s.JobDetails.Where(w => w.JobCode.CodeName == "Skills").Select(s => new JobDescription
                    {
                        Description = s.Description,
                    }).ToList(),
                });
                return CommonHelper.GetApiSuccessResponse(response);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> GetResumeCompatibiltiy(int resumeId, int jobId, int? clientId)
        {
            try
            {
                var response = _dbContext.Resumes.Where(w => w.ClientId == clientId && w.JobId == jobId && w.Id == resumeId && w.IsCompatibility == true).Select(s => new ResumeCompatibility()
                {
                    ResumeId = s.Id,
                    CandidateName = s.FileName + " " + s.LastName,
                    MobieNo = s.MobileNo,
                    Email = s.Email,
                    CompatiblePercentage = s.Compatibility,
                    GPA = !String.IsNullOrEmpty(s.Gpa) ? s.Gpa : "Not Available",
                    InstituteMatch = s.InstituteMatch,
                    Educations = s.Educations.Where(w => w.ResumeId == s.Id).Select(a => new CandidateEducation()
                    {
                        EduId = a.Id,
                        DegreeName = a.DegreeName,
                        InstituteName = a.InstituteName,
                        CGPA = a.Cgpa,
                        StartDate = (a.StartDate != null) ? Convert.ToDateTime(a.StartDate).ToString(_dateFormat) : null,
                        EndDate = (a.EndDate != null) ? Convert.ToDateTime(a.EndDate).ToString(_dateFormat) : null
                    }).ToList(),
                    Experience = s.Experiences.Where(w => w.ResumeId == s.Id).Select(b => new CandidateExperience()
                    {
                        ExperienceId = b.Id,
                        CompanyName = b.CompanyName,
                        Designation = b.Designation,
                        Responsiblility = b.Responsibility,
                        StartDate = (b.StartDate != null) ? Convert.ToDateTime(b.StartDate).ToString(_dateFormat) : null,
                        EndDate = (b.EndDate != null) ? Convert.ToDateTime(b.EndDate).ToString(_dateFormat) : null,
                        TotalExperience = b.TotalExperience
                    }).ToList(),
                    Skills = s.TechnicalSkills.Where(w => w.ResumeId == s.Id).Select(c => new Skills()
                    {
                        SkillsId = c.Id,
                        SkillName = c.Skills
                    }).ToList(),
                    Links = s.Links.Where(w => w.ResumeId == s.Id).Select(d => new Links()
                    {
                        LinkId = d.Id,
                        LinkType = d.LintType,
                        Link = d.Links
                    }).ToList()
                }); ;
                return CommonHelper.GetApiSuccessResponse(response);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> ResumeShorlisting(int resumeId, int? clientId)
        {
            try
            {
                var resume = _dbContext.Resumes.Where(w => w.ClientId == clientId && w.Id == resumeId && w.IsCompatibility == true && w.IsShortlisted != true).FirstOrDefault();
                if (resume != null)
                {
                    resume.IsShortlisted = true;
                    resume.ShortlistDate = DateTime.Now;
                    _dbContext.Entry(resume).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return CommonHelper.GetApiSuccessResponse("Successfully Shorlisted.");
                }
                return CommonHelper.GetApiSuccessResponse("Resume Not Found.", 400);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }

        public async Task<ApiResponseMessage> Login(string email, string password)
        {
            try
            {
                var login = await _dbContext.Logins.Where(w => w.Email == email).FirstOrDefaultAsync();
                if (login == null)
                {
                    return CommonHelper.GetApiSuccessResponse("Email Not Found", 400);
                }
                else if (login?.Password != password)
                {
                    return CommonHelper.GetApiSuccessResponse("Password Doesn't Match", 400);
                }
                else if (login.Password == password && login.IsActive == false)
                {
                    return CommonHelper.GetApiSuccessResponse("Account is not active", 400);
                }
                else if (login.Password == password && login.IsActive == true)
                {
                    return CommonHelper.GetApiSuccessResponse("Login Successfully");
                }
                return CommonHelper.GetApiSuccessResponse("Login Unsuccessfully", 400);

            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }
        public async Task<ApiResponseMessage> AddJob(AddJobDetail jobDetails)
        {
            try
            {
                Job job = new Job();
                job.JobName = jobDetails.JobTitle ?? "";
                job.ClientId = jobDetails.ClientId ?? 0;
                job.DepartmentId = jobDetails.DepartId;
                job.StartDate = jobDetails.StartDate ?? DateTime.Now;
                job.EndDate = jobDetails.EndDate;
                job.IsActive = true;
                job.CreatedBy = jobDetails.ClientId.ToString();
                job.CreatedOn = DateTime.Now;
                job.ExperienceFrom = jobDetails.ExperienceFrom;
                job.ExperienceTo = jobDetails.ExperienceTo;

                _dbContext.Jobs.Add(job);
                await _dbContext.SaveChangesAsync();

                if (jobDetails.JobShift != null)
                {
                    JobDetail jobDetail = new JobDetail();
                    jobDetail.JobId = job.JobId;
                    jobDetail.JobCodeId = 7;
                    jobDetail.Description = jobDetails.JobShift;
                    jobDetail.ClientId = jobDetails.ClientId ?? 0;
                    jobDetail.CreatedBy = jobDetails.ClientId.ToString();
                    jobDetail.CreatedOn = DateTime.Now;
                    _dbContext.JobDetails.Add(jobDetail);
                    _dbContext.SaveChanges();
                }
                if (jobDetails.AdditionalDesc != null)
                {
                    JobDetail jobDetail = new JobDetail();
                    jobDetail.JobId = job.JobId;
                    jobDetail.JobCodeId = 8;
                    jobDetail.Description = jobDetails.AdditionalDesc;
                    jobDetail.ClientId = jobDetails.ClientId ?? 0;
                    jobDetail.CreatedBy = jobDetails.ClientId.ToString();
                    jobDetail.CreatedOn = DateTime.Now;
                    _dbContext.JobDetails.Add(jobDetail);
                    _dbContext.SaveChanges();
                }
                if (jobDetails.Salary != null)
                {
                    JobDetail jobDetail = new JobDetail();
                    jobDetail.JobId = job.JobId;
                    jobDetail.JobCodeId = 6;
                    jobDetail.Description = jobDetails.Salary;
                    jobDetail.ClientId = jobDetails.ClientId ?? 0;
                    jobDetail.CreatedBy = jobDetails.ClientId.ToString();
                    jobDetail.CreatedOn = DateTime.Now;
                    _dbContext.JobDetails.Add(jobDetail);
                    _dbContext.SaveChanges();
                }
                if (jobDetails.JobType != null)
                {
                    JobDetail jobDetail = new JobDetail();
                    jobDetail.JobId = job.JobId;
                    jobDetail.JobCodeId = 5;
                    jobDetail.Description = jobDetails.JobType;
                    jobDetail.ClientId = jobDetails.ClientId ?? 0;
                    jobDetail.CreatedBy = jobDetails.ClientId.ToString();
                    jobDetail.CreatedOn = DateTime.Now;
                    _dbContext.JobDetails.Add(jobDetail);
                    _dbContext.SaveChanges();
                }
                if (jobDetails.Skills != null && jobDetails.Skills.Count > 0)
                {
                    List<JobDetail> jobDetail = new List<JobDetail>();
                    foreach (var skil in jobDetails.Skills)
                    {
                        jobDetail.Add(new JobDetail()
                        {
                            JobId = job.JobId,
                            JobCodeId = 3,
                            Description = skil,
                            ClientId = jobDetails.ClientId ?? 0,
                            CreatedBy = jobDetails.ClientId.ToString(),
                            CreatedOn = DateTime.Now,
                        });
                    }
                    _dbContext.JobDetails.AddRange(jobDetail);
                    _dbContext.SaveChanges();
                }
                if (jobDetails.Educations != null)
                {
                    List<JobDetail> jobDetail = new List<JobDetail>();
                    jobDetail.Add(new JobDetail()
                    {
                        JobId = job.JobId,
                        JobCodeId = 4,
                        Description = jobDetails.Educations,
                        ClientId = jobDetails.ClientId ?? 0,
                        CreatedBy = jobDetails.ClientId.ToString(),
                        CreatedOn = DateTime.Now,
                    });
                    _dbContext.JobDetails.AddRange(jobDetail);
                    _dbContext.SaveChanges();
                }
                //if (jobDetails.Experiences != null && jobDetails.Experiences.Count > 0)
                //{
                //    List<JobDetail> jobDetail = new List<JobDetail>();
                //    foreach (var exp in jobDetails.Experiences)
                //    {
                //        jobDetail.Add(new JobDetail()
                //        {
                //            JobId = job.JobId,
                //            JobCodeId = 2,
                //            Description = exp,
                //            ClientId = jobDetails.ClientId ?? 0,
                //            CreatedBy = jobDetails.ClientId.ToString(),
                //            CreatedOn = DateTime.Now,
                //        });
                //    }
                //    _dbContext.JobDetails.AddRange(jobDetail);
                //    _dbContext.SaveChanges();
                //}
                if (jobDetails.Responsibility != null)
                {
                    JobDetail jobDetail = new JobDetail();
                    jobDetail.JobId = job.JobId;
                    jobDetail.JobCodeId = 1;
                    jobDetail.Description = jobDetails.Responsibility;
                    jobDetail.ClientId = jobDetails.ClientId ?? 0;
                    jobDetail.CreatedBy = jobDetails.ClientId.ToString();
                    jobDetail.CreatedOn = DateTime.Now;
                    _dbContext.JobDetails.Add(jobDetail);
                    _dbContext.SaveChanges();
                }
                if (jobDetails.Requirement != null)
                {
                    JobDetail jobDetail = new JobDetail();
                    jobDetail.JobId = job.JobId;
                    jobDetail.JobCodeId = 9;
                    jobDetail.Description = jobDetails.Requirement;
                    jobDetail.ClientId = jobDetails.ClientId ?? 0;
                    jobDetail.CreatedBy = jobDetails.ClientId.ToString();
                    jobDetail.CreatedOn = DateTime.Now;
                    _dbContext.JobDetails.Add(jobDetail);
                    _dbContext.SaveChanges();
                }
                return CommonHelper.GetApiSuccessResponse("Success", 200);
            }
            catch (Exception ex)
            {
                string exceptionString = ex.Message + ex.StackTrace + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                return CommonHelper.GetApiSuccessResponse(exceptionString, 400);
            }
        }
    }
}
