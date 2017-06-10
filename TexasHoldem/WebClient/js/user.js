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