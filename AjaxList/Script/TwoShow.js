$(document).ready(function () {


    $.post("../AjaxCom/Handler.ashx?cmd=listShow", function (data) {
        if (data != null) {
            var list = document.getElementById("listCon");
            list.appendChild = "<a>sdfs</a>";
            list.innerHTML = data;
            //  alert(data);
        }
    });


});