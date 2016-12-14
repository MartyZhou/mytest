var http = require('http');
var https = require('https');
var fs = require('fs');
var querystring = require('querystring');
var exifparser = require('./node_modules/exif-parser');
    
exports.save_pic_metadata = function(req, res, next) {
    /*console.log("headers " + req.method);
    console.log(req.headers);
    console.log("rawHeaders");
    console.log(req.rawHeaders);
    console.log("rawTrailers");
    console.log(req.rawTrailers);*/

    if(req.method === "POST"){
        var body='';

        req.on('data', (chunk) => {
            body += chunk;
            console.log(`BODY: ${chunk}`);
        });

        req.on('end', () => {
            var data = querystring.parse(body);
            var pic_url = data.pic_url;
            
            extractExif(pic_url, (imgData) => {
                /*res.writeHead(200, {'Content-Type': 'text/html'});
                res.write('<html><body><img src="data:image/jpeg;base64,')
                res.write(new Buffer(imgData).toString('base64'));
                res.end('"/></body></html>');*/
                res.end(JSON.stringify(imgData));
            });
        });
    }else{
        res.end("method not supported.");
    }
}

function extractExif(url, callback){
    https.get('https://0nnwaa.bn1303.livefilestore.com/y3mcfj_pf4G6erWYpiOaz-vAwe489-H9WxGEfOM1vAeZucJTLP4w89S05-PAN7oJd5dbOVOgctKYmnXeuyE0sorMqV72YVCPmdZu6rFK4Jd2KgHBhdCMkA82U9LwGD2v_-XH9W5_-ruprD-2PieTTNlpBML3-cmoIQ9D4Hh3NPCh58?width=495&amp;height=660&amp;cropmode=none', (res) => {
        res.on('data', (chunk) => {
            console.log("this is the");
        });
        
        res.on('end', () => {
            console.log("this is the end");
            /*var parser = exifparser.create(chunk);
            var exifInfo = parser.parse();
            console.log(`EXIF: ${JSON.stringify(exifInfo)}`);*/
            callback("exifInfo");
        });
    });


    /*fs.readFile('./imgs/IMG_5136.JPG', (err, data) => {
        var parser = exifparser.create(data);
        var exifInfo = parser.parse();
        console.log(`EXIF: ${JSON.stringify(exifInfo)}`);
        var lat = exifInfo.tags.GPSLatitude;
        var lon = exifInfo.tags.GPSLongitude;
        var latRef = exifInfo.tags.GPSLatitudeRef;
        var lonRef = exifInfo.tags.GPSLongitudeRef;

        console.log(`EXIF with location: ${lat}, ${lon}, ${latRef}, ${lonRef}`);
        if (lat && lon && latRef && lonRef) {
            console.log(`EXIF with location: start`);
            var latResult = lat * (latRef == "N" ? 1 : -1);
            var lonResult = lon * (lonRef == "W" ? -1 : 1);

            exifInfo.location = { lat: latResult, lng: lonResult };
            console.log(`EXIF with location: ${JSON.stringify(exifInfo)}`);
        }

        callback(exifInfo);
    });*/
}