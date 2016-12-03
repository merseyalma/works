var utils = {
    getDateStr: function (date) {
        var yy = date.getFullYear();
        var mm = date.getMonth() + 1;
        var dd = date.getDate();
        return this.add_zero(yy) + "-" + this.add_zero(mm) + "-" + this.add_zero(dd);
    }, add_zero: function (temp) {
        if (temp < 10) return "0" + temp;
        else return temp;
    },
    getWeekStr: function (date) {
        var a = new Array("日", "一", "二", "三", "四", "五", "六");
        var week = date.getDay();
        var str = "星期" + a[week];
        return str;
    }

};