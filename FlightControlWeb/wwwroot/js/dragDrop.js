let size = 0;
let flagPic = 0;
let imageIsHere = 0;
let inSide = 0;


function dropHandler(ev) {

    // Prevent default behavior (Prevent file from being opened)
    ev.preventDefault();
    if (ev.dataTransfer.items) {

        // Use DataTransferItemList interface to access the file(s)
        for (var i = 0; i < ev.dataTransfer.items.length; i++) {

            // If dropped items aren't files, reject them
            if (ev.dataTransfer.items[i].kind === 'file') {
                let file = ev.dataTransfer.items[i].getAsFile();
                let filename = file.name;
                var parts = filename.split('.');
                let extension = parts[parts.length - 1];
                size = file.size;
                if (extension === "json") {
                    getFileContent(file);
                }
            }
        }
    }
    let table = document.getElementById("flightsTable");
    let dropZone = document.getElementById("drop_zone");
    dropZone.style.backgroundImage = "";
    table.style.visibility = 'visible';
    let check = document.getElementById("check");
    check.innerHTML = "file was realeased" + inSide;
    imageIsHere = 0;
    inSide = 0;
}

function endDrag(event) {
    event.preventDefault();
}

function enterZone(event) {

    event.preventDefault();

    let check = document.getElementById("check");
    check.innerHTML = "in entrance zone";

    let dropZone = document.getElementById("drop_zone");

    let table = document.getElementById("flightsTable");

    table.addEventListener('leaveArea', ondragleave, false);


    dropZone.style.backgroundImage = 'url(drag_drop.png)';
    imageIsHere = 1;
    table.style.visibility = "hidden";

}

function leaveArea(event) {

    event.preventDefault();
    let target = event.relatedTarget;

    let locName = target.className;

    if (locName === "col-lg-4 sidenav") {
        let dropZone = document.getElementById("drop_zone");

        let table = document.getElementById("flightsTable");

        if (imageIsHere === 1) {

            table.style.visibility = "visible";
            dropZone.style.backgroundImage = "";
            imageIsHere = 0;
        }
    }
}

function dragOverHandler(ev) {

    let dropZone = document.getElementById("drop_zone");
    let table = document.getElementById("flightsTable");

    if (imageIsHere !== 1) {

        table.style.visibility = "hidden";
        let urlString = 'url(drag_drop.png)';
        dropZone.style.backgroundImage = urlString;

        imageIsHere = 1;
    }
    imageIsHere = 1;

    ev.preventDefault();
}

function getFileContent(file) {

    changeFile(file);
    return;
}


function readFile(event) {

    let sendJson = event.target.result;

    fetch('/api/FlightPlan', {
        method: 'POST',
        headers: {
            'content-length': size,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: sendJson
    })
        .then(response => response.json())
        .then(result => {
            console.log('Success:', result);
        })
        .catch(error => {
            console.error('Error:', error);
        });

}

function changeFile(file) {

    let reader = new FileReader();
    reader.addEventListener('load', readFile);
    reader.readAsText(file);
}
