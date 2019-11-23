var un = document.getElementById("userName");
un.onblur = function () {
    this.style.border = "none";
    this.style.borderBottom = "1px solid #000";  
};

var us = document.getElementById("userSurname");
us.onblur = function () {
    this.style.border = "none";
    this.style.borderBottom = "1px solid #000";
};

var up = document.getElementById("userPatronymic");
up.onblur = function () {
    this.style.border = "none";
    this.style.borderBottom = "1px solid #000";
};

function validation() {
    var password = document.getElementById("passwordField"),
        passwordAgain = document.getElementById("passwordFieldAgain");

    var nsp = [document.getElementById("userSurname"),
                document.getElementById("userName"), document.getElementById("userPatronymic")];

    for (let val of nsp) {
        if (!val.value) {
            val.style.border = "2px solid red";
            alert("Не все поля заполнены!");
            return false;
        }
        for (let simbol of val.value) {
            if (TryParseInt(simbol, 10) != 10) {
                val.style.border = "2px solid red";
                alert("Фамилия, имя или отчество не может содержать цифр!");
                return false;
            }
        }
    }
    if (password != passwordAgain) {
        alert("Пароль и подтверждение пароля не совпадают!");
        password.style.border = "2px solid red";
        passwordAgain.style.border = "2px solid red";
        return false;
    }
    return true;
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