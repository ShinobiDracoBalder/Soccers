function validateData() {

    if ($("#Username").val() == "") {
        swal("Please enter Username !");
        return false;
    } else if ($("#Document").val() == "") {
        swal("Please enter Document !");
        return false;
    } else if ($("#FirstName").val() == "") {
        swal("Please enter FirstName !");
        return false;
    } else if ($("#FirstName").val() == "") {
        swal("Please enter FirstName !");
        return false;
    } else if ($("#LastName").val() == "") {
        swal("Please enter LastName !");
        return false;
    } else if ($("#Address").val() == "") {
        swal("Please enter Address !");
        return false;
    } else if ($("#PhoneNumber").val() == "") {
        swal("Please enter PhoneNumber !");
        return false;
    } else if ($("#Password").val() == "") {
        swal("Please enter Password !");
        return false;
    } else if ($("#PasswordConfirm").val() == "") {
        swal("Please enter Password Confirm !");
        return false;
    }
    else {
        return true;
    }
}

function Validate(ctl, event) {
    event.preventDefault();
    swal({
        title: "Do you want to save it?",
        text: "Please check Information before Submiting!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Save",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                if (validateData() == true) {
                    $("#CreateForm").submit();
                }
            } else {
                swal("Cancelled", "You have Cancelled Form Submission!", "error");
            }
        });
}
function Itemsdelete(ID) {
    debugger;
    swal({
        title: "Are you sure?",
        text: "Are you sure that you want to delete this Items?",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-outline-danger",
        confirmButtonText: "Delete",
        confirmButtonColor: "#ec6c62"
    },
        function () {
            $.ajax({
                url: "/DeleteConfirmation/delete/",
                data: { "OrderID": OrderID },
                type: "DELETE"
            })
                .done(function (data) {
                    sweetAlert
                        ({
                            title: "Deleted!",
                            text: "Your file was successfully deleted!",
                            type: "success"
                        },
                            function () {
                                window.location.href = '/DeleteConfirmation/Details';
                            });
                })
                .error(function (data) {
                    swal("Oops", "We couldn't connect to the server!", "error");
                });
        });
}