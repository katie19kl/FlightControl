function getFlights() {
    let xhttp = new XMLHttpRequest();
    
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            let flight = JSON.parse(this.responseText);

            // Clear out the table rows.
            let node = document.getElementById("tablebody");
            while (node.hasChildNodes()) {
                node.removeChild(node.lastChild);
            }

            if (Object.keys(flight[0]).length !== 0) {
                addToTable(flight);
                addFlightIcon();
            }
        }
    };
    let date = new Date();

    xhttp.open("GET", "/api/Flights?relative_to=" + date.toISOString(), true);
    xhttp.send();
}


function addToTable(flight) {
    let table = document.getElementById("flightsTable").getElementsByTagName("tbody")[0];

    flight.forEach(function (item) {

        let row = table.insertRow(-1);
        let cellId = row.insertCell(-1);
        let cellCompanyName = row.insertCell(-1);
        let deleteCell = row.insertCell(-1);
        let btn = document.createElement("button");
        btn.innerHTML = "Delete";
        btn.className = "btn btn-danger btn-xs"; 

        cellId.style.textAlign = "center";
        cellCompanyName.style.textAlign = "center";
        cellId.innerHTML = item.flight_Id;
        cellCompanyName.innerHTML = item.company_Name;
        deleteCell.appendChild(btn); // Add the delete button to the a table cell.

    });  
}

function addFlightIcon() {
    let mymap = mapsPlaceholder.pop();
    let planeIcon = L.icon({
        iconUrl: 'styles/plane.png',

        iconSize: [38, 95], // size of the icon
        iconAnchor: [22, 94], // point of the icon which will correspond to marker's location
        popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
    });

    L.marker([51.5, -0.09], { icon: planeIcon }).addTo(mymap);
    //let marker = L.marker([51.5, -0.09]).addTo(mymap);
}