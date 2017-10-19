//function to clear all error messages
function clearMessageErr() {
    $("#ERR_COQUANBANHANH_ID").html("");
    $("#ERR_SOKYHIEU").html("");
    $("#ERR_LOAIVANBAN_ID").html("");
    $("#ERR_NGAYDEN").html("");
    $("#ERR_NGAYVANBAN").html("");
    $("#ERR_SOVANBANDEN_ID").html("");
    $("#ERR_SODEN").html("");
    $("#ERR_SOTRANG").html("");
}
//end
//function to check is integer
function isIntegerNotNative(x) {
    x = parseInt(x);
    return (typeof x === 'number') && (x > 0) && (x % 1 === 0);
}
//end
//prepare data for upload file automatic
function prepareUpload(event) {
    files = event.target.files;
    var data = new FormData();
    $.each(files, function (key, value) {
        data.append(key, value);
    });
    $.ajax({
        url: '/VanBanDenArea/VanBanDen/Upload',
        type: 'POST',
        data: data,
        cache: false,
        dataType: 'json',
        processData: false, // Don't process the files
        contentType: false, // Set content type to false as jQuery will tell the server its a query string request
        success: function (data, textStatus, jqXHR) {
            for (var item in data) {
                var filename = data[item].Data.filename;
                var html = '<div id="file_dinh_kem_' + data[item].Data.file_id + '" style="border-bottom: 1px solid black; height: 100%; margin-top: 6px;">';
                html += '<span class="name"><a target="_blank" href="/' + data[item].Data.path + '">' + filename + '</a></span>';
                html += '<a title="Xóa" class="btnDelete" onclick="DeleteFile(' + data[item].Data.file_id + ')" href="javascript:void(0)" style="float: right"></a>';
                html += '<input type="hidden" value="' + data[item].Data.file_id + '" name="TaiLieuDinhKem">';
                html += '</div>';
                $(".tbl_content_file_upload_related").append(html);
            }
            $("#vbd_related_documents").val("");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle errors here
            console.log('ERRORS: ' + textStatus);
            // STOP LOADING SPINNER
        }
    });
}
//end
//function delete file uploaded
function DeleteFile(ID) {
    $.confirm({
        'title': 'Delete Confirmation',
        'message': 'Bạn có chắc chắn muốn xóa file đính kèm này?',
        'buttons': {
            'Yes': {
                'class': 'btn-confirm-yes',
                'action': function () {
                    $.ajax({
                        type: "POST",
                        url: '/VanBanDenArea/VanBanDen/DeleteFile',
                        cache: false,
                        data: {
                            ID: ID
                        },
                        success: function (data) {
                            $("#file_dinh_kem_" + ID).remove();
                            notif({
                                type: 'success',
                                position: 'bottom',
                                msg: 'Delete successfully!',
                            });
                        },
                        error: function (err) {
                            CommonJS.alert(err.responseText);
                        }
                    })
                }
            },
            'No': {
                'class': 'btn-info',
                'action': function () { }	// Nothing to do in this case. You can as well omit the action property.
            }
        }
    });


}