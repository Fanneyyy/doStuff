$(function () {

    $("#datepicker").datetimepicker({ dateFormat: 'dd/mm/yy' });

    countdown();
});

function countdown() {
    var countdownElements = $(".countdown");
    var created;
    for (var i = 0; i < countdownElements.length; i++) {
        created = $(countdownElements[i]).data("created");
        var date = new Date(created);
        var dateNow = new Date();
        var diff = Math.abs(dateNow - date);
        var minutes = Math.floor(diff / 1000 / 60);
        $(countdownElements[i]).text(minutes);
    }
}