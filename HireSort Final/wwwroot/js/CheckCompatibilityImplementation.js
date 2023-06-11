

var jobID;
var resumeID;
/*var departID;*/
$(document).ready(function () {

     //$('#spinner').addClass('show');
    //var queryString = window.location.href;
    ////window.location.href.slice(window.location.href.indexOf('?') + 1);
    //const searchParams = queryString.searchParams;
    //var deptID = searchParams.get('departId');
    //var vacID = searchParams.get('vacancyId');
    const params = new URLSearchParams(window.location.search);

    jobID = params.get('jobId');
    resumeID = params.get('resumeId');
    departID = params.get('departId');

    getResumeInd();
    getCompatibilty();
    //$('#spinner').removeClass('show');
});







function resumeShortlist(resumeId) {

    $('#spinner').addClass('show');
    //const uriCheckCompatibilty = `/api/dashboard/check-resume-compatibility?jobId=${jobID}&resumeId=${resumeID}`
    //fetch(uriCheckCompatibilty)
    //    .then(response => response.json())
    //    .then(data => _displayCompatibilty(data))
    //    .catch(error => console.error('Unable to get items.', error));

    $.ajax(
        {
            //url: "/api/dashboard/uploadfile?jobId=1",
            url: `/api/dashboard/resume-shorlisting?resumeId=${resumeID}`,
            //data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function () {

                getCompatibilty();
                location.reload();

                $('#spinner').removeClass('show');
            }


        }
    );
}

function getCompatibilty() {

    $('#spinner').addClass('show');
    //const uriCheckCompatibilty = `/api/dashboard/check-resume-compatibility?jobId=${jobID}&resumeId=${resumeID}`
    //fetch(uriCheckCompatibilty)
    //    .then(response => response.json())
    //    .then(data => _displayCompatibilty(data))
    //    .catch(error => console.error('Unable to get items.', error));

    $.ajax(
        {
            //url: "/api/dashboard/uploadfile?jobId=1",
            url: `/api/dashboard/check-resume-compatibility?jobId=${jobID}&resumeId=${resumeID}`,
            //data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function () {
                getResumeInd();

                $('#spinner').removeClass('show');
                //alert("Files Uploaded!");
            }


        }
    );
}
function getResumeInd() {
    $('#spinner').addClass('show');

    const uriResumeList = `/api/dashboard/resume-compatibitlity?resumeId=${resumeID}&jobId=${jobID}`
    fetch(uriResumeList)
        .then(response => response.json())
        .then(data => _displayResumeInd(data))
        .catch(error => console.error('Unable to get items.', error));
    $('#spinner').removeClass('show');

}

