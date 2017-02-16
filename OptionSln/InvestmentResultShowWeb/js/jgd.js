var jgd = {
    init: function () {
        this.bindEvent();
        var nowdt = new Date();
        var now = utils.getDateStr(nowdt);
        $('#txtEnd').val(now);
        var last = new Date(nowdt.getTime() - 730 * 86400000);
        $('#txtStart').val(utils.getDateStr(last));
        var stockid = $("#slstock").val();
        for (var item in stocks) {
            $("#slstock").append("<option value='" + item + "'>" + stocks[item] + "(" + item + ")</option>");
        }

    }, bindEvent: function () {
        var self = this;
        $("#btnSearch").click(function () { self.query(); });

    }
    , query: function () {
        var startDate = $("#txtStart").val();
        var endDate = $("#txtEnd").val();
        var stockid = $("#slstock").val();
        if (stockid == '') {
            alert('请选择股票');
            return;
        }
        stockid = stockid.substring(1, 7);

        var jgdhtml = '';
        var altercss = '';
        var orginalTime = '';
        var stockamount = 0;
        var stockasset = 0;
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
                        var isbuy = jdgitem[0] == '1';
                        jgdhtml += '<tr class="' + (isbuy ? 'bmp_buy' : 'bmp_sell') + ' ' + altercss + '"><td>' + item.t + '</td><td>' + jdgitem[1] + '</td><td>' + stocks['s' + jdgitem[1]] + '</td><td>' + (isbuy ? '买入' : '卖出') + '</td><td>' + jdgitem[2] + '</td><td>' + jdgitem[3] + '</td><td>' + jdgitem[4] + '</td><td>' + jdgitem[5] + '</td></tr>';

                        if (isbuy) {
                            stockamount += Math.abs(jdgitem[3]);
                            stockasset -= Math.abs(jdgitem[5]);
                        }
                        else {
                            stockamount -= Math.abs(jdgitem[3]);
                            stockasset += Math.abs(jdgitem[5]);
                        }

                    }
                }
            }
        }
        $("#tbjgd >tbody").html(jgdhtml);
        $("#pstockdes").html(' 持仓:' + stockamount + '股,盈亏:' + stockasset.toFixed(2) + '元');
    }
};