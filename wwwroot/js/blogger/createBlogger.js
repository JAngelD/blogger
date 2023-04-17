$(document).ready(() => {
    // $(document).on("click", "#btnSearchAppointment", searchAppointments)
    $(document).on("click", "#saveBlogger", saveBlogger)
        // $(document).on("click", "#btnReschedule", rescheduleAppointment)
        // $(document).on("hide.bs.modal", "#appointmentModal", clearForm)

})

const saveBlogger = e => {
    e.preventDefault();
    if ($("#createBloggerForm").valid()) {
        var formData = new FormData($("#createBloggerForm")[0]);

        //$("body").waitMe({ text: "Getting Patients" });
        $.ajax({
            type: "POST",
            url: "/Blogger/SaveBlogger",
            data: formData,
            processData: false, // tell jQuery not to process the data
            contentType: false // tell jQuery not to set contentType
        }).done((response) => {
            if (response.success) {
                successAlert("Blogger Registered");
                clearFormBlogger();
            }
        });
    }

}
const clearFormBlogger = () => {
    $("#createBloggerForm")[0].reset();
}