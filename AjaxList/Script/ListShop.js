$(document).ready(function () {

    $("#btnSend").click(function() {
        $.post("../AjaxCom/Handler.ashx?cmd=listShow", function(data) { //post请求数据
            if (data != null) {
                alert(data);
                var list = document.getElementById("listCon"); //获取数据显示div ID；        
                list.innerHTML = data; //显示数据
                alert(data);
            }
        });
    });


});