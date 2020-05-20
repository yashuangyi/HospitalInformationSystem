'use strict'

layui.config({
    base: '/Scripts/Project/Log/' //静态资源所在路径
}).use(['form', 'table'], function () {
    var form = layui.form
        , table = layui.table;

    //运行日志数据表格
    table.render({
        elem: '#table_log',
        height: 600,
        width: 1500,
        url: '/Log/GetLog', //数据接口
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "编号", sort: "true" },
            { field: "Time", title: "时间", sort: "true"  },
            { field: "Content", title: "内容" },
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });

    //监听查询按钮
    $("#search").click(function () {
        table.reload('table_log', {
            where: { search: $('#input').val() }
        });
    });
});