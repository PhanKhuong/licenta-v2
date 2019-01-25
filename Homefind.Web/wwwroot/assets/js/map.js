
jQuery(document).ready(function ($) {

    'use strict';

    var urlParams = new URLSearchParams(window.location.search);
    ShouldDisplayAddedFeedbackInfo(urlParams);

    var propertyId = urlParams.get('propertyId');
    var geocoder = new google.maps.Geocoder();
    var currentGeoposition;
    var map;

    //google map custom marker icon - .png fallback for IE11
    var is_internetExplorer11 = navigator.userAgent.toLowerCase().indexOf('trident') > -1;
    var $marker_url = (is_internetExplorer11) ? '/assets/img/location.png' : '/assets/img/location.png';

    //define the basic color of your map, plus a value for saturation and brightness
    var $main_color = '#2d313f',
        $saturation = -20,
        $brightness = 5;

    //we define here the style of the map
    var style = [
        {
            //set saturation for the labels on the map
            elementType: "labels",
            stylers: [
                { saturation: $saturation }
            ]
        },
        {	//poi stands for point of interest - don't show these lables on the map 
            featureType: "poi",
            elementType: "labels",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            //don't show highways lables on the map
            featureType: 'road.highway',
            elementType: 'labels',
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            //don't show local road lables on the map
            featureType: "road.local",
            elementType: "labels.icon",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            //don't show arterial road lables on the map
            featureType: "road.arterial",
            elementType: "labels.icon",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            //don't show road lables on the map
            featureType: "road",
            elementType: "geometry.stroke",
            stylers: [
                { visibility: "off" }
            ]
        },
        //style different elements on the map
        {
            featureType: "transit",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "poi",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "poi.government",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "poi.sport_complex",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "poi.attraction",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "poi.business",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "transit",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "transit.station",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "landscape",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]

        },
        {
            featureType: "road",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "road.highway",
            elementType: "geometry.fill",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        },
        {
            featureType: "water",
            elementType: "geometry",
            stylers: [
                { hue: $main_color },
                { visibility: "on" },
                { lightness: $brightness },
                { saturation: $saturation }
            ]
        }
    ];

    function getMapOptions(geoposition) {
        //set google map options
        var map_options = {
            center: geoposition,
            zoom: 16,
            panControl: false,
            zoomControl: false,
            mapTypeControl: false,
            streetViewControl: false,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            scrollwheel: false,
            styles: style
        };

        return map_options;
    }

    $.ajax({
        url: "/Property/GetPropertyLocationAddress",
        type: "GET",
        dataType: "text",
        data: { propertyId: propertyId },
        success: function (address) {
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status === 'OK') {
                    currentGeoposition = results[0].geometry.location;
                    SetMapMarkerDetails(results[0], 14);
                } else {
                    alert('Geocode was not successful for the following reason: ' + status);
                }
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {
            var err = thrownError;
        }
    });

    $(".fa-graduation-cap").click(function () {
        findNearbyPlaces('school');
    });

    $(".fa-utensils").click(function () {
        findNearbyPlaces('restaurant');
    });

    $(".fa-dollar-sign").click(function () {
        findNearbyPlaces('atm');
    });

    $(".fa-shopping-cart").click(function () {
        findNearbyPlaces('supermarket');
    });

    function findNearbyPlaces(place) {
        map = new google.maps.Map(document.getElementById('conatiner-map'), getMapOptions(currentGeoposition));

        var request = {
            location: currentGeoposition,
            radius: '500',
            type: ['' + place + '']
        };

        var service = new google.maps.places.PlacesService(map);
        service.nearbySearch(request, nearbyCallback);
    }

    function nearbyCallback(results, status) {
        if (status === google.maps.places.PlacesServiceStatus.OK) {
            for (var i = 0; i < results.length; i++) {
                var place = results[i];
                createMarker(place, map);
            }
        }
    }

    var createMarker = function (place, map) {
        var marker = new google.maps.Marker({
            position: place.geometry.location,
            map: map,
            visible: true,
            icon: $marker_url
        });

        var infowindow = new google.maps.InfoWindow();
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.setContent('<div>' + (place.formatted_address || place.name) + '</div>');
            infowindow.open(map, this);
        });
    };

    var SetMapMarkerDetails = function (place, map_zoom) {
        //set google map options
        var map_options = {
            center: place.geometry.location,
            zoom: map_zoom,
            panControl: false,
            zoomControl: false,
            mapTypeControl: false,
            streetViewControl: false,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            scrollwheel: false,
            styles: style
        };
        //inizialize the map
        var map = new google.maps.Map(document.getElementById('conatiner-map'), map_options);
        //add a custom marker to the map				
        createMarker(place, map);

        //add custom buttons for the zoom-in/zoom-out on the map
        function CustomZoomControl(controlDiv, map) {
            //grap the zoom elements from the DOM and insert them in the map 
            var controlUIzoomIn = document.getElementById('cd-zoom-in'),
                controlUIzoomOut = document.getElementById('cd-zoom-out');

        }

        var zoomControlDiv = document.createElement('div');
        var zoomControl = new CustomZoomControl(zoomControlDiv, map);

        //insert the zoom div on the top left of the map
        map.controls[google.maps.ControlPosition.LEFT_TOP].push(zoomControlDiv);
    };

    AddSendFeedbackEventHandler();
});

function ShowLoader() {
    $("#preloader").fadeIn();
}

function HideLoader() {
    $("#preloader").fadeOut();
}

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

function ShouldDisplayAddedFeedbackInfo(urlParams) {
    var action = urlParams.get('redirectAction');
    if (action && action === "alert") {
        $(".alert").delay(5000).fadeOut();
    } else {
        $(".alert").remove();
    }
}