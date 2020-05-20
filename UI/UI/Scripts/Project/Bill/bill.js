'use strict'

layui.config({
    base: '/Scripts/Project/Bill/' //静态资源所在路径
}).use(['form', 'table'], function () {
    var form = layui.form
        , table = layui.table;

    //账单数据表格
    table.render({
        elem: '#table_bill',
        height: 600,
        width: 1500,
        url: '/Bill/GetBill', //数据接口
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "编号", sort: "true" },
            { field: "Type", title: "费用类型", sort: "true" },
            { field: "PatientName", title: "患者名" },
            { field: "Time", title: "时间", sort: "true"},
            { field: "Cost", title: "费用" },
            { field: "Status", title: "状态" ,sort: "true", templet: '#statusbar'},
            { fixed: 'right', align: 'center', toolbar: '#toolbar', width: 200 }
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });

    //监听表格工具栏
    table.on('tool(table_bill)', function (obj) {
        var data = obj.data;
        var recordId = data.RecordId;
        if (obj.event === 'print') {
            layer.confirm('确认生成收据?', function (index) {
                layer.close(index);
                window.open('/Bill/Print?recordId=' + recordId + '&type=' + data.Type);
            });
        }
        else if (obj.event === 'cancel') {
            layer.confirm('确认对该收据进行作废处理?', function (index) {
                $.getJSON('/Bill/Cancel', { recordId: recordId, type: data.Type }, function (res) {
                    if (res.code === 200) {
                        layer.alert("作废成功!", function success() {
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
        table.reload('table_bill', {
            where: { search: $('#input').val() }
        });
    });

    //监听统计报表按钮
    window.btn_exportExcel = function () {
        layer.confirm('确认输出当前财务报表?', function (index) {
            layer.close(index);
            window.open('/Bill/ExportExcel');
        });
    }
});