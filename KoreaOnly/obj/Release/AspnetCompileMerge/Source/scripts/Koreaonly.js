﻿//var LOCK = !1; $(document).ready(function () { function i(i) { LOCK && $("#btnsearchsubmit").click() } $("input[name='stop1']").change(function () { i() }), $("input[name='stop2']").change(function () { i() }), $("input[name='stop3']").change(function () { i() }), $(".onlythisF").click(function (e) { $("input[name='stop1']").prop("checked", !0), $("input[name='stop2']").prop("checked", !0), $("input[name='stop3']").prop("checked", !0), $("label[id^='flight'] > input[name='AirlineChk']").prop("checked", !1); for (var t = $("label[id^='flight'] > input[name='Airline']"), n = 0; n < t.length; n++) t[n].value.substring(0, 2) != this.id.substring(10, this.id.length) ? t[n].value = t[n].value.substring(0, 2) + "0" : t[n].value = t[n].value.substring(0, 2) + "1"; var p = this.id.substring(4, this.id.length); setTimeout(function () { $("label[id='" + p + "'] > input[name='AirlineChk']").prop("checked", !0) }, 200), LOCK = !0, i(this.id), LOCK = !1 }), $(".onlythisFA").click(function (e) { $("input[name='stop1']").prop("checked", !0), $("input[name='stop2']").prop("checked", !0), $("input[name='stop3']").prop("checked", !0), $("label[id^='flight'] > input[name='AirlineChk']").prop("checked", !1); for (var t = $("label[id^='flight'] > input[name='Airline']"), n = 0; n < t.length; n++) t[n].value.substring(0, 2) != this.id.substring(10, this.id.length) ? t[n].value = t[n].value.substring(0, 2) + "0" : t[n].value = t[n].value.substring(0, 2) + "1"; $("label[id='" + this.id.substring(4, this.id.length) + "'] > input[name='AirlineChk']").prop("checked", !0), LOCK = !0, i(this.id), LOCK = !1 }), $(".SelectAll").click(function () { $("input[name='stop1']").prop("checked", !0), $("input[name='stop2']").prop("checked", !0), $("input[name='stop3']").prop("checked", !0), $(".subval > input[name='AirlineChk']").prop("checked", !0); for (var e = $("label[id^='flight'] > input[name='Airline']"), t = 0; t < e.length; t++) e[t].value = e[t].value.substring(0, 2) + "1"; $("#slider-range").slider("option"), LOCK = !0, i(), LOCK = !1 }), $(".stopclickE").click(function (e) { $("input[name='stop1']").prop("checked", !1), $("input[name='stop2']").prop("checked", !1), $("input[name='stop3']").prop("checked", !1), $(".subval > input[name='AirlineChk']").prop("checked", !1), "All" == $(this).attr("flightid") ? $(".subval > input[name='AirlineChk']").prop("checked", !0) : $("#" + $(this).attr("flightid") + " > input[name='AirlineChk']").prop("checked", !0); var t = $("label[id^='flight'] > input[name='Airline']"); if ("All" == $(this).attr("flightid")) for (var n = 0; n < t.length; n++) t[n].value = t[n].value.substring(0, 2) + "1"; else for (n = 0; n < t.length; n++) t[n].value = t[n].value.substring(0, 2) + "0"; "All" != $(this).attr("flightid") && $("#" + $(this).attr("flightid") + " > input[name='Airline']").val($("#" + $(this).attr("flightid") + " > input[name='Airline']").val().substring(0, 2) + "1"), $("input[name='" + this.id.substring(4, 10) + "']").prop("checked", !0), LOCK = !0, i(this.id), LOCK = !1, $("input[name='" + this.id.substring(4, 10) + "']").prop("checked", !0), $("input[name='" + this.id.substring(4, 10) + "']").prop("checked") }), $("input[name='AirlineChk']").change(function () { var i = $("#" + this.id).attr("datae").split("="); checkit(i[0], i[1], i[2]) }), $(".CLickonly").click(function () { console.log("check Trig"), $(".subval > input[name='AirlineChk']").prop("checked", !0); for (var e = $("label[id^='flight'] > input[name='Airline']"), t = 0; t < e.length; t++) e[t].value = e[t].value.substring(0, 2) + "1"; $("#idstop1").prop("checked", !1), $("#idstop2").prop("checked", !1), $("#idstop3").prop("checked", !1), setCookie("stopid", "id" + $(this).get(0).id.substring(4, 9), 1), $("#id" + $(this).get(0).id.substring(4, 9)).prop("checked", !0), LOCK = !0, i(this.id), LOCK = !1 }) });