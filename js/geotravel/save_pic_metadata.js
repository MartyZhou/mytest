var http = require('http');
var https = require('https');
var fs = require('fs');
var querystring = require('querystring');
var exifparser = require('./node_modules/exif-parser');
var mysql = require('./node_modules/mysql');
    
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
                persistImageMetadata({exif: imgData, url: pic_url});
                res.end(JSON.stringify(imgData));
            });
        });
    }else{
        res.end("method not supported.");
    }
}

function extractExif(url, callback){
    /*https.get('https://0nnwaa.bn1303.livefilestore.com/y3mcfj_pf4G6erWYpiOaz-vAwe489-H9WxGEfOM1vAeZucJTLP4w89S05-PAN7oJd5dbOVOgctKYmnXeuyE0sorMqV72YVCPmdZu6rFK4Jd2KgHBhdCMkA82U9LwGD2v_-XH9W5_-ruprD-2PieTTNlpBML3-cmoIQ9D4Hh3NPCh58?width=495&amp;height=660&amp;cropmode=none', (res) => {
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
    });*/

    fs.readFile('./imgs/IMG_5136.JPG', (err, data) => {
        var parser = exifparser.create(data);
        var exifInfo = parser.parse();
        console.log(`EXIF: ${JSON.stringify(exifInfo)}`);
        appendGPS(exifInfo);
        callback(exifInfo);
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

function persistImageMetadata(metadata){
    var connection = mysql.createConnection({
        host     : 'example.org',
        user     : 'bob',
        password : 'secret',
    });

    connection.connect((err) => {
        console.log(`Mysql connection error: ${JSON.stringify(err)}`);
    });

    connection.query('INSERT INTO posts SET ?', metadata, (err, result) => {
        console.log(`Mysql insert error: ${JSON.stringify(err)}`);
    });
}


    