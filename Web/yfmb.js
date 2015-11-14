/*-----运费模板style ----*/
/*-----Auther:LinTao-----*/
/*----Version:1.0.0.0----*/

//删除运费模板
function delfareTemplate(id) {
    $.ajax({
        cache: false,
        url: '/Admin/FareTemplate/Delete',
        data: { id: id },
        type: 'post',
        success: function (data) {
            if (data.isdel) {
                alert("删除成功！");
                setLocation('/Admin/FareTemplate');
            }
            else {
                alert("删除失败,模板已使用！");
            }
        },
        error: function () {
            alert('系统错误！');
        }
    });
}

/****运费模板新增页脚本*****/
$(document).ready(function () {
    var tpltext = "件";
    //是否包邮
    $("input[name='bearFreight']").each(function () {
        $(this).change(function () {
            if ($(this).attr('checked')) {
                if ($(this).val() == '0') {
                    $(".set-free").show();
                    $("input[name='tplType']").each(function () {
                        if ($(this).attr("checked")) {
                            $(this).parent().next("div").show();
                        }
                    });
                    return;
                }
                else {
                    $(".set-free").hide();
                    $("input[name='tplType']").each(function () {
                        if ($(this).attr("checked")) {
                            $(this).parent().next("div").hide();
                        }
                    });
                    return;
                }
            }
        });
    });
    //计价方式
    $(".J_CalcRule").each(function () {
        $(this).change(function () {
            if ($(this).attr('checked')) {
                if ($(this).val() == '1') {
                    tpltext = "件";
                    changepost(tpltext, "件数");
                    $("label[name='lb']").html("件(件)");
                }
                if ($(this).val() == '2') {
                    tpltext = "kg";
                    changepost(tpltext, "重量");
                    $("label[name='lb']").html("重(kg)");
                }
                if ($(this).val() == '3') {
                    tpltext = "m³";
                    changepost(tpltext, "体积");
                    $("label[name='lb']").html("体积(m³)");
                }
                $(".val_mode").html(tpltext);
                return;
            }
        });
    });
    //运送方式选择
    $("input[name='tplType']").each(function () {
        $(this).change(function () {
            var value = $(this).val();
            var text = $(this).next("label").text();
            if ($(this).attr('checked')) {
                if ($("#J_buyerBearFre").attr('checked')) {
                    $(this).parent().next("div").show();
                }
                $(".J_Service").each(function () {
                    $(this).append("<option value='" + value + "'>" + text + "</option>");
                });
                return;
            }
            else {
                $(this).parent().next("div").hide();
                $(".J_Service option[value='" + value + "']").remove();
                return;
            }
        });
    });
    //指定包邮条件
    $("#J_SetFree").change(function () {
        if ($(this).attr('checked')) {
            $(this).parent().next("table").show();
            return;
        }
        else {
            $(this).parent().next("table").hide();
        }
    });
    $(".J_ChageContion").each(function () {
        $(this).change(function () {
            var val = $(this).val();
            if (val == '0') {
                $(this).next().find(".full_mode").html(tpltext);
                $(this).next().find("em.hidden").hide();
                return;
            }
            if (val == '1') {
                $(this).next().find(".full_mode").html("元");
                $(this).next().find("em.hidden").hide();
                return;
            }
            if (val == '2') {
                $(this).next().find(".full_mode").html(tpltext);
                $(this).next().find("em.hidden").show();
                return;
            }
        });
    });

    //选择省份加载地区
    $("#proselect").change(function () {
        var id = "li.pro_" + $(this).val();
        $("#J_CityList").children().hide();
        $("#J_CityList").children().first().show();
        $(id).show();
    });
    //选中省份或者城市全选中下属地区
    $(".J_Group,.J_Province").each(function () {
        $(this).change(function () {
            if ($(this).parent().hasClass("group-label")) {
                //省份
                if ($(this).attr('checked')) {
                    $(this).parent().parent().next("div").find("input[type='checkbox']").attr("checked", true);
                    return;
                } else {
                    $(this).parent().parent().next("div").find("input[type='checkbox']").attr("checked", false);
                    return;
                }
            }
            else {
                //城市
                if ($(this).attr('checked')) {
                    $(this).parent().parent().children().find("input[type='checkbox']").attr("checked", true);
                } else {
                    $(this).parent().parent().children().find("input[type='checkbox']").attr("checked", false);
                }
                var checkAll = true;
                $(this).parent().parent().parent().find("input.J_Province").each(function () {
                    if (!$(this).attr("checked")) {
                        checkAll = false;
                    }
                    return checkAll;
                });
                $(this).parent().parent().parent().prev(0).find("input.J_Group").attr("checked", checkAll);
            }
        });
    });
    //选择区县
    $(".J_City").each(function () {
        $(this).change(function () {
            var sum = 0;
            var cksum = 0;
            $(this).parent().parent().children().find("input").each(function () {
                sum = sum + 1;
                if ($(this).attr("checked")) {
                    cksum++;
                }
            });
            if ($(this).attr("checked")) {
                cksum = cksum + 1;
            }
            else {
                cksum = cksum - 1;
            }
            if (cksum == sum) {
                $(this).parent().parent().prev().find("input").attr("checked", true);
            }
            else {
                $(this).parent().parent().prev().find("input").attr("checked", false);
            }
        });
    });
    $("input[name='number']").each(function () {
        $(this).css("ime-mode", "disabled");
        $(this).keyup(function () {
            var val = $(this).val();
            if (isNaN(val)) {
                val = val.substr(0, val.length - 1);
                $(this).val(val);
            }
        });
        $(this).keydown(function (event) {
            if ((event.ctrlKey && event.which == 67) || (event.ctrlKey && event.which == 86)) {
                return false;
            }
            return true;
        });
    });
});
//更改计价方式时改变显示的计价单位
function changepost(text, value) {
    $(".J_ChageContion").each(function () {
        $(this).get(0).options[0].text = value;
        $(this).get(0).options[2].text = value + "+金额";
        var val = $(this).val();
        if (val == '0') {
            $(".full_mode").html(text);
            return;
        }
        if (val == '2') {
            $(".full_mode").html(text);
            return;
        }
    });
};
//添加删除包邮条件
function inclpostifcopy(obj, or) {
    if (or) {
        var rows = $("#J_Tbody").find("tr").length;
        var newrow = $(obj).parent().parent().clone(true);
        newrow.find("p.area").attr("id", "incl" + rows);
        newrow.appendTo("#J_Tbody");
        return false;
    } else {
        $(obj).parent().parent().remove();
        return false;
    }
}
//添加删除指定地区邮费
function changerule(obj, or) {
    var tbody = $(obj).parent().prev().find("tbody");
    var rows = tbody.find("tr").length;
    var id = tbody.attr("mode") + rows;
    if (or) {
        var div = $(obj).parent().prev();//包含地区规则的div
        var tr = div.find("tbody").children().first();//地区规则中的第一条
        if (div.is(":hidden")) {
            div.show();
            return false;
        } else {
            var newtr = tr.clone(true);
            newtr.find("p.area").attr("id", id);
            newtr.appendTo(tr.parent());
            return false;
        }
    } else {
        if ($(obj).parent().parent().parent().children().length > 1) {
            $(obj).parent().parent().remove();
            return false;
        } else {
            $(obj).parent().parent().parent().parent().parent().hide();
            return false;
        }
    }
}
//显示区县
function ShowCounty(obj) {
    if ($(obj).parent().parent().hasClass("showCityPop")) {
        $(obj).parent().parent().removeClass("showCityPop");
    } else {
        $("div").removeClass("showCityPop");
        $(obj).parent().parent().addClass("showCityPop");
    }
}
//隐藏区县
function HideCounty(obj) {
    $(obj).parent().parent().parent().removeClass("showCityPop");
}
//取消选择地区
function HideArea() {
    $("#area").children().find("input[type='checkbox']").attr("checked", false);
    $("#area").hide();
}
//选择地区
function CheckArea(obj) {
    var mode = $(obj).next().find("p").attr("id");
    var offset = $(obj).offset();
    $("#area").css({ position: "absolute", 'top': offset.top + 10, 'left': offset.left - 20, 'z-index': 2 });
    $("#area").attr("mode", mode);
    $("#area").show();
}
//选定地区
function AreaSure() {
    var pid = $("#area").attr("mode");
    var text = ",";
    var value = "|";
    $("#area").find(".J_Group").each(function () {
        if ($(this).attr("checked")) {
            text = text + $(this).next().text() + ",";
            value = value + $(this).val() + "|";
            return;
        }
        $(this).parent().parent().next().find(".J_Province").each(function () {
            if ($(this).attr("checked")) {
                text = text + $(this).next().text() + ",";
                value = value + $(this).val() + "|";
                return;
            }
            $(this).parent().next().find(".J_City").each(function () {
                if ($(this).attr("checked")) {
                    text = text + $(this).next().text() + ",";
                    value = value + $(this).val() + "|";
                    return;
                }
            });
        });
    });
    text = text.substr(1, text.length - 2);
    value = value.substr(1, value.length - 2);
    if ($("#proselect").val() == "0") {
        text = "全国";
        value = "0";
    }
    $("p[id='" + pid + "']").html(text);
    $("p[id='" + pid + "']").parent().next().val(value);
    $("#area").find("input[type='checkbox']").attr("checked", false);
    $("#area").hide();
}
//满足条件包邮模板
function InclPostage(secand) {
    Region = "";
    CarryWay = 0;
    First = 0;
    SelectIf = 0;
    this.Secand = secand;
}
//运送方式模板
function CarryMode(area, isdefault, carryway) {
    this.Region = area;
    this.IsDefault = isdefault;
    this.CarryWay = carryway;
    FirstSum = 0;
    SecondSum = 0;
    FirstAmount = 0;
    SecondAmount = 0;
}

