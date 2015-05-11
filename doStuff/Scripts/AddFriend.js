$(document).ready(function () {
    $("#AddFriendButton").click(function (event) {
        event.preventDefault();

        var $form = $("#AddFriend");
        var url = $form.attr('action');
        var data = $form.serialize();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data: data,
            success: function (friend) {
                var li = $("<li class=\"eventfeed-friend\"></li>");
                li.append(friend.DisplayName + "<form action=\"/User/RemoveFriend\" method=\"post\"><input name=\"friendId\" type=\"hidden\" value=\"" + friend.UserID + "\"><button type=\"submit\" class=\"btn btn-primary remove-button\"><i class=\"glyphicon glyphicon-remove right\"></i></button></form>");
                $("#FriendList").append(li);
                FriendList("#FriendList")
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
        for(var i = 0; i < listitems.length; i++)
        {
            mylist.append(listitems[i]);
        }
    }
});