function _displayResumeInd(data) {
    $('#spinner').addClass('show');

    const resumeProfile = document.getElementById('candidateProfile');
    const progressBarInd = document.getElementById('progressBarInd');

    const divCGPAandIns = document.getElementById('divCGPAandIns');
    const resumeExp = document.getElementById('experience');
    const resumeEdu = document.getElementById('education');
    const resumeSkills = document.getElementById('skills');
   // const resumeLinks = document.getElementById('links');

  
    var status = data.statusCode
    var parsedata = data.successData
   
    if (status == 200) {
        parsedata.forEach(item => {

            let txtCandidate = document.createElement('h5');
            txtCandidate.classList.add('card-title','my-3');
            txtCandidate.textContent = item.candidateName;

            let divCardEmail= document.createElement('div');
            divCardEmail.classList.add('ps-4');

            let divCardNo = document.createElement('div');
            divCardNo.classList.add('ps-4');


            let txtEmail = document.createElement('span');
            txtEmail.classList.add('text-truncate', 'me-3');
            txtEmail.textContent = item.email;

            let iconEmail = document.createElement('i');
            iconEmail.classList.add('fa', 'fa-envelope', 'text-primary', 'me-2');

            let txtMobile = document.createElement('span');
            txtMobile.classList.add('text-truncate', 'me-3');
            txtMobile.textContent = item.mobieNo;

            let iconMobile = document.createElement('i');
            iconMobile.classList.add('fa', 'fa-mobile', 'text-primary', 'me-2');

            let btnShortlist = document.createElement('a');
            btnShortlist.classList.add('btn', 'btn-primary', 'ms-3');
          //  let textShortlist = document.createTextNode("Shortlist");
           // btnShortlist.href = `javascript:resumeShortlist(${resumeID});`

           // btnShortlist.appendChild(textShortlist);

            if (item.isShortlisted == true) {
                let textShortlist = document.createTextNode("Shortlisted!");
                btnShortlist.setAttribute('style', 'background-color:#85d6bb !important');
                btnShortlist.href = `javascript:void(0);`
                btnShortlist.appendChild(textShortlist);

            }

            else {
                let textShortlist = document.createTextNode("\n Shortlist \n");
                btnShortlist.classList.add('px-4');
                btnShortlist.href = `javascript:resumeShortlist(${resumeID});`
                btnShortlist.appendChild(textShortlist);


            }

            //txtEmail.appendChild(iconEmail);
            // txtMobile.appendChild(iconMobile);
            divCardEmail.appendChild(iconEmail);
            divCardEmail.appendChild(txtEmail);
            divCardNo.appendChild(iconMobile);
            divCardNo.appendChild(txtMobile);

            resumeProfile.appendChild(divCardEmail); 
            resumeProfile.appendChild(divCardNo);
            resumeProfile.appendChild(txtCandidate);
            resumeProfile.appendChild(btnShortlist)

            let progressMain = document.createElement('div');
            progressMain.classList.add('progress');
            progressMain.setAttribute('style', 'height: 5%');

            var width = 'width: ' + `${item.compatiblePercentage}` + '%';
            let progressBar = document.createElement('div');
            progressBar.classList.add('progress-bar', 'progress-bar-striped', 'bg-success');
            progressBar.textContent = item.compatiblePercentage + '%';
            
            progressBar.setAttribute('style',width);
            progressBar.setAttribute('aria-valuenow', '25');
            progressBar.setAttribute('aria-valuemin', '25');
            progressBar.setAttribute('aria-valuemax', '25');

            progressMain.appendChild(progressBar);

            progressBarInd.appendChild(progressMain);

            let textGPA = document.createElement('p');
            textGPA.textContent = "CGPA: \n" + item.gpa;
            let textIns = document.createElement('p');
            textIns.textContent = "Institute Criteria Match: \n" + item.instituteMatch;

            divCGPAandIns.appendChild(textGPA)
            divCGPAandIns.appendChild(textIns)

            var parsedataExp = item.experience
            var parsedataEdu = item.educations

            var parsedataSkills = item.skills
            var parsedataLinks = item.links

            //Experience
            parsedataExp.forEach(item => {



                let txtCompany = document.createElement('p');
                txtCompany.innerHTML = "<b>Company Name:\n</b>" + item.companyName

                let txtDesignation = document.createElement('p');
                txtDesignation.innerHTML = "<b>Designation:\n</b>" + item.designation

                let txtResp = document.createElement('p');
                txtResp.innerHTML = "<b>Responsibility:\n</b>" + item.responsiblility

                let txtSDate = document.createElement('p');
                txtSDate.innerHTML = "<b>Start Date:\n</b>" + item.startDate

                let txtEdate = document.createElement('p');
                txtEdate.innerHTML = "<b>End Date:\n</b>" + item.endDate

                let txtExp = document.createElement('p');
                txtExp.innerHTML = "<b>Experience:\n</b>" + item.totalExperience + "\nMonths"

                let txtHR = document.createElement('hr');

                resumeExp.appendChild(txtCompany);
                resumeExp.appendChild(txtDesignation);
                resumeExp.appendChild(txtResp);
                resumeExp.appendChild(txtSDate);
                resumeExp.appendChild(txtEdate);
                resumeExp.appendChild(txtExp);
                resumeExp.appendChild(txtHR);


            });

            //Education
            parsedataEdu.forEach(item => {



                let txtInstitute = document.createElement('p');
                txtInstitute.innerHTML = "<b>Institute Name:\n</b>" + item.instituteName

                let txtDegree = document.createElement('p');
                txtDegree.innerHTML = "<b>Degree:\n</b>" + item.degreeName

                let txtCgpa = document.createElement('p');
                txtCgpa.innerHTML = "<b>CGPA:\n</b>" + item.cgpa

                let txtSDate = document.createElement('p');
                txtSDate.innerHTML = "<b>Start Date:\n</b>" + item.startDate

                let txtEdate = document.createElement('p');
                txtEdate.innerHTML = "<b>End Date:\n</b>" + item.endDate


                let txtHR = document.createElement('hr');

                resumeEdu.appendChild(txtInstitute);
                resumeEdu.appendChild(txtDegree);
                resumeEdu.appendChild(txtCgpa);
                resumeEdu.appendChild(txtSDate);
                resumeEdu.appendChild(txtEdate);
                resumeEdu.appendChild(txtHR);


            });

            //Skills
            parsedataSkills.forEach(item => {

                let txtSkillName = document.createElement('span');
                txtSkillName.classList.add('skilltags');
                txtSkillName.textContent = item.skillName;
              //  let txtHR = document.createElement('hr');
                resumeSkills.appendChild(txtSkillName);
              //  resumeSkills.appendChild(txtHR);


            });

            ////Links
            //parsedataLinks.forEach(item => {
            //    let txtHR = document.createElement('hr');
            //    let txtLinkName = document.createElement('a');
            //    txtLinkName.textContent = 'https://www.' + item.link; 
            //    txtLinkName.href = 'https://www.' + item.link;
            //    txtLinkName.target = "_blank";
            //    resumeLinks.appendChild(txtHR);
            //    resumeLinks.appendChild(txtLinkName);
            


            //});

        });



    }
    $('#spinner').removeClass('show');

}
