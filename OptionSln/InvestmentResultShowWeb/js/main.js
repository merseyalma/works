stockmain = {
    assetChart: null,
    szChart: null,
    timeArr: [],
    assetOrginal: 10,
    assetDataArray: [],
    assetMax: 0,
    assetMin: 200,
    rateDataArray: [],
    szDataArray: [],
    szMax: 0,
    szMin: 8000,
    dotNum: 20,
    orginalType: -1,
    init: function () {
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
                    color: "#277eab",
                    display:'none'
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
        document.getElementById('idtime').innerHTML = utils.getDateStr(now) + ' ' + utils.getWeekStr(now);
        this.showView(0);

    }, bindEvent: function () {

    },

    showView: function (type) {
        if (this.orginalType != type) {
            this.orginalType = type;
            $("#ulvwtype li").removeClass("highlight");
            $("#li" + type).addClass("highlight");
            this.assetDataArray.splice(0, this.assetDataArray.length);
            this.timeArr.splice(0, this.timeArr.length);
            this.rateDataArray.splice(0, this.rateDataArray.length);
            this.szDataArray.splice(0, this.szDataArray.length); 
            this.assetMax = 0;
            this.assetMin = 200;
           
            this.szMax = 0;
            this.szMin = 8000;
           
            var info = stockdayinfo;
            //if (type == 1)
            //    info = weekinfo;
            //if (type == 2)
            //    info = monthinfo;
            ///资产 start

            for (var i = 0; i < info.length; i++) {
                if (i < this.dotNum) {
                    this.timeArr.push(info[i].t);
                    var m_asset = info[i].d[0] / 10000.0;
                    m_asset = Number(m_asset.toFixed(2));
                    this.assetDataArray.push(m_asset);
                    if (this.assetMax < m_asset)
                        this.assetMax = m_asset;
                    if (this.assetMin > m_asset)
                        this.assetMin = m_asset;
                }
            }
            if (this.assetMin > this.assetOrginal)
                this.assetMin = this.assetOrginal;

            if (this.assetMax < this.assetOrginal)
                this.assetMax = this.assetOrginal;

            this.assetMax = this.assetMax + 0.2;
            this.assetMin = this.assetMin - 0.2;

            this.timeArr = this.timeArr.reverse();
            this.assetDataArray = this.assetDataArray.reverse();


            $('#divasset').highcharts({
                title: {
                    text: '资产',
                    x: 10
                },
                xAxis: {
                    categories: this.timeArr
                },
                yAxis: {
                    title: {
                        text: '金额'
                    },
                    plotLines: [{
                        value: this.assetOrginal,
                        width: 2,
                        color: '#ff0000',
                        label: {
                            text: "初始资产" + this.assetOrginal + '万', style: {
                                color: 'red',
                                fontWeight: 'bold',
                                fontSize: '14px'
                            }
                        }
                    }],
                    labels: {
                        format: "{value}万"
                    },
                    min: this.assetMin,
                    max: this.assetMax
                },

                plotOptions: {
                    line: {
                        dataLabels: {
                            style: { color: '#e15f00' },
                            enabled: true
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
                    data: this.assetDataArray,
                    showInLegend: false
                }]
            });


            //资产end 
            // 上证 start
            for (var i = 0; i < info.length; i++) {
                if (i < this.dotNum) {
                    var m_sz = info[i].d[3];
                    this.szDataArray.push(m_sz);
                    if (this.szMax < m_sz)
                        this.szMax = m_sz;
                    if (this.szMin > m_sz)
                        this.szMin = m_sz;
                }
            }
            this.szMax = Math.ceil(this.szMax + 30);
            this.zMin = Math.floor(this.szMin - 30);

            this.szDataArray = this.szDataArray.reverse();

            $('#divsz').highcharts({
                title: {
                    text: '上证指数',
                    x: 10
                },
                xAxis: {
                    categories: this.timeArr
                },
                yAxis: {
                    title: {
                        text: '价格'
                    },
                    min: this.szMin,
                    max: this.szMax
                },
                plotOptions: {
                    line: {

                        dataLabels: {
                            style: { color: '#e15f00' },
                            enabled: true
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
                    data: this.szDataArray,
                    showInLegend: false
                }]
            });

            //上证end

             

        }
    }
};