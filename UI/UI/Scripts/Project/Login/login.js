'use strict'

layui.config({
    base: '/Scripts/Project/Login/'
}).use(['layer', 'form'], function () {
    var layer = layui.layer
        ,form = layui.form;

    //监听登录按钮
    $("#btn_login").click(function () {
        $.ajax({
            url: "/Login/Check",
            dataType: "json",
            type: "post",
            data: {
                account: $('#account').val(),
                password: $('#password').val(),
            },
            success: function (res) {
                if (res.code === 200) {
                    layer.open({
                        title: '欢迎管理员~'
                        , content: '登录成功!'
                        , end: function () {
                            location.href = "/Home/Home";
                        }
                    });
                }
                else if (res.code === 300) {
                    layer.open({
                        title: '家属您好~'
                        , content: '登录成功!'
                        , end: function () {
                            location.href = "/Home/Parent";
                        }
                    });
                }
                else if (res.code === 401) {
                    layer.open({
                        title: 'Fail'
                        , content: '请输入账号及密码!'
                    });
                }
                else if (res.code === 404) {
                    layer.open({
                        title: 'Fail'
                        , content: '账号或密码错误，请重新输入!'
                    });
                    $('#password').val("");
                }
            },
        });
    });

    //监听“回车键”
    $(document).keyup(function (event) {
        if (event.keyCode === 13) {
            $("#btn_login").click();
        }
    });
});