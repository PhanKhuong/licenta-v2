jQuery(document).ready(function ($) {
    AddSendFeedbackEventHandler();
});

function AddSendFeedbackEventHandler() {
    $("#btn-send-review").click(function () {
        SendFeedback();
    });
}

function GetFeedbackModel() {
    var rating = $("input[name='group1']:checked").val();
    var comment = $("#form79textarea").val();
    var propertyOwner = $("#propertyOwner").val();

    return review = {
        RatedUserId: propertyOwner,
        Rating: rating,
        Comment: comment
    };
}

function SendFeedback() {
    var model = GetFeedbackModel();
    if (!model.Comment) {
        ShowReviewAlert(GetErrorAlert());
        return;
    }

    ShowLoader();
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        type: "POST",
        url: "/Profile/Feedback",
        data: JSON.stringify(model),
        dataType: "html",
        success: function (data) {
            $("#review-component").empty();
            $("#review-component").append(data);
            $('#modalPoll-1').modal('show');
            AddSendFeedbackEventHandler();
            HideLoader();
            ShowReviewAlert(GetSuccessAlert());
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function GetSuccessAlert() {
    return '<div class="alert alert-success">' +
        '<button type="button" class="close" data-dismiss="alert">' +
        '&times;</button>Your feedback was sent!</div>';
}

function GetErrorAlert() {
    return '<div class="alert alert-danger">' +
        '<button type="button" class="close" data-dismiss="alert">' +
        '&times;</button>Please fill in the feedback section.</div>';
}

function ShowReviewAlert(alert) {
    $("#review-alert").animate({
        height: '+=72px'
    }, 300);

    $(alert).hide().appendTo('#review-alert').fadeIn(1000);

    $(".alert").delay(3000).fadeOut(
        "normal",
        function () {
            $(this).remove();
        });

    $("#review-alert").delay(4000).animate({
        height: '-=72px'
    }, 300);
}
