'use strict'

layui.config({
    base: '/Scripts/Project/Settle/' //静态资源所在路径
}).use(['form', 'table'], function () {
    var form = layui.form
        , table = layui.table;

    //结算数据表格
    table.render({
        elem: '#table_settle',
        height: 600,
        width: 1500,
        url: '/Settle/GetSettle', //数据接口
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "编号", sort: "true" },
            { field: "Type", title: "费用类型", sort: "true" },
            { field: "PatientName", title: "患者名" },
            { field: "Time", title: "时间", sort: "true" },
            { field: "Cost", title: "费用" },
            { fixed: 'right', align: 'center', toolbar: '#toolbar', width: 300 }
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });

    //监听表格工具栏
    table.on('tool(table_settle)', function (obj) {
        var data = obj.data;
        var recordId = data.RecordId;
        if (obj.event === 'pay') {
            layer.confirm('确认登记结账?', function (index) {
                $.getJSON('/Settle/Pay', { recordId: recordId, type: data.Type }, function (res) {
                    if (res.code === 200) {
                        layer.alert("登记完成!", function success() {
                            window.location.reload();
                        });
                    }
                    else {
                        layer.alert("登记失败，请联系代码小哥!");
                    }
                });
            });
        }
        else if (obj.event === 'press') {
            layer.confirm('确认进行催款操作?', function (index) {
                $.getJSON('/Settle/Press', { recordId: recordId, type: data.Type }, function (res) {
                    if (res.code === 200) {
                        layer.alert("已向其家属发送催款消息!", function success() {
                            window.location.reload();
                        });
                    }
                    else {
                        layer.alert("登记失败，请联系代码小哥!");
                    }
                });
            });
        }
        else if (obj.event === 'arrears') {
            layer.confirm('确认进行欠款处理操作?', function (index) {
                $.getJSON('/Settle/Arrears', { recordId: recordId, type: data.Type}, function (res) {
                    if (res.code === 200) {
                        layer.alert("已向其家属发送欠款处理相关信息!", function success() {
                            window.location.reload();
                        });
                    }
                    else {
                        layer.alert("登记失败，请联系代码小哥!");
                    }
                });
            });
        }
    });

    //监听查询按钮
    $("#search").click(function () {
        table.reload('table_settle', {
            where: { search: $('#input').val() }
        });
    });
});