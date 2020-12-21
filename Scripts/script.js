window.addEventListener("DOMContentLoaded", function () {
    var counter = 1;

    var startButton = document.getElementById("start-button");
    var stopButton = document.getElementById("stop-button");
    var admitWinnerButton = document.getElementById("admit-winner-button");
    var closeButton = document.getElementById("close-button");
    var returnButton = document.getElementById("return-button");
    var view1 = document.getElementById("view-1");
    var view2 = document.getElementById("view-2");
    var baseUrl = window.location.href;

    var winnerAccountDetails;

    random = function (seed) {
        var rnd = Math.floor(Math.random() * Math.floor(seed));
        if (seed) {
            return rnd;
        } else {
            return Math.floor(Math.random());
        }
    };

    timerCounter = function () {
        setTimeout(function () {
            var today = new Date();
            var time = today.toLocaleTimeString();
            document.getElementById("timer-label").innerHTML = time;
            counter += 1;

            timerCounter();
        }, 1000);
    };
    timerCounter();

    var buttons = document.getElementsByTagName("button");
    Array.prototype.forEach.call(buttons, function (button, index) {
        button.addEventListener("click", function (evt) {
            switch (this.getAttribute("id")) {
                case "view-winners-button":
                    viewWinners();
                    break;
                case "return-button":
                    returnToLotteryBoard();
                    break;
                case "start-button":
                    startLottery();
                    break;
                case "stop-button":
                    stopLottery();
                    break;
                case "admit-winner-button":
                    admitWinner();
                    break;
                case "close-button":
                    restartLottery();
                    break;
            }
        });
    });

    viewWinners = function () {
        view1.setAttribute("class", "d-none");
        view2.setAttribute("class", "d-block");

        $.get(baseUrl + "/home/GetWinners").then(function (response) {
            console.log(response);
            document.getElementById("winners-datagrid-container").innerHTML = response;
        });
    };

    returnToLotteryBoard = function () {
        view2.setAttribute("class", "d-none");
        view1.setAttribute("class", "d-block");
    };

    var lotteryTimer;

    startLottery = function () {
        var rndm = random();

        // disable stop button
        startButton.disabled = true;
        stopButton.disabled = false;

        lotteryTimer = setTimeout(function () {
            var today = new Date();
            var time = today.toLocaleTimeString();
            var entries = qualifiedMillionairesList;
            var numberOfEntries = entries.length;
            var offset = (numberOfEntries - 1) * random(numberOfEntries);
            accountDetails = entries[random(numberOfEntries)];
            var accountNumber = accountDetails.AccountNo;

            var led1 = accountNumber.substr(0, 2);
            var led2 = accountNumber.substr(2, 2);
            var led3 = accountNumber.substr(4, 2);
            var led4 = accountNumber.substr(6, 2);
            var led5 = accountNumber.substr(8, 2);

            document.getElementById("led-1").innerHTML = led1;
            document.getElementById("led-2").innerHTML = led2;
            document.getElementById("led-3").innerHTML = led3;
            document.getElementById("led-4").innerHTML = led4;
            document.getElementById("led-5").innerHTML = led5;

            var winningAccountNumber = led1 + led2 + led3 + led4 + led5;
            winnerAccountDetails = entries.find(function (account, index) {
                if (account.AccountNo === winningAccountNumber) {
                    return accountDetails;
                }
            });

            startLottery();
        }, 250);
    };

    stopLottery = function () {
        clearTimeout(lotteryTimer);
        init();

        admitWinnerButton.disabled = false;
        closeButton.disabled = false;

        // get winner's details
        getWinnerDetails();
    };

    restartLottery = function () {
        clearTimeout(lotteryTimer);
        init();
    };

    getWinnerDetails = function () {
        document.getElementById("winner-account-number").innerHTML = winnerAccountDetails.AccountNo;
        document.getElementById("winner-account-name").innerHTML = winnerAccountDetails.AccountName;
        document.getElementById("winner-mobile-number").innerHTML = winnerAccountDetails.MobileNo;
    };

    admitWinner = function () {
        var confirm = window.confirm("Are you sure you want to continue?");
        if (confirm) {
            admitWinnerButton.disabled = true;
            $.post(baseUrl + "/home/admitwinner", winnerAccountDetails).then(function (response) {
                restartLottery();
                alert("Congratulations, winner admitted successfully.");
            });
        }
    };

    getQualified = function (evt) {
        if ((evt) && (evt.target.value)) { // branch
            $.get(baseUrl + "/home/getbranchmillionaires").then(function (response) {
                qualifiedMillionairesList = JSON.parse(response);
            });
        } else {
            $.get(baseUrl + "/home/getregionalmillionaires").then(function (response) {
                qualifiedMillionairesList = JSON.parse(response);
            });
        }
    };

    init = function () {
        startButton.disabled = false;
        stopButton.disabled = true;
        admitWinnerButton.disabled = true;
        closeButton.disabled = true;

        //
        document.getElementById("led-1").innerHTML = "00";
        document.getElementById("led-2").innerHTML = "00";
        document.getElementById("led-3").innerHTML = "00";
        document.getElementById("led-4").innerHTML = "00";
        document.getElementById("led-5").innerHTML = "00";

        document.getElementById("winner-account-number").innerHTML = "0000000000";
        document.getElementById("winner-account-name").innerHTML = "&nbsp;";
        document.getElementById("winner-mobile-number").innerHTML = "&nbsp;";

        // get qualified
        getQualified();
    };
    init();
});