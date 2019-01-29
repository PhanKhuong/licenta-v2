function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
      /** @type {!HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'] });
}

function ChangePriceRangeEventHandler() {
    var rangeValues = $("#range").val().split(";");
    if (rangeValues) {
        $("#hdn-price-from").val(rangeValues[0]);
        $("#hdn-price-to").val(rangeValues[1]);
    }
}

$(document).ready(function () {
    $(".range-slider").on('DOMSubtreeModified', ChangePriceRangeEventHandler);
});
