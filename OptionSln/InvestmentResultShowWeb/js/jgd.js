var jgd = {
    init: function () {
        this.bindEvent();
        var nowdt = new Date();
        var now = utils.getDateStr(nowdt);
        $('#txtEnd').val(now);
        var last = new Date(2015, 4, 20, 0, 0, 0);
        $('#txtStart').val(utils.getDateStr(last));
        var stockid = $("#slstock").val();
        for (var item in stocks) {
            $("#slstock").append("<option value='" + stocks[item][0] + "' d='" + stocks[item][2] + " " + stocks[item][3] + "'>" + stocks[item][1] + "(" + stocks[item][0] + ")</option>");
        }

    }, bindEvent: function () {
        var self = this;
        $("#btnSearch").click(function () { self.query(); });
    }
    , query: function () {
        var startDate = $("#txtStart").val();
        var endDate = $("#txtEnd").val();
        var stockid = $("#slstock").val();
        var selObj = $("#slstock").find("option:selected");
        var stockname = selObj.text();
        var index = stockname.indexOf('(');
        stockname = stockname.substring(0, index);
        if (stockid == '') {
            alert('请选择股票');
            return;
        }
        var jgdhtml = '';
        var altercss = '';
        var orginalTime = '';
        var stockyl = selObj.attr("d").split(' ');
        var stockamount = stockyl[1];
        var stockasset = stockyl[0];
        for (var i = 0; i < jiaogedans.length; i++) {
            var item = jiaogedans[i];
            if (orginalTime != item.t) {
                if (altercss == '')
                    altercss = 'bmp_alter';
                else
                    altercss = '';
            }
            if (item.t >= startDate && item.t <= endDate) {
                for (var j = 0; j < item.d.length; j++) {
                    var jdgitem = item.d[j];
                    if (jdgitem[1] == stockid) {
                        if (jdgitem[0] != '新股入账') {
                            var isbuy = (jdgitem[0] == '证券买入') || (jdgitem[0] == '红利入账') || (jdgitem[0] == '红股入账') || (jdgitem[0] == '新股IPO配售确认');
                            jgdhtml += '<tr class="' + (isbuy ? 'bmp_buy' : 'bmp_sell') + ' ' + altercss + '"><td>' + item.t + '</td><td>' + jdgitem[1] + '</td><td>' + stockname + '</td><td>' + jdgitem[0] + '</td><td>' + jdgitem[2] + '</td><td>' + jdgitem[3] + '</td><td>' + jdgitem[4] + '</td><td>' + jdgitem[5] + '</td></tr>';

                        }
                    }
                }
            }
        }
        $("#tbjgd >tbody").html(jgdhtml);
        $("#pstockdes").html(' 持仓:' + stockamount + '股,盈亏:' + Number(stockasset).toFixed(2) + '元');
    }
};