$(document).ready(function () {
    $("#tinhhinh-odich").show();
    $(".chuyen").hide();

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, yearRange: "-120:+0", maxDate: 0 });

    $("#DANTOC").val($("#hdn_DANTOC_ID").val());
    $("#NGHENGHIEP").val($("#hdn_NGHENGHIEP_ID").val());
    $("#TINH_ID_CREATE").val($("#hdn_TINH_ID").val());
    $("#HUYEN_ID_CREATE").val($("#hdn_HUYEN_ID").val());
    $("#XA_ID_CREATE").val($("#hdn_XA_ID").val());

    $('#NGHENGHIEP').change(function () {
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
            $("#NGAYRAVIENRegex").hide();
            $("#NGAYRAVIENNull").hide();
            $("#NGAYNHAPVIEN").removeAttr("disabled");
            $("#NGAYNHAPVIEN").focus();
        }
        else {
            $("#NGAYNHAPVIEN").attr("disabled", "disabled");
        }
        //ra viện hoặc tử vong
        if (tinhtrang == 2 || tinhtrang == 3) {
            $("#NGAYNHAPVIENRegex").hide();
            $("#NGAYNHAPVIENNull").hide();
            $("#NGAYRAVIEN").removeAttr("disabled");
            $("#NGAYRAVIEN").focus();
        }
        else {
            $("#NGAYRAVIEN").attr("disabled", "disabled");
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
            $(".tinhtrangkhac").hide();
        }
    });
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


    $('input[name=XACDINHTHBDAUTIEN]').change(function () {
        var thbDauTien = $('input[name=XACDINHTHBDAUTIEN]:radio:checked').val();
        //thbDauTien == 0 là có xác định trường hợp bệnh
        if (thbDauTien != undefined) {
            $("#tinhhinh-odich").show();
            if (thbDauTien == 0) {
                $("#tt-thb").show();
                $("#HOTEN").focus();
            }
            else {
                $("#tt-thb").hide();
            }
        }
        else {
            $("#tinhhinh-odich").hide();
        }
    });

    $('input[name=SUDUNGVACXIN]').change(function () {
        $('input[name=SUDUNGVACXIN]').attr("checked", false);
        $(this).attr("checked", true);
        var sdVacXin = $('input[name=SUDUNGVACXIN]:checkbox:checked').val();
        if (sdVacXin == 0) {
            $(".CoVacXin").show();
        }
        else {
            $(".CoVacXin").hide();
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
    $("#TINH_ID_CREATE_ODICH").change(function () {
        var tinh_tp = $("#TINH_ID_CREATE_ODICH").val().trim().length;
        if (tinh_tp <= 0) {
            $("#TINH_ID_CREATE_ODICHNull").show();
        }
        else { $("#TINH_ID_CREATE_ODICHNull").hide(); }
    });
    $("#HUYEN_ID_CREATE_ODICH").change(function () {
        var url_loader = "/ODichArea/ODich/LoadDiaDiem";
        // lấy id của tỉnh, huyện, xã đã được chọn
        var tinh_id = $("#TINH_ID_CREATE_ODICH").val();
        var huyen_id = $("#HUYEN_ID_CREATE_ODICH").val();
        var xa_id = $("#XA_ID_CREATE_ODICH").val();
        if (huyen_id.length == 0) {
            huyen_id = -1;
        }
        if (xa_id.length == 0) {
            xa_id = -1;
        }
        var TYPE = 2;
        if (tinh_id.length >= 3) {
            $.ajax({
                url: url_loader,
                type: 'POST',
                data: { TINH_ID: tinh_id, HUYEN_ID: huyen_id, XA_ID: xa_id, TYPE: TYPE },
                success: function (data) {
                    $("#HUYEN_ID_CREATE_ODICHNull").hide();
                    $("#XA_ID_CREATE_ODICHNull").hide();
                    var items = "";
                    if (TYPE == 1) {
                        items = "<option value='-1'>[-- Quận/Huyện --]</option>";
                    } else if (TYPE == 2) {
                        items = "<option value='-1'>[-- Phường/Xã --]</option>";
                    }
                    $.each(data, function (i, type) {
                        //Tỉnh load Huyện
                        if (TYPE == 1) {
                            items += "<option value='" + type.HUYEN_ID + "'>" + type.TENHUYEN + "</option>";
                        }
                        //Huyện load xã
                        if (TYPE == 2) {
                            items += "<option value='" + type.XA_ID + "'>" + type.TENXA + "</option>";
                        }
                    });
                    if (TYPE == 1) {
                        $("#HUYEN_ID_CREATE_ODICH").html(items);
                        $('#XA_ID_CREATE_ODICH').html("<option value='-1'>[-- Phường/Xã --]</option>");
                    }
                    //Huyện load xã
                    if (TYPE == 2) {
                        $('#XA_ID_CREATE_ODICH').html(items);
                        $(".rowcm").each(function () {
                            var id = $(this).attr("data-id");
                            $('#XA_ID_CAMAC_' + id).html(items);
                        });
                        $(".rowxn").each(function () {
                            var id = $(this).attr("data-id");
                            $('#XA_ID_XN_' + id).html(items);
                        });
                    }
                },
                error: function (data) {
                    $('#HUYEN_ID_CREATE_ODICH').html("<option value='-1'>[-- Quận/Huyện --]</option>");
                    $('#XA_ID_CREATE_ODICH').html("<option value='-1'>[-- Phường/Xã --]</option>");
                }
            });
        }
        else {
            $('#HUYEN_ID_CREATE_ODICH').html("<option value='-1'>[-- Quận/Huyện --]</option>");
            $('#XA_ID_CREATE_ODICH').html("<option value='-1'>[-- Phường/Xã --]</option>");
        }
    });
    $("#XA_ID_CREATE_ODICH").change(function () {
        var xa = $("#XA_ID_CREATE_ODICH").val().trim().length;
        if (xa <= 0) {
            $("#XA_ID_CREATE_ODICHNull").show();
        }
        else { $("#XA_ID_CREATE_ODICHNull").hide(); }
    });
    $("#BENHTRUYENNHIEM_ID").change(function () {
        var benhtruyennhiem = $("#BENHTRUYENNHIEM_ID").val().trim().length;
        if (benhtruyennhiem <= 0) {
            $("#BENHTRUYENNHIEMNull").show();
        }
        else { $("#BENHTRUYENNHIEMNull").hide(); }
    });

    $("#TRANGTHAI").change(function () {
        var trangthai = $("#TRANGTHAI").val().trim().length;
        if (trangthai <= 0) {
            $("#TRANGTHAINull").show();
        }
        else { $("#TRANGTHAINull").hide(); }
    });
    $("#TENODICH").keyup(function () {
        var TENODICH = $("#TENODICH").val().trim().length;
        if (TENODICH <= 0) {
            $("#TENODICHNull").show();
        }
        else { $("#TENODICHNull").hide(); }
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
    $("#BENHTRUYENNHIEM_ID").change(function () {
        var benhtruyennhiem = $("#BENHTRUYENNHIEM_ID").val().trim().length;
        if (benhtruyennhiem <= 0) {
            $("#BENHTRUYENNHIEMNull").show();
        }
        else { $("#BENHTRUYENNHIEMNull").hide(); }
    });

    $("#TRANGTHAI").change(function () {
        var trangthai = $("#TRANGTHAI").val().trim().length;
        if (trangthai <= 0) {
            $("#TRANGTHAINull").show();
        }
        else { $("#TRANGTHAINull").hide(); }
    });

    $("#TENODICH").keyup(function () {
        var TENODICH = $("#TENODICH").val().trim().length;
        if (TENODICH <= 0) {
            $("#TENODICHNull").show();
        }
        else { $("#TENODICHNull").hide(); }
    });

    $(".liveseachthb").keyup(function (event) {
        $(".widget-doituong-content").html("");
        $(".widget-doituong-content").hide();
        var keycode = event.keyCode;

        if (keycode == 9) {
            $(".widget-doituong-content").html("");
            $(".widget-doituong-content").hide();
        }
        else {
            var ELEMENT_ID = $(this).attr('id');
            var MINLENAUTO = $(this).attr('data-len');
            var ELEMENT_VAL = $(this).val().trim();
            var searchContinue = false;
            if (event.which == 32 || event.which == 13) {
                searchContinue = true;
            }
            else {
                searchContinue = false;
            }
            if (searchContinue == true) {
                //auto complete
                if (ELEMENT_VAL.length >= MINLENAUTO) {
                    var param = {
                        keyword: $("#KEYWORD").val().trim(),
                        tinh_id: $("#TINH_TIMKIEM_ID").val(),
                        huyen_id: $("#HUYEN_TIMKIEM_ID").val(),
                        xa_id: $("#XA_TIMKIEM_ID").val(),
                        diachi: $("#DIACHITIMKIEM").val().trim()
                    };
                    $.ajax({
                        dataType: "html",
                        type: 'GET',
                        url: "/TruongHopBenhArea/GhiNhanTruongHopBenh/SearchDoiTuongMacBenh",
                        data: param,
                        success: function (data) {
                            $("#RESULT_" + ELEMENT_ID).html(data);
                            $("#RESULT_" + ELEMENT_ID).show();
                            $("#" + ELEMENT_ID).focus();
                        }
                    });
                }
                else {
                    $("#RESULT_" + ELEMENT_ID).hide();
                }
            }
        }
    });


    $(".liveseachthb").mouseup(function () {
        var ELEMENT_ID = $(this).attr('id');
        var MINLENAUTO = $(this).attr('data-len');
        var ELEMENT_VAL = $(this).val().trim();
        if (ELEMENT_VAL.length >= MINLENAUTO) {
            var param = {
                keyword: $("#KEYWORD").val().trim(),
                tinh_id: $("#TINH_TIMKIEM_ID").val(),
                huyen_id: $("#HUYEN_TIMKIEM_ID").val(),
                xa_id: $("#XA_TIMKIEM_ID").val(),
                diachi: $("#DIACHITIMKIEM").val().trim()
            };
            $.ajax({
                dataType: "html",
                type: 'GET',
                url: "/TruongHopBenhArea/GhiNhanTruongHopBenh/SearchDoiTuongMacBenh",
                data: param,
                success: function (data) {
                    $("#RESULT_" + ELEMENT_ID).html(data);
                    $("#RESULT_" + ELEMENT_ID).show();
                    $("#" + ELEMENT_ID).focus();
                }
            });
        }
        else {
            $("#RESULT_" + ELEMENT_ID).hide();
        }
    });
    $("#search_dt").click(function () {
        $("#search_content_dt").toggle("slow");
        if ($("#search_content_dt").attr("class") == "hide") {
            $("#search_content_dt").removeClass("hide").addClass("show");
            $("#KEYWORD").focus();
        }
        else {
            $("#search_content_dt").removeClass("show").addClass("hide");
            $("#HOTEN").focus();
        }
    });
    $("#NGAYKHOIPHAT").datepicker("option", "minDate", $("#NGAYSINH").val());
    $("#NGAYNHAPVIEN").datepicker("option", "minDate", $("#NGAYSINH").val());
    $("#NGAYRAVIEN").datepicker("option", "minDate", $("#NGAYSINH").val());
});