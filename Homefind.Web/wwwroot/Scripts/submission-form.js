$(document).ready(function () {
    document.getElementById('images').addEventListener('change', readImage, false);

    $(".image-container").sortable();

    ShowNotification();
});

function ShowNotification() {
    var input = $("#notification-message").val();
    switch (input) {
        case 'Error':
            Notify(input, 'An error occured while saving your listing.');
            break;
        case 'Success':
            Notify(input, 'Your listing was successfully saved.');
            break;
        default:
            return;
    }
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