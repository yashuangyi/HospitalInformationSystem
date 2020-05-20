'use strict'

layui.config({
    base: '/Scripts/Project/Record/' //静态资源所在路径
}).use(['form', 'table'], function () {
    var form = layui.form
        , table = layui.table;

    //病历数据表格
    table.render({
        elem: '#table_record',
        height: 600,
        width: 1500,
        url: '/Record/GetRecord', //数据接口
        page: true,//开启分页
        cols: [[
            { field: "Id", title: "病历号", sort: "true"},
            { field: "Name", title: "患者名" },
            { field: "Sex", title: "性别" },
            { field: "Age", title: "年龄" },
            { field: "Phone", title: "联系方式", width: 150 },
            { field: "Department", title: "临床科室名", width: 100  },
            { field: "Bed", title: "床位名", width: 150 },
            { field: "InTime", title: "入院时间", sort: "true", width: 170 },
            { field: "OutTime", title: "出院时间", sort: "true", width: 170 },
            { field: "Status", title: "状态", sort: "true", templet: '#statusbar', width: 135 },
            { fixed: 'right', align: 'center', toolbar: '#toolbar', width: 250 }
        ]],
        text: {
            none: '查无记录~' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
    });

    //监听科室选择
    form.on('select(department)', function (data) {
        $("#bed").find("option").not(":first").remove();
        $.ajax({
            type: 'post',
            url: '/Bed/ShowBedChoice',
            dataType: 'json',
            data: { department: data.value },
            success: function (res) {
                if (res.code === 200) {
                    $.each(res.choice, function (index, choice) {
                        $('#bed').append(new Option(choice));//往下拉菜单中添加元素
                    });
                    $('#medicalCost').val(res.cost);
                    form.render();//菜单渲染,加载内容
                } else {
                    layer.alert("当前选择的科室已无空闲床位！");
                }
            }
        });
    });
    form.on('select(change_department)', function (data) {
        $("#change_bed").find("option").not(":first").remove();
        $.ajax({
            type: 'post',
            url: '/Bed/ShowBedChoice',
            dataType: 'json',
            data: { department: data.value },
            success: function (res) {
                if (res.code === 200) {
                    $.each(res.choice, function (index, choice) {
                        $('#change_bed').append(new Option(choice));//往下拉菜单中添加元素
                    });
                    form.render();//菜单渲染,加载内容
                } else {
                    layer.alert("当前选择的科室已无空闲床位！");
                }
            }
        });
    });

    //监听"入院登记"按钮
    window.btn_addRecord = function () {
        $('#bedId').val(0);
        $('#recordId').val(0);
        $('#departmentId').val(0);
        $('#status').val("待付押金");
        $('#inTime').val("");
        $('#outTime').val("");
        $('#account').val("");
        $('#password').val("");

        //加载科室选择框选项
        $.getJSON('/Department/ShowDepartmentChoice', function (data) {
            $("#department").find("option").not(":first").remove();
            $("#bed").find("option").not(":first").remove();
            if (data.code === 200) {
                $.each(data.choice, function (index, choice) {
                    $('#department').append(new Option(choice));//往下拉菜单中添加元素
                });
                form.render();//菜单渲染,加载内容
            } else {
                layer.alert("请先新增临床科室！");
            }
        });

        layer.open({
            type: 1, //页面层
            title: "入院登记",
            area: ['500px', '600px'],
            btn: ['保存', '取消'],
            btnAlign: 'c', //按钮居中
            content: $('#div_addRecord'),
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
                    if ($('#age').val() < 0) {
                        layer.alert("年龄输入不合法，请重新输入！");
                        return false;
                    }
                    $.ajax({
                        type: 'post',
                        url: '/Record/AddRecord',
                        dataType: 'json',
                        data: data.field,
                        success: function (res) {
                            if (res.code === 200) {
                                layer.alert("入院登记成功，请到结算中心结算费用！", function (index) {
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

    //监听表格工具栏
    table.on('tool(table_record)', function (obj) {
        var data = obj.data;
        if (obj.event === 'change') {
            //加载科室选择框选项
            $.getJSON('/Department/ShowDepartmentChoice', function (data) {
                $("#change_department").find("option").not(":first").remove();
                $("#change_bed").find("option").not(":first").remove();
                if (data.code === 200) {
                    $.each(data.choice, function (index, choice) {
                        $('#change_department').append(new Option(choice));//往下拉菜单中添加元素
                    });
                    form.render();//菜单渲染,加载内容
                } else {
                    layer.alert("请先新增临床科室！");
                }
            });
            var recordId = data.Id;
            layer.open({
                type: 1, //页面层
                title: "转科调床",
                area: ['400px', '400px'],
                btn: ['确认', '且慢'],
                btnAlign: 'c', //按钮居中
                content: $('#div_change'),
                success: function (layero, index) {// 弹出layer后的回调函数,参数分别为当前层DOM对象以及当前层索引
                    // 解决按回车键重复弹窗问题
                    $(':focus').blur();
                    // 为当前DOM对象添加form标志
                    layero.addClass('layui-form');
                    // 将保存按钮赋予submit属性
                    layero.find('.layui-layer-btn0').attr({
                        'lay-filter': 'btn_select',
                        'lay-submit': ''
                    });
                    // 表单验证
                    form.verify();
                    // 刷新渲染(否则开关按钮不会显示)
                    form.render();
                },
                yes: function (index, layero) {// 确认按钮回调函数,参数分别为当前层索引以及当前层DOM对象
                    form.on('submit(btn_select)', function (data) {//data按name获取
                        $.ajax({
                            type: 'post',
                            url: '/Record/Change',
                            dataType: 'json',
                            data: { recordId: recordId, bed: $("#change_bed").val(), department: $("#change_department").val()},
                            success: function (res) {
                                if (res.code === 200) {
                                    layer.alert("转科调床操作成功！", function (index) {
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
        else if (obj.event === 'out') {
            layer.confirm('确认登记出院?', function (index) {
                $.getJSON('/Record/Out', { recordId: data.Id }, function (res) {
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
        else if (obj.event === 'print') {
            var recordId = data.Id;
            layer.confirm('确认生成病案首页?', function (index) {
                layer.close(index);
                window.open('/Record/Print?recordId=' + recordId);
            });
        }
    });

    //监听查询按钮
    $("#search").click(function () {
        table.reload('table_record', {
            where: { search: $('#input').val() }
        });
    });
});