'use strict'

layui.config({
    base: '/Scripts/Project/Department/' //静态资源所在路径
}).use(['form', 'table'], function () {
    var form = layui.form
        , table = layui.table;

    //临床科室数据表格
    table.render({
        elem: '#table_department',
        height: 600,
        width: 1500,
        url: '/Department/GetDepartment', //数据接口
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "科室编号" },
            { field: "Name", title: "科室名" },
            { field: "FreeDivTotal", title: "可用床位/总床位" },
            { field: "Cost", title: "床位费" },
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });

    //监听"新增科室"按钮
    window.btn_addDepartment = function () {
        $('#freeBedNum').val(0);
        $('#departmentId').val(0);
        layer.open({
            type: 1, //页面层
            title: "新增科室",
            area: ['500px', '300px'],
            btn: ['保存', '取消'],
            btnAlign: 'c', //按钮居中
            content: $('#div_addDepartment'),
            success: function (layero, index) {// 弹出layer后的回调函数,参数分别为当前层DOM对象以及当前层索引
                // 解决按回车键重复弹窗问题
                $(':focus').blur();
                // 为当前DOM对象添加form标志
                layero.addClass('layui-form');
                // 将保存按钮赋予submit属性
                layero.find('.layui-layer-btn0').attr({
                    'lay-filter': 'btn_saveAdd',
                    'lay-submit': ''
                });
                // 表单验证
                form.verify();
                // 刷新渲染(否则开关按钮不会显示)
                form.render();
            },
            yes: function (index, layero) {// 确认按钮回调函数,参数分别为当前层索引以及当前层DOM对象
                form.on('submit(btn_saveAdd)', function (data) {//data按name获取
                    if ($('#bedNum').val() <= 0) {
                        layer.alert("床位数必须大于0！");
                        return false;
                    }
                    $.ajax({
                        type: 'post',
                        url: '/Department/AddDepartment',
                        dataType: 'json',
                        data: data.field,
                        success: function (res) {
                            if (res.code === 200) {
                                layer.alert("新增科室及其床位成功!", function (index) {
                                    window.location.reload();
                                });
                            } else {
                                layer.alert(res.msg);
                            }
                        }
                    });
                });
            }
        });
    }

    //监听查询按钮
    $("#search").click(function () {
        table.reload('table_department', {
            where: { search: $('#input').val()}
        });
    });
});