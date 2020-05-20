'use strict'

// 初始化信息
window.onload = function () {
    $.ajax({
        type: "get",
        url: "/Home/ReadPageState",
        success: function (res) {
            if (res.code === 200) {
                $('#inCount').text(res.inCount);
                $('#outCount').text(res.outCount);
                $('#freeBed').text(res.freeBed);
            } else {
                layer.msg("账号异常，请联系系统管理员！");
                location.href = "/Login/Login";
            }
        }
    });
};