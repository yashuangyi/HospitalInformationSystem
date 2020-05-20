'use strict'

layui.config({
    base: '/Scripts/Project/Bed/' //静态资源所在路径
}).use(['form', 'table'], function () {
    var form = layui.form
        , table = layui.table;

    //临床科室数据表格
    table.render({
        elem: '#table_bed',
        height: 600,
        width: 1500,
        url: '/Bed/GetBed', //数据接口
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "编号", sort: "true" },
            { field: "Name", title: "床名" },
            { field: "DepartmentName", title: "所属科室名" },
            { field: "IsFree", title: "状态", sort: "true", templet: '#statusbar'},
            { field: "PatientName", title: "患者名" },
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });

    //监听查询按钮
    $("#search").click(function () {
        table.reload('table_bed', {
            where: { search: $('#input').val() }
        });
    });
});