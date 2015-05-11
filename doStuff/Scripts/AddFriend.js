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
                var li = $("<li></li>").append(friend.DisplayName);
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
        $("#FriendList").empty();
        listitems.sort(function (a, b) { return a.innerText <= b.innerText });
        for(var i = 0; i < listitems.length; i++)
        {
            $("#FriendList").append(listitems[i]);
        }
    }
});