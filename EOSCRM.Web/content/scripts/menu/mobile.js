
window.onload = function () {
    init();
    //alert("Hello!");
}

function init() {
    if (window.innerWidth <= 600) {
        //alert("Helloerger!");
        //var portfolioDiv = document.getElementById('wrapper');

        document.getElementById('headerDiv').hidden = true;
        document.getElementById('headerDivContent').hidden = false;
    }
    else {
        document.getElementById('headerDiv').hidden = false;
        document.getElementById('headerDivContent').hidden = true;
    }

    
}
