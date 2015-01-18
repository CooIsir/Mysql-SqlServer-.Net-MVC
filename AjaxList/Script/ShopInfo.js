function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
$(document).ready(function () {

    var id = getQueryString("id");
    if (id != null) {
        $.post("../AjaxCom/Handler.ashx?cmd=ShopInfo", { "id": id }, function (data) {              //post请求地址，加请求信息
            if (data != null) {
                var list = document.getElementById("listCon");
               
                list.innerHTML = data;                                                              //显示数据
            
            }
        });
    }

});