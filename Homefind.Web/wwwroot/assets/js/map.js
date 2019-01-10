
jQuery(document).ready(function ($) {

    'use strict';
    //set your google maps parameters
    var $latitude = 34.1215659,
        $longitude = -118.2095611,
        $map_zoom = 14;

    var propertyId = location.search.substr(1).split("=")[1];
    var geocoder = new google.maps.Geocoder();
    $.ajax({
        url: "/Property/GetPropertyLocationAddress",
        type: "GET",
        dataType: "text",
        data: { propertyId: propertyId },
        success: function (address) {
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status === 'OK') {
                    RetrieveMap(results[0].geometry.location, results[0].formatted_address, 14);
                } else {
                    alert('Geocode was not successful for the following reason: ' + status);
                }
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {
            var err = thrownError;
        }
    });

    var RetrieveMap = function (location, complete_name, map_zoom) {
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

        //set google map options
        var map_options = {
            center: location,
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
        var marker = new google.maps.Marker({
            position: location,
            map: map,
            visible: true,
            icon: $marker_url
        });

        var infowindow = new google.maps.InfoWindow();
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.setContent('<div>' + complete_name + '</div>');
            infowindow.open(map, this);
        });

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
});