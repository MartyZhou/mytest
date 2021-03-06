// Your Client ID can be retrieved from your project in the Google
// Developer Console, https://console.developers.google.com
var CLIENT_ID = '674886615915-fihch74jdj8s85mpoms819kprvu3vt4g.apps.googleusercontent.com';

var SCOPES = ['https://www.googleapis.com/auth/drive.metadata.readonly'];

var gmap;
/**
 * Check if current user has authorized this application.
 */
function checkAuth() {
    gapi.auth.authorize(
        {
            'client_id': CLIENT_ID,
            'scope': SCOPES.join(' '),
            'immediate': true
        }, handleAuthResult);
}

/**
 * Handle response from authorization server.
 *
 * @param {Object} authResult Authorization result.
 */
function handleAuthResult(authResult) {
    var authorizeDiv = document.getElementById('authorize-div');
    if (authResult && !authResult.error) {
        // Hide auth UI, then load client library.
        authorizeDiv.style.display = 'none';
        //loadDriveApi();
    } else {
        // Show auth UI, allowing the user to initiate authorization by
        // clicking authorize button.
        authorizeDiv.style.display = 'inline';
    }
}

/**
 * Initiate auth flow in response to user clicking authorize button.
 *
 * @param {Event} event Button click event.
 */
function handleAuthClick(event) {
    gapi.auth.authorize(
        { client_id: CLIENT_ID, scope: SCOPES, immediate: false },
        handleAuthResult);
    return false;
}

/**
 * Load Drive API client library.
 */
function loadDriveApi() {
    gapi.client.load('drive', 'v2', listFiles);
    // gapi.client.load('drive', 'v2', getImageMetadata);
}

/**
 * Print files.
 */
function listFiles() {
    var request = gapi.client.drive.files.list({
        'q': "mimeType='image/jpeg'",
        'maxResults': 1000
    });

    request.execute(function (resp) {
        var files = resp.items;
        if (files && files.length > 0) {
            var icon = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                if (file.imageMediaMetadata && file.imageMediaMetadata.location) {
                    var location = { lat: file.imageMediaMetadata.location.latitude, lng: file.imageMediaMetadata.location.longitude };
                    if (gmap) {
                        var markerOptions;
                        if (file.title[0] === 'a') {
                            markerOptions = {
                                position: location,
                                map: gmap,
                                icon: icon
                            };
                        } else if (file.title[0] === 'm') {
                            markerOptions = {
                                position: location,
                                map: gmap,
                                icon: {
                                    path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
                                    scale: 2
                                }
                            };
                        } else {
                            markerOptions = {
                                position: location,
                                map: gmap
                            };
                        }
                        var testMarker = new google.maps.Marker(markerOptions);
                    }
                }
            }
        } else {
            appendPre('No files found.');
        }
    });
}

function getImageMetadata() {
    // https://www.googleapis.com/drive/v2/files/0B3EEP4o_-j5AUktwVW5IT0tBQjA
    var fileId = '0B3EEP4o_-j5AUktwVW5IT0tBQjA';
    var request = gapi.client.drive.files.get({
        'fileId': fileId
    });

    request.execute(function (resp) {
        console.log('Title: ' + resp.title);
        console.log('Description: ' + resp.description);
        console.log('MIME type: ' + resp.mimeType);
        appendPre('Image metadata: ');
        appendPre(JSON.stringify(resp));
    });
}

/**
 * Append a pre element to the body containing the given message
 * as its text node.
 *
 * @param {string} message Text to be placed in pre element.
 */
function appendPre(message) {
    var pre = document.getElementById('output');
    var textContent = document.createTextNode(message + '\n');
    pre.appendChild(textContent);
}

function initMap() {
    var uluru = { lat: 45.363, lng: 131.044 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 3,
        center: uluru
    });
    gmap = map;
}

document.getElementById("btnMetadata").onclick = function () {
    loadDriveApi();
}