var assetChart = null;
var riskChart = null;
var szChart = null;
var sz50Chart = null;
var timeArr = [];
var assetDataArray = [];
var assetMax = 0;
var assetMin = 200;

var riskDataArray = [];
var riskMax = 0;
var riskMin = 100;

var rateDataArray = [];

var szDataArray = [];
var szMax = 0;
var szMin = 8000;

var sz50DataArray = [];
var sz50Max = 0;
var sz50Min = 10;

var dotNum = 20;

var orginalType = -1;
var orgianleAsset = 53;
function loadData(dotNum, type) {
    var now = new Date();
    document.getElementById('idtime').innerHTML = getDateStr(now) + ' ' + getWeekStr(now);
    assetChart = echarts.init(document.getElementById('divasset'));

    riskChart = echarts.init(document.getElementById('divrisk'));

    szChart = echarts.init(document.getElementById('divsz'));

    sz50Chart = echarts.init(document.getElementById('divsz50'));

    showView(dotNum, type)
}

function showView(dotNum, type) {
    var isShowLabel = dotNum > 0;
    if (orginalType == type) {
        return;
    }
    orginalType = type;
    if (dotNum == 0) {
        info = dayinfo;
        if (type == 1) {
            info = weekinfo;
        }
        if (type == 2) {
            info = monthinfo;
        }
        dotNum = info.length;
    }

    var lis = document.getElementById("ulvwtype").childNodes;
    for (var i = 0; i < lis.length; i++) {
        if (lis[i].tagName && lis[i].tagName.toLowerCase() == "li")
            lis[i].removeAttribute("class");
    }
    document.getElementById("li" + type).className = "highlight";

    assetDataArray.splice(0, assetDataArray.length);
    timeArr.splice(0, timeArr.length);
    rateDataArray.splice(0, rateDataArray.length);
    sz50DataArray.splice(0, sz50DataArray.length);
    szDataArray.splice(0, szDataArray.length);
    riskDataArray.splice(0, riskDataArray.length);
    assetMax = 0;
    assetMin = 200;
    riskMax = 0;
    riskMin = 100;
    szMax = 0;
    szMin = 8000;
    sz50Max = 0;
    sz50Min = 10;
    var info = dayinfo;
    if (type == 1)
        info = weekinfo;
    if (type == 2)
        info = monthinfo;

    ///资产 start

    for (var i = 0; i < info.length; i++) {
        if (i < dotNum) {
            timeArr.push(info[i].t);
            var m_asset = info[i].asset / 10000.0;
            m_asset = Number(m_asset.toFixed(2));
            assetDataArray.push(m_asset);
            if (assetMax < m_asset)
                assetMax = m_asset;
            if (assetMin > m_asset)
                assetMin = m_asset;
        }
    }
    if (orgianleAsset < assetMin)
        assetMin = orgianleAsset;
    assetMax = Math.ceil(assetMax + 0.5);
    assetMin = Math.floor(assetMin - 0.5);

    timeArr = timeArr.reverse();
    assetDataArray = assetDataArray.reverse();

    var assetOpt = {
        title: {
            text: '资产',
            subtext: '',
            textStyle: { color: "#277eab" }

        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['资产'],
            show: false
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        calculable: true,
        xAxis: [
            {
                type: 'category', 
                data: timeArr 
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value}万'
                }, max: assetMax,
                min: assetMin
            }
        ],
        series: [
            {
                name: '资产',
                type: 'line',
                data: assetDataArray,
                label: {
                    normal: {
                        show: false,
                        formatter: '{c}万'
                    }
                },
                markLine: {
                    data: [
                        {
                            name: '初始资产53万',
                            yAxis: 53
                        } 
     
                    ],
                    lineStyle:
                        {
                            normal: { type: 'solid',width:2 }
                        },
                    label: { normal: { position: 'start' } }
                },
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                }

            }
        ]
    };
    assetChart.setOption(assetOpt);

    //资产end

    ///风险度 start


    for (var i = 0; i < info.length; i++) {
        if (i < dotNum) {
            var m_risk = info[i].risk;
            riskDataArray.push(m_risk);
            var m_annual = info[i].annualrate;
            rateDataArray.push(m_annual);
            if (riskMax < m_risk)
                riskMax = m_risk;

            if (riskMin > m_risk)
                riskMin = m_risk;

            if (riskMax < m_annual)
                riskMax = m_annual;

            if (riskMin > m_annual)
                riskMin = m_annual;
        }
    }
    riskMax = Math.ceil(riskMax + 1);
    riskMin = Math.floor(riskMin - 1);
    riskDataArray = riskDataArray.reverse();
    rateDataArray = rateDataArray.reverse();
    var riskOpt = {
        title: {
            text: '风险度与年化收益率',
            subtext: '',
            textStyle: { color: "#277eab" }

        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['风险度', '年化收益率']


        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                boundaryGap: true,
                data: timeArr
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value}'
                }, max: riskMax,
                min: riskMin
            }
        ],
        series: [
            {
                name: '风险度',
                type: 'line',
                data: riskDataArray,
                label: {
                    normal: {
                        show: isShowLabel,
                        formatter: '{c}%'
                    }
                },
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                }

            },
            {
                name: '年化收益率',
                type: 'line',
                data: rateDataArray,
                label: {
                    normal: {
                        show: isShowLabel,
                        formatter: '{c}%'
                    }
                },
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                }

            }
        ]
    };
    riskChart.setOption(riskOpt);

    //风险度end


    ///上证 start


    for (var i = 0; i < info.length; i++) {
        if (i < dotNum) {
            var m_sz = info[i].sz;
            szDataArray.push(m_sz);
            if (szMax < m_sz)
                szMax = m_sz;
            if (szMin > m_sz)
                szMin = m_sz;
        }
    }
    szMax = Math.ceil(szMax + 30);
    szMin = Math.floor(szMin - 30);

    szDataArray = szDataArray.reverse();
    var szOpt = {
        title: {
            text: '上证指数',
            subtext: '',
            textStyle: { color: "#277eab" }

        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['上证指数'],
            show: false

        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                boundaryGap: true,
                data: timeArr
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value}'
                }, max: szMax,
                min: szMin
            }
        ],
        series: [
            {
                name: '上证指数',
                type: 'line',
                data: szDataArray,
                label: {
                    normal: {
                        show: isShowLabel
                    }
                },
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                }

            }
        ]
    };
    szChart.setOption(szOpt);

    //上证end


    ///上证50 start


    for (var i = 0; i < info.length; i++) {
        if (i < dotNum) {
            var m_sz50 = info[i].sz50;
            sz50DataArray.push(m_sz50);
            if (sz50Max < m_sz50)
                sz50Max = m_sz50;
            if (sz50Min > m_sz50)
                sz50Min = m_sz50;
        }
    }
    sz50Max = (sz50Max + 0.1).toFixed(2);
    sz50Min = (sz50Min - 0.1).toFixed(2);

    sz50DataArray = sz50DataArray.reverse();
    var sz50Opt = {
        title: {
            text: '上证50ETF',
            subtext: '',
            textStyle: { color: "#277eab" }
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['上证50ETF'],
            show: false
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                boundaryGap: true,
                data: timeArr
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value}'
                }, max: sz50Max,
                min: sz50Min
            }
        ],
        series: [
            {
                name: '上证50ETF',
                type: 'line',
                data: sz50DataArray,
                label: {
                    normal: {
                        show: isShowLabel
                    }
                },
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                }

            }
        ]
    };
    sz50Chart.setOption(sz50Opt);

    //上证50end
}


function getDateStr(date) {


    var yy = date.getFullYear();
    var mm = date.getMonth() + 1;
    var dd = date.getDate();
    return add_zero(yy) + "-" + add_zero(mm) + "-" + add_zero(dd);
}
function add_zero(temp) {
    if (temp < 10) return "0" + temp;
    else return temp;
}
function getWeekStr(date) {
    var a = new Array("日", "一", "二", "三", "四", "五", "六");
    var week = date.getDay();
    var str = "星期" + a[week];
    return str;
}