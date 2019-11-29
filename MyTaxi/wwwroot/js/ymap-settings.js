//TODO: Реализовать добавление в базу поездки.

let myMap,
    multiRoute,
    allIsCorrect = false,
    currentBaseSumm,
    currentLengthRoute,
    currentTimeRoute,
    currentMainSummRoute;

let inputs = new Array();
inputs[0] = document.getElementById("suggest1");
inputs[1] = document.getElementById("suggest2");

refresh_onchange();

let suggest_number = 2,
    suggestViews = new Array(), // Подсказки для input от Yandex API
    current_input_clone = document.getElementById("suggest2").cloneNode(true),
    add_input_btn = document.getElementById("add_input_btn"),
    add_route_btn = document.getElementById("create_route"),
    car_class_name = document.getElementById("carClassName");

current_input_clone.id = `${suggest_number + 1}`;

add_input_btn.addEventListener("click", function () {
    add_input_btn.before(current_input_clone);
    inputs[inputs.length] = current_input_clone;
    refresh_onchange();

    suggestViews[suggestViews.length] = new ymaps.SuggestView(current_input_clone.id);
    current_input_clone = document.getElementById(`${suggest_number + 1}`).cloneNode(true);
    suggest_number++;
    current_input_clone.id = `${suggest_number + 1}`;
}, false);

add_route_btn.addEventListener("click", function () {
    init_route(inputs);
}, false);

car_class_name.addEventListener("change", function () {

    var dataValue = { name: car_class_name.value }
    let summField = document.getElementById("mainSumm");

    //TODO: Добавить автоматический перерасчет стоимости при изменении тарифа.

    $.ajax({
        type: "POST",
        url: "/Order/GetBaseSumm",
        data: dataValue,
        //contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            if (result.resultValue != "Error") {
                if (currentLengthRoute != null) {
                    currentMainSummRoute = parseInt(result.resultValue, 10) + currentLengthRoute * 5;
                    summField.innerHTML = `Сумма поездки: ${currentMainSummRoute}₽`;
                }
                else {
                    summField.innerHTML = `Сумма поездки: от ${result.resultValue}₽`;
                }
                currentBaseSumm = parseInt(result.resultValue, 10);
            }
            else {
                summField.innerHTML = "MyTaxi не может предоставить выбранный тариф";
                currentBaseSumm = null;
            }
            console.log(result.resultValue);
        }
    });

}, false);

ymaps.ready(function () {
    myMap = new ymaps.Map("map", {
        center: [48.4861, 135.0797],
        zoom: 12,
        controls: ["zoomControl"],
        behaviors: ["drag"]
    });

    suggestViews[0] = new ymaps.SuggestView("suggest1");
    suggestViews[1] = new ymaps.SuggestView("suggest2");
});

//Валидация адреса
function geocode(elem) {
    // Забираем запрос из поля ввода.
    var request = $(`#${elem.id}`).val();
    // Геокодируем введённые данные.
    ymaps.geocode(request).then(function (res) {
        var obj = res.geoObjects.get(0),
            error, error_help;

        if (obj) {
            switch (obj.properties.get('metaDataProperty.GeocoderMetaData.precision')) {
                case 'exact':
                    break;
                case 'number':
                case 'near':
                case 'range':
                    error = 'Неточный адрес, требуется уточнение';
                    error_help = 'Уточните номер дома';
                    break;
                case 'street':
                    error = 'Неполный адрес, требуется уточнение';
                    error_help = 'Уточните номер дома';
                    break;
                case 'other':
                default:
                    error = 'Неточный адрес, требуется уточнение';
                    error_help = 'Уточните адрес';
            }
        } else {
            error = 'Адрес не найден';
            error_help = 'Уточните адрес';
        }
        // Если геокодер возвращает пустой массив или неточный результат, то показываем ошибку.
        if (error) {
            let warning_msg = document.createElement('p');
            warning_msg.className = "error_message";
            warning_msg.innerHTML = `${error}. ${error_help}!`;
            elem.after(warning_msg);
            elem.style.border = "1.5px solid red";
            allIsCorrect = false;
        } else {
            let temp_elem = elem.nextSibling;
            while (temp_elem.nodeName == "P") {
                let temp = temp_elem.nextSibling;
                temp_elem.remove();
                temp_elem = temp;
            }
            elem.style.border = "1.5px solid gold";
            allIsCorrect = true;
        }
    }, function (e) {
        console.log(e)
    })
};

//Вешаем каждому элементу массива обработчик события onblur для валидации адреса
function refresh_onchange() {
    inputs.forEach(element => {
        if (element.onblur) {
            return;
        }
        else {
            element.onblur = function () {
                if (element.value != "") {
                    geocode(element);
                }
            };
        }
    });
};

//Запрещаем по нажатию Enter генерировать Submit 
$(function () {
    $('input[name="test1"]').keypress(function (event) {
        if (event.which == '13') {
            event.preventDefault();
        }
    })
});

//Создание маршрута
function init_route(elements_with_streets) {
    var myReferncePoints = new Array();

    elements_with_streets.forEach(element => {
        if (element.value) {
            myReferncePoints[myReferncePoints.length] = element.value;
        }
    });

    if (myReferncePoints.length > 1 && allIsCorrect) {
        if (multiRoute) {
            myMap.geoObjects.remove(multiRoute);
            multiRoute = null;
            currentLengthRoute = null;
            currentTimeRoute = null;
            tempLenghtRoute = null;
        }
        multiRoute = new ymaps.multiRouter.MultiRoute({
            referencePoints: myReferncePoints,
            params: {
                //avoidTrafficJams: true, учесть пробки
                routingMode: "auto"
            }
        }, {
            routeStrokeStyle: "none",
            routeActiveStrokeStyle: "solid",
            routeActiveStrokeColor: "#2b50c8",
            boundsAutoApply: true //Атвомат. границы, для отображения всего маршрута
        });

        myMap.geoObjects.add(multiRoute);

        multiRoute.model.events.add('requestsuccess', function () {
            var activeRoute = multiRoute.getActiveRoute();
            currentLengthRoute = Number(activeRoute.properties.get("distance").text.replace(/\D+/g, ""))
            currentTimeRoute = activeRoute.properties.get("duration").text;

            let summField = document.getElementById("mainSumm");

            if (currentBaseSumm) {
                if (document.getElementById("info_message")) {
                    document.getElementById("info_message").innerHTML = `Время поездки: ~ ${currentTimeRoute}`;
                }
                else {
                    let time_info = document.createElement('p');
                    time_info.className = "info_message";
                    time_info.id = "info_message";
                    time_info.innerHTML = `Время поездки: ~ ${currentTimeRoute}`;
                    document.getElementById("submitBtn").before(time_info);
                }
                currentMainSummRoute = currentBaseSumm + currentLengthRoute * 5;
                summField.innerHTML = `Сумма поездки: ${currentMainSummRoute}₽`;
            }
            else {
                summField.innerHTML = "Тариф не выбран или он не соответсвует действующим тарифам!";
            }

        });
    }
    else {
        alert("Недостаточно точек для построения маршрута (необходимо минимум 2)! Или адреса указаны с ошибкой!");
    }
};

function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
};
