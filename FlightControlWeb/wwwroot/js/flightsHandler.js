let idToIcon = new Map();
let idsOfCurrFlights = [];
let idsOfExternalFlights = [];
let globalVar;
let smnIsPressed = false;
let idPressed = 0;
let pressedStart = [];
let flagWasHere = 0;
let polyline = L.polyline([0, 0],
    {className: "my_polyline"});
let currAmount = 0;
let prevAmount = 0;
let externalCurrAmount = 0;
let externalPrevAmount = 0;
let originalColor;
let deleteClicked = false;
let idPressedDeleted = 0;
let endArrayPath = [];
let idCurPos = new Map();
let sizeArr = 0;
let flights = [], externalFlights = [];
let planeIcon = L.icon({
    iconUrl: "notPressed.png",
    iconSize: [38, 40],
    iconAnchor: [18, 18]
});

let planeIconWithShadow = L.icon({

    iconUrl: "pressed.png",
    iconSize: [38, 40],
    iconAnchor: [18, 18]
});

let planeIconInit = L.icon({
    iconUrl: "notPressed.png",
    iconSize: [38, 40],
    iconAnchor: [18, 18]
});


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

    let date = new Date();

    xhttp.open("GET",
        "/api/Flights?relative_to=" +
        date.toISOString() + "&sync_all", true);
    xhttp.send();
}


/* 
 * Handles the response received after sending
 * the asynchronous request in getFlights function.
 */
function handleRequest() {
    if (this.readyState === 4 && this.status === 200) {
        maintainFlightLists(this);
        checkForUpdates();
        let myFlights = JSON.parse(this.responseText);
        addFlightIcon(myFlights);
        mymap = mapsPlaceholder.pop();

        // Removes elements when a flight has finished.
        maintainMap(mymap);
        mapsPlaceholder.push(mymap);

        // Changes in internal flights.
        if (prevAmount !== currAmount) {
            prevAmount = currAmount;
            if (Object.keys(flights[0]).length !== 0) {
                addToTable(flights);
            }
        }
        prevAmount = currAmount;

        // Changes in external flights.
        if (externalPrevAmount !== externalCurrAmount) {
            externalPrevAmount = externalCurrAmount;
            if (Object.keys(externalFlights[0]).length !== 0) {
                addToExternalsTable(externalFlights);
            }
        }
        externalPrevAmount = externalCurrAmount;
    }
}

/* 
 * Maintains the flights ids to check if
 * something changed in both internal and
 * external flights.
 */
function maintainFlightLists(obj) {

    // Clear the array to update according to current flights.
    idsOfCurrFlights = [];
    idsOfExternalFlights = [];

    let myFlights = JSON.parse(obj.responseText);
    flights = [];
    externalFlights = [];

    currAmount = 0;
    externalCurrAmount = 0;
    myFlights.forEach(function (item) {

        // For maintaining the ids(of the curr flight).
        if (item.is_External === false) {
            idsOfCurrFlights.push(item.flight_Id);
            currAmount++;
            flights.push(item);
        } else {

            // Is external.
            idsOfExternalFlights.push(item.flight_Id);
            externalCurrAmount++;
            externalFlights.push(item);
        }
    });
}

/* 
 * Check if there was an update in the received
 * list of flights.
 */
function checkForUpdates() {

    // Check for updates
    if (prevAmount !== currAmount) {

        // Clear out the table rows.
        let node = document.getElementById("tablebody");
        while (node.hasChildNodes()) {
            node.removeChild(node.lastChild);
        }
    }

    // Check for updates in external flights.
    if (externalPrevAmount !== externalCurrAmount) {

        let node = document.getElementById("externalTableBody");
        while (node.hasChildNodes()) {
            node.removeChild(node.lastChild);
        }
    }
}

/*
 * addToTable (flights) - given a list of flights,
 * the function adds new rows to the table and
 * inputs the relevants info, into the table of
 * the internal flights.
 */
function addToTable(flights) {

    // Removes elements when a flight has finished.
    let table = document.getElementById("flightsTable").
        getElementsByTagName("tbody")[0];

    flights.forEach(function (item) {

        let row = table.insertRow(-1);
        originalColor = row.style.backgroundColor;
        row.onclick = rowClick;
        row.id = item.flight_Id;

        if (row.id === idPressedDeleted) {

            row.style.backgroundColor = '#FF0000';
            idPressed = idPressedDeleted;
            smnIsPressed = true;
        }

        if (row.id === idPressed) {
            row.style.backgroundColor = '#FF0000';
            idPressedDeleted = idPressed;
            smnIsPressed = true;
        }

        fillRow(row, item);
    });
}

