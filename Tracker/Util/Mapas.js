
var googleMap;
var imei;
var fechaSeleccionada;

(function () {
    // Añado a Map un array con los markers que contiene
    google.maps.Map.prototype.markers = new Array();

    // Añado a Map un método clearMakers que borrar los markers
    google.maps.Map.prototype.clearMarkers = function () {
        for (var i = 0; i < this.markers.length; i++) {
            this.markers[i].setMap(null);
        }
        this.markers = new Array();
    };

    google.maps.Map.prototype.hacerZoomOnClick = function () {

        for (var i = 0; i < this.markers.length; i++) {
            
            google.maps.event.addListener(this.markers[i], 'click', function() {
                googleMap.panTo(this.getPosition());
                googleMap.setZoom(16);
            });
        }
    }

    // Reescribo el método setMap de Marker para que cuando se 
    // asigne el map se guarde en la propiedad markers del map
    // OJO: almaceno en oldSetMap el antiguo método setMap
    //      para poder seguir utilizándolo
    var oldSetMap = google.maps.Marker.prototype.setMap;
    google.maps.Marker.prototype.setMap = function (map) {
        if (map) {
            map.markers.push(this);
        }
        oldSetMap.call(this, map);
    }
})();


function obtenerFechas() {
    
    fechaSeleccionada = moment(fechaSeleccionada).format('LLL');

    var datosFecha = [{ imeiUsuario: imei, fecha: fechaSeleccionada }];

    $.ajax({
        url: "../GetMarkersDate?imeiUsuario=" + imei + "&fecha=" + fechaSeleccionada,
        type: "POST",
        data: datosFecha,
        success: function (data, textStatus, jqXHR) {
            for (var i = 0; i < data.length; i++) {
                placeMarker(data[0].Latitude, data[0].Longitude);
            }
            googleMap.hacerZoomOnClick();
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}



function iniciarMapa() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    }

    if (imei !== '') {

        var bounds = new google.maps.LatLngBounds();
        var options = {
            zoom: 12,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        googleMap = new google.maps.Map($("#map")[0], options);

        var infoWindow = new google.maps.InfoWindow({ content: "Cargando..." });

        $.ajax({
            type: "POST",
            url: "../GetMarkers",
            data: { imei: imei },
            datatype: "json",
            success: function (data) {

                if (data.length > 0) {
                    var myLatlng = new google.maps.LatLng(data[0].Latitude, data[0].Longitude);
                    googleMap.setCenter(myLatlng);
                    googleMap.setZoom(12);

                    for (var i = 0; i < data.length; i++) {

                        var point = new google.maps.LatLng(data[i].Latitude, data[i].Longitude);

                        bounds.extend(point);

                        var marker = new google.maps.Marker({
                            position: point,
                            map: googleMap,
                            html: data[i].InfoWindow
                        });

                        google.maps.event.addListener(marker, "click", function () {
                            infoWindow.setContent(this.html);
                            infoWindow.open(googleMap, this);
                        });
                    }

                    googleMap.hacerZoomOnClick();
                }
                else {
                    var myLatlng = new google.maps.LatLng('4.604772', '-74.135332');
                    googleMap.setCenter(myLatlng);
                    googleMap.setZoom(12);
                }
            }

        });

        googleMap.fitBounds(bounds);
    }
}


function placeMarker(Latitude, Longitude) {
    var point = new google.maps.LatLng(Latitude, Longitude)
    var marker = new google.maps.Marker({
        position: point,
        map: googleMap,
        animation: google.maps.Animation.DROP,
    });
}


$(function () {
    var chat = $.connection.chatHub;
    chat.client.addNewMessageToPage = function (name, message) {
        /// Si coresponde al imei muestre el punto
        if (name === imei) {
            var cordenadas = message.toString().split(',');
            placeMarker(cordenadas[0], cordenadas[1]);
        }
    };

    $.connection.hub.start().done(function () {
    });
});

function htmlEncode(value) {
}


function showPosition(position) {
    var myLatlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    googleMap.setCenter(myLatlng);
    googleMap.setZoom(10);
}

function limpiarMarcadores() {
    googleMap.clearMarkers();
}