jQuery(document).ready(function ($) {
    $("#btn-send-review").on('click', SendFeedback);
});

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
        Notify('Error', 'Please fill in the feedback section.');
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
            $("#btn-send-review").on('click', SendFeedback);
            HideLoader();
            Notify('Success', 'Your feedback was sent!');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}
