function initializeMap() {

    let mymap = L.map('mymap').setView([51.5074, 0.1278], 3);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1Ijoia2F0aWUxOSIsImEiOiJja2FnZng5ZHMwM2hxMnNsc29wNnZldnA0In0.Ax_qIMk6gULQg993pCqnGQ'
    }).addTo(mymap);

    mymap.on('click', onMapClick);
}

function onMapClick(e) {

    let initIcon = L.icon({
        iconUrl: 'styles/plane.png',
        iconSize: [28, 30],
        iconAnchor: [18, 18]
    });

    if (smnIsPressed === true) {

        // Remove the highlight from the corresponding row.
        highlightedRow = document.getElementById(idPressed); //
        highlightedRow.style.backgroundColor = originalColor; //

        // Remove the polyline:
        mymap = mapsPlaceholder.pop();
        mymap.removeLayer(polyline);
        mapsPlaceholder.push(mymap);

        flagWasHere = 0;

        idPressed = 0; // The path should be removed from the map.
    }

    globalVar.setIcon(initIcon);
    smnIsPressed = false;
    let flightDetailBlock = document.getElementById("FlightDetailsBlock");
    flightDetailBlock.innerHTML = "";
}