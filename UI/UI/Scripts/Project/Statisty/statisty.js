//echarts饼图，需要注意先让DOM元素加载出来
$(document).ready(function () {
    var chart = echarts.init($('#echarts').get(0));
    var data1 = [];
    var data2 = [];

    var option = {
        tooltip: {
            trigger: 'item',
            formatter: '{a} <br/>{b}: {c} ({d}%)'
        },
        legend: {
            orient: 'vertical',
            left: 10,
            data: data1
        },
        series: [
            {
                name: '各科医疗费用情况',
                type: 'pie',
                radius: ['50%', '70%'],
                avoidLabelOverlap: false,
                label: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    label: {
                        show: true,
                        fontSize: '30',
                        fontWeight: 'bold'
                    }
                },
                labelLine: {
                    show: false
                },
                data: data2
            }
        ]
    };
    $.get("/Statisty/GetEchartsOne", null,
        function (res) {
            if (res.code === 200) {
                for (let i = 0; i < res.count; i++) {
                    data1.push(res.data[i].name)
                    data2.push(res.data[i]);
                }
                chart.setOption(option);
            }
        },
        "json"
    );

    var chart2 = echarts.init($('#echarts2').get(0));
    var data3 = [];
    var data4 = [];
    var data5 = [];

    var option2 = {
        title: {
            text: '各科床位使用情况',
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['可用床位', '已用床位']
        },
        toolbox: {
            show: true,
            feature: {
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                data: data3
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '可用床位',
                type: 'bar',
                data: data4,
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                }
            },
            {
                name: '已用床位',
                type: 'bar',
                data: data5,
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                }
            }
        ]
    };
    $.get("/Statisty/GetEchartsTwo", null,
        function (res) {
            if (res.code === 200) {
                for (let i = 0; i < res.count; i++) {
                    data3.push(res.data[i].name)
                    data4.push(res.data[i].value1);
                    data5.push(res.data[i].value2);
                }
                chart2.setOption(option2);
            }
        },
        "json"
    );
});