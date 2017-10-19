//Duynt
function drawBreadCrumbManual(opt_breadCrumbs) {
    $("#ribbon .breadcrumb").html('<li>Home</li>');
    var lstBreadCrumb = opt_breadCrumbs.split("/");
    var total = lstBreadCrumb.length;
    for (var i = 0; i < total; i++) {
        $("#ribbon .breadcrumb").append('<li>' + lstBreadCrumb[i] + '</li>');
    }
}
// NAMDV
function elementInViewport2(el) {
    var top = el.offsetTop;
    var left = el.offsetLeft;
    var width = el.offsetWidth;
    var height = el.offsetHeight;

    while (el.offsetParent) {
        el = el.offsetParent;
        top += el.offsetTop;
        left += el.offsetLeft;
    }

    return (
        top < (window.pageYOffset + window.innerHeight) &&
        left < (window.pageXOffset + window.innerWidth) &&
        (top + height) > window.pageYOffset &&
        (left + width) > window.pageXOffset
    );
}



function elementInViewport(el) {
    var top = el.offsetTop;
    var left = el.offsetLeft;
    var width = el.offsetWidth;
    var height = el.offsetHeight;

    while (el.offsetParent) {
        el = el.offsetParent;
        top += el.offsetTop;
        left += el.offsetLeft;
    }

    return (
        top >= window.pageYOffset &&
        left >= window.pageXOffset &&
        (top + height) <= (window.pageYOffset + window.innerHeight) &&
        (left + width) <= (window.pageXOffset + window.innerWidth)
    );
}


(function ($) {
    $.fn.fixMe = function () {
        return this.each(function () {
            var $this = $(this),
                $t_fixed;
            function init() {
                $this.wrap('<div class="container" />');
                $t_fixed = $this.clone();
                $t_fixed.find("tbody").remove().end().addClass("fixed").insertBefore($this);
                resizeFixed();
            }
            function resizeFixed() {

            }
            function scrollFixed() {
                var offset = $(this).scrollTop(),
                    tableOffsetTop = $this.offset().top,
                    tableOffsetBottom = tableOffsetTop + $this.height() - $this.find("thead").height();
                if (offset < tableOffsetTop || offset > tableOffsetBottom)
                    $t_fixed.hide();
                else if (offset >= tableOffsetTop && offset <= tableOffsetBottom && $t_fixed.is(":hidden"))
                    $t_fixed.show();
            }
            $(window).resize(resizeFixed);
            $(window).scroll(scrollFixed);
            init();
        });
    };
})(jQuery);


