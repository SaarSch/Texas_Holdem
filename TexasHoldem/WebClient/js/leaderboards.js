(function() {
    "use strict";
    /* code here */

    var urlData1 = "http://localhost:57856/api/StatisticsLst?kind=1";
    var urlData2 = "http://localhost:57856/api/StatisticsLst?kind=2";
    var urlData3 = "http://localhost:57856/api/StatisticsLst?kind=3";
    var grossData = [];
    var cashData = [];
    var numData = [];


    function init() {
        console.log("Inicio");
        getData(urlData1);
    }

    function goOn() {
        document.getElementById("theadLast").addEventListener("click",
            function() {
                populateTable(grossData);
            });
        document.getElementById("theadAll").addEventListener("click",
            function() {
                populateTable(cashData);
            });
        document.getElementById("thead3").addEventListener("click",
            function () {
                populateTable(numData);
            });
    }

    function populateTable(dataSet) {
        var orderedByGross = "";
        var orderedByCash = "";
        var orderedByNum = "";

        if (dataSet === grossData) {
            orderedByGross = '<img class="arrow" ' +
                'src="https://raw.githubusercontent.com/brusbilis/freeCodeCamp/master/old-v1/2-data/react/leaderboard/images/arrowDown20x20.png">';
        } else if (dataSet === cashData) {
            orderedByCash = '<img class="arrow" ' +
                'src="https://raw.githubusercontent.com/brusbilis/freeCodeCamp/master/old-v1/2-data/react/leaderboard/images/arrowDown20x20.png">';
        }
        else if (dataSet === numData) {
            orderedByNum = '<img class="arrow" ' +
                'src="https://raw.githubusercontent.com/brusbilis/freeCodeCamp/master/old-v1/2-data/react/leaderboard/images/arrowDown20x20.png">';
        }

        const table = '<table class="table table-sm table-striped table-bordered text-xs-center">';
        const theadPos = '<thead class="thead-inverse"><tr><th class="col-xs-1 text-xs-center">#</th>';
        const theadName = '<th class="col-xs-4 text-xs-center">Username</th>';
        const theadLast = `<th id="theadLast" class="col-xs-3 text-xs-center">Gross Profit ${orderedByGross}</th>`;
        const theadAll = `<th id="theadAll" class="col-xs-2 text-xs-center">Max Cash Gain ${orderedByCash}</th>`;
        const thead3 = `<th id="thead3" class="col-xs-2 text-xs-center">Games Played ${orderedByNum}</th>` +
            "</tr></thead><tbody>";
        var res = table + theadPos + theadName + theadLast + theadAll + thead3;

        for (let i = 0; i < dataSet.length; i++) {
            const name = `<a>${dataSet[i].Username}</a>`;
            res += `<tr><th class="col-xs-1 text-warning text-xs-center" scope="row">${i + 1}</th>`;
            res += `<td class="col-xs-4 text-warning text-xs-left"><img class="logo" src="css/${dataSet[i].AvatarPath.substring(10)}" alt="avatar"> `;
            res += ` &nbsp; ${name}</td>`;
            res += `<td class="col-xs-3 text-warning">${dataSet[i].GrossProfit}</td>`;
            res += `<td class="col-xs-2 text-warning">${dataSet[i].HighestCashGain}</td>`;
            res += `<td class="col-xs-2 text-warning">${dataSet[i].NumOfGames}</td></tr>`;
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
                    if (url === urlData1) {
                        grossData = JSON.parse(xhr.responseText);
                        getData(urlData2);
                    }
                    if (url === urlData2) {
                        cashData = JSON.parse(xhr.responseText);
                        getData(urlData3);
                    }
                    if (url === urlData3) {
                        numData = JSON.parse(xhr.responseText);
                        populateTable(grossData);
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
    //restore current user
    if (sessionStorage.getItem("tuser")) {
        sessionStorage.setItem("user", sessionStorage.getItem("tuser"));
        sessionStorage.setItem("gross", sessionStorage.getItem("tgross"));
        sessionStorage.setItem("cash", sessionStorage.getItem("tcash"));
        sessionStorage.setItem("avatar", sessionStorage.getItem("tavatar"));
    }
    window.location = "user.html"; // Redirecting to other page.
    return false;
}