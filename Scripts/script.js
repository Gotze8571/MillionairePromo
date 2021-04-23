window.addEventListener("DOMContentLoaded", function () {
    var counter = 1;

    var noOfEntriesAccountNumber;
    var startButton = document.getElementById("start-button");
    var stopButton = document.getElementById("stop-button");
    var admitWinnerButton = document.getElementById("admit-winner-button");
    var closeButton = document.getElementById("close-button");
    var returnButton = document.getElementById("return-button");
    var view1 = document.getElementById("view-1");
    var view2 = document.getElementById("view-2");
    var baseUrl = window.location.href;

    var winnerAccountDetails;
    var getdropdownList;

    random = function (seed) {
        var rnd = Math.floor(Math.random() * Math.floor(seed));
        if (seed) {
            return rnd;
        } else {
            return Math.floor(Math.random());
        }
    };
    // Stop watch 
    //timerCounter = function () {
    //    setTimeout(function () {
    //        var today = new Date();
    //        var time = today.toLocaleTimeString();
    //        document.getElementById("timer-label").innerHTML = time;
    //        counter += 1;

    //        timerCounter();
    //    }, 100000);
    //};
    //timerCounter();

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

        $.get(baseUrl + "/GetWinners").then(function (response) {
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
            console.log(numberOfEntries)
            console.log(offset)
            console.log(entries)
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

    // Get number of Account in the database.
    noOfEntriesAccountNumber = function () {
        $.post(baseUrl + "/getnoOfEntries").then(function (response) {
            NoOfEntiries = JSON.parse(response);
        });
    };
    
    ////
    noOfEntriesAccountNumberA = function () {
        document.getElementById("EntriesNo").innerHTML = NoOfEntiries.number;
    };
    ///

    getWinnerDetails = function () {
        document.getElementById("winner-account-number").innerHTML = winnerAccountDetails.AccountNo;
        document.getElementById("winner-account-name").innerHTML = winnerAccountDetails.AccountName;
        document.getElementById("winner-mobile-number").innerHTML = winnerAccountDetails.MobileNo;
    };

    admitWinner = function () {
        var confirm = window.confirm("Are you sure you want to continue?");
        if (confirm) {
            admitWinnerButton.disabled = true;
            $.post(baseUrl + "/admitwinner", winnerAccountDetails).then(function (response) {
                restartLottery();
                alert("Congratulations, winner admitted successfully.");
            });
        }
    };

    getQualified = function (evt) {
        if ((evt) && (evt.target.value)) { // branch
            $.get(baseUrl + "/getbranchmillionaires").then(function (response) {
                qualifiedMillionairesList = JSON.parse(response);
            });
        } else {
            $.get(baseUrl + "/getRegion").then(function (response) {
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
    // Set the date we're counting down to
    var setDate = "May 30, 2021 15:37:25";
    var countDownDate = new Date(setDate).getTime();
    //var countDownDate = new Date("May 30, 2021 15:37:25").getTime();

    // Update the count down every 1 second
    var x = setInterval(function () {

        // Get today's date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Output the result in an element with id="demo"
        document.getElementById("timer-label").innerHTML = days + "d " + hours + "h "
            + minutes + "m " + seconds + "s ";

        // If the count down is over, write some text 
        if (distance < 0) {
            clearInterval(x);
            document.getElementById("timer-label").innerHTML = "EXPIRED";
        }
    }, 1000);

    // populating dropdown

    getdropdownList = function populate(s1, s2, s3)
    {
        var s1 = document.getElementById(s1);
        var s2 = document.getElementById(s2);
        s2.innerHTML = "";
        if (s1.value == "LAGOS") {
            var optionArray = ["|", "l|LAGOS", "n|NORTH", "sess|SOUTHEAST&SOUTHSOUTH", "sw|SOUTHWEST"];
        }
        for (var option in optionArray) {
            var pair = optionArray[option].split("|");
            var newOption = document.createElement("option");
            newOption.value = pair[0];
            newOption.innerHTML = pair[1];
            s2.option.add(newOption);
        }
    }

    // populating 
    $(document).ready(function () {
        //Populate Region
       
            $.ajax({
                type: "Get",
                /*url: "/EnquiryRegister/EnquiryReasonSubTypeList?enquiryReasonId=" + regionId,*/
                url: "Home/getRegion?regCode",
                contentType: "html",
                success: function (response) {
                    $("#regionId").html("");                    

                    let objArray = $.parseJSON(response)
                    
                    objArray.forEach(function (arrayElem) { 

                        $("#regionId").append(new Option(arrayElem.Name, arrayElem.Id));
                    });
                    //debugger
                    
                }
            })
        

        $("#regionId").change(function () {
            var regionalId = $(this).val();
            //debugger
            $.ajax({
                type: "Get",
            /*url: "/EnquiryRegister/EnquiryReasonSubTypeList?enquiryReasonId=" + regionId,*/
                url: "Home/getZone?regCode=" + regionalId,
                contentType: "html",
                success: function (response) {
                    $("#zoneId").html("");

                    console.log(response)

                    let objArray = $.parseJSON(response)
                    console.log(objArray)
                    objArray.forEach(function (arrayElem) {


                        $("#zoneId").append(new Option(arrayElem.Name, arrayElem.Id));
                    });
                }
            })
        });
        $("#zoneId").change(function () {
            var selected = $("#zoneId option:selected").text();
            var zoneTypeId = $(this).val();
            $("#branchId").html(selected);
            //debugger
            $.ajax({
                type: "Post",
                //url: "/EnquiryRegister/ContactTypeDetail?contactTypeId=" + zoneTypeId,
                url: "/Zone/Branch?zoneId=" + zoneTypeId,
                contentType: "html",
                success: function (response) {
                    // debugger
                    $("#branchId").empty();
                    $("#branchId").append(response);
                }
            })
        });

    });

});