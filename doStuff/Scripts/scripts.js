$(function () {

    $("#datepicker").datetimepicker({ dateFormat: 'dd/mm/yy' });

    countdown();

});

function countdown() {
    function checkTime() {
        var elements = $(".countdown");
        for (var i = 0; i < elements.length; i++) {
            debugger;
            created = $(elements[i]).data("created");
            var numberOfMinutes = $(elements[i]).data("minutes");
            if (typeof created === "string") {
                created = parseInt(created.split(",")[0]);
            }
            var initialTime = new Date(created);
            var timeDifference = (numberOfMinutes * 60000) - (Date.now() - initialTime);
            if (timeDifference < 0) {
                //location.reload();
            } else {
                var formatted = convertTime(timeDifference);
                $(elements[i]).text('' + formatted);
            }
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



