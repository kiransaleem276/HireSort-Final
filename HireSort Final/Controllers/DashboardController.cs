using HireSort.Areas.Admin.Controllers;
using HireSort.Entity.DbModels;
using HireSort.Models;
using HireSort.Repository.Interface;
using HireSort_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HireSort.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private IDashboard _dashboard;
        private readonly IResumeParsing _resumeParsing;
        //private readonly int? _clientID = 0;

        public DashboardController(ILogger<HomeController> logger, IDashboard dashboard, IResumeParsing resumeParsing)
        {
            _logger = logger;
            _dashboard = dashboard;
            _resumeParsing = resumeParsing;
            //_clientID = HttpContext?.Session?.GetInt32("_ClientID");
        }

        [HttpGet]
        [Route("departments")]
        public async Task<IActionResult> GetDepartment(bool candidate = false)
        {
            var result = await _dashboard.GetDepartment((!candidate) ? HttpContext?.Session?.GetInt32("_ClientID") : 1);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }

        [HttpGet]
        [Route("vacancies-department-wise")]
        public async Task<IActionResult> GetVacanciesDepartmentWise([FromQuery] int departId, bool candidate = false)
        {
            var result = await _dashboard.GetVacanciesDepartmentWise(departId, (!candidate) ? HttpContext?.Session?.GetInt32("_ClientID") : 1);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("department-and-vacancies-details")]
        public async Task<IActionResult> GetDepartAndVacancyDetails([FromQuery] int departId, int vacancyId, bool candidate = false)
        {
            var result = await _dashboard.GetDepartAndVacacyDetails((!candidate) ? HttpContext?.Session?.GetInt32("_ClientID") : 1, departId, vacancyId);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("resume-list")]
        public async Task<IActionResult> GetAllResumes([FromQuery] int departId, int vacancyId, bool isShortListedResume = false)
        {
            var result = await _dashboard.GetAllResumes(departId, vacancyId, HttpContext?.Session?.GetInt32("_ClientID"), isShortListedResume);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("depart-vacancy-count")]
        public async Task<IActionResult> GetDepartmentVacancyCount()
        {
            var result = await _dashboard.GetDepartmentVacancyCount(HttpContext?.Session?.GetInt32("_ClientID"));
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("depart-vacancy-list")]
        public async Task<IActionResult> GetDepartmentJobs([FromQuery] int departId, bool candidate = false)
        {
            var result = await _dashboard.GetDepartmentJobs(departId, (!candidate) ? HttpContext?.Session?.GetInt32("_ClientID") : 1);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("job-detail")]
        public async Task<IActionResult> GetJobDetail([FromQuery] int departId, int jobId, bool candidate = false)
        {
            var result = await _dashboard.GetJobDetail(departId, jobId, (!candidate) ? HttpContext?.Session?.GetInt32("_ClientID") : 1);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("resume-compatibitlity")]
        public async Task<IActionResult> GetResumeCompatibiltiy([FromQuery] int resumeId, int jobId)
        {
            var result = await _dashboard.GetResumeCompatibiltiy(resumeId, jobId, HttpContext?.Session?.GetInt32("_ClientID"));
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("uploadfile")]

        public async Task<IActionResult> UploadFiles(IFormFile file, int jobId)
        {
            //foreach (IFormFile file in files)
            //{
            if (file.Length > 0)
            {
                var result = _resumeParsing.ResumeUpload(file, jobId, HttpContext?.Session?.GetInt32("_ClientID"));
                if (result.Result.StatusCode == 400)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("check-resume-compatibility")]
        public async Task<IActionResult> GetResumeContent([FromQuery] int resumeId, int jobId)
        {
            //var result = await _resumeParsing.resumeCheckCompatibility(resumeId, jobId);
            var result = await _resumeParsing.resumeCheckCompatibility(resumeId, jobId, HttpContext?.Session?.GetInt32("_ClientID"));
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpPost]
        [Route("resume-shorlisting")]
        public async Task<IActionResult> ResumeShortlisting(int resumeId)
        {
            var result = await _dashboard.ResumeShorlisting(resumeId, HttpContext?.Session?.GetInt32("_ClientID"));
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test(IFormFile files, int jobId, bool candidate = false)
        {
            if (files.Length > 0)
            {
                var result = _resumeParsing.ResumeUpload(files, jobId, (!candidate) ? HttpContext?.Session?.GetInt32("_ClientID") : 1);
                if (result.Result.StatusCode == 400)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            const string SessionEmail = "_Email";
            const string ClientID = "_ClientID";
            var result = await _dashboard.Login(email, password);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            HttpContext.Session.SetString(SessionEmail, email);
            HttpContext.Session.SetInt32(ClientID, 1);
            return Ok(result);
        }

        [HttpPost]
        [Route("apply-now")]
        public async Task<IActionResult> ApplyNow(IFormFile file, int jobId, string firstName, string lastName, string email, string coverLetter)
        {
            //foreach (IFormFile file in files)
            //{
            if (file.Length > 0)
            {
                var result = _resumeParsing.ResumeUpload(file, jobId, 1, firstName, lastName, email, coverLetter);
                if (result.Result.StatusCode == 400)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("add-job")]
        public async Task<IActionResult> AddJob(string jobDetail="")
        {
            var detail = JsonConvert.DeserializeObject<AddJobDetail>(jobDetail);
            var result = await _dashboard.AddJob(detail);
            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
