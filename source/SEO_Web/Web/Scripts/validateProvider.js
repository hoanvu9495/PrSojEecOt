//var jsArray = {};

//function loadScript(scriptName, callback) {

//    if (!jsArray[scriptName]) {
//        var promise = jQuery.Deferred();

//        // adding the script tag to the head as suggested before
//        var body = document.getElementsByTagName('body')[0],
//            script = document.createElement('script');
//        script.type = 'text/javascript';
//        script.src = scriptName;

//        // then bind the event to the callback function
//        // there are several events for cross browser compatibility
//        script.onload = function () {
//            promise.resolve();
//        };

//        // fire the loading
//        body.appendChild(script);

//        // clear DOM reference
//        //body = null;
//        //script = null;

//        jsArray[scriptName] = promise.promise();

//    } else if (debugState)
//        root.root.console.log("This script was already loaded %c: " + scriptName, debugStyle_warning);

//    jsArray[scriptName].then(function () {
//        if (typeof callback === 'function')
//            callback();
//    });
//}


function NotiSuccess(message) {
    $.smallBox({
        title: "Thông báo",
        content: message,
        color: "#27ae60",
        iconSmall: "fa fa-check fa-2x fadeInRight animated",
        timeout: 4000
    });
}

function NotiError(message) {
    $.smallBox({
        title: "Có lỗi",
        content: message,
        color: "#e67e22",
        iconSmall: "fa fa-warning fa-2x fadeInRight animated",
        timeout: 4000
    });
}

function checkRequireWithIDErr(formID, fieldID, errID) {
    var check_err = false;
    if ($("#" + formID + " #" + fieldID).val().trim() == "") {
        $("#" + formID + " #" + errID).html("Bạn phải nhập thông tin này");
        $("#" + formID + " #" + errID).css('display', 'inline');
        check_err = true;
    } else {
        $(this).next().find(".error").css('display', 'none');
    }
    return check_err;
}

function checkRequireElement(formID, fieldID) {
    var check_err = true;
    var item = $("#" + formID + " #" + fieldID);
    var parent = item.parents(" .form-group").first();
    var errText = parent.find(".error");
    if (item.val() == null || item.val().trim() == "") {

        errText.html("Bạn phải nhập thông tin này");
        errText.css('display', 'inline');

        check_err = false;
    } else {
        errText.css('display', 'none');
    }
    return check_err;
}
function checkRequireElementSelect(formID, fieldID) {
    var check_err = true;
    var item = $("#" + formID + " #" + fieldID);
    var parent = item.parents(" .form-group").first();
    var errText = parent.find(".error");
    if (item.val() == null || item.val().length == 0) {

        errText.html("Bạn phải nhập thông tin này");
        errText.css('display', 'inline');

        check_err = false;
    } else {
        errText.css('display', 'none');
    }
    return check_err;
}

function RequireDropDownlist(formID) {
    var check_err = true;
    var item = $("#" + formID + " select.requiredDropDownList");
    item.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val() == null || $(this).val().length == 0) {

            errText.html("Bạn phải nhập thông tin này");
            errText.css('display', 'inline');
            check_err = false;
        } else {
            errText.css('display', 'none');
        }
    })

    return check_err;
}

function checkRequireTextArea(form_id) {
    var check_err = true;
    $("#" + form_id + " .requiredTextArea").each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).html() == null || $(this).html().trim() == "") {

            errText.html("Bạn phải nhập thông tin này");
            errText.css('display', 'inline');

            check_err = false;
        } else {
            errText.css('display', 'none');
        }
    });
    return check_err;
}

function ValidateFieldNumber(formID) {
    var valid = true;
    var pattern = /^[0-9]+$/;
    var obj = $("#" + formID + " .validateNumber");
    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() != "") {
            if (!pattern.test($(this).val())) {
                errText.html("Bạn chỉ được nhập số");
                errText.css("display", "inline");
                valid = false;
            }
            else {
                errText.css("display", "none");
            }

        }
    });
    return valid;
}

function ValidateFieldDate(formID) {
    var valid = true;
    var pattern = /^[0-3][0-9]\/[01][0-9]\/[12][0-9][0-9][0-9]$/;

    var obj = $("#" + formID + " .validateDate");

    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() == "") {
            errText.html("Bạn phải nhập thông tin này");
            errText.css("display", "inline");
            valid = false;
        } else {
            if (!pattern.test($(this).val())) {
                errText.html("Vui lòng nhập đúng định dạng ngày dd/mm/yyyy");
                errText.css("display", "inline");
                valid = false;
            }
            else {
                errText.css("display", "none");

            }
        }
    })
    return valid;
}
function ConvertToDateISO(str) {
    var dateint = parseInt(str.match(/\d+/)[0]);
    return new Date(dateint).toISOString();
}

