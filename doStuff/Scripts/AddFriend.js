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
                    $(".remove-friend").submit(RemoveFriend);
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

    /*var currentdate = new Date();
    var lastrequest = currentdate.getDate() + "-"
                    + (currentdate.getMonth() + 1) + "-"
                    + currentdate.getFullYear() + " "
                    + currentdate.getHours() + ":"
                    + currentdate.getMinutes() + ":"
                    + currentdate.getSeconds();

    function UpdateFeed() {
        var url = '/User/GetEvents';
        var data = { time: lastrequest };

        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            data: data,
            success: function (list) {
                $("#event-feed").empty();
                for(var i = 0; i < list.Events.length; i++)
                {
                    DisplayEvent(list.Events[i]);
                }
            },
            error: function (xhr, err) {
                alert(xhr);
                alert(err);
            },
            complete: function (list) {
                setInterval(UpdateFeed, 10000);
            }
        });
    }

    function DisplayEvent(item)
    {
        var li = $("<li></li>");
        var eventbox = $('<div class="row event-box" id=event' + item.Event.EventID + '></div>');
        var banner = CreateBanner(item);
        var content = CreateContent(item);
        var comments = CreateComments(item);
        var commentform = CreateCommentForm(item);

        eventbox.append(banner);
        eventbox.append(content);
        eventbox.append(comments);
        eventbox.append(commentform);

        li.append(eventbox);

        $("#event-feed").append(li);
    }

    function CreateBanner(item)
    {
        var banner = $('<div class="col-md-12 event-banner"></div>');
        var row = $('<div class="row"></div>');
        row.append('<div class="col-md-4 event-owner">' + item.Owner + '</div>');
        row.append('<div class="col-md-3 event-group"></div>');
        row.append('<div class="col-md-2 event-countdown">' + "ITS ON" + '</div>');
        row.append('<div class="col-md-3 event-name">' + item.Event.Name + '</div>');
        banner.append(row);
        return banner;
    }

    function CreateContent(item)
    {
        Parent = $('<div class="col-md-12 event-middle"></div>');
        var Content = $('<div class="row"></div>');

        var InformationParent = $('<div class="col-md-6 event-photo-time-location"></div>');
        var Information = $('<div class="row"></div>');
        var Attendees = $('<div class="event-photo-space-attendees"></div>');
        var TimeAndPlace = $('<div class="row event-time-place"></div>');
        var Time = $('<div class="col-md-6 event-time">' + item.Event.TimeOfEvent + '</div>');
        var Location = $('<div class="col-md-6 event-location">' + item.Event.Location + '</div>');

        var Description = $('<div class="col-md-6 event-description-buttons"><div class="row"><div class="col-md-12 event-description">fdsgsdfg</div></div><div class="hidden" id="form-result23"><div class="col-md-12 event-joined">You Have Joined!</div><div class="col-md-12 event-declined">You Have Declined!</div></div><div class="row event-join-decline"><div class="col-md-12 event-joined">You Have Joined!</div></div></div>');

        Parent.append(Content);
        Content.append(InformationParent);
        InformationParent.append(Information);
        Information.append(Attendees);
        Information.append(TimeAndPlace);
        TimeAndPlace.append(Time);
        TimeAndPlace.append(Location);
        Content.append(Description);

        return Parent;
    }

    function CreateComments(item)
    {
        var Comments = $('<div class="comment-title">comments</div>');
        return Comments;
    }

    function CreateCommentForm(item)
    {
        var CommentForm = $('<form action="/User/Comment" method="post"></form>');
        return CommentForm;
    }

    UpdateFeed();*/
});