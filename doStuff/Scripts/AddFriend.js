$(document).ready(function () {
    $(".add-friend").submit(function (event) {
        event.preventDefault();

        var $form = $(this);
        var url = $form.attr('action');
        var data = $form.serialize();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data: data,
            success: function (friend) {
                var li = $("<li class=\"eventfeed-friend\" id=\"friend" + friend.UserID + "\"></li>");
                li.append(friend.DisplayName + "<a href=\"/User/Index\"><button class=\"btn btn-primary remove-button\" id=\"button" + friend.UserID + "\"><i class=\"glyphicon glyphicon-remove right\"></i></button></a>");
                li.children("form").addClass("remove-friend");
                $("#FriendList").append(li);
                FriendList("#FriendList")
            },
            error: function (xhr, err) {
                alert("SOME ERROR OCCURED");
            }
        });
    })
 
    $(".remove-friend").submit(function (event) {
        event.preventDefault();

        var $form = $(this);
        var url = $form.attr('action');
        var data = $form.serialize();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data: data,
            success: function (friend) {
                $("#friend" + friend.UserID).remove();
            },
            error: function (xhr, err) {
                alert("SOME ERROR OCCURED");
            }
        });
    })

    function FriendList(id) {
        var mylist = $(id);
        var listitems = mylist.children('li').get();
        mylist.empty();
        listitems.sort(function (a, b) { return a.innerText.localeCompare(b.innerText) === 1 });
        for (var i = 0; i < listitems.length; i++) {
            mylist.append(listitems[i]);
        }
        return;
    }
});