function GetTextDateTime(str) {
    var dt = ConvertToDateISO(str);
    return convertDateTime(dt);
}

function PageSetup() {
    $(".pagination>li:first>a").attr("href", "javascript:GetDataPage(1)");
    $(".pagination>li>a").click(function () {
        $(".pagination>li").removeClass("active");
        $(this).parent().addClass("active");
    })
}







function ValidateFieldDateExist(formID) {
    var valid = true;
    var pattern = /^[0-3][0-9]\/[01][0-9]\/[12][0-9][0-9][0-9]$/;

    var obj = $("#" + formID + " .validateDateExist");

    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() != "") {
            if (!pattern.test($(this).val())) {
                errText.html("Vui lòng nhập đúng định dạng ngày dd/mm/yyyy");
                errText.css("display", "inline");
                valid = false;
            }
            else {
                errText.css("display", "none");

            }
        } else {
            errText.css("display", "none");
        }
    })
    return valid;
}


function ValidateFieldEmail(formID) {
    var valid = true;
    var pattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var obj = $("#" + formID + " .validateEmail");

    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() == "") {
            errText.html("Bạn phải nhập thông tin này");
            errText.css("display", "inline");
            valid = false;


        } else {
            if (!pattern.test($(this).val())) {
                errText.html("Vui lòng nhập đúng định dạng email.");
                errText.css("display", "inline");
                valid = false;

            }
            else {
                errText.css("display", "none");
            }
        }
    })
    return valid;
}

function AjaxCheckExist(url, para) {
    var result = false;
    $.ajax({
        url: url,
        data: para,
        type: "Post",
        async: false,
        success: function (rs) {
            if (rs == true) {
                result = true;
            }
        }
    });
    return result;
}

function ValidateFieldPhone(formID) {
    var valid = true;
    var pattern = /^0[1-9]{1}[0-9]{8,9}$/;

    var obj = $("#" + formID + " .validatePhone");

    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() == "") {
            errText.html("Bạn phải nhập thông tin này");
            errText.css("display", "inline");
            valid = false;


        } else {
            if (!pattern.test($(this).val())) {
                errText.html("Vui lòng nhập đúng định dạng số điện thoại 0xxxxxxxxx. Độ dài 10 đến 11 chữ số");
                errText.css("display", "inline");
                valid = false;

            }
            else {
                errText.css("display", "none");
            }
        }
    })
    return valid;
}

function RegexPhone(formID, idfield) {
    var valid = true;
    var pattern = /^0[1-9]{1}[0-9]{8,9}$/;

    var obj = $("#" + formID + " #" + idfield);

    var parent = obj.parents(" .form-group").first();
    var errText = parent.find(".error");
    if (obj.val().trim() == "") {
        errText.html("Bạn phải nhập thông tin này");
        errText.css("display", "inline");
        valid = false;


    } else {
        if (!pattern.test(obj.val())) {
            errText.html("Vui lòng nhập đúng định dạng số điện thoại 0xxxxxxxxx. Độ dài 10 đến 11 chữ số");
            errText.css("display", "inline");
            valid = false;

        }
        else {
            errText.css("display", "none");
        }
    }

    return valid;
}


function ValidateFieldPassword(formID) {
    var valid = true;
    var pattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[0-9A-Za-z\d$@#$!%*?&]{8,100}/;

    var obj = $("#" + formID + " .validatePassword");

    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() == "") {
            errText.html("Bạn phải nhập thông tin này");
            errText.css("display", "inline");
            valid = false;
            $(this).addClass("state-error");

        } else {
            if (!pattern.test($(this).val())) {
                errText.html("Tối thiểu gồm 8 ký tự, bao gồm ký tự hoa, ký tự thường và ký tự đặc biệt");
                errText.css("display", "inline");
                valid = false;
                $(this).addClass("state-error");
            }
            else {
                errText.css("display", "none");
                $(this).removeClass("state-error");
            }
        }
    })
    return valid;
}

