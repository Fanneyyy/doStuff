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
        var minutes = Math.floor(totalSeconds / 60);
        var seconds = totalSeconds - minutes * 60;
        return minutes + ':' + seconds;
    }
    window.setInterval(checkTime, 1000);
}

