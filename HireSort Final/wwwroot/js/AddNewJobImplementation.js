
//<script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>

//var script = document.createElement('script');
//script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
//document.getElementsByTagName('head')[0].appendChild(script);



const uriDept = '/api/Dashboard/departments';
//const uriVacancyList = 'api/Dashboard/department-and-vacancies-details';
//const uriVacancyCount = 'api/Dashboard/depart-vacancy-count';

const ddl_Dept = document.getElementById('department');


$(document).ready(function () {

    getItemsDept();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
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
    var text = document.getElementById("tb_skills").value;
    var cbInvisibleSkill = `<input name='Skills' value='${text}' class='d-none'>`;
    var li = "<li>" + text + "</li>";
    document.getElementById("list_skills").insertAdjacentHTML('beforeend', li);
    document.getElementById("cb_skills").insertAdjacentHTML('beforeend', cbInvisibleSkill);
    document.getElementById("tb_skills").value = ""; // clear the value
}

function handleFormSubmit(event) {
    event.preventDefault();

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
            alert("Job Posted");
            document.getElementById("frmAddNewJob").reset();
        },
        error: function (data) {
            alert(data);
        }
    }
);
}

const form = document.querySelector('.addjob-form');
form.addEventListener('submit', handleFormSubmit);



