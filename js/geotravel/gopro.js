document.getElementById("gopro").onclick = function() {
    EXIF.getData(this, function() {
        alert(EXIF.pretty(this));
    });
}