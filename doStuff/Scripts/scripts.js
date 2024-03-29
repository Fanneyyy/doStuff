$(document).ready(function () {

    // The datepicker
    $("#datepicker").datetimepicker({ dateFormat: 'dd/mm/yy' });

    // Countdown for events
    countdown();

    // To stop the dropdown from closing when clicking the text input box
    $(".banner-dropdown").click(function (e) {
        e.stopPropagation();
    });

    InitializeSelectors();
});

function InitializeSelectors() {
    $(".add-friend").submit(AddFriend);

    $(".remove-friend").submit(RemoveFriend);

    $(".add-member").submit(AddMember);

    $(".remove-member").submit(RemoveMember);

    $(".remove-event").submit(RemoveEvent);

    $(".event-comment-form").submit(Comment);
    $(".group-comment-form").submit(CommentGroup);

    $(".join-event").click(function (event) {
        var form = $(this).closest("form");
        AnswerEvent(true, form, event);
    });
    $(".decline-event").click(function (event) {
        var form = $(this).closest("form");
        AnswerEvent(false, form, event);
    });

    $(".comment-title").click(function () {
        toggleComment(this);
    });
}

var lastcomment;
var iscommenting = false;

function Comment(event) {
    event.preventDefault();

    if (iscommenting) {
        return;
    }
    iscommenting = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            if (data.id != null) {
                lastcomment = "#comment-title" + data.id;
            }
            UpdateFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            iscommenting = false;
        }
    });
}

var iscommentinggroup = false;

function CommentGroup(event) {
    event.preventDefault();

    if (iscommentinggroup) {
        return;
    }
    iscommentinggroup = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            if (data.id != null) {
                lastcomment = "#comment-title" + data.id;
            }
            UpdateGroupFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            iscommentinggroup = false;
        }
    });
}

var isaddingfriend = false;

function AddFriend(event) {
    event.preventDefault();

    if (isaddingfriend) {
        return;
    }
    isaddingfriend = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateFriendList();
            UpdateFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            isaddingfriend = false;
        }
    });
}

var isremovingfriend = false;

function RemoveFriend(event) {
    event.preventDefault();

    if (isremovingfriend) {
        return;
    }
    isremovingfriend = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            if (data.friend != null) {
                $("#friend" + data.friend.UserID).remove();
            }
            SetFeedback(data.message);
            UpdateFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            isremovingfriend = false;
        }
    });
}

var isaddingmember = false;

function AddMember(event) {
    event.preventDefault();

    if (isaddingmember) {
        return;
    }
    isaddingmember = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateMemberList();
            UpdateGroupFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            isaddingmember = false;
        }
    });
}

var isremovingmember = false;

function RemoveMember(event) {
    event.preventDefault();

    if (isremovingmember) {
        return;
    }
    isremovingmember = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            if (data.member != null) {
                $("#member" + data.member.UserID).remove();
            }
            SetFeedback(data.message);
            UpdateGroupFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            isremovingmember = false;
        }
    });
}

var isremovingevent = false;

function RemoveEvent(event) {
    event.preventDefault();

    if (isremovingevent) {
        return;
    }
    isremovingevent = true;

    var $form = $(this);
    var url = $form.attr('action');
    var data = $form.serialize();

    $form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            if (data.id != null) {
                $("#event-box" + data.id).remove();
            }
            SetFeedback(data.message);
            UpdateFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            isremovingevent = false;
        }
    });
}

var isansweringevent = false;

function AnswerEvent(answer, form, event) {
    event.preventDefault();

    if (isansweringevent) {
        return;
    }
    isansweringevent = true;

    var url = form.attr('action');
    var data = form.serialize() + "&answer=" + answer;

    form.addClass("hidden");

    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        data: data,
        success: function (data) {
            SetFeedback(data.message);
            UpdateFeed();
        },
        error: function () {
            $form.removeClass("hidden");
        },
        complete: function () {
            isansweringevent = false;
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
            if (lastcomment != null) {
                toggleComment(lastcomment);
            }
            InitializeSelectors();
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
            if (lastcomment != null) {
                toggleComment(lastcomment);
            }
            InitializeSelectors();
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
            InitializeSelectors();
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
            InitializeSelectors();
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

function toggleComment(element) {
    $(element).siblings(".all-comments").toggle();
    $(element).children(".comment-plus").toggle();
    $(element).children(".comment-minus").toggle();
}