//提交表单
function SubmitTemplate() {
    if (!CheckInput())
        return;
    var name = $("#J_TemplateTitle").val();//模板名
    var shopAddr = $("#txtAddress").val();//宝贝地址
    var dispatchTime = $("#J_prescription").val();//发货时间
    var isInclPostage = $("input[name='bearFreight']:checked").val();//包邮
    var valuationModel = parseInt($("input[name='valuation']:checked").val());//计价方式
    var isInclPostageByif = $("#J_SetFree").attr("checked") ? 1 : 0;//条件包邮
    //包邮条件
    var inclPostages = new Array();
    if ($("#J_SetFree").attr("checked")) {
        var inclpost = $("#J_Tbody").find("tr");
        for (var i = 0; i < inclpost.length; i++) {
            var inclPostage = new InclPostage(0);
            inclPostage.Region = inclpost.eq(i).find("input[name='inclpost']").val();
            inclPostage.CarryWay = inclpost.eq(i).find("select[name='transType']").val();
            inclPostage.First = inclpost.eq(i).find("input[firstname='preferentialStandard']").val();
            inclPostage.SelectIf = inclpost.eq(i).find("select[name='designated']").val();
            if (inclPostage.SelectIf == "2") {
                inclPostage.Secand = inclpost.eq(i).find("input[firstname='preferentialMoney']").val();
            }
            inclPostages.push(inclPostage);
        }
    }
    //运送方式
    var carryModes = new Array();
    if ($("#J_buyerBearFre").attr("checked")) {
        //自有物流的记录数
        if ($("#J_DeliveryEXPRESS").attr("checked")) {
            var expressMode = new CarryMode("0", 1, 0);
            var expressInput = $("div[data-delivery='express']").find("div.default").find(":text");
            expressMode.FirstSum = parseFloat(expressInput.eq(0).val());
            expressMode.SecondSum = parseFloat(expressInput.eq(2).val());
            expressMode.FirstAmount = parseFloat(expressInput.eq(1).val());
            expressMode.SecondAmount = parseFloat(expressInput.eq(3).val());
            carryModes.push(expressMode);

            if ($("#expressdiv").is(":visible")) {
                $("tbody[mode='express']").find("tr").each(function () {
                    var carryMode = new CarryMode("0", 0, 0);
                    var inputs = $(this).find(":text");
                    carryMode.FirstSum = parseFloat(inputs.eq(0).val());
                    carryMode.SecondSum = parseFloat(inputs.eq(2).val());
                    carryMode.Region = $(this).find("input[name='express']").val();
                    carryMode.FirstAmount = parseFloat(inputs.eq(1).val());
                    carryMode.SecondAmount = parseFloat(inputs.eq(3).val());
                    carryModes.push(carryMode);
                });
            }
        }
        //ems的记录数
        if ($("#J_DeliveryEMS").attr("checked")) {
            var emsMode = new CarryMode("0", 1, 1);
            var emsInput = $("div[data-delivery='ems']").find("div.default").find(":text");
            emsMode.FirstSum = parseFloat(emsInput.eq(0).val());
            emsMode.SecondSum = parseFloat(emsInput.eq(2).val());
            emsMode.FirstAmount = parseFloat(emsInput.eq(1).val());
            emsMode.SecondAmount = parseFloat(emsInput.eq(3).val());
            carryModes.push(emsMode);

            if ($("#emsdiv").is(":visible")) {
                $("tbody[mode='ems']").find("tr").each(function () {
                    var inputs = $(this).find(":text");
                    var carryMode = new CarryMode("0", 0, 1);
                    carryMode.FirstSum = parseFloat(inputs.eq(0).val());
                    carryMode.SecondSum = parseFloat(inputs.eq(2).val());
                    carryMode.Region = $(this).find("input[name='ems']").val();
                    carryMode.FirstAmount = parseFloat(inputs.eq(1).val());
                    carryMode.SecondAmount = parseFloat(inputs.eq(3).val());
                    carryModes.push(carryMode);
                });
            }
        }
        //快递的记录数
        if ($("#J_DeliveryPOST").attr("checked")) {
            var postMode = new CarryMode("0", 1, 2);
            var postInput = $("div[data-delivery='post']").find("div.default").find(":text");
            postMode.FirstSum = parseFloat(postInput.eq(0).val());
            postMode.SecondSum = parseFloat(postInput.eq(2).val());
            postMode.FirstAmount = parseFloat(postInput.eq(1).val());
            postMode.SecondAmount = parseFloat(postInput.eq(3).val());
            carryModes.push(postMode);

            if ($("#postdiv").is(":visible")) {
                $("tbody[mode='post']").find("tr").each(function () {
                    var inputs = $(this).find(":text");
                    var carryMode = new CarryMode("0", 0, 2);
                    carryMode.FirstSum = parseFloat(inputs.eq(0).val());
                    carryMode.SecondSum = parseFloat(inputs.eq(2).val());
                    carryMode.Region = $(this).find("input[name='post']").val();
                    carryMode.FirstAmount = parseFloat(inputs.eq(1).val());
                    carryMode.SecondAmount = parseFloat(inputs.eq(3).val());
                    carryModes.push(carryMode);
                });
            }
        }
    }
    //模板对象
    var fareTemplate =
        {
            Name: name,
            ShopAddr: shopAddr,
            DispatchTime: dispatchTime,
            IsInclPostage: isInclPostage,
            ValuationModel: valuationModel,
            IsInclPostageByif: isInclPostageByif,
            InclPostageProvisos: inclPostages,
            CarryModes: carryModes
        };
    $.ajax({
        url: "Create",
        data: fareTemplate,
        type: "POST",
        success: function (data) {
            alert(data.msg);
            if (data.isok)
                location.href = "index";
        },
        error: function () {
            alert("添加失败！");
        }
    });
}
//提交前验证
function CheckInput() {
    //判断运送方式
    var sum = 0;
    var isok = true;
    $("input[name='tplType']").each(function () {
        if ($(this).attr("checked"))
            sum++;
    });
    if (sum <= 0) {
        alert("请至少选择一种运送方式！");
        return false;
    }
    //判断发货时间
    if ($("#J_prescription").val() == "0") {
        alert("请设置发货时间！");
        return false;
    }
    //判断所有可见表单
    $("input[type='text']:visible").each(function () {
        if ($.trim($(this).val()) == "") {
            $(this).focus();
            isok = false;
            return false;
        }
        else return true;
    });
    return isok;
}