function ValidateFieldCMND(formID) {
    var valid = true;
    var pattern = /^[0-9]{9,12}$/;

    var obj = $("#" + formID + " .validateCMND");

    obj.each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val().trim() == "") {
            errText.html("Bạn phải nhập thông tin này");
            errText.css("display", "inline");
            valid = false;

        } else {
            if (!pattern.test($(this).val())) {
                errText.html("Bạn chỉ được nhập số độ dài từ 9 đến 12 chữ số");
                errText.css("display", "inline");
                valid = false;

            }
            else {
                errText.css("display", "none");

            }
        }
    })
    return valid;
}


function requiredField() {
    var check_err = false;
    $(".required").each(function () {
        if ($(this).val().trim() == "") {
            $(this).next().find(".error").html("Bạn phải nhập thông tin này");
            $(this).next().find(".error").css('display', 'inline');
            check_err = true;
        } else {
            $(this).next().find(".error").css('display', 'none');
        }
    });
    return check_err;
}

function IsDate(formID, fieldID) {
    var valid = true;
    var dateStr = $("#" + formID + " #" + fieldID).val();

    var pattern = /^[0-3][0-9]\/[01][0-9]\/2[0-9][0-9][0-9]$/;
    if ($("#" + formID + " #" + fieldID).val().trim() != "") {
        if (pattern.test(dateStr) == false) {

            valid = false;
        }
    } else {
        valid = false;

    }
    return valid;
}


function regexTenDangNhap(str) {

    var Regex = /^[0-9a-zA-Z\_]+$/;
    if (Regex.test(str)) {
        return true;
    } else {
        return false;
    }
}

function regexDate(formID, fieldID, errID) {
    var valid = false;
    var dateStr = $("#" + formID + " #" + fieldID).val();

    var pattern = /^[0-3][0-9]\/[01][0-9]\/2[0-9][0-9][0-9]$/;
    if ($("#" + formID + " #" + fieldID).val().trim() == "") {
        $("#" + formID + " #" + errID).html("Vui lòng nhập thông tin này");
        $("#" + formID + " #" + errID).css('display', 'inline');
        valid = true;
    } else {

        if (pattern.test(dateStr) == false) {
            $("#" + formID + " #" + errID).html("Sai đinh dạng ngày VD: dd/mm/yyyy");
            $("#" + formID + " #" + errID).css('display', 'inline');
            valid = true;
        } else {
            $("#" + formID + " #" + errID).css('display', 'none');

        }
    }
    return valid;
}

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}


function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}

function parseDateFromMonth(str) {
    var mdy = str.split('/');
    return new Date(mdy[1], mdy[0] - 1, 1);
}
//chuyển date sang dd/MM/yyyy
function DateToText(obj) {
    var mon = '';
    if ((obj.getMonth() + 1) < 10) {
        mon = "0" + (obj.getMonth() + 1);
    } else {
        mon = (obj.getMonth() + 1);
    }
    var day = "";
    if (obj.getDate() < 10) {
        day = '0' + obj.getDate();
    } else {
        day = obj.getDate();
    }
    var date_string = day + "/" + mon + "/" + obj.getFullYear();
    return date_string;
}

//modify by duynn (20/6/2017)
function requiredFieldForFormId(form_id) {
    var check_err = true;
    $("#" + form_id + " .required").each(function () {
        var parent = $(this).parents(" .form-group").first();
        var errText = parent.find(".error");
        if ($(this).val() == null || $(this).val().length == 0 || $(this).val().toString().trim() == "") {
            errText.html("Bạn phải nhập thông tin này");
            errText.css('display', 'inline');
            check_err = false;
        } else {
            errText.css('display', 'none');
        }
    });
    return check_err;
}

function regexEmail(form_id) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    var check_err = false;
    $("#" + form_id + " .email").each(function () {
        if ($(this).val().trim() != "") {
            if (pattern.test($(this).val().trim()) == false) {
                $(this).next().find(".error").html("Bạn phải nhập đúng định dạng email");
                $(this).next().find(".error").css('display', 'inline');
                check_err = true;
            } else {
                $(this).next().find(".error").css('display', 'none');
            }
        }
    });
    return check_err;
}

function isURL(str) {
    var urlRegex = '^(?!mailto:)(?:(?:http|https|ftp)://)(?:\\S+(?::\\S*)?@@)?(?:(?:(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}(?:\\.(?:[0-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))|(?:(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)(?:\\.(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)*(?:\\.(?:[a-z\\u00a1-\\uffff]{2,})))|localhost)(?::\\d{2,5})?(?:(/|\\?|#)[^\\s]*)?$';
    var url = new RegExp(urlRegex, 'i');
    return str.length < 2083 && url.test(str);
}

