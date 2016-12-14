var http = require('http');
var https = require('https');
var fs = require('fs');
var querystring = require('querystring');
var exifparser = require('./node_modules/exif-parser');
    
exports.save_pic_metadata = function(req, res, next) {
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
                res.end(JSON.stringify(imgData));
            });
        });
    }else{
        res.end("method not supported.");
    }
}

function extractExif(url, callback){
    https.get('https://0nnwaa.bn1303.livefilestore.com/y3mcfj_pf4G6erWYpiOaz-vAwe489-H9WxGEfOM1vAeZucJTLP4w89S05-PAN7oJd5dbOVOgctKYmnXeuyE0sorMqV72YVCPmdZu6rFK4Jd2KgHBhdCMkA82U9LwGD2v_-XH9W5_-ruprD-2PieTTNlpBML3-cmoIQ9D4Hh3NPCh58?width=495&amp;height=660&amp;cropmode=none', (res) => {
        var chunks = [];
        var length = 0;
        res.on('data', (chunk) => {
            if(length < 65536){
                chunks.push(chunk);
            }else{
                console.log("process exif after enough data loaded.");
                res.destroy();
            }
            length += chunk.length;
            console.log(`the length of chunk is ${length}`);
        });
        
        
        res.on('end', () => {
                var buffer = Buffer.concat(chunks);
                var parser = exifparser.create(buffer);
                var exifInfo = parser.parse();
                appendGPS(exifInfo);
                callback(exifInfo);
        });
    });
}

function appendGPS(exifInfo){
        var lat = exifInfo.tags.GPSLatitude;
        var lon = exifInfo.tags.GPSLongitude;
        var latRef = exifInfo.tags.GPSLatitudeRef;
        var lonRef = exifInfo.tags.GPSLongitudeRef;

        if (lat && lon && latRef && lonRef) {
            var latResult = lat * (latRef == "N" ? 1 : -1);
            var lonResult = lon * (lonRef == "W" ? -1 : 1);

            exifInfo.location = { lat: latResult, lng: lonResult };
        }
}