/* Puts new flights into the external flights table. */
function addToExternalsTable(externalFlights) {

    // Removes elements when a flight has finished.
    let table = document.getElementById("externalTableBody");

    externalFlights.forEach(function (item) {

        let row = table.insertRow(-1);
        originalColor = row.style.backgroundColor;
        row.onclick = rowClick;
        row.id = item.flight_Id;

        if (row.id === idPressedDeleted) {

            row.style.backgroundColor = '#FF0000';
            idPressed = idPressedDeleted;
            smnIsPressed = true;
        }

        if (row.id === idPressed) {

            row.style.backgroundColor = '#FF0000';
            idPressedDeleted = idPressed;
            smnIsPressed = true;
        }

        fillRow(row, item);
    });
}

/* 
 * Given an obejct of a row and a flight
 * object, the function fills the row 
 * in accordance.
 */
function fillRow(row, flightItem) {

    let isFlightExternal = flightItem.is_External;
    let cellId = row.insertCell(-1);
    let cellCompanyName = row.insertCell(-1);
    let btn;
    let deleteCell;

    if (isFlightExternal === false) {

        deleteCell = row.insertCell(-1);
        btn = document.createElement("button");
        btn.innerHTML = "Delete";
        btn.className = "btn btn-danger btn-xs";
        btn.id = flightItem.flight_Id;
        btn.onclick = deleteFlight;

        // Add the delete button to the a table cell.
        deleteCell.appendChild(btn);
    }

    cellId.style.textAlign = "center";
    cellCompanyName.style.textAlign = "center";
    cellId.innerHTML = flightItem.flight_Id;
    cellCompanyName.innerHTML = flightItem.company_Name;
}

/* 
 * The event which handles a row click.
 * When clicked, a row would be highlighted.
 */
function rowClick() {

    if (deleteClicked === false) {

        if (smnIsPressed === true) { // An icon is pressed

            /* 
             * Check if it's not the corresponding icon 
             * => should change to the icon with the row's id.
            */
            if (idPressed !== this.id) {

                rowClickWithPrev(this);

            }
            // else - the current row is pressed=>change nothing.

        // Nothing is pressed.
        } else {

            rowClickNew(this);
        }

        this.style.backgroundColor = '#FF0000';
    }
    deleteClicked = false;
}

/* 
 * The event helper of row click.
 * Would be called when no other row 
 * was highlighted before.
 */
function rowClickNew(row) {

    idPressed = row.id;
    smnIsPressed = true;

    // Define globalVar to be the corresponding icon.
    idToIcon.get(row.id).setIcon(planeIconWithShadow);
    globalVar = idToIcon.get(row.id);

    // There is no poly-line.
    flagWasHere = 0;

    // Flight-details.
    ShowDataOfFlight(row.id);
}

/*
 * The event helper of row click.
 * Would be called when another row
 * was highlighted before.
 */
function rowClickWithPrev(row) {

    let prevId = idPressed;
    idPressed = row.id;

    // Convert color of prev row to original color.
    let prevRow = document.getElementById(prevId);
    prevRow.style.backgroundColor = originalColor;

    // Update the marker.
    globalVar.setIcon(planeIconInit);
    globalVar = idToIcon.get(row.id);
    globalVar.setIcon(planeIconWithShadow);

    // Update the polyline.
    mymap = mapsPlaceholder.pop();
    mymap.removeLayer(polyline);
    mapsPlaceholder.push(mymap);

    // There is no polyline for the clicked flight.
    flagWasHere = 0;

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

    // Go over each flight.
    flights.forEach(function (item) {

        // Add new marker or update an existing one.
        flightIconHelper(item, mymap);
        
    });
    mapsPlaceholder.push(mymap);
}

/* Adds a new marker or update an existing one. */
function flightIconHelper(item, mymap) {
    if (item.flight_Id === idPressed) {
        planeIcon = planeIconWithShadow;
    } else {
        planeIcon = planeIconInit;
    }
    if (!idToIcon.has(item.flight_Id)) {
        let myMarker = L.marker([item.latitude, item.longitude],
            {
                myCustomId: item.flight_Id,
                icon: planeIcon
            }).on(
                "click", onClickIcon).addTo(mymap);
        idToIcon.set(item.flight_Id, myMarker);
        let listOfLocations = [];
        listOfLocations.push([item.latitude, item.longitude]);
    } else {
        if (item.flight_Id === idPressed) {
            idToIcon.get(item.flight_Id).setIcon(planeIcon);
        }
        idToIcon.get(item.flight_Id).setLatLng(
            [item.latitude, item.longitude]).update();
    }
    idCurPos.set(item.flight_Id, [item.latitude, item.longitude]);
    if (item.flight_Id === idPressed) {
        ShowTrajectoryOfFlight(mymap);
        flagWasHere = 1;
    }
}


/* 
 * Deletes pairs of id-icon whenever 
 * the flight has finished or was deleted.
 */
function maintainMap(mymap) {

    // Keys are ids stored in the map.
    let listOfKeys = idToIcon.keys();

    for (let id of listOfKeys) {

        // The flight has ended(neither in the internal nor external flights).
        if (!idsOfCurrFlights.includes(id) &&
            !idsOfExternalFlights.includes(id)) {

            // Should remove the element with this id from the map.
            mymap.removeLayer(idToIcon.get(id));
            idToIcon.delete(id);

            // When the deleted flights was pressed.
            if (idPressed === id) {
                mymap.removeLayer(polyline);
                idPressed = 0;
                document.getElementById("FlightDetailsBlock").innerHTML = "";
            }
        }
    }
}

