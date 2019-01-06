$(document).ready(function () {
    // data-* attributes to scan when populating modal values
    var ATTRIBUTES = ['myvalue', 'myvar', 'bb'];

    $('[data-toggle="modal"]').on('click', function (e) {
        // convert target (e.g. the button) to jquery object
        var $target = $(e.target);
        // modal targeted by the button
        var modalSelector = $target.data('target');

        // iterate over each possible data-* attribute
        ATTRIBUTES.forEach(function (attributeName) {
            // retrieve the dom element corresponding to current attribute
            var $modalAttribute = $(modalSelector + ' #modal-' + attributeName);
            var dataValue = $target.data(attributeName);

            // if the attribute value is empty, $target.data() will return undefined.
            // In JS boolean expressions return operands and are not coerced into
            // booleans. That way is dataValue is undefined, the left part of the following
            // Boolean expression evaluate to false and the empty string will be returned
            $modalAttribute.text(dataValue || '');
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