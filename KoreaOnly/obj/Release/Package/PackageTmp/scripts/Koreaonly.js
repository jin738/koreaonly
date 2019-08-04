
$(document).ready(function () {

    function ActiveButton() {
        $(".searchtxt").click(function () {
            this.value = "";
        });
        $(".searchtxt").blur(function () {
            if (this.value == "") {
                if (!$("#" + this.id).prop("disabled")) {
                    $("#" + this.id).css("border-color", "red");
                    $("#error" + this.id).html("Please fill in here");
                    $("#btnSubmit").attr("Allow", "NotAccess");
                } else {
                    $("#" + this.id).css("border-color", "#ccc");
                    $("#error" + this.id).html("");
                    $("#btnSubmit").attr("Allow", "Access");
                }
            } else {
                $("#error" + this.id).html("");
                $("#" + this.id).css("border-color", "#ccc");
                $("#btnSubmit").attr("Allow", "Access");
            }
        });

        $(".hasDatepicker").blur(function () {
            if (this.value == "") {
                if (!$("#" + this.id).prop("disabled")) {
                    $("#" + this.id).css("border-color", "red");
                    $("#btnSubmit").attr("Allow", "NotAccess");
                } else {
                    $("#" + this.id).css("border-color", "#ccc");
                    $("#btnSubmit").attr("Allow", "Access");
                }
            } else {
                $("#" + this.id).css("border-color", "#ccc");
                $("#btnSubmit").attr("Allow", "Access");
            }
        });

        $(".hasDatepicker").change(function () {
            if (this.value == "") {
                if (!$("#" + this.id).prop("disabled")) {
                    $("#" + this.id).css("border-color", "red");
                    $("#error" + this.id).html("Please fill in here");
                    $("#btnSubmit").attr("Allow", "NotAccess");
                } else {
                    $("#" + this.id).css("border-color", "#ccc");
                    $("#btnSubmit").attr("Allow", "Access");
                    $("#error" + this.id).html("");
                }
            } else {
                $("#" + this.id).css("border-color", "#ccc");
                $("#btnSubmit").attr("Allow", "Access");
                $("#error" + this.id).html("");
            }
        });
    }

    $("input[name='stop1']").change(function () {
        dosearch("");
    });

    $("input[name='stop2']").change(function () {
        dosearch("");
    });

    $("input[name='stop3']").change(function () {
        dosearch("");
    });

    function Dochecked(id) {
        $("label[id='" + id + "'] > input[name='AirlineChk']").prop('checked', true);
    }

    $(".onlythisF").click(function (e) {

        $("input[name='stop1']").prop('checked', true);
        $("input[name='stop2']").prop('checked', true);
        $("input[name='stop3']").prop('checked', true);

        $("label[id^='flight'] > input[name='AirlineChk']").prop('checked', false);

        var Chec = $("label[id^='flight'] > input[name='Airline']");

        for (var i = 0; i < Chec.length; i++) {
            if (Chec[i].value.substring(0, 2) != this.id.substring(10, this.id.length))
                Chec[i].value = (Chec[i].value.substring(0, 2) + "0");
            else
                Chec[i].value = (Chec[i].value.substring(0, 2) + "1");
        }

        var ee = this.id.substring(4, this.id.length);
        setTimeout(function () {
            Dochecked(ee);
        }, 200);

        dosearch("check box " + this.id);
    });

    $(".onlythisFA").click(function (e) {

        $("input[name='stop1']").prop('checked', true);
        $("input[name='stop2']").prop('checked', true);
        $("input[name='stop3']").prop('checked', true);

        $("label[id^='flight'] > input[name='AirlineChk']").prop('checked', false);

        var Chec = $("label[id^='flight'] > input[name='Airline']");

        for (var i = 0; i < Chec.length; i++) {
            if (Chec[i].value.substring(0, 2) != this.id.substring(10, this.id.length))
                Chec[i].value = (Chec[i].value.substring(0, 2) + "0");
            else
                Chec[i].value = (Chec[i].value.substring(0, 2) + "1");
        }

        $("label[id='" + this.id.substring(4, this.id.length) + "'] > input[name='AirlineChk']").prop('checked', true);

        dosearch("check box SEARCH" + this.id);

    });

    $(".SelectAll").click(function () {

        $("input[name='stop1']").prop('checked', true);
        $("input[name='stop2']").prop('checked', true);
        $("input[name='stop3']").prop('checked', true);


        $(".subval > input[name='AirlineChk']").prop('checked', true);

        var Chec = $("label[id^='flight'] > input[name='Airline']");

        for (var i = 0; i < Chec.length; i++) {
            Chec[i].value = (Chec[i].value.substring(0, 2) + "1");
        }

        var options = $("#slider-range").slider('option');

        //$("#slider-range").slider('values', [options.min, options.max]);

        dosearch("all flight");
    });

    function ChangeChecked(id) {
        $("input[name='" + id + "']").prop('checked', true);
    }

    $(".stopclickE").click(function (e) {

        $("input[name='stop1']").prop('checked', false);
        $("input[name='stop2']").prop('checked', false);
        $("input[name='stop3']").prop('checked', false);

        $(".subval > input[name='AirlineChk']").prop('checked', true);

        var Chec = $("label[id^='flight'] > input[name='Airline']");

        for (var i = 0; i < Chec.length; i++) {
            Chec[i].value = (Chec[i].value.substring(0, 2) + "1");
        }


        var ee = this.id.substring(4, this.id.length);
        setTimeout(function () {
            ChangeChecked(ee);
        }, 200);

        dosearch("E " + this.id);

    });


    $("input[name='AirlineChk']").change(function () {
        var tt = $("#" + this.id).attr("datae").split('=');
        checkit(tt[0], tt[1], tt[2]);
    });




    //$(".SelectAll").click(function () {

    //    $("input[name='stop1']:checked").prop('checked', false);
    //    $("input[name='stop2']:checked").prop('checked', false);
    //    $("input[name='stop3']:checked").prop('checked', false);

    //    $("input[name='stop1']").prop('checked', true);
    //    $("input[name='stop2']").prop('checked', true);
    //    $("input[name='stop3']").prop('checked', true);

    //    $(".subval > input[name='AirlineChk']:checked").prop('checked', false);
    //    $(".subval > input[name='AirlineChk']").prop('checked', true);
    //    $("#btnsearchsubmit").click();
    //});

    //$(".stopclickE").click(function (e) {

    //    $("input[name='stop1']:checked").prop('checked', false);
    //    $("input[name='stop2']:checked").prop('checked', false);
    //    $("input[name='stop3']:checked").prop('checked', false);

    //    $(".subval > input[name='AirlineChk']:checked").prop('checked', false);
    //    $(".subval > input[name='AirlineChk']").prop('checked', true);

    //    $("input[name='" + this.id.substring(4, this.id.length) + "']").prop('checked', true);

    //});


    //$(".onlythisF").click(function (e) {

    //    $("input[name='stop1']:checked").prop('checked', false);
    //    $("input[name='stop2']:checked").prop('checked', false);
    //    $("input[name='stop3']:checked").prop('checked', false);

    //    $("input[name='stop1']").prop('checked', true);
    //    $("input[name='stop2']").prop('checked', true);
    //    $("input[name='stop3']").prop('checked', true);

    //    $("label[id^='flight'] > input[name='AirlineChk']:checked").prop('checked', false);
    //    $("label[id='" + this.id.substring(4, this.id.length) + "'] > input[name='AirlineChk']").prop('checked', true);
    //});

    //$(".onlythisFA").click(function (e) {

    //    $("label[id^='flight'] > input[name='AirlineChk']:checked").prop('checked', false);
    //    $("label[id='" + this.id.substring(4, this.id.length) + "'] > input[name='AirlineChk']").prop('checked', true);

    //    //if(this.classList.contains("SelectAllStops")){
    //    //    $("input[name='stop1']:checked").click();
    //    //    $("input[name='stop2']:checked").click();
    //    //    $("input[name='stop3']:checked").click();

    //    //    $("input[name='stop1']").click();
    //    //    $("input[name='stop2']").click();
    //    //    $("input[name='stop3']").click();

    //    //}

    //    $("#btnsearchsubmit").click();

    //});

});