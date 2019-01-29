$(document).ready(function () {
    document.getElementById('images').addEventListener('change', readImage, false);

    $(".image-container").sortable();

    Notify();
});

function Notify() {
    var input = $("#notification-message").val();
    if (input === "Success") {
        $("#notification-wrapper").html(NotifySuccess());
    }
    else if (input === "Error") {
        $("#notification-wrapper").html(NotifyError());
    }
    else {
        $("#notification-wrapper").empty();
    }

    $("#notification-wrapper").fadeIn().delay(5000).fadeOut();
}

function NotifySuccess() {
    return "<div class='alert alert-success alert-dismissible fade show' role='alert'>" +
        "<strong>Thank you!</strong> Your listing was successfully saved." +
        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
        "<span aria-hidden='true'>&times;</span>" +
        "</button></div>";
}

function NotifyError() {
    return "<div class='alert alert-danger alert-dismissible fade show' role='alert'>" +
        "<strong>Oops!</strong> An error occured while saving your listing." +
        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
        "<span aria-hidden='true'>&times;</span>" +
        "</button></div>";
}

var num = 1;
function readImage() {
    $(".image-container").empty();
    if (window.File && window.FileList && window.FileReader) {
        var files = event.target.files;
        var output = $(".image-container");

        for (let i = 0; i < files.length; i++) {
            var file = files[i];
            if (!file.type.match('image')) continue;

            var picReader = new FileReader();

            picReader.addEventListener('load', function (event) {
                var picFile = event.target;
                var html = '<div class="preview-image preview-show-' + num + '">' +
                    '<div class="image-zone"><img id="pro-img-' + num + '" src="' + picFile.result + '"></div>' +
                    '</div>';

                output.append(html);
                num = num + 1;
            });

            picReader.readAsDataURL(file);
        }
    } else {
        console.log('Browser support issue.');
    }
}