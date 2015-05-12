$(document).ready(function () {

   /* $(".add-friend").submit(Add);

    $(".remove-friend").submit(Remove);

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
    }*/
    /*
    function SetFeedback(message)
    {
        $("#Error").addClass("hidden");
        $("#Warning").addClass("hidden");
        $("#Information").addClass("hidden");
        $("#Success").addClass("hidden");
        if(message == null)
        {
            return;
        }
        if(message.ErrorMessage != null)
        {
            $("#ErrorMessage").empty();
            $("#Error").removeClass("hidden");
            $("#ErrorMessage").text(message.ErrorMessage)
        }
        if (message.WarningMessage != null)
        {
            $("#WarningMessage").empty();
            $("#Warning").removeClass("hidden");
            $("#WarningMessage").text(message.WarningMessage)
        }
        if (message.InformationMessage != null)
        {
            $("#InformationMessage").empty();
            $("#Information").removeClass("hidden");
            $("#InformationMessage").text(message.InformationMessage)
        }
        if (message.SuccessMessage != null)
        {
            $("#SuccessMessage").empty();
            $("#Success").removeClass("hidden");
            $("#SuccessMessage").text(message.SuccessMessage)
        }
    }
    */
    function Add(event) {
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

    function Remove(event) {
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
});