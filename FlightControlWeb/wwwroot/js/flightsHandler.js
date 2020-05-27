let idToIcon = new Map(); // Map id to a an icon.
let idsOfCurrFlights = []; // List of current active flight ids.
let globalVar;
let smnIsPressed = false;
let idLocation = new Map();
let idPressed = 0;
let pressedEnd = [];
let pressedStart = [];
let flagWasHere = 0;
let polyline = L.polyline([0, 0], { className: 'my_polyline' });
let currAmount = 0;
let prevAmount = 0;
let originalColor; // enough setting it once.

let planeIcon = L.icon({
    iconUrl: 'styles/plane.png',
    iconSize: [28, 30],
    iconAnchor: [18, 18]
}); //

let planeIconWithShadow = L.icon({

    iconUrl: 'styles/plane.png',
    shadowUrl: 'styles/red-shadow.png',
    iconSize: [28, 30],
    shadowSize: [38, 40],
    iconAnchor: [18, 18],
    shadowAnchor: [18, 23]
}); //

/* 
 * getFlights()- produces AJAX reguests, 4 times a second.
 * The request is for the current internal flights.
 * When the response has been received, the function:
 * - adds the flight to the table if new.
 * - adds an icon(if wasn't created).
 */
function getFlights() {
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = handleRequest;

    Date.prototype.addHours = function (h) {
        this.setTime(this.getTime() + h * 60 * 60 * 1000);
        return this;
    };
    let date = new Date().addHours(3);

    xhttp.open("GET", "/api/Flights?relative_to=" + date.toISOString(), true);
    xhttp.send();
}

function handleRequest() {
    if (this.readyState === 4 && this.status === 200) {

        // Clear the array to update according to current flights.
        idsOfCurrFlights = [];

        let flights = JSON.parse(this.responseText);

        currAmount = 0;
        flights.forEach(function (item) {
            // For maintaining the ids(of the curr flight).
            idsOfCurrFlights.push(item.flight_Id);
            currAmount++;
        });

        if (prevAmount !== currAmount) {
            // Clear out the table rows.
            let node = document.getElementById("tablebody");
            while (node.hasChildNodes()) {
                node.removeChild(node.lastChild);
            }

        }

        addFlightIcon(flights);

        mymap = mapsPlaceholder.pop();

        // Removes elements when a flight has finished.
        maintainMap(mymap);
        mapsPlaceholder.push(mymap);

        if (prevAmount !== currAmount) {
            prevAmount = currAmount;


            if (Object.keys(flights[0]).length !== 0) {
                addToTable(flights);
            }
        }

        prevAmount = currAmount;

    }
}

/* 
 * addToTable (flights) - given a list of flights,
 * the function adds new rows to the table and
 * inputs the relevants info.
 */
function addToTable(flights) {
    let table = document.getElementById("flightsTable").
        getElementsByTagName("tbody")[0];

    flights.forEach(function (item) {
        let row = table.insertRow(-1);
        originalColor = row.style.backgroundColor; //
        row.onclick = rowClick;
        row.id = item.flight_Id;
        let cellId = row.insertCell(-1);
        let cellCompanyName = row.insertCell(-1);
        let deleteCell = row.insertCell(-1);
        let btn = document.createElement("button");

        btn.innerHTML = "Delete";
        btn.className = "btn btn-danger btn-xs";
        btn.id = item.flight_Id;
        btn.onclick = deleteFlight;
        cellId.style.textAlign = "center";
        cellCompanyName.style.textAlign = "center";
        cellId.innerHTML = item.flight_Id;
        cellCompanyName.innerHTML = item.company_Name;

        // Add the delete button to the a table cell.
        deleteCell.appendChild(btn);
    });
}

function rowClick() {

    if (smnIsPressed === true) { // An icon is pressed

        // Check if it's not the corresponding icon 
        //=> should change to the icon with the row's id.
        if (idPressed !== this.id) {

            rowClickWithPrev(this);

        } // else - the current row is pressed=>change nothing.

    } else { // Nothing is pressed.

        rowClickNew(this);
    }

    this.style.backgroundColor = '#FF0000';

}

function rowClickNew(row) { //
    idPressed = row.id;
    smnIsPressed = true;

    // Define globalVar to be the corresponding icon.
    idToIcon.get(row.id).setIcon(planeIconWithShadow); // highlight marker.
    globalVar = idToIcon.get(row.id);

    flagWasHere = 0; // There is no poly-line.

    ShowDataOfFlight(row.id); // Flight-details.
}

function rowClickWithPrev(row) { //
    let prevId = idPressed;
    idPressed = row.id; // Update current id of pressed marker + row.

    // Convert color of prev row to original color.
    let prevRow = document.getElementById(prevId);
    prevRow.style.backgroundColor = originalColor;

    // Update the marker.
    globalVar.setIcon(planeIcon);
    globalVar = idToIcon.get(row.id); // Set it to icon with row's id.
    globalVar.setIcon(planeIconWithShadow);

    // Update the polyline.
    mymap = mapsPlaceholder.pop();
    mymap.removeLayer(polyline);
    mapsPlaceholder.push(mymap);
    flagWasHere = 0; // There is no polyline for the clicked flight.

    ShowDataOfFlight(row.id);
}

/* 
 * addFlightIcon(flights) - adds a new icon if the
 * flight info is received for the first time.
 * Otherwise, updates the location of the icon on
 * the map according to flight info.
 */
