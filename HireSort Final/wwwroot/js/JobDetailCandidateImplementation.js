//const { error } = require("jquery");



var deptID;
var vacID;

$(document).ready(function () {
    //var queryString = window.location.href;
    ////window.location.href.slice(window.location.href.indexOf('?') + 1);
    //const searchParams = queryString.searchParams;
    //var deptID = searchParams.get('departId');
    //var vacID = searchParams.get('vacancyId');

    const params = new URLSearchParams(window.location.search);
    deptID = params.get('departId');
    vacID = params.get('vacancyId');
    candidate = params.get('candidate');


    if (window.location.search == "Admin") {
        document.getElementById("btnAddNew").style.display = "inline-block";
        //document.getElementById("btnHome").style.display = "inline-block";
    }
    else {
        document.getElementById("btnAddNew").style.display = "none";
        //document.getElementById("btnHome").style.display = "none";
    }

    getJobDetails();
    //resumeShortlist();
});



//Apply Now Functionality
function applyNow(fname, lname, email, files, coverLetter) {
    var firstname = document.getElementById(fname).value;
    var lastname = document.getElementById(lname).value;
    var emailAdd = document.getElementById(email).value;
    var cover = document.getElementById(coverLetter).value;
    var fileName = document.getElementById(files);
    var file = fileName.files;

    if (firstname == "" || lastname == "" || emailAdd == "" || file.length == 0) {
        //alert("Please fill the form correctly.");
        $('#validateform').modal('toggle');
        return;
    }
    var formData = new FormData();
    formData.append("file", file[0]);

    $.ajax(
        {
            //url: "/api/dashboard/uplojjjjjjjadfile?jobId=1",
            url: `/api/dashboard/apply-now?jobId=${vacID}&firstName=${firstname}&lastName=${lastname}&email=${emailAdd}&coverletter="${cover}"&candidate=${candidate}`,
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
                //location.reload();
                //getResumeList();
               // alert("Files Uploaded!");
                $('#jobapplied').modal('toggle');

                document.getElementById("frmApply").reset();
            },
            error: function (data) {
                // location.reload();
                //getResumeList();
                console.log(data);
            }
        }
    );
}


//Email Validation

const validateEmail = (email) => {
    return email.match(
        /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};

const validate = () => {
    const $result = $('#result');
    const email = $('#email').val();
    $result.text('');

    if (!validateEmail(email)) {
        $result.removeClass('d-none');
        $result.addClass('d-block');
        $result.addClass('mt-2');
        $result.text('Email is invalid.');
      
    } else {
        $result.remove('d-block');
        $result.addClass('d-none');
    }
    return false;
}

$('#email').on('keyup', validate);



function getJobDetails() {

    const uriResumeList = `/api/dashboard/job-detail?departId=${deptID}&jobId=${vacID}&candidate=${candidate}`
    fetch(uriResumeList)
        .then(response => response.json())
        .then(data => _displayJobDetails(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayJobDetails(data) {
    const jobdetailist = document.getElementById('jobDesc');
    const jobTenureList = document.getElementById('jobSummary');
    const jobResponsibility = document.getElementById('jobResponsibility');
    const jobSkills = document.getElementById('jobSkills');


    var status = data.statusCode
    var parsedata = data.successData
    if (status == 200) {
        parsedata.forEach(item => {

            //Job Desc
            let jobDescription = document.createElement('h3');
            jobDescription.classList.add('mb-3');
            jobDescription.classList.add('headingColor');
            jobDescription.textContent = item.jobName;
            //jobDescription.style.color = '#5384C9 !important';
            jobdetailist.appendChild(jobDescription);

            //Start Date
            let startDate = document.createElement('p');
            let iconArr1 = document.createElement('i');
            iconArr1.classList.add('fa', 'fa-angle-right', 'text-primary', 'me-2');

            startDate.appendChild(iconArr1);
            startDate.innerHTML = "<b>Start Date:\n</b>" + item.jobStartDate;
            jobTenureList.appendChild(startDate);

            //END Date
            let endDate = document.createElement('p');
            let iconArr2 = document.createElement('i');
            iconArr2.classList.add('fa', 'fa-angle-right', 'text-primary', 'me-2');

            endDate.appendChild(iconArr2);
            endDate.innerHTML = "<b>End Date:\n</b>" + item.jobEndDate;
            jobTenureList.appendChild(endDate);


            //job Type
            let jobType = document.createElement('p');
            let iconArr3 = document.createElement('i');
            iconArr3.classList.add('fa', 'fa-angle-right', 'text-primary', 'me-2');

            jobType.appendChild(iconArr3);
            jobType.innerHTML = "<b>Job Type:\n</b>" + item.jobType;
            jobTenureList.appendChild(jobType);


            //Job shift
            let jobShift = document.createElement('p');
            let iconArr4 = document.createElement('i');
            iconArr4.classList.add('fa', 'fa-angle-right', 'text-primary', 'me-2');

            jobShift.appendChild(iconArr4);
            jobShift.innerHTML = "<b>Job Shift:\n</b>" + item.jobShift;
            jobTenureList.appendChild(jobShift);

            //Job Description in Detail
            var parsedataJobDesc = item.jobDesc

            parsedataJobDesc.forEach(item => {
                //Job Responsibility

                let responsibility = document.createElement('h3');
                responsibility.classList.add('mb-3');
                responsibility.textContent = item.jobCode;
                jobResponsibility.appendChild(responsibility);

                let reponsibilityDetail = document.createElement('p');
                reponsibilityDetail.textContent = item.description;
                jobResponsibility.appendChild(reponsibilityDetail);
            });


            //Job Skills in Detail

            var parseJobSkills = item.jobSkills
            parseJobSkills.forEach(item => {
                let skills = document.createElement('span');
                skills.classList.add('skilltags');
                skills.textContent = item.description;
                jobSkills.appendChild(skills);
            });
        });
    }
}
