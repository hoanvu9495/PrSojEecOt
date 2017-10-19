$(document).ready(function () {
    $("#DANTOC").val($("#hdn_DANTOC_ID").val());
    $("#NGHENGHIEP").val($("#hdn_NGHENGHIEP_ID").val());
    $("#TINH_ID_CREATE").val($("#hdn_TINH_ID").val());
    $("#HUYEN_ID_CREATE").val($("#hdn_HUYEN_ID").val());
    $("#XA_ID_CREATE").val($("#hdn_XA_ID").val());
    $(".chuyen").hide();
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0" });
    $("#NGAYSINH").datepicker({
        dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0", maxDate: '0',
        onSelect: function (date) {
            if (date.length <= 0) {
                $("#NGAYSINHNull").show();
            } else {
                $("#NGAYSINHNull").hide();
                var ngaysinh = $("#NGAYSINH").datepicker('getDate');
                var ngaysinh_date = new Date(ngaysinh.getTime());
                ngaysinh_date.setDate(ngaysinh_date.getDate());
                $("#NGAYKHOIPHAT").datepicker("option", "minDate", ngaysinh_date);
                $("#NGAYNHAPVIEN").datepicker("option", "minDate", ngaysinh_date);
                $("#NGAYRAVIEN").datepicker("option", "minDate", ngaysinh_date);
            }
        }
    });
    $("#NGAYKHOIPHAT").datepicker({
        dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0", maxDate: '0',
        onSelect: function () {
            if ($("#NGAYKHOIPHAT").val().trim().length <= 0) {
                $("#NGAYKHOIPHATNull").show();
            } else {
                $("#NGAYKHOIPHATNull").hide();
            }
        }
    });
    $("#NGAYNHAPVIEN").datepicker({
        dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0", maxDate: '0',
        onSelect: function () {
            if ($("#NGAYNHAPVIEN").val().trim().length <= 0) {
                $("#NGAYNHAPVIENNull").show();
            } else {
                $("#NGAYNHAPVIENNull").hide();
            }
        }
    });
    $("#NGAYRAVIEN").datepicker({
        dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0", maxDate: '0',
        onSelect: function () {
            if ($("#NGAYRAVIEN").val().trim().length <= 0) {
                $("#NGAYRAVIENNull").show();
            } else {
                $("#NGAYRAVIENNull").hide();
            }
        }
    });
    $("#NGHENGHIEP").change(function () {
        var nghe = $("#NGHENGHIEP").val().trim().length;
        if (nghe <= 0) {
            $("#NGHENGHIEPNull").show();
        }
        else { $("#NGHENGHIEPNull").hide(); }
    });
    $('#DANTOC').change(function () {
        var dantoc = $('#DANTOC').val();
        if (dantoc.length > 0) {
            $("#DANTOCNull").hide();
        }
    });
    $("input[name='GIOITINH']").change(function () {
        $("#GIOITINHNull").hide();
    });
    $('input[name=BENHCHUANDOAN]').change(function () {
        var benhchuandoan = $('input[name=BENHCHUANDOAN]:checked').val();
        if (benhchuandoan != undefined) {
            $("#required-benh").hide();
            $(".dsbenhtbl").css("border-color", "#6b7076");
        }
    });
    $('input[name=SUDUNGVACXIN]').change(function () {
        $('input[name=SUDUNGVACXIN]').attr("checked", false);
        $(this).attr("checked", true);
        var sdVacXin = $('input[name=SUDUNGVACXIN]:checked').val();
        if (sdVacXin == 0) {
            $(".CoVacXin").show();
            $("#SOLANSUDUNG").focus();
        }
        else {
            $(".CoVacXin").hide();
        }
    });
    $('input[name=PHANLOAICHUANDOAN]').change(function () {
        $('input[name=PHANLOAICHUANDOAN]').attr("checked", false);
        $(this).attr("checked", true);
        var phanloai = $('input[name=PHANLOAICHUANDOAN]:checked').val();
        if (phanloai != undefined) {
            $("#required-phanloai").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        if (phanloai == "1") {
            $(".thongtinxetnghiem").show();
            //hiện thông tin về xét nghiệm
        }
        else if (phanloai == "0") {
            $(".thongtinxetnghiem").hide();
            //ẩn thông tin về xét nghiệm
        }
    });
    $('input[name=LAYMAUXETNGHIEM]').change(function () {
        $('input[name=LAYMAUXETNGHIEM]').attr("checked", false);
        $(this).attr("checked", true);
        var mau = $('input[name=LAYMAUXETNGHIEM]:checked').val();
        if (mau != undefined) {
            $("#required-mauxn").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        if (mau == "0") {
            $(".loaixetnghiem").show();
            //hiện thông tin về loại xét nghiệm
        }
        else {
            $(".loaixetnghiem").hide();
            //ẩn thông tin về loại xét nghiệm
        }
    });
    $('input[name=LOAIXETNGHIEM]').change(function () {
        $('input[name=LOAIXETNGHIEM]').attr("checked", false);
        $(this).attr("checked", true);
        var loai = $('input[name=LOAIXETNGHIEM]:checked').val();
        if (loai != undefined) {
            $("#required-loaixn").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        //loại xét nghiệm khác
        if (loai == 3) {
            $(".loaixnkhac").fadeIn(100);
            $("#LOAIXETNGHIEMKHAC").focus();
        }
        else {
            $(".loaixnkhac").hide();
        }
    });
    $('input[name=KETQUAXETNGHIEM]').change(function () {
        $('input[name=KETQUAXETNGHIEM]').attr("checked", false);
        $(this).attr("checked", true);
        var kq = $('input[name=KETQUAXETNGHIEM]:checked').val();
        $(".tbl-chuandoan").css("border-color", "#6b7076");
        if (kq != undefined) {
            $("#required-ketqua").hide();
        }
    });
    $('input[name=TINHTRANGHIENNAY]').change(function () {
        $('input[name=TINHTRANGHIENNAY]').attr("checked", false);
        $(this).attr("checked", true);
        //TINHTRANGHIENNAY co value = 4 la chuyển viện
        var tinhtrang = $('input[name=TINHTRANGHIENNAY]:checked').val();
        if (tinhtrang != undefined) {
            $("#required-tinhtrang").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        //điều trị nội trú
        if (tinhtrang == 1) {
            $("#NGAYNHAPVIEN").removeAttr("disabled");
            $("#NGAYNHAPVIEN").focus();
        }
        else {
            $("#NGAYNHAPVIEN").attr("disabled", "disabled");
        }
        //ra viện hoặc tử vong
        if (tinhtrang == 2 || tinhtrang == 3) {
            $("#NGAYRAVIEN").removeAttr("disabled");
            $("#NGAYRAVIEN").focus();
            //tình trạng ra viện
            if (tinhtrang == 2) {
                $(".tinhtrangravien").fadeIn(100);
            }
            else {
                $(".tinhtrangravien").hide();
            }
        }
        else {
            $("#NGAYRAVIEN").attr("disabled", "disabled");
            $(".tinhtrangravien").hide();
        }
        //chuyển viện
        if (tinhtrang == 4) {
            $(".chuyen").fadeIn(100);
            $("#BENHVIENCHUYENTOI").focus();
        }
        else {
            $(".chuyen").hide();
        }
        //tình trạng khác
        if (tinhtrang == 5) {
            $(".tinhtrangkhac").fadeIn(100);
            $("#TINHTRANGKHAC").focus();
        }
        else {
            $(".tinhtrangkhac").hide();
        }
    });
    $("#TINH_ID_CREATE").change(function () {
        var tinh_tp = $("#TINH_ID_CREATE").val().trim().length;
        if (tinh_tp <= 0) {
            $("#TINH_ID_CREATENull").show();
        }
        else { $("#TINH_ID_CREATENull").hide(); }
    });
    $("#HUYEN_ID_CREATE").change(function () {
        var huyen = $("#HUYEN_ID_CREATE").val().trim().length;
        if (huyen <= 0) {
            $("#HUYEN_ID_CREATENull").show();
        }
        else { $("#HUYEN_ID_CREATENull").hide(); }
    });
    $("#XA_ID_CREATE").change(function () {
        var xa = $("#XA_ID_CREATE").val().trim().length;
        if (xa <= 0) {
            $("#XA_ID_CREATENull").show();
        }
        else { $("#XA_ID_CREATENull").hide(); }
    });
    $('#DANTOC').change(function () {
        var dantoc = $('#DANTOC').val();
        if (dantoc.length > 0) {
            $("#DANTOCNull").hide();
        }
    });
    $("#HOTEN").keyup(function () {
        var HOTEN = $("#HOTEN").val().trim().length;
        if (HOTEN <= 0) {
            $("#HOTENNull").show();
        }
        else { $("#HOTENNull").hide(); }
    });
    $("#CMND").keyup(function () {
        var CMND = $("#CMND").val().trim().length;
        if (CMND <= 0) {
            $("#CMNDLenght").show();
        }
        else { $("#CMNDLenght").hide(); $("#CMNDNumber").hide(); }
    });
    $("#SOLANSUDUNG").keyup(function () {
        var SOLANSUDUNG = $("#SOLANSUDUNG").val().trim().length;
        if (SOLANSUDUNG <= 0) {
            $("#SOLANSUDUNGNull").show();
        }
        else { $("#SOLANSUDUNGNull").hide(); $("#SOLANSUDUNGNumber").hide(); }
    });
    $("#LOAIXETNGHIEMKHAC").keyup(function () {
        var LOAIXETNGHIEMKHAC = $("#LOAIXETNGHIEMKHAC").val().trim().length;
        if (LOAIXETNGHIEMKHAC <= 0) {
            $("#LOAIXETNGHIEMKHACNull").show();
        }
        else { $("#LOAIXETNGHIEMKHACNull").hide(); }
    });
    $("#TINHTRANGKHAC").keyup(function () {
        var TINHTRANGKHAC = $("#TINHTRANGKHAC").val().trim().length;
        if (TINHTRANGKHAC <= 0) {
            $("#TINHTRANGKHACNull").show();
        }
        else { $("#TINHTRANGKHACNull").hide(); }
    });
    $("input[name='GIOITINH']").change(function () {
        $("#GIOITINHNull").hide();
    });
    $('input[name=BENHCHUANDOAN]').change(function () {
        var benhchuandoan = $('input[name=BENHCHUANDOAN]:checked').val();
        if (benhchuandoan != undefined) {
            $("#required-benh").hide();
            $(".dsbenhtbl").css("border-color", "#6b7076");
        }
    });
    $('input[name=SUDUNGVACXIN]').change(function () {
        $('input[name=SUDUNGVACXIN]').attr("checked", false);
        $(this).attr("checked", true);
        var sdVacXin = $('input[name=SUDUNGVACXIN]:checked').val();
        if (sdVacXin == 0) {
            $(".CoVacXin").show();
            $("#SOLANSUDUNG").focus();
        }
        else {
            $(".CoVacXin").hide();
        }
    });
    $('input[name=PHANLOAICHUANDOAN]').change(function () {
        $('input[name=PHANLOAICHUANDOAN]').attr("checked", false);
        $(this).attr("checked", true);
        var phanloai = $('input[name=PHANLOAICHUANDOAN]:checked').val();
        if (phanloai != undefined) {
            $("#required-phanloai").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        if (phanloai == "1") {
            $(".thongtinxetnghiem").show();
            //hiện thông tin về xét nghiệm
        }
        else if (phanloai == "0") {
            $(".thongtinxetnghiem").hide();
            //ẩn thông tin về xét nghiệm
        }
    });
    $('input[name=LAYMAUXETNGHIEM]').change(function () {
        $('input[name=LAYMAUXETNGHIEM]').attr("checked", false);
        $(this).attr("checked", true);
        var mau = $('input[name=LAYMAUXETNGHIEM]:checked').val();
        if (mau != undefined) {
            $("#required-mauxn").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        if (mau == "0") {
            $(".loaixetnghiem").show();
            //hiện thông tin về loại xét nghiệm
        }
        else {
            $(".loaixetnghiem").hide();
            //ẩn thông tin về loại xét nghiệm
        }
    });
    $('input[name=LOAIXETNGHIEM]').change(function () {
        $('input[name=LOAIXETNGHIEM]').attr("checked", false);
        $(this).attr("checked", true);
        var loai = $('input[name=LOAIXETNGHIEM]:checked').val();
        if (loai != undefined) {
            $("#required-loaixn").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        //loại xét nghiệm khác
        if (loai == 3) {
            $(".loaixnkhac").fadeIn(100);
            $("#LOAIXETNGHIEMKHAC").focus();
        }
        else {
            $(".loaixnkhac").hide();
        }
    });
    $('input[name=KETQUAXETNGHIEM]').change(function () {
        $('input[name=KETQUAXETNGHIEM]').attr("checked", false);
        $(this).attr("checked", true);
        var kq = $('input[name=KETQUAXETNGHIEM]:checked').val();
        $(".tbl-chuandoan").css("border-color", "#6b7076");
        if (kq != undefined) {
            $("#required-ketqua").hide();
        }
    });
    $('input[name=TINHTRANGHIENNAY]').change(function () {
        $('input[name=TINHTRANGHIENNAY]').attr("checked", false);
        $(this).attr("checked", true);
        //TINHTRANGHIENNAY co value = 4 la chuyển viện
        var tinhtrang = $('input[name=TINHTRANGHIENNAY]:checked').val();
        if (tinhtrang != undefined) {
            $("#required-tinhtrang").hide();
            $(".tbl-chuandoan").css("border-color", "#6b7076");
        }
        //điều trị nội trú
        if (tinhtrang == 1) {
            $("#NGAYNHAPVIENRegex").hide();
            $("#NGAYNHAPVIENNull").hide();
            $("#NGAYNHAPVIEN").removeAttr("disabled");
            $("#NGAYNHAPVIEN").focus();
        }
        else {
            $("#NGAYNHAPVIEN").attr("disabled", "disabled");
        }
        //ra viện hoặc tử vong
        if (tinhtrang == 2 || tinhtrang == 3) {
            $("#NGAYRAVIENRegex").hide();
            $("#NGAYRAVIENNull").hide();
            $("#NGAYRAVIEN").removeAttr("disabled");
            $("#NGAYRAVIEN").focus();
            //tình trạng ra viện
            if (tinhtrang == 2) {
                $(".tinhtrangravien").fadeIn(100);
            }
            else {
                $(".tinhtrangravien").hide();
            }
        }
        else {
            $("#NGAYRAVIEN").attr("disabled", "disabled");
            $(".tinhtrangravien").hide();
        }
        //chuyển viện
        if (tinhtrang == 4) {
            $("#NGAYRAVIENRegex").hide();
            $("#NGAYRAVIENNull").hide();
            $("#NGAYNHAPVIENRegex").hide();
            $("#NGAYNHAPVIENNull").hide();
            $(".chuyen").fadeIn(100);
            $("#BENHVIENCHUYENTOI").focus();
        }
        else {
            $(".chuyen").hide();
        }
        //tình trạng khác
        if (tinhtrang == 5) {
            $("#NGAYRAVIENRegex").hide();
            $("#NGAYRAVIENNull").hide();
            $("#NGAYNHAPVIENRegex").hide();
            $("#NGAYNHAPVIENNull").hide();
            $(".tinhtrangkhac").fadeIn(100);
            $("#TINHTRANGKHAC").focus();
        }
        else {
            $("#NGAYRAVIENRegex").hide();
            $("#NGAYRAVIENNull").hide();
            $("#NGAYNHAPVIENRegex").hide();
            $("#NGAYNHAPVIENNull").hide();
            $(".tinhtrangkhac").hide();
        }
    });
    $("#NGAYKHOIPHAT").datepicker("option", "minDate", $("#NGAYSINH").val());
    $("#NGAYNHAPVIEN").datepicker("option", "minDate", $("#NGAYSINH").val());
    $("#NGAYRAVIEN").datepicker("option", "minDate", $("#NGAYSINH").val());
});
$("#ds_benh_h3").click(function () {
    $("#ds_benh_table").toggle("slow");
});
$("#ThongBaoThanhCong").dialog({
    autoOpen: false,
    modal: true,
    width: 630,
    height: 115,
    resizable: false,
    show: {
        effect: "blind",
        duration: 200
    },
    hide: {
        effect: "blind",
        duration: 200
    }
});
$(".SearchResult").click(function () {
    $("#DANHSACHTHBTRONGNGAY").hide();
    $(".widget-doituong-content").hide();
});

function LoadTruongHopBenhTrongNgay() {
    $.ajax({
        dataType: "html",
        type: 'GET',
        url: "/TruongHopBenhArea/GhiNhanTruongHopBenh/TruongHopBenhTrongNgay",
        data: {},
        success: function (data) {
            $("#DANHSACHTHBTRONGNGAY").html(data);
        }
    });
}


function CloseThongBao() {
    $("#ThongBaoThanhCong").dialog("close");
}
function ShowDateTime(id) {
    $("#" + id).datepicker({ dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0" });
}
function addDiaDiem(id) {
    $.ajax({
        url: '/TruongHopBenhArea/GhiNhanTruongHopBenh/ThemDiaDiemLamViec',
        type: 'post',
        cache: false,
        data: { id: id },
        success: function (data) {
            var append_id = id - 1;
            var tr_appen = $("#tr_f_" + append_id);
            if (append_id > 0) {
                tr_appen.after(data);
            }
            else {
                $("#tr_first").after(data);
            }
            var new_id = id + 1;
            $("#addDiaDiemLamViecDiChuyen").attr("href", "javascript:addDiaDiem(" + new_id + ")");
            $("#max_item").val(new_id);
            $("#THOIGIANBATDAU_" + id).mouseover();
            $("#THOIGIANKETTHUC_" + id).mouseover();
        },
        error: function (xhr) {
            CommonJS.alert(xhr.responseText);
        }
    });
}
function DeleteDiaDiemLamViec(id) {
    var conf = confirm("Bạn chắc chắn muốn xóa địa điểm này?");
    if (conf == true) {
        $("#tr_f_" + id).remove();
        $("#tr_" + id).remove();
        var max_item = $("#max_item").val();

        var max_current = 0;
        $(".diadiem-lv-item").each(function () {
            var item_id = $(this).attr("data-id");
            if (item_id >= max_current) {
                max_current = item_id;
            }
        });
        var new_id = parseInt(max_current) + 1;
        $("#addDiaDiemLamViecDiChuyen").attr("href", "javascript:addDiaDiem(" + new_id + ")");
        $("#max_item").val(new_id);
    }
}

function validate_diadiemlamviec() {
    var result = 1;
    var list_item = "";
    $(".diadiem-lv-item").each(function () {
        var id = $(this).attr("data-id");
        var start = $("#THOIGIANBATDAU_" + id).val().trim().length;
        var end = $("#THOIGIANKETTHUC_" + id).val().trim().length;
        var diachi = $("#DIADIEMLAMVIEC_" + id).val().trim().length;
        var tinh = -1;
        var tinh_id = $("#TINH_ID_DIADIEM_" + id).val().trim();
        if (tinh_id != "") {
            tinh = parseInt($("#TINH_ID_DIADIEM_" + id).val().trim());
        }
        var huyen = parseInt($("#HUYEN_ID_DIADIEM_" + id).val().trim());
        var xa = parseInt($("#XA_ID_DIADIEM_" + id).val().trim());
        if (start > 0 || end > 0 || diachi > 0 || tinh > 0 || huyen > 0 || xa > 0) {
            list_item += id + ",";
        }
        //neu nhap ngay bat dau hoac ngay ket thuc
        if (start > 0 || end > 0) {
            if (start > 0 && end <= 0) {
                $("#DIADIEMLAMVIECNull_" + id).html("Bạn phải nhập thông tin ngày kết thúc");
                $("#DIADIEMLAMVIECNull_" + id).show();
                $("#THOIGIANKETTHUC_" + id).focus();
                result = 0;
                return false;
            }
            else if (start <= 0 && end > 0) {
                $("#DIADIEMLAMVIECNull_" + id).html("Bạn phải nhập thông tin ngày bắt đầu");
                $("#DIADIEMLAMVIECNull_" + id).show();
                $("#THOIGIANBATDAU_" + id).focus();
                result = 0;
                return true;
            }
            else {
                //thi phai nhap dia chi
                if (diachi <= 0 && tinh <= 0 && huyen <= 0 && xa <= 0) {
                    $("#DIADIEMLAMVIEC_" + id).focus();
                    $("#DIADIEMLAMVIECNull_" + id).html("Bạn phải nhập thông tin địa chỉ");
                    $("#DIADIEMLAMVIECNull_" + id).show();
                    result = 0;
                    return false;
                }
                else {
                    $("#DIADIEMLAMVIECNull_" + id).hide();
                }
            }
        }
        //Nếu nhập thông tin địa chỉ
        if (diachi > 0 || tinh > 0 || huyen > 0 || xa > 0) {
            //thì phải nhập thông tin ngày tháng
            if (start <= 0 || end <= 0) {
                if (start <= 0) {
                    $("#DIADIEMLAMVIECNull_" + id).html("Bạn phải nhập thông tin ngày bắt đầu");
                    $("#DIADIEMLAMVIECNull_" + id).show();
                    $("#THOIGIANBATDAU_" + id).focus();
                    result = 0;
                    return false;
                }
                else if (end <= 0) {
                    $("#DIADIEMLAMVIECNull_" + id).html("Bạn phải nhập thông tin ngày kết thúc");
                    $("#DIADIEMLAMVIECNull_" + id).show();
                    $("#THOIGIANKETTHUC_" + id).focus();
                    result = 0;
                    return false;
                }
                else {
                    $("#DIADIEMLAMVIECNull_" + id).hide();
                }
            }
            else {
                $("#DIADIEMLAMVIECNull_" + id).hide();
            }
        }
    });
    $("#hdnDiaDiemLamViec").val(list_item);
    return result;
}
function Validate_Gioitinh() {
    var gt = $("input[name='GIOITINH']:checked").val();
    if (typeof gt == "undefined") {
        $('html,body').animate({
            scrollTop: $("#NOILAMVIEC").offset().top
        }, 'slow');
        $("#GIOITINHNull").show();
        return 0;
    }
    else {
        $("#GIOITINHNull").hide();
    }
    return 1;
}


function SelectTab(tab_id) {
    var tab_current = $("li[aria-selected='true'] a").attr("id");
    var current_id = tab_current.split('-')[2];
    if (tab_id != current_id) {
        $("#thbtabs-t-" + tab_id).click();
    }
}
$("#btnSave").click(function () {
    //validate thong tin doi tuong
    //if (validate_element_regex($("#HOTEN"), $("#HOTENNull"), "1", "^[a-zA-Z0-9\s\\\/]+$", $("#HOTENRegex"), "0") == 0) {
    if (validate_element($("#HOTEN"), $("#HOTENNull"), "", "", "0") == 0) {
        SelectTab(0);
        $("#HOTEN").focus();
        return false;
    }
    if (validate_element($("#NGAYSINH"), $("#NGAYSINHNull"), "", "", "0") == 0) {
        SelectTab(0);
        $("#NGAYSINH").focus();
        return false;
    }
    if (checkDateTime($("#NGAYSINH").val()) == false) {
        SelectTab(0);
        $("#NGAYSINH").show();
        $("#NGAYSINH").focus();
        return false;
    }
    else {
        $("#NGAYSINHRegex").hide();
    }
    if (validate_element($("#NGHENGHIEP"), $("#NGHENGHIEPNull"), "", "", "1") == 0) {
        SelectTab(0);
        $("#NGHENGHIEP").focus();
        return false;
    }
    if (validate_element($("#DANTOC"), $("#DANTOCNull"), "", "", "1") == 0) {
        SelectTab(0);
        $("#DANTOC").focus();
        return false;
    }
    if (Validate_Gioitinh() == 0) {
        SelectTab(0);
        return false;
    }
    if ($("#DIENTHOAI").val().length > 0) {
        if (validate_lenght_regex($("#DIENTHOAI"), 10, 11, $("#DIENTHOAILenght"), "1", "^[0-9]+$", $("#DIENTHOAINumber")) == 0) {
            SelectTab(0);
            $("#DIENTHOAI").focus();
            return false;
        }
    }
    if (validate_element($("#TINH_ID_CREATE"), $("#TINH_ID_CREATENull"), "", "", "1") == 0) {
        SelectTab(0);
        $("#TINH_ID_CREATE").focus();
        return false;
    }
    if (validate_element($("#HUYEN_ID_CREATE"), $("#HUYEN_ID_CREATENull"), "", "", "1") == 0) {
        SelectTab(0);
        $("#HUYEN_ID_CREATE").focus();
        return false;
    }
    if (validate_element($("#XA_ID_CREATE"), $("#XA_ID_CREATENull"), "", "", "1") == 0) {
        SelectTab(0)
        $("#XA_ID_CREATE").focus();
        return false;
    }
    if (validate_lenght_regex($("#CMND"), 9, 12, $("#CMNDLenght"), "1", "^[0-9]+$", $("#CMNDNumber")) == 0) {
        SelectTab(0);
        $("#CMND").focus();
        return false;
    }
    //validate thong tin dia diem lam viec
    if (validate_diadiemlamviec() == 0) {
        return false;
    }
    //validate thong tin ca benh
    if ($('input[name=BENHCHUANDOAN]:checked').val() == undefined) {
        SelectTab(1);
        $("#required-benh").show();
        $(".dsbenhtbl").css("border-color", "red");
        return false;
    }
    if ($('input[name=PHANLOAICHUANDOAN]:checked').val() == undefined) {
        SelectTab(1);
        $("#required-phanloai").show();
        $(".tbl-chuandoan").css("border-color", "red");
        return false;
    }
    if ($('input[name=LAYMAUXETNGHIEM]:checked').val() == undefined) {
        SelectTab(1);
        $("#required-mauxn").show();
        $(".tbl-chuandoan").css("border-color", "red");
        return false;
    }
    if ($('input[name=LOAIXETNGHIEM]:checked').val() == undefined) {
        SelectTab(1);
        $("#required-loaixn").show();
        $(".tbl-chuandoan").css("border-color", "red");
        return false;
    }
    if ($('input[name=KETQUAXETNGHIEM]:checked').val() == undefined) {
        SelectTab(1);
        $("#required-ketqua").show();
        $(".tbl-chuandoan").css("border-color", "red");
        return false;
    }
    if ($('input[name=TINHTRANGHIENNAY]:checked').val() == undefined) {
        SelectTab(1);
        $("#required-tinhtrang").show();
        $(".tbl-chuandoan").css("border-color", "red");
        return false;
    }
    var sdVacXin = $('input[name=SUDUNGVACXIN]:checked').val();
    if (sdVacXin == 0) {
        if (validate_element_regex($("#SOLANSUDUNG"), $("#SOLANSUDUNGNull"), "1", "^[0-9]+$", $("#SOLANSUDUNGNumber"), "0") == 0) {
            SelectTab(1);
            $("#SOLANSUDUNG").focus();
            return false;
        }
    }
    if ($('input[name=PHANLOAICHUANDOAN]:checked').val() == 1) {
        if ($('input[name=LAYMAUXETNGHIEM]:checked').val() == 0) {
            var phanloai = $('input[name=PHANLOAICHUANDOAN]:checked').val();
            if (phanloai == 1) {
                var loai = $('input[name=LOAIXETNGHIEM]:checked').val();
                //loại xét nghiệm khác
                if (loai == 3) {
                    if (validate_element($("#LOAIXETNGHIEMKHAC"), $("#LOAIXETNGHIEMKHACNull"), "", "", "0") == 0) {
                        SelectTab(1);
                        $("#LOAIXETNGHIEMKHAC").focus();
                        return false;
                    }
                }
            }
        }
    }
    if (validate_element($("#NGAYKHOIPHAT"), $("#NGAYKHOIPHATNull"), "", "", "0") == 0) {
        SelectTab(1);
        $("#NGAYKHOIPHAT").focus();
        return false;
    }
    if (checkDateTime($("#NGAYKHOIPHAT").val()) == false) {
        SelectTab(1);
        $("#NGAYKHOIPHATRegex").show();
        $("#NGAYKHOIPHAT").focus();
        return false;
    }
    else {
        $("#NGAYKHOIPHATRegex").hide();
    }
    //trạng thái hiện tại
    var tinhtrang = $('input[name=TINHTRANGHIENNAY]:checked').val();
    //điều trị nội trú
    if (tinhtrang == 1) {
        $("#NGAYRAVIENRegex").hide();
        $("#NGAYRAVIENNull").hide();
        if (validate_element($("#NGAYNHAPVIEN"), $("#NGAYNHAPVIENNull"), "", "", "0") == 0) {
            SelectTab(1);
            $("#NGAYNHAPVIEN").focus();
            return false;
        }
        if (checkDateTime($("#NGAYNHAPVIEN").val()) == false) {
            SelectTab(1);
            $("#NGAYNHAPVIENRegex").show();
            $("#NGAYNHAPVIEN").focus();
            return false;
        }
        else {
            $("#NGAYNHAPVIENRegex").hide();
        }
    }
    //ra viện hoặc tử vong
    if (tinhtrang == 2 || tinhtrang == 3) {
        $("#NGAYNHAPVIENRegex").hide();
        $("#NGAYNHAPVIENNull").hide();
        if (validate_element($("#NGAYRAVIEN"), $("#NGAYRAVIENNull"), "", "", "0") == 0) {
            SelectTab(1);
            $("#NGAYRAVIEN").focus();
            return false;
        }
        if (checkDateTime($("#NGAYRAVIEN").val()) == false) {
            SelectTab(1);
            $("#NGAYRAVIENRegex").show();
            $("#NGAYRAVIEN").focus();
            return false;
        }
        else {
            $("#NGAYRAVIENRegex").hide();
        }
    }
    //chuyển viện
    if (tinhtrang == 4) {
        $("#NGAYRAVIENRegex").hide();
        $("#NGAYRAVIENNull").hide();
        $("#NGAYNHAPVIENRegex").hide();
        $("#NGAYNHAPVIENNull").hide();
        if (validate_element($("#BENHVIENCHUYENTOI"), $("#BENHVIENCHUYENTOINull", "0"), "", "") == 0) {
            SelectTab(1);
            $("#BENHVIENCHUYENTOI").focus();
            return false;
        }
    }
    //tình trạng khác
    if (tinhtrang == 5) {
        $("#NGAYRAVIENRegex").hide();
        $("#NGAYRAVIENNull").hide();
        $("#NGAYNHAPVIENRegex").hide();
        $("#NGAYNHAPVIENNull").hide();
        if (validate_element($("#TINHTRANGKHAC"), $("#TINHTRANGKHACNull"), "", "", "0") == 0) {
            SelectTab(1);
            $("#TINHTRANGKHAC").focus();
            return false;
        }
    }
    $("#btnSave").submit();
    return true;
});

$("#HOTEN").focusout(function () {
    if ($("#HOTEN").val().trim().length <= 0) {
        $("#HOTENNull").show();
        //$(this).focus();
    } else {
        $("#HOTENNull").hide();
    }
});
$("#NGAYSINH").focusout(function () {
    if ($("#NGAYSINH").val().trim().length <= 0) {
        $("#NGAYSINHNull").show();
        //$(this).focus();
    } else {
        $("#NGAYSINHNull").hide();
        if (checkDateTime($("#NGAYSINH").val()) == false) {
            $("#NGAYSINHRegex").show();
        } else {
            $("#NGAYSINHRegex").hide();
        }
    }
});
$("#NGAYKHOIPHAT").focusout(function () {
    if ($("#NGAYKHOIPHAT").val().trim().length <= 0) {
        $("#NGAYKHOIPHATNull").show();
        //$(this).focus();
    } else {
        $("#NGAYKHOIPHATNull").hide();
        if (checkDateTime($("#NGAYKHOIPHAT").val()) == false) {
            $("#NGAYKHOIPHATRegex").show();
        } else {
            $("#NGAYKHOIPHATRegex").hide();
        }
    }
});
$("#NGAYNHAPVIEN").focusout(function () {
    if ($("#NGAYNHAPVIEN").val().trim().length <= 0) {
        $("#NGAYNHAPVIENNull").show();
        //$(this).focus();
    } else {
        $("#NGAYNHAPVIENNull").hide();
        if (checkDateTime($("#NGAYNHAPVIEN").val()) == false) {
            $("#NGAYNHAPVIENRegex").show();
        } else {
            $("#NGAYNHAPVIENRegex").hide();
        }
    }
});
$("#NGAYRAVIEN").focusout(function () {
    if ($("#NGAYRAVIEN").val().trim().length <= 0) {
        $("#NGAYRAVIENNull").show();
        //$(this).focus();
    } else {
        $("#NGAYRAVIENNull").hide();
        if (checkDateTime($("#NGAYRAVIEN").val()) == false) {
            $("#NGAYRAVIENRegex").show();
        } else {
            $("#NGAYRAVIENRegex").hide();
        }
    }
});

function failure(xhr, error) {
    CommonJS.alert(xhr.responseText);
}

function updateSuccess() {
    CommonJS.alert("Tạo mới trường hợp bệnh thành công");
    $("#ThongBaoThanhCong").dialog("open");
    LoadTruongHopBenhTrongNgay();
    document.getElementById("frmTruongHopBenhCreate").reset();
}