var CommonJS = {
    HEADER_HIGHT: 271,
    baseAlert: function (msg) {
        if (msg.Message != null) {
            alert(msg.Message);
            return msg;
        }
        json = {};
        try {
            json = $.parseJSON(msg);
        } catch (e) {
            alert(msg);
            return;
        }

        alert(json.Message);
        return json;
    },

    alert: function (msg) {
        $("#_GlobalMessage").attr("class", "");
        $("#_GlobalMessage").addClass("msg-type-SUCCESS");
        if (msg.Message != null) {
            $("#_GlobalMessage").html(msg.Message);
            $("#_GlobalMessage").fadeIn();
            setTimeout('$("#_GlobalMessage").fadeOut();', 6000);
            return msg;
        }
        json = {};
        try {
            json = $.parseJSON(msg);
        } catch (e) {
            $("#_GlobalMessage").html(msg);
            $("#_GlobalMessage").fadeIn();
            setTimeout('$("#_GlobalMessage").fadeOut();', 6000);
            return;
        }
        if (json == null) {
            $("#_GlobalMessage").addClass("msg-type-ERROR");
            $("#_GlobalMessage").html("null");
        } else {
            $("#_GlobalMessage").addClass("msg-type-" + json.Type);
            $("#_GlobalMessage").html(json.Message);
        }
        $("#_GlobalMessage").fadeIn();
        setTimeout('$("#_GlobalMessage").fadeOut();', 6000);
        return json;
    },

    back: function () {
        history.back(1);
    },

    checkDate: function checkDate(date) {
        var minYear = 1902;
        var errorMsg = "";
        // regular expression to match required date format 
        re = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;

        if (date != '') {
            if (regs = date.match(re)) {
                if (regs[1] < 1 || regs[1] > 31) {
                    errorMsg = "Invalid value for day: " + regs[1];
                    return false;
                }
                else if (regs[2] < 1 || regs[2] > 12) {
                    errorMsg = "Invalid value for month: " + regs[2];
                    return false;
                }
                else if (regs[3] < minYear) {
                    errorMsg = "Invalid value for year: " + regs[3] + " - must be between " + minYear;
                    return false;
                }
            }
            else {
                errorMsg = "Invalid date format: " + date;
                return false;
            }
        }
        else {
            errorMsg = "Empty date not allowed!";
            return false;
        }

        return true;
    },
    isNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },

    makeSizeMatchParent: function (elementID, parentID) {
        var pheight = $("#" + parentID).height();
        var pwidth = $("#" + parentID).width();
        $("#" + elementID).css({
            width: pwidth,
            height: pheight,
        });
    },

    FullScreen: function (elementID, isFull) {
        if (isFull) {
            $("#container").css({
                width: '1px',
                height: '1px',
                overflow: 'hidden'
            });

            $("#" + elementID).css({
                position: 'absolute',
                width: $(window).width() - 3,
                height: $(window).height() - 5,
                top: 0,
                left: 0,
                'z-index': 100,
                overflow: 'hidden',
                background: '#fff'
            });

            $(".form-enter-data").css({
                width: $(window).width() - 290,
                height: $(window).height() - 100,
                'max-height': $(window).height() - 100,
                overflow: 'auto',
            });
            $("#DivGroupTree .panelScroll").width($("#DivGroupTree").width());
        } else {
            $("#container").css({
                width: '',
                height: '',
                overflow: ''
            });
            $("#" + elementID).css({
                position: '',
                width: '',
                height: '',
                top: '',
                left: '',
                'z-index': '',
                overflow: '',
                background: ''
            });

            $(".form-enter-data").css({
                width: '',
                height: '',
                overflow: '',
                'max-height': $(window).height() - CommonJS.HEADER_HIGHT,
                background: ''
            });
            $("#DivGroupTree .panelScroll").width(194);
        }


    },

    EditableFullScreen: function (elementID, isFull) {
        $(window).resize(function () {
            CommonJS.FullScreen(elementID, isFull);
        });
        CommonJS.FullScreen(elementID, isFull);
        if (isFull) {
            $("#btnFullScreen").hide();
            $("#btnUnFull").show();
            $("#TemplateHide").show();
            $("#isFullScreen").val(1);
            $(".PanelContentScroll").css({
                'max-height': $(window).height() - 90,
            });
        } else {
            $("#btnFullScreen").show();
            $("#btnUnFull").hide();
            $("#isFullScreen").val(0);
            $(".PanelContentScroll").css({
                'max-height': $(window).height() - CommonJS.HEADER_HIGHT,
            });
        }
    },

    PrepareEditableFullScreen: function () {
        if ($("#isFullScreen").val() * 1 == 1) {
            $("#btnFullScreen").click();
        } else {
            $("#btnUnFull").click();
        }
    },
    ReloadPage: function () {
        window.location.href = window.location.href;
    },
    ClearAllCache: function () {
        sessionStorage.clear();
    },
    TreeClickFirstLeave: function () {
        var cnode = $($("div.hitarea")[0])
        var _count = 1;
        while (cnode.length > 0) {
            if (!cnode.hasClass("lastCollapsable-hitarea")) {
                cnode.click();
            }
            cnode = $(cnode.parent().find("ul li div.hitarea")[0])
            _count++;
            if (_count > 20) break;
        }

        $($("li span[id^=Template]")[0]).find("a.treeitem").click();
    },
    showNotifySuccess: function (message) {
        notif({
            type: 'success',
            position: 'bottom',
            msg: message,
            autohide: false,
            multiline: true,
            clickable: true
        });
    },
    showNotifyWarning: function (message) {
        notif({
            type: 'warning',
            position: 'bottom',
            msg: message,
            autohide: false,
            multiline: true,
            clickable: true
        });
    },
    showNotifyError: function (message) {
        notif({
            type: 'error',
            position: 'bottom',
            msg: message,
            autohide: false,
            multiline: true,
            clickable: true
        });
    },
    removeVietnameseChars: function (str) {
        str = str.trim().toLowerCase();
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        return str;
    }
}

