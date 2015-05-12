$(document).ready(function () {

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

    $(".add-friend").submit(AddFriend);

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
                    $(".remove-friend").submit(Remove);
                }
            },
            error: function (xhr, err) {
                alert("SOME ERROR OCCURED");
            }
        });
    }

    $(".remove-friend").submit(RemoveFriend);

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
            },
            error: function (xhr, err) {
                alert("SOME ERROR OCCURED");
            }
        });
    }

    $(".remove-event").submit(RemoveEvent);

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
                }
            },
            error: function (xhr, err) {
                alert(xhr);
                alert(err);
            }
        });
    }

    $(".join-event").click(function (event) {
        var form = $(this).closest("form");
        AnswerEvent(true, form, event);
    });
    $(".decline-event").click(function (event) {
        var form = $(this).closest("form");
        AnswerEvent(false, form, event);
    });

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
            },
            error: function (xhr, err) {
                alert(xhr);
                alert(err);
            }
        });
    }
});