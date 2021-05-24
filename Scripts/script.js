
var counter = 0;
var startButton = document.getElementById("start-button");
var stopButton = document.getElementById("stop-button");
var admitWinnerButton = document.getElementById("admit-winner-button");
var closeButton = document.getElementById("close-button");
var returnButton = document.getElementById("return-button");

var view1 = document.getElementById("view-1");
var view2 = document.getElementById("view-2");
$(document).ready(function () {
    //Populate Region

    var allQualifiedMillionairesList

    qualifiedMillionairesList();

    timerCounter();

    $.ajax({
        type: "Get",
        url: "Home/getRegion?regCode",
        contentType: "html",
        success: function (response) {
            $("#regionId").html("");
            let objArray = $.parseJSON(response)
            objArray.forEach(function (arrayElem) {
                $("#regionId").append(new Option(arrayElem.Name, arrayElem.Id));
            });
        }
    })

    $("#regionId").change(function () {
        var regionalId = $(this).val();
        qualifiedMillionairesList("region")
        $.ajax({
            type: "Get",
            url: "Home/getZone?regCode=" + regionalId,
            contentType: "html",
            success: function (response) {
                $("#zoneId").html("");
                let objArray = $.parseJSON(response)
                objArray.forEach(function (arrayElem) {
                    $("#zoneId").append(new Option(arrayElem.Name, arrayElem.Id));
                });
            }
        })
    });

    $("#zoneId").change(function () {
        var zoneTypeId = $(this).val();
        qualifiedMillionairesList("zone")
        $("#branchId").html("");
        $.ajax({
            type: "get",
            url: "Home/getBranch?zoneCode=" + zoneTypeId,
            contentType: "html",
            success: function (response) {
                let objArray = $.parseJSON(response)
                console.log(objArray)

                objArray.forEach(function (arrayElem) {
                    $("#branchId").append(new Option(arrayElem.Name, arrayElem.Id));
                });
            }
        })
    });

    $("#branchId").change(function () {
        qualifiedMillionairesList("branch")
    });

    $("#start-button").click(function ()
    {
       // disable stop button
      
        $("#start-button").attr('disabled', 'disabled');
        $("#stop-button").removeAttr('disabled');
        $("#branchId").attr('disabled', 'disabled');
        $("#zoneId").attr('disabled', 'disabled');
        $("#regionId").attr('disabled', 'disabled');

        startLottery();
    })

    $("#stop-button").click(function () {
        clearTimeout(lotteryTimer);
        $("#admit-winner-button").removeAttr('disabled');
        $("#close-button").removeAttr('disabled');
        $("#stop-button").attr('disabled', 'disabled');
        getWinnerDetails();

    })

    $("#admit-winner-button").click(function () {
        admitWinner()
    })

    $("#close-button").click(function () {
        init()
    })

    $("#view-winners-button").click(function () {
        viewWinners()
    })

    $("#return-button").click(function () {
        returnToLotteryBoard()
    })


    

});

function qualifiedMillionairesList(critatia) {
    let Url = "Home/getmillionaires"
    if (critatia == "zone") {
        Url = "Home/getzonemillionaires?zoneCode=" + $("#zoneId").val()
    }
    else if (critatia == "branch") {
        Url = "Home/getbranchmillionaires?branchCode=" + $("#branchId").val()
    }
    else if (critatia == "region") {
        Url = "Home/getregionalmillionaires?regCode=" + $("#regionId").val()
    }

    $.ajax({
        type: "get",
        url: Url,
        success: function (response) {
            allQualifiedMillionairesList = $.parseJSON(response)
            $("#EntriesNo").html(allQualifiedMillionairesList.length)           
        }
    })
}

function doRandom(seed) {
    var rnd = Math.floor(Math.random() * Math.floor(seed));
    if (seed) {
        return rnd;
    } else {
        return Math.floor(Math.random());
    }
};

function timerCounter() {
    setTimeout(function () {
        var today = new Date();
        var time = today.toLocaleTimeString();
        document.getElementById("timer-label").innerHTML = time;
        counter += 1;
        timerCounter();
    }, 1000);
};

function startLottery () {
    lotteryTimer = setTimeout(function () {
        var today = new Date();
        var time = today.toLocaleTimeString();
        var entries = allQualifiedMillionairesList;
        var numberOfEntries = entries.length;
        var offset = (numberOfEntries - 1) * doRandom(numberOfEntries);
        accountDetails = entries[doRandom(numberOfEntries)];
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
    }, 100);
};

function getWinnerDetails() {
        document.getElementById("winner-account-number").innerHTML = winnerAccountDetails.AccountNo;
        document.getElementById("winner-account-name").innerHTML = winnerAccountDetails.AccountName;
        document.getElementById("winner-mobile-number").innerHTML = winnerAccountDetails.MobileNo;
    };

function admitWinner() {
        var confirm = window.confirm("Are you sure you want to continue?");
        if (confirm) {
            admitWinnerButton.disabled = true;
            $.post("home/admitwinner", winnerAccountDetails).then(function (response) {
                restartLottery();
                alert("Congratulations, winner admitted successfully.");
            });
        }
    };

function init() {

    $("#start-button").removeAttr('disabled');
    $("#stop-button").attr('disabled', 'disabled');
    $("#close-button").attr('disabled', 'disabled');
    $("#admit-winner-button").attr('disabled', 'disabled');

    $("#branchId").removeAttr('disabled');
    $("#zoneId").removeAttr('disabled');
    $("#regionId").removeAttr('disabled');

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
    qualifiedMillionairesList();
};

function viewWinners () {
        view1.setAttribute("class", "d-none");
        view2.setAttribute("class", "d-block");

        $.get("home/GetWinners").then(function (response) {
            console.log(response);
            document.getElementById("winners-datagrid-container").innerHTML = response;
        });
};

function returnToLotteryBoard() {
        view2.setAttribute("class", "d-none");
        view1.setAttribute("class", "d-block");
    };

