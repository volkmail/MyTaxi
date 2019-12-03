let currentStatuses = new Map(),
    currentAddressesFields = document.getElementsByClassName("trip-body__routes__list__address"),
    currentAddresses = new Array(),
    changeStatusBtn = document.getElementById("change_status_btn"),
    currentStatusKey = "3";


document.addEventListener("DOMContentLoaded", function (event) {
    let statusesId = Cookies.get('Statuses');
    Cookies.remove('Statuses', { path: '/', domain: 'localhost' })
    let statusesArray = statusesId.split("1");

    //В цикле заполняем статусы, которых еще нет в заказе
    for (var i = 0; i < statusesArray.length; i++) {
        let tempKeyValue = statusesArray[i].split("0");
        if (tempKeyValue.length == 2) {
            currentStatuses.set(tempKeyValue[0], tempKeyValue[1]);
        }
    }

    changeStatusBtn.value = currentStatuses.get(currentStatusKey);
});

changeStatusBtn.addEventListener("click", function () {

}, false);

ymaps.ready(function () {

    //В цике заполняем список адресов для отображения маршрута
    for (var i = 0; i < currentAddressesFields.length; i++) {
        if (currentAddressesFields[i].innerHTML) {
            currentAddresses[i] = currentAddressesFields[i].innerHTML.substring(3, currentAddressesFields[i].innerHTML.length - 1);
        }
    }

    console.log(currentAddresses);

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