var excelGridSelectedRowIds = [];
var excelImportData = [];
var gridDataPerPage = 20;
function CommonExcelGrid() {
    //this.toolButtonsId;
    this.selectorId = '';
    this.selectorErrorId = '';
    this.parentId = '';
    this.parentErrorId = '';
    this.pagerId = '';
    this.pagerErrorId = '';
    this.columnNames = [];
    this.columnModels = [];
    this.data = [];
    this.dataError = [];
    this.dataError = [];
    this.tableName = '';
    this.tableErrorName = '';
    this.rowList = [10, 20, 30, 40, 50, 100, 200, 500, 'Tất cả'];

    this.buildGrid = function () {
        var rowList = this.rowList;
        var columnNames = this.columnNames;
        var columnModels = this.columnModels;

        var parentId = this.parentId;
        var parentErrorId = this.parentErrorId;

        var data = this.data;
        var dataError = this.dataError;

        var selectorId = this.selectorId;
        var selectorErrorId = this.selectorErrorId;

        var pagerId = this.pagerId;
        var pagerErrorId = this.pagerErrorId;

        var caption = this.tableName;
        var captionError = this.tableErrorName;
        var saveRowData = function (data, rowId, selectorId, parentId, caption, updateParameters) {
            var row = $('#' + selectorId).getRowData(rowId);
            loadScript('/js/plugin/jqgrid/grid.locale-en.min.js', function () {
                loadScript('/js/plugin/jqgrid/jquery.jqGrid.min.js', function () {
                    $("#" + selectorId).delRowData(rowId);
                    $('#' + selectorId).trigger('reloadGrid');
                    $('#' + selectorId).jqGrid('setCaption', caption + ' ' + '(' + $('#' + selectorId).jqGrid('getGridParam', 'data').length + ')');
                    $('#' + parentId + " .ui-pg-selbox option[value='" + (data.length - 1) + "']").val(data.length);

                });

                if (updateParameters !== null) {
                    updateParameters.data.push(row);
                    $('#' + updateParameters.selectorId).jqGrid('setGridParam', { data: updateParameters.data });
                    $('#' + updateParameters.selectorId).trigger('reloadGrid');
                    $('#' + updateParameters.selectorId).jqGrid('setCaption', updateParameters.caption + ' ' + '(' + updateParameters.data.length + ')');
                    $('#' + updateParameters.parentId + " .ui-pg-selbox option[value='" + (updateParameters.data.length - 1) + "']").val(updateParameters.data.length);
                }
            });
        }
        var pageFunction = function (rowList, data, columnNames, selectorId, columnModels, pagerId, parentId, caption, isDataError, updateParameters) {
            $('#' + selectorId).jqGrid({
                data: data,
                url: 'clientArray',
                editurl: 'clientArray',
                datatype: "local",
                height: 'auto',
                colNames: columnNames,
                colModel: columnModels,
                rowNum: 10,
                rowList: rowList,
                pager: '#' + pagerId,
                viewrecords: true,
                sortorder: "desc",
                caption: caption,
                toolbarfilter: true,
                viewrecords: true,
                multiselect: !isDataError,
                recordtext: "Bản ghi {0} - {1} của {2}",
                emptyrecords: "Không có dữ liệu",
                loadtext: "Đang tải...",
                pgtext: "Trang {0} của {1}",
                gridComplete: function () {
                    var ids = $("#" + selectorId).jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        be = "<button class='btn btn-xs btn-default' data-original-title='Chỉnh sửa' title='Chỉnh sửa' onclick=\"$('#" + selectorId + "').editRow('" + cl + "');\"><i class='fa fa-pencil'></i></button>";
                        se = "<button class='btn btn-xs btn-default' data-original-title='Lưu lại' title='Lưu lại' onclick=\"$('#" + selectorId + "').saveRow('" + cl + "');\"><i class='fa fa-save'></i></button>";
                        ce = "<button class='btn btn-xs btn-default' data-original-title='Thoát' title='Thoát chỉnh sửa' onclick=\"$('#" + selectorId + "').restoreRow('" + cl + "');\"><i class='fa fa-times'></i></button>";
                        $("#" + selectorId).jqGrid('setRowData', ids[i], {
                            act: be + se + ce
                        });
                    }
                    //set row num:
                    $("#" + selectorId).setGridParam({ rowNum: isDataError ? 10 : data.length });
                }, onSelectRow: function (id, status, e) {
                    if (!isDataError) {
                        const index = excelGridSelectedRowIds.indexOf(id);
                        if (status) {
                            if (index === -1) {
                                excelGridSelectedRowIds.push(id);
                            }
                        } else {
                            if (index !== -1) {
                                excelGridSelectedRowIds.splice(index, 1);
                            }
                        }
                    }
                    else {
                        if (e.target.className === 'fa fa-pencil' || (e.target.className === 'btn btn-xs btn-default' && e.target.firstChild.className == 'fa fa-pencil')) {
                            //on edit events
                        } else if (e.target.className === 'fa fa-save' || (e.target.className === 'btn btn-xs btn-default' && e.target.firstChild.className == 'fa fa-save')) {
                            $('#' + selectorId).jqGrid('editRow', id, {
                                keys: true,
                                url: 'clientArray',
                                "oneditfunc": function (rowId) {
                                    $('#' + selectorId).jqGrid('saveRow', rowId, {
                                        successfunc: function (response) {
                                            return true;
                                        }
                                    });
                                    saveRowData(data, rowId, selectorId, parentId, caption, updateParameters);
                                }
                            });
                        } else if (e.target.className === 'fa fa-times' || (e.target.className === 'btn btn-xs btn-default' && e.target.firstChild.className == 'fa fa-times')) {
                            //on exit events
                        }
                    }
                }, onSelectAll: function (id, status) {
                    if (!isDataError) {
                        excelGridSelectedRowIds = status ? id : [];
                    }
                }
            });

            //setting paging table
            var pagerSetting = $("#" + selectorId).jqGrid('navGrid', "#" + pagerId, {
                edit: false,
                add: false,
                del: true
            });

            //reload data incase import multiple times
            $('#' + selectorId).jqGrid('clearGridData');
            $('#' + selectorId).jqGrid('setGridParam', { data: data });
            $('#' + selectorId).jqGrid('setCaption', caption + ' ' + '(' + data.length + ')');
            $('#' + selectorId).trigger('reloadGrid');

            //setting "All options"
            $('#' + parentId + " .ui-pg-selbox option[value='Tất cả']").val(data.length);
            if (!isDataError) {
                $('#' + parentId + " .ui-pg-selbox option").last().prop('selected', true);
            }
            //redesign buttons
            $("#" + parentId + " .ui-pg-div").removeClass().addClass("btn btn-sm btn-primary");
            $("#" + parentId + " .ui-icon.ui-icon-plus").removeClass().addClass("fa fa-plus");
            $("#" + parentId + " .ui-icon.ui-icon-pencil").removeClass().addClass("fa fa-pencil");
            $("#" + parentId + " .ui-icon.ui-icon-trash").removeClass().addClass("fa fa-trash-o");
            //remove trash button
            $('#' + parentId + ' #del_' + selectorId).remove();
            $("#" + parentId + " .ui-icon.ui-icon-search").removeClass().addClass("fa fa-search");
            $("#" + parentId + " .ui-icon.ui-icon-refresh").removeClass().addClass("fa fa-refresh");
            //remove refresh button
            $('#' + parentId + ' #refresh_' + selectorId).remove();
            $("#" + parentId + " .ui-icon.ui-icon-disk").removeClass().addClass("fa fa-save").parent(".btn-primary").removeClass("btn-primary").addClass("btn-success");
            $("#" + parentId + " .ui-icon.ui-icon-cancel").removeClass().addClass("fa fa-times").parent(".btn-primary").removeClass("btn-primary").addClass("btn-danger");

            //redesign download button
            $("#" + parentId + " .ui-icon.ui-icon-download").removeClass().addClass("fa fa-download").parent(".btn-primary").removeClass("btn-primary").addClass("btn-success");

            //redesign navigation icons
            $("#" + parentId + " .ui-icon.ui-icon-seek-prev").wrap("<div class='btn btn-sm btn-default'></div>");
            $("#" + parentId + " .ui-icon.ui-icon-seek-prev").removeClass().addClass("fa fa-backward");

            $("#" + parentId + " .ui-icon.ui-icon-seek-first").wrap("<div class='btn btn-sm btn-default'></div>");
            $("#" + parentId + " .ui-icon.ui-icon-seek-first").removeClass().addClass("fa fa-fast-backward");

            $("#" + parentId + " .ui-icon.ui-icon-seek-next").wrap("<div class='btn btn-sm btn-default'></div>");
            $("#" + parentId + " .ui-icon.ui-icon-seek-next").removeClass().addClass("fa fa-forward");

            $("#" + parentId + " .ui-icon.ui-icon-seek-end").wrap("<div class='btn btn-sm btn-default'></div>");
            $("#" + parentId + " .ui-icon.ui-icon-seek-end").removeClass().addClass("fa fa-fast-forward");

            //resize table full width '100%'
            $('#' + selectorId).jqGrid('setGridWidth', $('#' + parentId).width(), true);

            //implement save row
        }

        loadScript('/js/plugin/jqgrid/grid.locale-en.min.js', function () {
            loadScript('/js/plugin/jqgrid/jquery.jqGrid.min.js', function () {
                //load invalid data
                var updateParameters = {
                    data: data,
                    selectorId: selectorId,
                    parentId: parentId,
                    caption: caption
                };

                //load valid data
                pageFunction(rowList, data, columnNames, selectorId, columnModels, pagerId, parentId, caption, false, null);
                //load invalid data
                pageFunction(rowList, dataError, columnNames, selectorErrorId, columnModels, pagerErrorId, parentErrorId, captionError, true, updateParameters);
            });
        });
    };
    this.uploadExcel = function (url, fileSelectorId) {
        var photo = document.getElementById(fileSelectorId);
        var file = photo.files[0];
        var formData = new FormData();
        var data = [];
        var defer = $.Deferred();
        formData.append('fileBase', file);
        $.ajax({
            context: this,
            url: url,
            type: 'POST',
            data: formData,
            async: true,
            cache: false,
            contentType: false,
            enctype: 'multipart/form-data',
            processData: false,
            success: function (response) {
                this.data = response.correctData;
                this.dataError = response.inCorrectData;
                this.buildGrid();
                defer.resolve(true);
            }, beforeSend: function () {
                WaitingLoad_Start()
            }, complete: function () {
                WaitingLoad_End()
            }, error: function (err) {
                defer.resolve(false);
            }
        });
        return defer.promise();
    }
    this.importExcel = function (url) {
        var result = false;
        var length = excelGridSelectedRowIds.length;
        for (var i = 0; i < length; i++) {
            var row = $('#' + this.selectorId).getRowData(excelGridSelectedRowIds[i]);
            excelImportData.push(row);
        }
        var defer = $.Deferred();
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(excelImportData),
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (response) {
                defer.resolve(response.success);
            }, beforeSend: function () {
                WaitingLoad_Start()
            }, complete: function () {
                WaitingLoad_End()
            }, error: function (err) {
                defer.resolve(false);
            }
        });
        return defer.promise();
    }
    this.importExcelDoanhNghiepNopChamThue = function (id, url) {
        var result = false;
        var length = excelGridSelectedRowIds.length;
        for (var i = 0; i < length; i++) {
            var row = $('#' + this.selectorId).getRowData(excelGridSelectedRowIds[i]);
            excelImportData.push(row);
        }
        var defer = $.Deferred();
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ "id": id, "listImport": excelImportData }),
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (response) {
                defer.resolve(response.success);
            }, beforeSend: function () {
                WaitingLoad_Start()
            }, complete: function () {
                WaitingLoad_End()
            }, error: function (err) {
                defer.resolve(false);
            }
        });
        return defer.promise();
    }

    this.exportExcel = function (url) {
        var data = JSON.stringify($('#' + this.selectorErrorId).jqGrid('getGridParam', 'data'));
        var defer = $.Deferred();
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                defer.resolve(response);
            }
        });
        return defer.promise();
    }
}

