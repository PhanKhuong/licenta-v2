function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
      /** @type {!HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'] });
}
