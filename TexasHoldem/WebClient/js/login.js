var attempt = 3; // Variable to count number of attempts.
// Below function Executes on click of login button.
function validate(){
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    if ( username === "admin" && password === "admin"){
        window.location = "user.html"; // Redirecting to other page.
    }
    else{
        attempt --;// Decrementing by one.
        alert(`Invalid username/password. You have ${attempt} attempts left`);
// Disabling fields after 3 attempts.
        if( attempt === 0){
            document.getElementById("username").disabled = true;
            document.getElementById("password").disabled = true;
            document.getElementById("submit").disabled = true;
        }
    }
    return false;
}