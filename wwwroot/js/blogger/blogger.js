$(document).ready(() => {
    $(document).on("click", "#btnDetails", showBloggerModal)
    $(document).on("click", "#btnSearchBlogger", searchBlogger)
    $(document).on("click", "#btnUpdateBlogger", updateBlogger)
    $(document).on("click", "#btnDeleteBlogger", deleteBlogger)
        // $(document).on("click", "#btnReschedule", rescheduleAppointment)
        // $(document).on("hide.bs.modal", "#appointmentModal", clearForm)

    loadBloggers();

})
var table = undefined;

const clearForm = () => {
    $("#detailBloggerModal")[0].reset();
}

const loadBloggers = () => {
    var formData = new FormData();
    //$("body").waitMe({ text: "Getting Bloggers" });
    $.ajax({
        url: "/Blogger/GetBloggers",
        data: formData,
        processData: false, // tell jQuery not to process the data
        contentType: false // tell jQuery not to set contentType
    }).done((bloggerData) => {
        if (table != undefined) {
            $("#bloggersTable").empty();
            table.destroy();
        }
        table = $("#bloggersTable").DataTable({
            data: bloggerData,
            bFilter: false,
            lengthChange: false,
            columns: [{
                    data: 'picture',
                    title: "Picture",
                    className: "text-center",
                    render: (data, type, row) => {
                        if (type == "display") {
                            if (row.picture != null) {
                                return `<img src="/img/${row.picture}" width="100px" height="100px"/>`;
                            } else {
                                return ``
                            }
                        }
                        return data;
                    },
                },
                {
                    data: 'name',
                    title: "Name",
                    className: "text-center"
                },
                {
                    data: 'website',
                    title: "Website",
                    render: (data, type, row) => {
                        if (type == "display") {
                            if (row.website) {
                                return `<a href=https://${row.website}>${row.website}</a>`
                            } else {
                                return ``
                            }
                        }
                        return data;
                    },
                    className: "text-center"
                }, {
                    data: 'id',
                    title: "Action",
                    width: "100px",
                    render: (data, type, row) => {
                        if (type == "display") {
                            if (row.id) {
                                return `<div class="d-flex justify-content-between"><button class="btn btn-sm btn-outline-primary d-flex justify-content-center align-items-center" id="btnDetails" data-id=${row.id}>
                                <span class="material-symbols-outlined">Visibility</span> &nbspDetails</button>
                                <button class="btn btn-sm btn-outline-success d-flex justify-content-center align-items-center" id="btnAdd" data-id=${row.id}>
                                <span class="material-symbols-outlined">group_add</span> &nbspAdd friend</button></div>`
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
    })
}
const searchBlogger = e => {
    e.preventDefault();
    var formData = new FormData($("#searchForm")[0]);
    $.ajax({
        type: "POST",
        url: "/Blogger/SearchBlogger",
        data: formData,
        processData: false, // tell jQuery not to process the data
        contentType: false // tell jQuery not to set contentType
    }).done((appointments) => {
        if (table != undefined) {
            $("#bloggersTable").empty();
            table.destroy();
        }
        table = $("#bloggersTable").DataTable({
            data: appointments,
            bFilter: false,
            lengthChange: false,
            columns: [{
                    data: 'picture',
                    title: "Picture",
                    className: "text-center",
                    render: (data, type, row) => {
                        if (type == "display") {
                            if (row.picture != null) {
                                return `<img src="/img/${row.picture}" width="100px" height="100px"/>`;
                            } else {
                                return ``
                            }
                        }
                        return data;
                    },
                },
                {
                    data: 'name',
                    title: "Name",
                    className: "text-center"
                },
                { data: 'website', title: "Website", className: "text-center" },
                {
                    data: 'id',
                    title: "Action",
                    width: "100px",
                    render: (data, type, row) => {
                        if (type == "display") {
                            if (row.id) {
                                return `<div class="d-flex justify-content-between"><button class="btn btn-sm btn-outline-primary d-flex justify-content-center align-items-center" id="btnDetails" data-id=${row.id}>
                            <span class="material-symbols-outlined">Visibility</span> &nbspDetails</button>
                            <button class="btn btn-sm btn-outline-success d-flex justify-content-center align-items-center" id="btnAdd" data-id=${row.id}>
                            <span class="material-symbols-outlined">group_add</span> &nbspAdd friend</button></div>`
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

/**
 * It shows a modal with a form to create or edit a blogger.
 */
const showBloggerModal = e => {
    var id = $(e.currentTarget).data("id");
    if (id == undefined) {
        $("#detailBloggerModal").modal("show");
    } else {
        //$("body").waitMe({text: "Getting Patient Data"});
        $("#detailBloggerModal").modal("show");
        $.ajax({ url: `/Blogger/GetBlogger/${id}` }).done((blogger) => {
            if (blogger.picture != null) {
                $("#imgDiv").append(`<img src="/img/${blogger.picture}" width="100px" height="100px"/>`);
            } else {
                $("#imgDiv").empty();
            }
            $("#bloggerDetailsForm #Id").val(blogger.id);
            $("#bloggerDetailsForm #Name").val(blogger.name);
            $("#bloggerDetailsForm #Email").val(blogger.email);
            $("#bloggerDetailsForm #Website").val(blogger.website);
            $("#bloggerDetailsForm #Picture").val(blogger.picture);
        });
    }
}

const updateBlogger = e => {
    e.preventDefault();
    if ($("#bloggerDetailsForm").valid()) {
        var formData = new FormData($("#bloggerDetailsForm")[0]);

        //$("body").waitMe({ text: "Getting Patients" });
        $.ajax({
            type: "POST",
            url: "/Blogger/SaveBlogger",
            data: formData,
            processData: false, // tell jQuery not to process the data
            contentType: false // tell jQuery not to set contentType
        }).done((response) => {
            if (response.success) {
                $("#detailBloggerModal").modal("hide");
                successAlert("Blogger Updated");
                loadBloggers();
            }
        });
    }

}
const deleteBlogger = e => {
    e.preventDefault();
    var id = $("#bloggerDetailsForm #Id").val();
    $.confirm({
        title: 'Need Confirmation',
        content: 'Are you sure to delete blogger?',
        theme: 'material',
        type: 'red',
        buttons: {
            confirm: {
                text: 'YES',
                btnClass: 'btn-danger',
                action: () => {
                    $.ajax({
                        url: `/Friend/SearchFriends/${id}`,
                    }).done((response) => {
                        console.log(response);
                        if (response.length != 0) {
                            response.forEach(function(friend) {
                                $.ajax({
                                    url: `/Friend/DeleteFriend/${friend.id}`,
                                })
                            });
                            $.ajax({
                                url: `/Blogger/DeleteBlogger/${id}`,
                            }).done((response) => {
                                successAlert("Blogger Deleted");
                                $("#detailBloggerModal").modal("hide");
                                loadBloggers();
                            });
                        } else {
                            $.ajax({
                                url: `/Blogger/DeleteBlogger/${id}`,
                            }).done((response) => {
                                successAlert("Blogger Deleted");
                                $("#detailBloggerModal").modal("hide");
                                loadBloggers();
                            });
                        }
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