var CommonGrid = {
    searchParam: {
    },
    columns: [],
    url: '',
    selectorId: '',
    pagerId: '',
    title: '',
    titleId: '',
    pageIndex: 0,
    data: [],
    total: 0,
    editColumns: function (id) {
        return "<div class='btn-group'>"
            + "<a href='javascript:void(0)' onclick='openModalEdit(" + id + ")'   title = 'Chỉnh sửa'><i class='glyphicon glyphicon-edit'> </i></a>"
            + "<a href='javascript:void(0)' onclick='openModalDelete(" + id + ")'  title = 'Xóa'><i class=' glyphicon glyphicon-remove' style='color:red'> </i></a>"
            + "</div>";
    },
    getData: function (url) {
        console.log(CommonGrid.searchParam);
        var defer = $.Deferred();
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(CommonGrid.searchParam),
            contentType: "application/json;charset=utf-8",
            dataType: 'json',
            success: function (data) {
                defer.resolve(data);
            }, beforeSend: function () {
                WaitingLoad_Start()
            }, complete: function () {
                WaitingLoad_End()
            }, error: function (err) {
                defer.resolve(false);
            }
        });
        return defer.promise();
    },
    buildGrid: function () {
        if (CommonGrid.url) {
            CommonGrid.getData(CommonGrid.url).done(function (result) {
                CommonGrid.data = result.listItemsPerPage;
                CommonGrid.total = result.totalItem;
                var startRow = (CommonGrid.pageIndex - 1) * gridDataPerPage + 1;
                var html = '';
                var target = CommonGrid.selectorId + "_container";
                html += "<table class='wtfayo display projects-table table table-striped table-bordered table-hover' cellspacing='0' width='100%' id=" + target + ">";
                html += '<thead><tr>';
                for (var i = 0; i < CommonGrid.columns.length; i++) {
                    html += '<th  class="thtableresponsive">';
                    html += escapeHTML(CommonGrid.columns[i].text);
                    html += '</th>';
                }
                html += '</tr></thead>';
                html += '<tbody>';
                if (CommonGrid.data.length >= 0) {
                    var dataLength = CommonGrid.data.length;
                    if (CommonGrid.data !== null && CommonGrid.data.length) {
                        var columnsLength = CommonGrid.columns.length;
                        for (var i = 0; i < dataLength; i++) {
                            html += '<tr>';
                            for (var j = 0; j < columnsLength; j++) {
                                html += '<td>';
                                if (CommonGrid.columns[j].isOrder) {
                                    html += (startRow + i);
                                } else if (CommonGrid.columns[j].hidden) {

                                } else if (CommonGrid.columns[j].isEdit) {
                                    var data = CommonGrid.data[i][CommonGrid.columns[j].field];
                                    html += CommonGrid.editColumns(data);
                                } else {
                                    var data = CommonGrid.data[i][CommonGrid.columns[j].field];
                                    html += escapeHTML(data);
                                }
                                html += '</td>';
                            }
                            html += '</tr>';
                        }
                    }
                    html += '</tbody>';
                    html += '</table>'
                    $('#' + CommonGrid.selectorId).html(html);
                    loadScript("/js/plugin/datatables/jquery.dataTables.min.js", function () {
                        loadScript("/js/plugin/datatables/dataTables.colVis.min.js", function () {
                            loadScript("/js/plugin/datatables/dataTables.tableTools.min.js", function () {
                                loadScript("/js/plugin/datatables/dataTables.bootstrap.min.js", function () {
                                    loadScript("/js/plugin/datatable-responsive/datatables.responsive.min.js", function () {
                                        $('#' + target).DataTable({
                                            "bDestroy": true,
                                            "paging": false,
                                            "bFilter": false,
                                            "bInfo": false
                                        });
                                    })
                                });
                            });
                        });
                    });
                    CommonGrid.pagingTable(CommonGrid.total);
                }
            })
        }
    },
    pagingTable: function (totalDatas) {
        var currentPageIndex = CommonGrid.pageIndex;
        var totalPages = 0;
        if (totalDatas <= gridDataPerPage) {
            totalPages = 1;
        } else if (totalDatas % gridDataPerPage !== 0) {
            totalPages = Math.floor(totalDatas / gridDataPerPage) + 1;
        } else {
            totalPages = Math.floor(totalDatas / gridDataPerPage);
        }
        var startRow = (currentPageIndex - 1) * gridDataPerPage + 1;
        var endRow = (startRow + CommonGrid.data.length - 1);
        var pagingHtml = '';
        pagingHtml += '<div class="dt-toolbar-footer">';
        //left content
        var leftContent = '';
        leftContent += '<div class="col-xs-12 col-sm-6">';
        leftContent += '<div class="dataTables_info" id="' + CommonGrid.selectorId + "_info" + '" role="status" aria-live="polite">';
        leftContent += 'Hiển thị <span class="txt-color-darken">' + startRow + '</span> đến <span class="txt-color-darken">' + endRow + '</span> của <span class="text-primary">' + totalDatas + '</span> bản ghi';
        leftContent += '</div>';
        leftContent += '</div>';
        //end left content

        //right content
        var rightContent = '';
        rightContent += '<div class="col-xs-12 col-sm-6">';
        rightContent += '<div class="dataTables_paginate paging_simple_numbers" id="' + CommonGrid.selectorId + '_paginate">'
        rightContent += '<ul class="pagination pagination-sm">';

        if (totalPages >= currentPageIndex) {
            if (totalPages <= 7) {
                if (totalPages === 1) {
                    rightContent += '<li class="paginate_button active" aria-controls="dt_basic" tabindex="0">';
                    rightContent += '<a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + totalPages + ')">' + totalPages + '</a>';
                    rightContent += '</li>';
                } else {
                    if (currentPageIndex === 1) {
                        rightContent += '<li class="paginate_button previous disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)">Trang trước</a></li>';
                    } else {
                        rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex - 1) + ')">Trang trước</a></li>';
                    }
                    for (var index = 1; index <= totalPages; index++) {
                        if (index === currentPageIndex) {
                            rightContent += '<li class="paginate_button active" aria-controls="dt_basic" tabindex="0">';
                        } else {
                            rightContent += '<li class="paginate_button" aria-controls="dt_basic" tabindex="0">';
                        }
                        rightContent += '<a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + index + ')">' + index + '</a>';
                        rightContent += '</li>';
                    }
                    if (currentPageIndex === totalPages) {
                        rightContent += '<li class="paginate_button next disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_next"><a href="javascript:void(0)">Trang tiếp</a></li>';
                    } else {
                        rightContent += '<li class="paginate_button next" aria-controls="dt_basic" tabindex="0" id="dt_basic_next"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex + 1) + ')">Trang tiếp</a></li>';
                    }
                }
            }
            else {
                if (currentPageIndex <= 4) {
                    if (currentPageIndex === 1) {
                        rightContent += '<li class="paginate_button previous disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)">Trang trước</a></li>';
                    } else {
                        rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex - 1) + ')">Trang trước</a></li>';
                    }
                    for (var index = 1; index <= 5; index++) {
                        if (index === currentPageIndex) {
                            rightContent += '<li class="paginate_button active" aria-controls="dt_basic" tabindex="0">';
                        } else {
                            rightContent += '<li class="paginate_button" aria-controls="dt_basic" tabindex="0">';
                        }
                        rightContent += '<a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + index + ')">' + index + '</a>';
                        rightContent += '</li>';
                    }
                    rightContent += '<li class="paginate_button disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_ellipsis"><a href="javascript:void(0)">…</a></li>';
                    rightContent += '<li class="paginate_button " aria-controls="dt_basic" tabindex="0"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (totalPages) + ')">' + totalPages + '</a></li>';
                    rightContent += '<li class="paginate_button next" aria-controls="dt_basic" tabindex="0" id="dt_basic_next"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (index + 1) + ')">Trang tiếp</a></li>';
                } else if (currentPageIndex >= 5 && currentPageIndex < (totalPages - 3)) {
                    rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex - 1) + ')">Trang trước</a></li>';
                    rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + 1 + ')">1</a></li>';
                    rightContent += '<li class="paginate_button disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_ellipsis"><a href="javascript:void(0)">…</a></li>';

                    rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex - 1) + ')">' + (currentPageIndex - 1) + '</a></li>';
                    rightContent += '<li class="paginate_button previous active" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex) + ')">' + (currentPageIndex) + '</a></li>';
                    rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex + 1) + ')">' + (currentPageIndex + 1) + '</a></li>';

                    rightContent += '<li class="paginate_button disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_ellipsis"><a href="javascript:void(0)">…</a></li>';
                    rightContent += '<li class="paginate_button " aria-controls="dt_basic" tabindex="0"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (totalPages) + ')">' + totalPages + '</a></li>';
                    rightContent += '<li class="paginate_button next" aria-controls="dt_basic" tabindex="0" id="dt_basic_next"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (index + 1) + ')">Trang tiếp</a></li>';

                } else {
                    rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (currentPageIndex - 1) + ')">Trang trước</a></li>';
                    rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + 1 + ')">1</a></li>';
                    rightContent += '<li class="paginate_button disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_ellipsis"><a href="javascript:void(0)">…</a></li>';
                    for (var index = (totalPages - 4); index <= totalPages; index++) {
                        if (index === currentPageIndex) {
                            rightContent += '<li class="paginate_button active" aria-controls="dt_basic" tabindex="0">';
                        } else {
                            rightContent += '<li class="paginate_button" aria-controls="dt_basic" tabindex="0">';
                        }
                        rightContent += '<a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + index + ')">' + index + '</a>';
                        rightContent += '</li>';
                    }

                    if (currentPageIndex === totalPages) {
                        rightContent += '<li class="paginate_button previous disabled" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)">Trang tiếp</a></li>';
                    } else {
                        rightContent += '<li class="paginate_button previous" aria-controls="dt_basic" tabindex="0" id="dt_basic_previous"><a href="javascript:void(0)" onclick="CommonGrid.pagingItem(' + (totalPages) + ')">Trang tiếp</a></li>';
                    }
                }
            }
        }
        rightContent += '</ul>';
        rightContent += '</div>';
        rightContent += '</div>';
        //end right content

        pagingHtml += (leftContent + rightContent);
        pagingHtml += "</div>";
        $('#' + CommonGrid.pagerId).html(pagingHtml);
        if ($('#' + CommonGrid.titleId).length > 0) {
            $('#' + CommonGrid.titleId).text(CommonGrid.title + ' (' + CommonGrid.total + ')')
        }
    },
    pagingItem: function (index) {
        CommonGrid.searchParam = {
            ...CommonGrid.searchParam,
            pageIndex: index,
        }
        CommonGrid.pageIndex = index;
        CommonGrid.buildGrid();
    }
}

function escapeHTML(str) {
    var htmlEscapes = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#x27;',
        '/': '&#x2F;'
    };
    // Regex containing the keys listed immediately above.
    var htmlEscaper = /[&<>"'\/]/g;
    return ('' + str).replace(htmlEscaper, function (match) {
        return htmlEscapes[match];
    });
}