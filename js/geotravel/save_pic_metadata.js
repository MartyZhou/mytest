document.getElementById("objecturltest").onclick = function () {
        var http = new XMLHttpRequest();
        
        http.open("GET", photos[clicktimes].url, true);
        http.responseType = "blob";
        http.onload = function (e) {
            if (this.status === 200) {
                var image = new Image();
                image.onload = function () {
                    EXIF.getData(image, function () {
                        var exifdata = this.exifdata;
                        var lat = exifdata.GPSLatitude;
                        var lon = exifdata.GPSLongitude;
                        var latRef = exifdata.GPSLatitudeRef;
                        var lonRef = exifdata.GPSLongitudeRef;
                        if (lat && lon && latRef && lonRef) {
                            var latResult = (lat[0] + lat[1] / 60 + lat[2] / 3600) * (latRef == "N" ? 1 : -1);
                            var lonResult = (lon[0] + lon[1] / 60 + lon[2] / 3600) * (lonRef == "W" ? -1 : 1);

                            var location = { lat: latResult, lng: lonResult };

                            if (gmap) {
                                var testMarker = new google.maps.Marker({
                                    position: location,
                                    map: gmap
                                })
                            }
                        }
                    });
                };
                image.src = URL.createObjectURL(http.response);
            }
        };
        http.send();
    }