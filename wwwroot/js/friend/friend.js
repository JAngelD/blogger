$(document).ready(() => {
    $(document).on("click", "#btnAdd", showFriendModal)
    $(document).on("click", "#btnAdd", getFriends)
    $(document).on("click", "#btnDetails", getFriends2)
    $(document).on("click", "#btnSaveFriend", saveFriend)
    $(document).on("click", "#btnRemove", deleteFriend)
})
var tableFriend = undefined;
const showFriendModal = e => {
    var id = $(e.currentTarget).data("id");
    $("#FriendModal").modal("show");
    if (id == undefined) {
        $("#FriendModal").modal("show");
    } else {
        //$("body").waitMe({text: "Getting Patient Data"});
        $("#FriendModal").modal("show");
        $.ajax({ url: `/Blogger/GetBlogger/${id}` }).done((blogger) => {
            console.log(blogger);
            $(`#FriendId option[value='${id}']`).remove();
            $("#FriendModal #BloggerId").val(id);
            $("#FriendModal #Name").val(blogger.name);
        });
    }
}

const saveFriend = e => {
    e.preventDefault();
    if ($("#createFriendForm").valid()) {
        var formData = new FormData($("#createFriendForm")[0]);

        //$("body").waitMe({ text: "Getting Patients" });
        $.ajax({
            type: "POST",
            url: "/Friend/SaveFriend",
            data: formData,
            processData: false, // tell jQuery not to process the data
            contentType: false // tell jQuery not to set contentType
        }).done((response) => {
            if (response.success) {
                successAlert("Friend Registered");
                $("#FriendModal").modal("hide");
                //clearForm();
            }
        });
    }
}

const getFriends = e => {
    var id = $(e.currentTarget).data("id");
    $.ajax({ url: `/Friend/GetFriends/${id}` }).done((blogger) => {
        blogger.forEach(function(blog) {
            //console.log($(`#FriendId option[value='${blog.friendId}']`).val());
            if ($(`#FriendId option[value='${blog.friendId}']`).val() == blog.friendId) {
                $(`#FriendId option[value='${blog.friendId}']`).remove();
            }
        });
    });
}

const getFriends2 = e => {
    var id = $(e.currentTarget).data("id");
    var formData = new FormData();
    $.ajax({
        type: "GET",
        url: `/Friend/GetFriends/${id}`,
        data: formData,
        processData: false, // tell jQuery not to process the data
        contentType: false // tell jQuery not to set contentType
    }).done((friends) => {
        if (tableFriend != undefined) {
            $("#friendsTable").empty();
            tableFriend.destroy();
        }
        tableFriend = $("#friendsTable").DataTable({
            data: friends,
            bFilter: false,
            lengthChange: false,
            columns: [{
                    data: 'friendName',
                    title: "Name",
                    className: "text-center"
                },
                { data: 'friendEmail', title: "Email", className: "text-center" },
                { data: 'friendWebsite', title: "Website", className: "text-center" },
                {
                    data: 'id',
                    title: "Action",
                    width: "100px",
                    render: (data, type, row) => {
                        if (type == "display") {
                            if (row.id) {
                                return `<div class="d-flex justify-content-between"><button class="btn btn-sm btn-outline-danger d-flex justify-content-center align-items-center" id="btnRemove" data-id=${row.id}>
                                <span class="material-symbols-outlined">Delete</span> &nbspRemove</button>`
                            } else {
                                return ``
                            }
                        }
                        return data;
                    },
                    className: "text-center"
                }
            ]
        })
    });
}
const deleteFriend = e => {
    e.preventDefault();
    var id = $(e.currentTarget).data("id");
    $.confirm({
        title: 'Need Confirmation',
        content: 'Are you sure to delete friend?',
        theme: 'material',
        type: 'red',
        buttons: {
            confirm: {
                text: 'YES',
                btnClass: 'btn-danger',
                action: () => {
                    $.ajax({
                        url: `/Friend/DeleteFriend/${id}`,
                    }).done((response) => {
                        successAlert("Friend Deleted");
                        $("#detailBloggerModal").modal("hide");
                    });
                }
            },
            cancel: {
                text: 'NO',
                btnClass: 'btn-success'
            },
        }
    });
}