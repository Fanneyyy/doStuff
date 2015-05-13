$(document).ready(function () {

    // The datepicker
    $("#datepicker").datetimepicker({ dateFormat: 'dd/mm/yy' });

    // Countdown for events
    countdown();

    // To stop the dropdown from closing when clicking the text input box
    $(".banner-dropdown").click(function (e) {
        e.stopPropagation();
    });

    $(".add-friend").submit(AddFriend);

    $(".remove-friend").submit(RemoveFriend);

    $(".add-member").submit(AddMember);

    $(".remove-member").submit(RemoveMember);

    $(".remove-event").submit(RemoveEvent);

    $(".join-event").click(function (event) {
        var form = $(this).closest("form");
        AnswerEvent(true, form, event);
    });
    $(".decline-event").click(function (event) {
        var form = $(this).closest("form");
        AnswerEvent(false, form, event);
    });
});

function AddFriend(event) {
    event.preventDefault();

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateFriendList();
        },
        error: function (xhr, err) {

        }
    });
}

function RemoveFriend(event) {
    event.preventDefault();

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            $("#friend" + data.member.UserID).remove();
            SetFeedback(data.message);
            UpdateFriendList();
        },
        error: function (xhr, err) {
        }
    });
}

function AddMember(event) {
    event.preventDefault();

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateMemberList();
        },
        error: function (xhr, err) {

        }
    });
}

function RemoveMember(event) {
    event.preventDefault();

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            $("#member" + data.member.UserID).remove();
            SetFeedback(data.message);
            UpdateMemberList();
        },
        error: function (xhr, err) {
        }
    });
}

function SetFeedback(message) {
    $("#Error").addClass("hidden");
    $("#Warning").addClass("hidden");
    $("#Information").addClass("hidden");
    $("#Success").addClass("hidden");
    if (message == null) {
        return;
    }
    if (message.ErrorMessage != null) {
        $("#ErrorMessage").empty();
        $("#Error").removeClass("hidden");
        $("#ErrorMessage").text(message.ErrorMessage)
    }
    if (message.WarningMessage != null) {
        $("#WarningMessage").empty();
        $("#Warning").removeClass("hidden");
        $("#WarningMessage").text(message.WarningMessage)
    }
    if (message.InformationMessage != null) {
        $("#InformationMessage").empty();
        $("#Information").removeClass("hidden");
        $("#InformationMessage").text(message.InformationMessage)
    }
    if (message.SuccessMessage != null) {
        $("#SuccessMessage").empty();
        $("#Success").removeClass("hidden");
        $("#SuccessMessage").text(message.SuccessMessage)
    }
}

function RemoveEvent(event) {
    event.preventDefault();

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateFeed();
        },
        error: function (xhr, err) {
        }
    });
}

function AnswerEvent(answer, form, event) {
    event.preventDefault();

    var url = form.attr('action');
    var data = form.serialize() + "&answer=" + answer;

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateFeed();
        },
        error: function (xhr, err) {
        }
    });
}

function UpdateFeed() {
    var url = '/User/Index';
    var data = "";

    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        data: data,
        success: function (list) {
            $("#event-feed").empty();
            $("#event-feed").append(list);
        }
    });
}

function UpdateGroupFeed() {
    var url = '/Group/Index';
    var data = "groupId=" + $("#member-list").data("groupid");

    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        data: data,
        success: function (list) {
            $("#event-feed").empty();
            $("#event-feed").append(list);
        }
    });
}

function UpdateFriendList() {
    var url = '/User/GetSideBar';
    var data = "";

    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        data: data,
        success: function (list) {
            $("#friend-list").empty();
            $("#friend-list").append(list);
        }
    });
}

function UpdateMemberList() {
    var url = '/Group/GetSideBar';
    var data = "groupId=" + $("#member-list").data("groupid");

    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        data: data,
        success: function (list) {
            $("#member-list").empty();
            $("#member-list").append(list);
        }
    });
}

function countdown() {
    function checkTime() {
        var elements = $(".countdown");
        for (var i = 0; i < elements.length; i++) {
            created = $(elements[i]).data("created");
            var numberOfMinutes = $(elements[i]).data("minutes");
            if (typeof created === "string") {
                created = parseInt(created.split(",")[0]);
            }
            var initialTime = new Date(created);
            var timeDifference = (numberOfMinutes * 60000) - (Date.now() - initialTime);
            if (timeDifference <= 0) {
                UpdateFeed();
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