function regexCMT(form_id) {
    var pattern = /^[0-9]{9,12}$/;
    var check_err = false;
    $("#" + form_id + " .regexCMT").each(function () {
        if ($(this).val().trim() != "") {
            if (pattern.test($(this).val().trim()) == false) {
                $(this).next().find(".error").html("Bạn chỉ được nhập số độ dài 9-12");
                $(this).next().find(".error").css('display', 'inline');
                check_err = true;
            } else {
                $(this).next().find(".error").css('display', 'none');
            }
        }
    });
    return check_err;
}

function CheckRangeDate(formID, DateStartID, DateEndID) {
    var valid = true;
    var dateStart = parseDate($("#" + formID + " #" + DateStartID).val());
    var dateEnd = parseDate($("#" + formID + " #" + DateEndID).val());
    if (dateEnd < dateStart) {
        valid = false;
    }
    return valid;
}



function regexNumber(form_id) {
    var pattern = /^[0-9]+$/;
    var check_err = false;
    $("#" + form_id + " .regexNumber").each(function () {
        if ($(this).val().trim() != "") {
            if (pattern.test($(this).val().trim()) == false) {
                $(this).next().find(".error").html("Bạn chỉ được nhập số");
                $(this).next().find(".error").css('display', 'inline');
                check_err = true;
            } else {
                if (parseInt($(this).val(), 10) < 0) {
                    $(this).next().find(".error").html("Thông tin này phải nhập số có giá trị lớn hơn 0");
                    $(this).next().find(".error").css('display', 'inline');
                    check_err = true;
                } else {
                    $(this).next().find(".error").css('display', 'none');
                }
            }
        }
    });
    return check_err;
}

function GetRangeDay(first, second) {
    return (second - first) / (1000 * 60 * 60 * 24);
}


function NotifErr(message) {
    notif({
        type: 'error',
        position: 'bottom',
        msg: message,
        timeout: 5000
    });
}

function StateAction(ObjectName, state, action) {
    if (state != "") {

        var message;

        if (state == "True") {
            switch (action) {
                case '1':
                    message = "Thêm mới " + ObjectName + " thành công";
                    break;
                case '2':
                    message = "Cập nhật " + ObjectName + " thành công";
                    break;
                case '3':
                    message = "Xóa " + ObjectName + " thành công";
                    break;
            }



            notif({
                type: 'success',
                position: 'bottom',
                msg: message,
                timeout: 5000
            });
        } else {

            switch (action) {
                case '1':
                    message = "Thêm mới " + ObjectName + " thất bại";
                    break;
                case '2':
                    message = "Cập nhật " + ObjectName + " thất bại";
                    break;
                case '3':
                    message = "Xóa " + ObjectName + " thất bại";
                    break;
            }
            notif({
                type: 'error',
                position: 'bottom',
                msg: message,
                timeout: 5000
            });
        }
    }
}


function commonNotifySuccess(mes) {
    notif({
        type: 'success',
        position: 'bottom',
        msg: mes,
        timeout: 5000
    });
}

function DeleteConfirm(url, para, objecName) {
    $.confirm({
        'title': 'Xác nhận xóa',
        'message': 'Bạn có chắc chắn muốn xóa ' + objecName + ' này?',
        'buttons': {
            'Đồng ý': {
                'class': 'btn-confirm-yes btn-info',
                'action': function () {
                    $.ajax({
                        url: url,
                        data: para,
                        type: 'Post',
                        dataType: 'json',
                        success: function (result) {
                            if (result.success) {
                                commonNotifySuccess('Đã xóa ' + objecName);
                                setTimeout(function () {
                                    refreshPageClient();
                                }, 500);
                            } else
                                commonNotifyError(result.message);
                        }, error: function (result) {
                            alert(result.responseText);
                        }
                    })
                }
            },
            'Hủy bỏ': {
                'class': 'btn-danger',
                'action': function () { }
            }
        }
    });


}

function refreshPage(url, para, updateID) {
    $.ajax({
        url: url,
        type: 'GET',
        data: para,
        dataType: 'html',
        success: function (result) {
            $("#" + updateID).html(result);
        }, error: function (result) {
            alert(result.responseText);
        }
    });

}
function GetPartial(url, para, updateID) {
    $.ajax({
        url: url,
        type: 'GET',
        data: para,
        dataType: 'html',
        success: function (result) {
            $("#" + updateID).html(result);
        }, error: function (result) {
            alert(result.responseText);
        }
    });
}

