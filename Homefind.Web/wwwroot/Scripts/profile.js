$(document).ready(function () {
    $("#favourite-properties").click(function () {
        $('#fav-list-container').load("/Property/Favourites");
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