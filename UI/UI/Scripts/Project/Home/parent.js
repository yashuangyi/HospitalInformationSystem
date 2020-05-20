'use strict'

// 初始化信息
window.onload = function () {
    if ($('#user_id').val() !== "") {
        $.ajax({
            type: "post",
            url: "/Home/ReadState",
            data: { userId: $('#user_id').val() },
            dataType: "json",
            success: function (res) {
                if (res.code === 200) {
                    $('#user_name').text(res.userName);
                    $('#user_power').val(res.userPower);
                }
                else if (res.code === 202) {
                    console.log(res);
                    $('#user_name').text(res.userName);
                    $('#user_power').val(res.userPower);
                    layer.alert(res.message);
                }
                else {
                    layer.msg("账号异常，请联系系统管理员！");
                    location.href = "/Login/Login";
                }
            }
        });
    } else {
        layer.msg("请重新登录！");
        location.href = "/Login/Login";
    }
};