function GetPartialThen(url, para, updateID) {
    $.ajax({
        url: url,
        type: 'GET',
        async: false,
        data: para,
        dataType: 'html',
        success: function (result) {
            $("#" + updateID).html(result);
        }, error: function (result) {
            alert(result.responseText);
        }
    });
}
//Lấy url ảnh ckeditor
function updateValue(id, value) {
    var dialog = CKEDITOR.dialog.getCurrent();
    dialog.setValueOf('info', 'txtUrl', value);
}
var listColorChart = [
    '#3366CC',
    '#DC3912',
    '#FF9900',
    '#109618',
    '#990099',
    '#3B3EAC',
    '#0099C6',
'#DD4477',
'#66AA00',
'#B82E2E',
'#316395',
'#994499',
'#22AA99',
'#AAAA11',
'#6633CC',
'#E67300',
'#8B0707',
'#329262',
'#5574A6',
'#3B3EAC'
];
function getColor(id) {
    if (id < listColorChart.length)
        return listColorChart[id];
    else
        return listColorChart[0];
}

//Page


function ActionPaging(id, total, updateID, process) {
    //process(id);
    if (process.length > 0) {
        var cb_fn = window[process];
        cb_fn(id);
    }
    GenPaging(id, total, updateID, process);

}


//Genpage 26/09/2017
function GenPaging(index, total, targetID, process) {
    var strPage = "";
    strPage += '<li><a href="javascript:ActionPaging(' + 1 + ',' + total + ',\'' + targetID + '\',\'' + process + '\');">Trang đầu</a></li>';
    if (index > 3) {
        strPage += '<li class="disabled"><a href="javascript:void(0);">...</a></li>';
    }
    for (var i = -3; i <= 3; i++) {
        var page = i + index;
        if (i == 0) {
            strPage += '<li class="active"><a href="javascript:void(0)">' + page + '</a></li>';
        } else {
            if (page > 0 && page <= total) {
                strPage += '<li><a href="javascript:ActionPaging(' + page + ',' + total + ',\'' + targetID + '\',\'' + process + '\');">' + page + '</a></li>';
            }

        }
    }
    if (index + 3 < total) {
        strPage += '<li class="disabled"><a href="javascript:void(0);">...</a></li>';
    }
    strPage += '<li><a href="javascript:ActionPaging(' + total + ',' + total + ',\'' + targetID + '\',\'' + process + '\');">Trang cuối</a></li>';


    $("#" + targetID).html(strPage);

}

var convertDate = function (dateObject) {
    var d = new Date(dateObject);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date = day + "/" + month + "/" + year;

    return date;
};


var convertDateTime = function (dateObject) {
    var d = new Date(dateObject);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date = day + "/" + month + "/" + year +" "+ d.getHours()+":"+ d.getMinutes();

    return date;
};

function DefineCssPageList() {
    $(".pagination>li:first>a").attr("href", "javascript:GetDataPage(1)");
    $(".pagination>li>a").click(function () {
        $(".pagination>li").removeClass("active");
        $(this).parent().addClass("active");
    })
}
function updateValue(id, value) {
    var dialog = CKEDITOR.dialog.getCurrent();
    dialog.setValueOf('info', 'txtUrl', value);
}

function RemoveUnicode(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
}



function SummaryText(str, len) {
    if (str.length > len) {
        return str.substring(0, len) + "...";
    } else {
        return str;
    }
}

function WaitingLoad_Start() {
    waitingDialog.show('Đang xử lý...', {

        // if the option is set to boolean false, it will hide the header and "message" will be set in a paragraph above the progress bar.
        // When headerText is a not-empty string, "message" becomes a content above the progress bar and headerText string will be set as a text inside the H3;
        headerText: '',

        // this will generate a heading corresponding to the size number
        headerSize: 3,

        // extra class(es) for the header tag
        headerClass: '',

        // bootstrap postfix for dialog size, e.g. "sm", "m"
        dialogSize: 'sm',

        // bootstrap postfix for progress bar type, e.g. "success", "warning";
        progressType: 'success',

        // determines the tag of the content element
        contentElement: 'p',

        // extra class(es) for the content tag
        contentClass: 'content'

    });

}
function WaitingLoad_End() {
    waitingDialog.hide()
}
