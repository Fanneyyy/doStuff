$(function () {

    $("#datepicker").datetimepicker({ dateFormat: 'dd/mm/yy' });

    countdown();

});

function countdown() {
    //function checkTime() {
    //    debugger;
    //    var elements = $(".countdown");
    //    for (var item in elements) {
    //        debugger;
    //        created = $(elements[item]).data("created");
    //        if (typeof created === "string") {
    //            created = parseInt(created.split(",")[0]);
    //        }
    //        var initialTime = new Date(created);
    //        var timeDifference = 1380000 - (Date.now() - initialTime);
    //        if (timeDifference < 0) {
    //            $(elements[item]).innerHTML = "ITS ON!";
    //        } else {
    //            var formatted = convertTime(timeDifference);
    //            $(elements[item]).innerHTML = '' + formatted;
    //        }
    //    }
    //}

    function checkTime() {
        var element = document.getElementById('time');
        created = $(element).data("created");
        if (typeof created === "string") {
            created = parseInt(created.split(",")[0]);
        }
        var initialTime = new Date(created);
        var timeDifference = 1380000 - (Date.now() - initialTime);
        if (timeDifference < 0) {
            element.innerHTML = "ITS ON!";
        } else {
            var formatted = convertTime(timeDifference);
            element.innerHTML = '' + formatted;
        }
    }

    function convertTime(miliseconds) {
        var totalSeconds = Math.floor(miliseconds / 1000);
        var minutes = leftPad(Math.floor(totalSeconds / 60), 2);
        var seconds = leftPad(totalSeconds - minutes * 60, 2);
        return minutes + ':' + seconds;
    }

    function leftPad(aNumber, aLength) {
        if (aNumber.toString().length >= aLength) {
            return aNumber;
        }
        return (Math.pow(10, aLength) + Math.floor(aNumber)).toString().substring(1);
    }

    window.setInterval(checkTime, 1000);
}



