(function() {
    "use strict";
    /* code here */

    var urlData = "https://fcctop100.herokuapp.com/api/fccusers/top/recent";
    var urlData2 = "https://fcctop100.herokuapp.com/api/fccusers/top/alltime";
    var recentData = [];
    var allData = [];

    function init() {
        console.log("Inicio");
        getData(urlData);
    }

    function goOn() {
        document.getElementById("theadLast").addEventListener("click",
            function() {
                populateTable(recentData);
            });
        document.getElementById("theadAll").addEventListener("click",
            function() {
                populateTable(allData);
            });
    }

    function populateTable(dataSet) {
        var orderedByLast = "";
        var orderedByAll = "";

        if (dataSet === recentData) {
            orderedByLast = '<img class="arrow" ' +
                'src="https://raw.githubusercontent.com/brusbilis/freeCodeCamp/master/old-v1/2-data/react/leaderboard/images/arrowDown20x20.png">';
        } else if (dataSet === allData) {
            orderedByAll = '<img class="arrow" ' +
                'src="https://raw.githubusercontent.com/brusbilis/freeCodeCamp/master/old-v1/2-data/react/leaderboard/images/arrowDown20x20.png">';
        }

        const table = '<table class="table table-sm table-striped table-bordered text-xs-center">';
        const theadPos = '<thead class="thead-inverse"><tr><th class="col-xs-1 text-xs-center">#</th>';
        const theadName = '<th class="col-xs-4 text-xs-center">Username</th>';
        const theadLast = `<th id="theadLast" class="col-xs-3 text-xs-center">Gross Profit ${orderedByLast}</th>`;
        const theadAll = `<th id="theadAll" class="col-xs-2 text-xs-center">Max Cash Gain ${orderedByAll}</th>`;
        const thead3 = '<th id="thead3" class="col-xs-2 text-xs-center">Games Played ' +
            "</th></tr></thead><tbody>";
        var res = table + theadPos + theadName + theadLast + theadAll + thead3;

        for (let i = 0; i < 20; i++) {
            const name = `<a>${dataSet[i].username}</a>`;
            res += `<tr><th class="col-xs-1 text-warning text-xs-center" scope="row">${i + 1}</th>`;
            res += `<td class="col-xs-4 text-warning text-xs-left"><img class="logo" src="${dataSet[i].img}"> `;
            res += ` &nbsp; ${name}</td>`;
            res += `<td class="col-xs-3 text-warning">${dataSet[i].recent}</td>`;
            res += `<td class="col-xs-2 text-warning">${dataSet[i].alltime}</td>`;
            res += `<td class="col-xs-2 text-warning">${0}</td></tr>`;
        }
        res += "</tbody>";
        document.getElementById("dataTable").innerHTML = res;
        // each population events listener disappears so we renove them
        goOn();
    }

    function getData(url) {
        // console.log('Getting data ...')
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function() {

            const done = 4;
            if (xhr.readyState === done) {
                const ok = 200;
                if (xhr.status === ok) {
                    if (url === urlData) {
                        recentData = JSON.parse(xhr.responseText);
                        getData(urlData2);
                    }
                    if (url === urlData2) {
                        allData = JSON.parse(xhr.responseText);
                        populateTable(recentData);
                    }
                } else {
                    console.log(`Error: ${xhr.status}`);
                }
            }
        };
        xhr.open("GET", url); // add false to synchronous request
        xhr.send();
    }

    window.addEventListener("load", init);
}());

function goto() {
    window.location = "user.html"; // Redirecting to other page.
    return false;
}