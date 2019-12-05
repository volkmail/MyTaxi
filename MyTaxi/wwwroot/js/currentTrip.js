let currentStatuses = new Map(),
    currentAddressesFields = document.getElementsByClassName("trip-body__routes__list__address"),
    currentAddresses = new Array(),
    changeStatusBtn = document.getElementById("change_status_btn"),
    nextStatusKey,
    nextRoadStatusKey = 5,
    changeRoadStatusBtn;


document.addEventListener("DOMContentLoaded", function (event) {
    let statusesId = Cookies.get('Statuses');
    Cookies.remove('Statuses', { path: '/', domain: 'localhost' })
    let statusesArray = statusesId.split("1");
    let firstRound = true;

    //В цикле заполняем статусы, которых еще нет в заказе
    for (var i = 0; i < statusesArray.length; i++) {
        let tempKeyValue = statusesArray[i].split("0");
        if (tempKeyValue.length == 2) {
            if (firstRound) {
                nextStatusKey = parseInt(tempKeyValue[0], 10);
                firstRound = false;
            }
            currentStatuses.set(tempKeyValue[0], tempKeyValue[1]);
        }
    }

    changeStatusBtn.value = currentStatuses.get(nextStatusKey.toString());
});

changeStatusBtn.addEventListener("click", function () {

    dataValue = {
        orderId: Number(document.getElementById("OrderID").innerHTML.replace(/\D+/g, "")),
        statusId: nextStatusKey > 4 && nextStatusKey < 7 ? nextRoadStatusKey : nextStatusKey
    }

    $.ajax({
        type: "POST",
        url: "/Trips/SetStatus",
        data: dataValue,
        dataType: 'text',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            if (result == "Success") {
                nextStatusKey++;
                if (nextStatusKey == 8) {
                    window.location = "/Trips/AwaitingTrips";
                }
                if (nextStatusKey == 5) {
                    nextStatusKey = 7;
                    changeStatusBtn.value = currentStatuses.get(nextStatusKey.toString());

                    changeRoadStatusBtn = document.createElement('input');
                    changeRoadStatusBtn.type = "button";
                    changeRoadStatusBtn.className = "trip-body__block-info__form__btn";
                    changeRoadStatusBtn.id = "change_status__in-road__btn";
                    changeRoadStatusBtn.value = currentStatuses.get(nextRoadStatusKey.toString());
                    changeRoadStatusBtn.addEventListener("click", CRSBHandler, false);

                    changeStatusBtn.before(changeRoadStatusBtn);
                }
                else {
                    changeStatusBtn.value = currentStatuses.get(nextStatusKey.toString());
                }
            }
        }
    });
}, false);

ymaps.ready(function () {

    //В цике заполняем список адресов для отображения маршрута
    for (var i = 0; i < currentAddressesFields.length; i++) {
        if (currentAddressesFields[i].innerHTML) {
            currentAddresses[i] = currentAddressesFields[i].innerHTML.substring(3, currentAddressesFields[i].innerHTML.length - 1);
        }
    }

    myMap = new ymaps.Map("map", {
        center: [48.4861, 135.0797],
        zoom: 12,
        controls: ["zoomControl"],
        behaviors: ["drag"]
    });

    multiRoute = new ymaps.multiRouter.MultiRoute({
        referencePoints: currentAddresses,
        params: {
            routingMode: "auto"
        }
    }, {
        routeStrokeStyle: "none",
        routeActiveStrokeStyle: "solid",
        routeActiveStrokeColor: "#2b50c8",
        boundsAutoApply: true
    });

    myMap.geoObjects.add(multiRoute);
});

function CRSBHandler() {
    dataValue = {
        orderId: Number(document.getElementById("OrderID").innerHTML.replace(/\D+/g, "")),
        statusId: nextRoadStatusKey
    }

    $.ajax({
        type: "POST",
        url: "/Trips/SetStatus",
        data: dataValue,
        dataType: 'text',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            if (nextRoadStatusKey == 6) {
                nextRoadStatusKey--;
                changeRoadStatusBtn.value = currentStatuses.get(nextRoadStatusKey.toString());
            }
            else {
                nextRoadStatusKey++;
                changeRoadStatusBtn.value = currentStatuses.get(nextRoadStatusKey.toString())
            }
        }
    });
}