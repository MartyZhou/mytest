var http = require('http');
var fs = require('fs');
var querystring = require('querystring');
var exif = require("./node_modules/exif-js/exif.js");
    
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
                res.writeHead(200, {'Content-Type': 'text/html'});
                res.write('<html><body><img src="data:image/jpeg;base64,')
                res.write(new Buffer(imgData).toString('base64'));
                res.end('"/></body></html>');
            });
        });
    }else{
        res.end("method not supported.");
    }
}

function extractExif(url, callback){
    /*http.get(url, (res) => {

        callback();
    });*/
    fs.readFile('./imgs/IMG_5136.JPG', (err, data) => {
        EXIF.getData(data, () => {
            
        });
        callback(data);
    });
}