/* 
 * The event that is triggered whenever
 * a delete button was clicked in one
 * of the internal flights.
 */
function deleteFlight() {
    deleteClicked = true;

    // The deleted flight is with pressed icon.
    if (idPressed === this.id) {

        document.getElementById("FlightDetailsBlock").innerHTML = "";
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

    if (smnIsPressed === true) {
        idPressedDeleted = idPressed;
    }

    let xhttp = new XMLHttpRequest();

    xhttp.open("DELETE", "/api/Flights/" + this.id, true);
    xhttp.send();
}

/* 
 * The event handler for a marker click.
 * On call would highlight the marker
 * and its corresponding row.
 */
function onClickIcon(e) {
    let id;
    if (idPressed === 0) {
        e.target.setIcon(planeIconWithShadow);
        smnIsPressed = true;
        globalVar = e.target;
        id = this.options.myCustomId;

        // Highlight table row.
        document.getElementById(id).style.backgroundColor = '#FF0000';
    }
    else {
        let prevId = globalVar.options.myCustomId;
        globalVar.setIcon(planeIconInit);
        globalVar = e.target;
        globalVar.setIcon(planeIconWithShadow);
        smnIsPressed = true;

        mymap = mapsPlaceholder.pop();
        mymap.removeLayer(polyline);
        mapsPlaceholder.push(mymap);
        flagWasHere = 0;

        // Update rows highlighting.
        document.getElementById(prevId).style.backgroundColor = originalColor;
        document.getElementById(this.options.myCustomId).
            style.backgroundColor = '#FF0000';
    }
    id = this.options.myCustomId;
    idPressed = id;
    ShowDataOfFlight(id);
}

/* 
 * Shows the path of the whole flight
 * on the map.
 */
async function ShowTrajectoryOfFlight(mymap) {

    var arrLoc = [];

    arrLoc.unshift(pressedStart);

    for (var i = 0; i < sizeArr; i++) {
        let lang = endArrayPath[i][0];
        let long = endArrayPath[i][1];
        arrLoc.push([lang, long]);
    }

    // Path is not showed on map.
    if (flagWasHere === 0) {

        polyline = L.polyline(arrLoc,
            {className: "my_polyline"}).addTo(mymap);

        return;
    } else {

        // Update the map(remove and insert the updated one).
        mymap.removeLayer(polyline);
    }

    polyline = L.polyline(arrLoc,
        {className: "my_polyline"}).addTo(mymap);
    return;
}

/* 
 * Fills up the flight details block
 * as well as preforms the calculation
 * in order to do so.
 */
async function ShowDataOfFlight(id) {
    let flightDetailBlock = document.getElementById("FlightDetailsBlock");
    flightDetailBlock.innerHTML =
        "Your request for flight details is now being procceded";

    let path = "api/Helper/" + id;
    let response = await fetch(path);
    let parsObject = await response.json();

    if (parsObject.numOfPassengers === -2) {
        printFlightDetailsError();
    }
    else {
        printFlightDetails(id, parsObject);
    }

    let pathSegments = parsObject.segmentsPath;
    let lenOfSegments = pathSegments.length;
    pressedStart = [], endArrayPath = [];
    pressedStart = [parsObject.startLatitude, parsObject.startLongitude];

    for (let i = 0; i < lenOfSegments; i++) {
        endArrayPath.push([pathSegments[i].latitude,
        pathSegments[i].longitude]);
    }
    idPressed = id;
    sizeArr = lenOfSegments;

    return;
}

function printFlightDetails(id, parsObject) {

    let flightDetailBlock = document.getElementById("FlightDetailsBlock");

    flightDetailBlock.innerHTML =
        " Company name is: " + parsObject.companyName + "<br>"
        + " Start position is: (longitude is: " + parsObject.startLongitude +
        " and latitude is: "
        + parsObject.startLatitude + ")<br>"
        + " Number of passengers is: " +
        parsObject.numOfPassengers + "<br>"
        + " Starting time is: " + parsObject.takeOffTime + " (according to UTC time)" + "<br>"
        + " End position is: (longitude is: " + parsObject.endLongitude +
        + " and latitude is: " + parsObject.endLatitude + ")" +"<br>"
        + "Land time is: " + parsObject.landTime + "<br>"
        + "Flight id is: " + id;

}

function printFlightDetailsError() {

    let flightDetailBlock = document.getElementById("FlightDetailsBlock");

    flightDetailBlock.innerHTML =
        "We are not able to create trajectory of flight!!"
        + "There is some problem with receiving"
        + "flight plan data from external service."
        + "There is a chance that external server is under congestion"
        + "Please press another icon";
}
