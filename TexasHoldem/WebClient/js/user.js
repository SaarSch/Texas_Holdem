function goto(page) {
    if (page === "login") {
        window.location = "login.html"; // Redirecting to other page.
        return false;
    }
    if (page === "leaderboards") {
        window.location = "leaderboards.html";
        return false;
    }
}

var userData = [];
// Below function Executes on click of search button.
function getNewUser() {
    const username = document.getElementById("newuser").value;
    getData(username);
    if (userData.Username === username) {
        // saves current user
        sessionStorage.setItem("tuser", sessionStorage.getItem("user"));
        sessionStorage.setItem("tgross", sessionStorage.getItem("gross"));
        sessionStorage.setItem("tcash", sessionStorage.getItem("cash"));
        sessionStorage.setItem("tavatar", sessionStorage.getItem("avatar"));
        //display new user
        sessionStorage.setItem("user", userData.Username);
        sessionStorage.setItem("gross", userData.AvgGrossProfit);
        sessionStorage.setItem("cash", userData.AvgCashGain);
        sessionStorage.setItem("avatar", userData.AvatarPath);
        window.location.reload();
    } else {
        alert("User not found.");
    }
    return false;
}

function getData(username) {
    const url = `http://localhost:57856/api/Statistics?userName=${username}`;
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