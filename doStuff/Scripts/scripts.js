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

function FriendList(id) {
    var mylist = $(id);
    var listitems = mylist.children('li').get();
    mylist.empty();
    listitems.sort(function (a, b) {
        var str1 = a.innerText.toLowerCase(), str2 = b.innerText.toLowerCase();
        return str1 == str2 ? 0 : str1 < str2 ? -1 : 1;
    });
    for (var i = 0; i < listitems.length; i++) {
        mylist.append(listitems[i]);
    }
    return;
}

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
            if (data.friend != null) {
                var selector = "#friend" + data.friend.UserID;
                $(selector).remove();
                $form.find("input[type=text]").val("");
                var li = $("<li class=\"eventfeed-friend\" id=\"friend" + data.friend.UserID + "\"></li>");
                li.append(data.friend.DisplayName + "<form action=\"/User/RemoveFriend\" class=\"remove-friend\" method=\"post\"><input name=\"friendId\" type=\"hidden\" value=" + data.friend.UserID + "><button type=\"submit\" class=\"btn btn-primary remove-button\"><i class=\"glyphicon glyphicon-remove right\"></i></button></form>");
                $("#FriendList").append(li);
                FriendList("#FriendList")
                $(".remove-friend").submit(RemoveFriend);
            }
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
            $("#friend" + data.friend.UserID).remove();
            SetFeedback(data.message);
            UpdateFriendList();
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
            if (data.theevent != null) {
                $("#event" + data.theevent.EventID).remove();
                UpdateFeed();
            }
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
            if (data.theevent != null) {
                form.addClass("hidden");
                var selector = "#form-result" + data.theevent.EventID;
                $(selector).removeClass("hidden");
                if (answer === true) {
                    $(selector).children(".event-declined").addClass("hidden");
                }
                else {
                    $(selector).children(".event-joined").addClass("hidden");
                }
            }
            UpdateFeed();
        },
        error: function (xhr, err) {
        }
    });
}

function UpdateFeed() {
    var url = '/User/GetEvents';
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