function addFlightIcon(flights) {
    let mymap = mapsPlaceholder.pop();

    // Define the plane icon.
    let planeIcon = L.icon({
        iconUrl: "styles/plane.png",
        iconSize: [28, 30],
        iconAnchor: [18, 18]
    });

    // Go over each flight.
    flights.forEach(function (item) {
        if (!idToIcon.has(item.flight_Id)) {

            // Create new marker.
            let myMarker = L.marker([item.latitude, item.longitude],
                { myCustomId: item.flight_Id, icon: planeIcon }).on('click', onClickIcon).addTo(mymap);
            idToIcon.set(item.flight_Id, myMarker);
            let listOfLocations = [];

            listOfLocations.push([item.latitude, item.longitude]);
            idLocation.set(item.flight_Id, listOfLocations);
        }
        else {
            // Update marker.
            idToIcon.get(item.flight_Id).setLatLng([item.latitude, item.longitude]).update();
            idLocation.get(item.flight_Id).push([item.latitude, item.longitude]);
        }

        if (item.flight_Id === idPressed) {
            ShowTrajectoryOfFlight(item.flight_Id, mymap);
            flagWasHere = 1;
        }
    });
    mapsPlaceholder.push(mymap); // Push again the map object.
}

function maintainMap(mymap) {

    // Keys are ids stored in the map.
    let listOfKeys = idToIcon.keys();

    for (let id of listOfKeys) {

        // The flight has ended.
        if (!idsOfCurrFlights.includes(id)) {

            // Should remove the element with this id from the map
            mymap.removeLayer(idToIcon.get(id));
            idToIcon.delete(id);



            idLocation.delete(id);

            if (idPressed === id) {
                mymap.removeLayer(polyline);
                idPressed = 0;
                document.getElementById("FlightDetailsBlock").innerHTML = "";
            }
        }
    }
}

function deleteFlight() {


    //mymap.removeLayer(this.id);
    //idToIcon.delete(this.id);

    if (idPressed === this.id) { // The deleted flight is with pressed icon.
        document.getElementById("FlightDetailsBlock").innerHTML = ""; // Empty the flight details.
        idPressed = 0;

        // Remove path from map:
        mymap = mapsPlaceholder.pop();
        mymap.removeLayer(polyline);
        mapsPlaceholder.push(mymap);

        // Initializing.
        smnIsPressed = false;
        globalVar = 0;
        flagWasHere = 0;
    }


    let xhttp = new XMLHttpRequest();

    xhttp.open("DELETE", "/api/Flights/" + this.id, true);
    xhttp.send();

}

function getFlightPlanById(id) {


    let xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {

        if (this.readyState === 4 && this.status === 200) {

            // Returns the flightPlan object.
            return JSON.parse(this.responseText);
        }
    };

    xhttp.open("GET", "/api/FlightPlan/" + id, true);
    xhttp.send();
}

function onClickIcon(e) {
    var id;

    if (idPressed === 0 ) { ///////// nothing is pressed.

        e.target.setIcon(planeIconWithShadow);
        smnIsPressed = true;

        // Mark marker that was pressed
        globalVar = e.target;
        id = this.options.myCustomId;

        // Highlight table row.
        document.getElementById(id).style.backgroundColor = '#FF0000';
    }

    // A marker was highlighted before.
    else {
        let prevId = globalVar.options.myCustomId;
        globalVar.setIcon(planeIcon);
        globalVar = e.target;
        globalVar.setIcon(planeIconWithShadow);
        smnIsPressed = true;

        // Remove path of previous marker(that was clicked until now).
        mymap = mapsPlaceholder.pop();
        mymap.removeLayer(polyline);
        mapsPlaceholder.push(mymap);
        flagWasHere = 0;

        // Update rows highlighting.
        document.getElementById(prevId).style.backgroundColor = originalColor; //
        document.getElementById(this.options.myCustomId).style.backgroundColor = '#FF0000'; //

    }

    id = this.options.myCustomId;
    ShowDataOfFlight(id);

}

async function ShowTrajectoryOfFlight(id, mymap) {

    var arrLoc = idLocation.get(id).slice(); // Get copy of array.

    arrLoc.push(pressedEnd); // Push the end of the path to the array.

    if (flagWasHere === 0) { // Path is not showed on map.

        polyline = L.polyline(arrLoc, { className: 'my_polyline' }).addTo(mymap);

        return;
    }
    else { // Update the map(remove and insert the updated one).

        mymap.removeLayer(polyline);
    }

    polyline = L.polyline(arrLoc, { className: 'my_polyline' }).addTo(mymap);
    return;
}


async function ShowDataOfFlight(id) {


    let flightDetailBlock = document.getElementById("FlightDetailsBlock");

    let path = "api/Helper/" + id;
    let response = await fetch(path);
    let pars_Object = await response.json();

    flightDetailBlock.innerHTML = " company name is " + pars_Object.companyName + "<br>"
        + " start position is ( longitude " + pars_Object.startLongitude + " and latitude is "
        + pars_Object.startLatitude + ")<br>"
        + " number of passengers is " + pars_Object.numOfPassengers + "<br>"
        + " starting time is " + pars_Object.takeOffTime + "<br>"
        + " end position is ( longitude " + pars_Object.endLongitude
        + " and latitude is " + pars_Object.endLatitude + "<br>"
        + "land time is " + pars_Object.landTime + "<br>"
        + "flight id is " + id;

    idPressed = id;
    pressedEnd = [pars_Object.endLatitude, pars_Object.endLongitude];
    pressedStart = [pars_Object.startLatitude, pars_Object.startLongitude];

    return;
}