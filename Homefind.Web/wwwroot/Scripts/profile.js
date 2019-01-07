$(document).ready(function () {
    var ATTRIBUTES = ['myvalue'];

    $('[data-toggle="modal"]').on('click', function (e) {
        var $target = $(e.target);
        // modal targeted by the button
        var modalSelector = $target.data('target');

        // iterate over each possible data-* attribute
        ATTRIBUTES.forEach(function (attributeName) {
            var $modalAttribute = $(modalSelector + ' #modal-' + attributeName);
            var dataValue = $target.data(attributeName);
            $modalAttribute.val(dataValue || '');
        });
    });

    $("#favourite-properties").click(function () {
        $('#fav-list-container').load("/Profile/Favourites");
    });

    $('#addFavBtn').click(function () {
        $("#addFavBtn").addClass("is-disabled");
        var action = "Add";
        if ($("#isFavourite").val() === "True") {
            action = "Remove";
            $("#isFavourite").val("False");
        }
        else {
            $("#isFavourite").val("True");
        }

        $.ajax({
            type: "POST",
            url: "/Property/ToggleFavourite",
            data: { propertyId: $('#modelId').val(), action: action },
            dataType: "json",
            success: function (data) {
                $("#addFavBtn").removeClass("is-disabled");
                if (action === "Add")
                    $(".fa-heart").addClass("fa-heart-red");
                else
                    $(".fa-heart").removeClass("fa-heart-red");
            }
        });
    });

    $("#btnRemoveFromFavourites").click(function () {
        $("#preloader").fadeIn();

        var propertyId = $('#modal-myvalue').val();
        $.ajax({
            type: "POST",
            url: "/Profile/DeleteFavourite",
            data: { id: propertyId },
            dataType: "text",
            success: function (result) {
                $("body").html(result);
                $("#preloader").fadeOut();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});

$(function () {
    $("#postal-codes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Property/AutocompleteLocations",
                type: "POST",
                dataType: "json",
                data: { typeName: $('#postal-codes').val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.typeName, value: item.typeName };
                    }));
                }
            });
        },
        delay: 0,
        messages: {
            noResults: "",
            results: function (resultsCount) { }
            //results: ""
        }
    });
});