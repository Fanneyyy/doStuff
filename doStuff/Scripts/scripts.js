$(function () {

    $("#datepicker").datetimepicker({ dateFormat: 'dd/mm/yy' });

    countdown();
});

function countdown() {
    function checkTime() {
        var element = document.getElementById('time');
        created = $(element).data("created");
        if (typeof created === "string") {
            created = parseInt(created.split(",")[0]);
        }
        var initialTime = new Date(created);
        var timeDifference = 1380000 - (Date.now() - initialTime);
        var formatted = convertTime(timeDifference);
        element.innerHTML = '' + formatted;
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



