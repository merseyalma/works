var assetChart = null;
var riskChart = null;
var szChart = null;
var sz50Chart = null;
var timeArr = [];
var assetOrginal = 53;
var assetDataArray = [];
var assetMax = 0;
var assetMin = 200;
 
var szDataArray = [];
var szMax = 0;
var szMin = 8000; 

var orginalType = -1;
var maxLabel = {
    enabled: true,
    y: -12,
    style: {
        fontWeight: 'bold'
    },

    verticalAlign: 'middle',
    overflow: true,
    crop: false
};
var minLabel = {
    enabled: true,
    y: 12,
    style: {
        fontWeight: 'bold'
    },

    verticalAlign: 'middle',
    overflow: true,
    crop: false
};

function loadData(dotNum, type) {

    Highcharts.theme = {
        colors: ["#7cb5ec", "#f7a35c", "#90ee7e", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
           "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
        chart: {
            backgroundColor: null,
            style: {
                fontFamily: "微软雅黑"
            }
        },
        title: {
            style: {
                fontSize: '18px',
                fontWeight: 'bold',
                textTransform: 'uppercase',
                color: "#277eab"
            }, align: 'left'
        },
        tooltip: {
            borderWidth: 0,
            backgroundColor: 'rgba(219,219,216,0.8)',
            shadow: false
        },
        legend: {
            itemStyle: {
                fontWeight: 'bold',
                fontSize: '14px'
            }
        },
        xAxis: {
            gridLineWidth: 1,
            labels: {
                style: {
                    fontSize: '14px', fontWeight: '500'
                }
            }
        },
        yAxis: {
            minorTickInterval: 'auto',
            title: {
                style: {
                    fontSize: '16px'
                }
                , rotation: 270
            },
            labels: {
                style: {
                    fontSize: '12px', fontWeight: '500',
                    fontColor: 'red'
                }
            }
        },
        plotOptions: {
            candlestick: {
                lineColor: '#404048'
            }
        }

    };
    Highcharts.setOptions(Highcharts.theme);

    var now = new Date();
    document.getElementById('idtime').innerHTML = getDateStr(now) + ' ' + getWeekStr(now);

    showView(dotNum, type);

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

function showView(dotNum, type) {
    var isShowLabel = false;//dotNum > 0;

    if (orginalType != type) {
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

        $("#ulvwtype li").removeClass("highlight");
        $("#li" + type).addClass("highlight");
        assetDataArray.splice(0, assetDataArray.length);
        timeArr.splice(0, timeArr.length); 
        szDataArray.splice(0, szDataArray.length);
     
        assetMax = 0;
        assetMin = 200;
        riskMax = 0;
        riskMin = 100;
        rateMax = 0;
        rateMin = 100;
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
                var m_asset = info[i].profit / 10000.0;
                m_asset = Number(m_asset.toFixed(2));
                if (assetMax < m_asset)
                    assetMax = m_asset;
                if (assetMin > m_asset)
                    assetMin = m_asset;
            }
        }
       
        for (var i = 0; i < info.length; i++) {
            if (i < dotNum) {
                timeArr.push(info[i].t);
                var m_asset = info[i].profit / 10000.0;
                m_asset = Number(m_asset.toFixed(2));
                if (m_asset == assetMax) {
                    assetDataArray.push({
                        dataLabels: maxLabel,
                        y: m_asset
                    });

                }
                else if (m_asset == assetMin) {
                    assetDataArray.push({
                        dataLabels: minLabel,
                        y: m_asset
                    });
                }
                else {
                    assetDataArray.push(m_asset);
                }

            }
        }
        if (assetMin > assetOrginal)
            assetMin = assetOrginal;

        assetMax = assetMax + 0.2;
        assetMin = assetMin - 0.2;
        console.log(assetMax.toString() + ' ' + assetMin.toString());
        //timeArr = timeArr.reverse();
        //assetDataArray = assetDataArray.reverse();


        $('#divasset').highcharts({
            title: {
                text: '资产',
                x: 10
            },
            xAxis: {
                categories: timeArr
            },
            yAxis: {
                title: {
                    text: '金额'
                },
                plotLines: [{
                    value: assetOrginal,
                    width: 2,
                    color: '#ff0000',
                    label: {
                        text: "初始资产" + assetOrginal + '万', style: {
                            color: 'red',
                            fontWeight: 'bold',
                            fontSize: '14px'
                        }
                    }
                }],
                labels: {
                    format: "{value}万"
                },
                min: assetMin,
                max: assetMax
            },

            plotOptions: {
                line: {
                    dataLabels: {
                        style: { color: '#e15f00' },
                        enabled: isShowLabel
                    },
                    color: '#e15f00'
                }
            },

            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },

            series: [{
                name: '资产',
                data: assetDataArray,
                showInLegend: false
            }]
        });


        //资产end

        //风险度 start

        for (var i = 0; i < info.length; i++) {
            if (i < dotNum) {
                var m_risk = info[i].risk;               
                var m_annual = info[i].annualrate;                
                if (riskMax < m_risk)
                    riskMax = m_risk;
                if (riskMin > m_risk)
                    riskMin = m_risk;
                if (rateMax < m_annual)
                    rateMax = m_annual;
                if (rateMin > m_annual)
                    rateMin = m_annual;
            }
        }
 

        // 上证 start

        for (var i = 0; i < info.length; i++) {
            if (i < dotNum) {
                var m_sz = info[i].sz  ; 
                if (szMax < m_sz)
                    szMax = m_sz;
                if (szMin > m_sz)
                    szMin = m_sz;
            }
        }


        for (var i = 0; i < info.length; i++) {
            if (i < dotNum) {
                var m_sz = info[i].sz;
                if (m_sz == szMax) {
                    szDataArray.push({
                        dataLabels: maxLabel,
                        y: m_sz
                    });

                }
                else if (m_sz == szMin) {
                    szDataArray.push({
                        dataLabels: minLabel,
                        y: m_sz
                    });
                }
                else {
                    szDataArray.push(m_sz);
                }
            }
        }
        szMax = Math.ceil(szMax + 5);
        szMin = Math.floor(szMin - 5);

      
        //szDataArray = szDataArray.reverse();

        $('#divsz').highcharts({
            title: {
                text: '上证指数',
                x: 10
            },
            xAxis: {
                categories: timeArr
            },
            yAxis: {
                title: {
                    text: '价格'
                },
                min: szMin,
                max: szMax
            },
            plotOptions: {
                line: {

                    dataLabels: {
                        style: { color: '#e15f00' },
                        enabled: isShowLabel
                    },
                    color: '#e15f00'
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },

            series: [{
                name: '上证指数',
                data: szDataArray,
                showInLegend: false
            }]
        });

        //上证end 
    }
}

function setSize()
{
    var winHeight = Number($(window).height()); 
    $(".hq01").height((winHeight - 76) / 2);
}