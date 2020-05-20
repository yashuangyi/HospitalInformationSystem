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
        url: '/Bill/GetParentBill', //数据接口
        where: { userId: $("#user_id", parent.document).val() },
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "编号", sort: "true" },
            { field: "Type", title: "费用类型", sort: "true" },
            { field: "PatientName", title: "患者名" },
            { field: "Time", title: "时间", sort: "true" },
            { field: "Cost", title: "费用" },
            { field: "Status", title: "状态", sort: "true", templet: '#statusbar' },
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });
});