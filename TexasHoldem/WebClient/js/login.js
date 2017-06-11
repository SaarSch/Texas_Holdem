var attempt = 3; // Variable to count number of attempts.
var userData = [];
// Below function Executes on click of login button.
function validate(){
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    getData(username, password);
    if (userData.Username === username) {
        sessionStorage.setItem("user", userData.Username);
        sessionStorage.setItem("gross", userData.AvgGrossProfit);
        sessionStorage.setItem("cash", userData.AvgCashGain);
        sessionStorage.setItem("avatar", userData.AvatarPath);
        window.location = "user.html"; // Redirecting to other page.
    } else {
        attempt --;// Decrementing by one.
        alert(`Invalid username/password. You have ${attempt} attempts left`);
// Disabling fields after 3 attempts.
        if(attempt === 0) {
            document.getElementById("username").disabled = true;
            document.getElementById("password").disabled = true;
            document.getElementById("submit").disabled = true;
        }
    }
    return false;
}

function getData(username, password) {
    const url = `http://localhost:57856/api/Statistics?userName=${username}&password=${password}`;
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {

        const done = 4;
        if (xhr.readyState === done) {
            const ok = 200;
            if (xhr.status === ok) {
                userData = JSON.parse(xhr.responseText);
            } else {
                console.log(`Error: ${xhr.status}`);
            }
        }
    };
    xhr.open("GET", url, false); // add false to synchronous request
    xhr.send();
}