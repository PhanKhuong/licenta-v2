
jQuery(document).ready(function ($) {

    'use strict';

    var urlParams = new URLSearchParams(window.location.search);

    var propertyId = urlParams.get('propertyId');
    var geocoder = new google.maps.Geocoder();
    var currentGeoposition;
    var map;

    //google map custom marker icon - .png fallback for IE11
    var is_internetExplorer11 = navigator.userAgent.toLowerCase().indexOf('trident') > -1;
    var $marker_red = (is_internetExplorer11) ? '/assets/img/marker-red.png' : '/assets/img/marker-red.png';
    var $marker_blue = (is_internetExplorer11) ? '/assets/img/marker-blue.png' : '/assets/img/marker-blue.png';

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
            zoom: 15,
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
                    currentGeoposition = results[0];
                    SetMapMarkerDetails(results[0], 15);
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

    $(".fa-car").click(function () {
        setTrafficLayer();
    });

    function setTrafficLayer() {
        var map = new google.maps.Map(document.getElementById('map-container'), getMapOptions(currentGeoposition));

        var trafficLayer = new google.maps.TrafficLayer();
        trafficLayer.setMap(map);

        createMarker(currentGeoposition, map, $marker_red);
        map.setCenter(currentGeoposition.geometry.location);
    }

    function findNearbyPlaces(place) {
        map = new google.maps.Map(document.getElementById('map-container'), getMapOptions(currentGeoposition));

        var request = {
            location: currentGeoposition.geometry.location,
            radius: '500',
            type: ['' + place + '']
        };

        var service = new google.maps.places.PlacesService(map);
        service.nearbySearch(request, nearbyCallback);

        map.setCenter(currentGeoposition.geometry.location);
    }

    function nearbyCallback(results, status) {
        if (status === google.maps.places.PlacesServiceStatus.OK) {
            for (var i = 0; i < results.length; i++) {
                var place = results[i];
                createMarker(place, map, $marker_blue);
            }
        }
        createMarker(currentGeoposition, map, $marker_red);
    }

    var createMarker = function (place, map, icon) {
        var marker = new google.maps.Marker({
            position: place.geometry.location,
            map: map,
            visible: true,
            icon: icon
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
        var map = new google.maps.Map(document.getElementById('map-container'), map_options);
        //add a custom marker to the map				
        createMarker(place, map, $marker_red);

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
        map.setCenter(currentGeoposition.geometry.location);
    };
});