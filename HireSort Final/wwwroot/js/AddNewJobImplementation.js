
//<script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>

//var script = document.createElement('script');
//script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
//document.getElementsByTagName('head')[0].appendChild(script);



const uriDept = '/api/Dashboard/departments';
//const uriVacancyList = 'api/Dashboard/department-and-vacancies-details';
//const uriVacancyCount = 'api/Dashboard/depart-vacancy-count';

const ddl_Dept = document.getElementById('department');


$(document).ready(function () {
    $('#spinner').addClass('show');
    getItemsDept();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
    $('#spinner').removeClass('show');
    //Start Date Validation
    document.getElementById('sDate').valueAsDate = new Date();

    //End Date Validation
    $(function () {
        var dtToday = new Date();

        var month = dtToday.getMonth() + 1;
        var day = dtToday.getDate();
        var year = dtToday.getFullYear();
        if (month < 10)
            month = '0' + month.toString();
        if (day < 10)
            day = '0' + day.toString();

        var maxDate = year + '-' + month + '-' + day;
        $('#eDate').attr('min', maxDate);
        $('#sDate').attr('min', maxDate);
    });
});

// Get Department Dropdown
function getItemsDept() {
    fetch(uriDept)
        .then(response => response.json())
        .then(data => _displayItemsDept(data))
        .catch(error => console.error('Unable to get items.', error));


}

function _displayItemsDept(data) {

    //
    ;

    var status = data.statusCode
    var parsedata = data.successData
    if (status == 200) {
        parsedata.forEach(item => {

            let option_elem = document.createElement('option');
            option_elem.value = item.departmentId;
            option_elem.textContent = item.departmentName;
            ddl_Dept.appendChild(option_elem);
        });

        //$("option").each(function (index) {
        //	$(this).on("click", function () {
        //		alert("hello")
        //	});
        //});
    }

}

document.getElementById("btn_addSkills").onclick = function () {
    document.getElementById("list_skills").style.display = "inline-block";
    var text = document.getElementById("tb_skills").value;
    var cbInvisibleSkill = `<input name='Skills' value='${text}' class='d-none'>`;
    var li = "<li>" + text + "</li>";
    document.getElementById("list_skills").insertAdjacentHTML('beforeend', li);
    document.getElementById("cb_skills").insertAdjacentHTML('beforeend', cbInvisibleSkill);
    document.getElementById("tb_skills").value = ""; // clear the value
}

function handleFormSubmit(event) {
    var tbDept = document.getElementById("department").value;
    var tbTitle = document.getElementById("tb_jobTitle").value;
    var tbType = document.getElementById("tb_jobType").value;
    //var tbSalary = document.getElementById("tb_Salary").value;
    var tbsDate = document.getElementById("sDate").value;
    var tbeDate = document.getElementById("eDate").value;
    var tbEdu = document.getElementById("tb_Edu").value;
    var tbSkills = document.getElementById("cb_skills").value;
    var tbShift = document.getElementById("tb_jobShift").value;
    var tbExpFrom = document.getElementById("tb_ExpFrom").value;
    var tbExpTo = document.getElementById("tb_ExpTo").value;
    //var tbReq = document.getElementById("tb_Req").value;
    //var tbResp = document.getElementById("tb_Resp").value;
    //var tbDesc = document.getElementById("tb_Desc").value;



    if (tbDept == "" || tbTitle == "" || tbType == ""  || tbsDate == "" || tbeDate == "" || tbEdu == "" || tbSkills == "" || tbShift == "" || tbExpFrom == "" || tbExpTo == "" ) {
        //alert("Please fill the form correctly.");
        $('#validateformAdd').modal('toggle');

    }
    event.preventDefault();
    $('#spinner').addClass('show');

    const getFormData = new FormData(event.target);

    const formJSON = Object.fromEntries(getFormData.entries());
    // for multi-selects, we need special handling
    formJSON.Skills = getFormData.getAll("Skills");

    //const results = document.querySelector('.results pre');
    //results.innerText = JSON.stringify(result, null, 2);

$.ajax(
    {
        url: `/api/dashboard/add-job`,
        data: JSON.stringify(formJSON) ,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: 'json',
        tradditional: true,
        success: function (data) {
            $('#spinner').removeClass('show');

            $('#jobposted').modal('toggle');

            document.getElementById("frmAddNewJob").reset();
            document.getElementById("list_skills").style.display = "none";
        },
        error: function (data) {
            $('#spinner').removeClass('show');

           // $('#jobposted').modal('toggle');
            console.log(data);
        }
    }
    );

}

const form = document.querySelector('.addjob-form');
form.addEventListener('submit', handleFormSubmit);


