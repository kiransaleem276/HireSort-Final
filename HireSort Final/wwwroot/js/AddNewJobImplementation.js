
//<script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>

//var script = document.createElement('script');
//script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
//document.getElementsByTagName('head')[0].appendChild(script);

const uriDept = '/api/Dashboard/departments';
//const uriVacancyList = 'api/Dashboard/department-and-vacancies-details';
//const uriVacancyCount = 'api/Dashboard/depart-vacancy-count';

const ddl_Dept = document.getElementById('department');

const ddl_Vac = $('#vacancy');


$(document).ready(function () {
        getItemsDept();   
});

// Get Department Dropdown
function getItemsDept() {
    fetch(uriDept)
        .then(response => response.json())
        .then(data => _displayItemsDept(data))
        .catch(error => console.error('Unable to get items.', error));

    getItemsVacancy();
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

$("#department").change(function () {
  
    getItemsVacancy();
    ddl_Vac.empty();

});

$("#btn_Search").click(function myfunction() {

    var dpt_id = ddl_Dept.value;
    var vac_Id = ddl_Vac.val();

    //alert("Handler for .change() called." + ` with ids dept: ${dpt_id} and vacId: ${vac_Id}`);


    //fetch(uri, {
    //    method: 'POST',
    //    headers: {
    //        'Accept': 'application/json',
    //        'Content-Type': 'application/json'
    //    },
    //    body: JSON.stringify(item)
    //})
    var filteredVacList = `/api/Dashboard/department-and-vacancies-details?departId=${dpt_id}&vacancyId=${vac_Id}`

    fetch(filteredVacList)
        .then(response => response.json())
        .then(data => _displayItemsVacancyList(data))
        .catch(error => console.error('Unable to get items.', error));

});
// Get Vacancy Dropdown
function getItemsVacancy() {

    var deptid = ddl_Dept.value;
    console.log(ddl_Dept.options[ddl_Dept.selectedIndex].text);
    console.log(ddl_Dept.value);
    if (deptid == "")
        deptid = '1';

    const uriVacancy = `/api/Dashboard/vacancies-department-wise?departId=${deptid}`;
    fetch(uriVacancy)
        .then(response => response.json())
        .then(data => _displayItemsVacancy(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayItemsVacancy(data) {

   
    const ddl_Vac = document.getElementById('vacancy');
   

    if (data.statusCode == 200) {
        var parsedata = data.successData;

        parsedata.forEach(item => {

            let option_elem = document.createElement('option');
            option_elem.value = item.vacancyId;
            option_elem.textContent = item.vacancyName;
            ddl_Vac.appendChild(option_elem);
        });

    }

}




