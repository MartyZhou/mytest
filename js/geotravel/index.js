var gmap;

var clicktimes = 0;
var photos = [
    { name: 'Sanya', url: 'https://0nnwaa.bn1303.livefilestore.com/y3mcfj_pf4G6erWYpiOaz-vAwe489-H9WxGEfOM1vAeZucJTLP4w89S05-PAN7oJd5dbOVOgctKYmnXeuyE0sorMqV72YVCPmdZu6rFK4Jd2KgHBhdCMkA82U9LwGD2v_-XH9W5_-ruprD-2PieTTNlpBML3-cmoIQ9D4Hh3NPCh58?width=495&amp;height=660&amp;cropmode=none' },
    { name: 'Shanhaiguan', url: 'https://luotda.bn1303.livefilestore.com/y3mmsPBOycAaFEMZ4GnhzOC-SR2JXuvW9WKC03YNAthBXkGhMtF8qI6pMR5cDFiQOqt6djoUE6xONc2tWSfdczpriwgfVOIdciRFVLc1BbEhJmaPR_XLW2WcWj-eAAX8SrsoXOUEoXfdCFr8kKB923oJPbD-BfiMaom-ygK5A2Ev7o?width=660&amp;height=495&amp;cropmode=none' },
    { name: 'Caijiazhuang, Shanxi', url: 'https://kenpag.bn1303.livefilestore.com/y3mDueQRoqt02wL-GVkWHk2RRnzA1oH9n4fShQUZvrmCCCeLanjhKtiAdIzxDdb4nYWVDL8jka8eJ8GJ9ZRxVVgI-LSjIPJfqK7yWEiOLqpp-88R0jyPHx1gyGDdskFpLa-wukH9AHObsoFx-LEP_iVD5XbEZUzrartbbhOKcLaT5g?width=660&amp;height=495&amp;cropmode=none' },
    { name: 'Datong, Shanxi', url: 'https://lkpcrg.bn1303.livefilestore.com/y3mrscRc4oCDG0wXwfPKQ-lvkBaFeWDabOnM-xm89LD--p22qaojP9KGQRMnUNEeyB5c1vWZcr_kcPXROu6_FIYgjPzGBHVyWZ81LW3dTl6rkvNdLySK7kisn61zLPP1gvOq5ZLJiOqUci6Oq460MIPwzFLZDBKM1b_jLDPYwxAID8?width=660&amp;height=264&amp;cropmode=none' },
    { name: 'Tuling, Shanxi', url: 'https://lemz3w.bn1303.livefilestore.com/y3m8nrO1xD6oM0xIBjodR531TG_EybhXnb136ZVJmZ4C_o5bAL3g2_WVy17J2T4dRjf-zVSzE9Ius2Q90iQxy8oi_MUyqKgiNo7S-2yVXFdgNL6V-M2VP_aBH3Gv1k7tSeAkYdxcrDlnUFriC6ilv8YMlF4zj54nwJdoW6VEr9asz4?width=660&amp;height=149&amp;cropmode=none' },
    { name: 'Hukou, Shanxi', url: 'https://iunpag.bn1303.livefilestore.com/y3mazGTPItjub2jamgMmHV_rJzIDPV0PCueBCtX5b25UcQq3j-W69hrYPE43uQJeXumiQ13_XZ93VULSGmKplbFVbYjcXpiZ0gdWE1ZFUl29hwrRYjyoecMyRv3PrUADJW-ut0nIBUukjO3rsTFILM-dDIvb3KcVvuj53oAJIoEOD8?width=660&amp;height=221&amp;cropmode=none' },
    { name: 'Hanging Temple, Shanxi', url: 'https://lunigq.bn1303.livefilestore.com/y3mdoD3YuVFGvx6ue3WTnFay7AN5pQgYU8nPXWKwHUMGrxYljgGpjKkEnz8J24SvtkoZVnCVh3hD65z7vOBaXiU7w0qgAhHX50ztbA-Pr_W7pG6NKFdhZMjHZPAlfE2LjiASd4RZk9H1lxCtJFCpFWEd19FVbfNkEjAPSP7Zmto1k0?width=660&amp;height=205&amp;cropmode=none' },
    { name: 'Wulanbutong', url: 'https://kkn3iw.bn1303.livefilestore.com/y3m7J2Iar2490CAGqhBdimVmmHRZB6MZxNILm7R7QhGohJ2tRlT_B3gSjoRFDWfSn5zZx6kwcDM5mhGh2uqeEZCEDIDwjgB5KzLLHWO_RYSuDoqBbEe5wdDiZHqP2xs-TF2txDcuAKV7W2cUKJpsmVa4lfINPnshAJcLPM14lH2pds?width=660&amp;height=495&amp;cropmode=none' }
];

function initMap() {
    var uluru = { lat: 45.363, lng: 131.044 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 3,
        center: uluru
    });
    gmap = map;
}

/*document.getElementById("imgtest").onclick = function () {
    EXIF.getData(this, function () {
        var exifdata = this.exifdata;
        var lat = exifdata.GPSLatitude;
        var lon = exifdata.GPSLongitude;
        var latRef = exifdata.GPSLatitudeRef;
        var lonRef = exifdata.GPSLongitudeRef;
        var latResult = (lat[0] + lat[1] / 60 + lat[2] / 3600) * (latRef == "N" ? 1 : -1);
        var lonResult = (lon[0] + lon[1] / 60 + lon[2] / 3600) * (lonRef == "W" ? -1 : 1);

        var location = { lat: latResult, lng: lonResult };

        if (gmap) {
            var testMarker = new google.maps.Marker({
                position: location,
                map: gmap
            })
        }
    });
}*/

document.getElementById("objecturltest").onclick = function () {
    if (clicktimes < photos.length) {
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
    clicktimes++;
}