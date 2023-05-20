
//<script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>

//var script = document.createElement('script');
//script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
//document.getElementsByTagName('head')[0].appendChild(script);



// Get Login
function getLogin() {

    var username = document.getElementById('tb_username').value;
    var password = document.getElementById('tb_password').value;

   
    const uriHome = `api/dashboard/login?email=${username}&password=${password}`;
    fetch(uriHome)
 //  .then(response => response.json())
   .then(data => _displayHome(data))
    .catch(error => console.error('Unable to get items.', error));
}

function _displayHome(data) {

    var status = data.status
  
    if (status == 200) {
        document.getElementById('btn_Login').href = "Admin/AdminDashboard/AdminDashboard"
    }

    else {
        alert("Login failure");
    }

}

