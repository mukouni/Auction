// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var cookieUtil = {
    // 设置cookie
    setItem: function (name, value, days) {
        var date = new Date();
        date.setDate(date.getDate() + days);
        document.cookie = name + '=' + value + ';expires=' + date;
    },

    // 获取cookie
    getItem: function (name) {
        var arr = document.cookie.replace(/\s/g, "").split(';');
        for (var i = 0; i < arr.length; i++) {
            var tempArr = arr[i].split('=');
            if (tempArr[0] == name) {
                return decodeURIComponent(tempArr[1]);
            }
        }
        return '';
    },

    // 删除cookie
    removeItem: function (name) {
        this.setItem(name, '1', -1);
    },

    // 检查是否含有某cookie
    hasItem: function (name) {
        return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(name).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie);
    },

    // 获取全部的cookie列表
    getAllItems: function () {
        var cookieArr = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nIdx = 0; nIdx < cookieArr.length; nIdx++) {
            cookieArr[nIdx] = decodeURIComponent(cookieArr[nIdx]);
        }
        return cookieArr;
    }
};


function selectPageSizeChange(obj){
    $.ajax({
        type: "get",
        url: "/equipment/index",
        data: {PageSize: $(obj).val()},
        success: function (response) {
            $('#equipment—index-table').html(response);
            $('[data-toggle="table"]').bootstrapTable(); 
        }
    });
}


function UploadPhoto(uploadUrl, deleteUrl, setHiddenPhototUrl, modelId) {
    this.uploadUrl          = uploadUrl
    this.deleteUrl          = deleteUrl
    this.setHiddenPhototUrl = setHiddenPhototUrl
    this.modelId            = modelId

    this.initInput = function(inputId, photoJson, photoType){
        var photos = photoJson;
        var _initialPreview = []
        var _initialPreviewConfig = []
        var newPhotos = []

        _addData(photos)

        // {'X-XSRF-TOKEN': cookieUtil.getItem("XSRF-TOKEN")}
        var $fileInput = $(inputId)
        $fileInput.fileinput({
            "language": 'zh',
            "theme": 'explorer-fas',
            uploadUrl: this.uploadUrl,
            ajaxSettings: {
                beforeSend: function(xhr) {
                    xhr.setRequestHeader('X-XSRF-TOKEN', cookieUtil.getItem("XSRF-TOKEN"))
                }
            },
            uploadExtraData: {
                '__RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val(),
                'EquipmentId': this.modelId,
                'PhotoType': photoType
            },
            allowedFileTypes: ["image"],
            overwriteInitial: false,
            initialPreviewAsData: true,
            initialPreview: _initialPreview,
            initialPreviewConfig: _initialPreviewConfig
        }).on("filebatchselected", function(event, files) {
            $fileInput.fileinput("upload");
        }).on('filepreupload', function(event, data, previewId, index) {
            //data.form.append('IsHiddenAfterSold', $("input#" + previewId).is(":checked"))
        }).on("fileuploaded", function (event, data, previewId, index) {
            _addData([data.response])
            
            if(data.response.Message == "操作成功"){
                var caption = $("#" + previewId).find(".explorer-caption")
                caption.attr("title", data.response.Data.FileName)
                caption.text(data.response.Data.FileName)
                $('input:checkbox[name="IsHiddenAfterSold"]:visible').on("click", this.SetHiddenPhototAfterSold)
            }
            newPhotos.push({
                PreviewId: previewId,
                FileName:  data.response.Data.FileName,
                PhotoId: data.response.Data.Id
            })
        }).on('filesorted', function(event, params) {
        }).on('filebatchpreupload', function(event, data, jqXHR) {
        }).on("filesuccessremove", function (event, id, fileindex) { 
            var previewId = $("#"+id).data("previewid")
            $.each(newPhotos, function(i, p){
                if(p.PreviewId == previewId){
                    $.ajax({
                        type: "post",
                        url: "/equipment/deletephoto",
                        data: { photoName: p.FileName },
                        success: function(result){
                            if(result.Message == "操作成功"){
                                delete newPhotos[i]
                            }
                        }
                    })
                }
            })
        });

        //初始化 initialPreview initialPreviewConfig
        function _addData(photos){
            $.each(photos, function(i, p){
                var photo     = { extra: {} }
                photo.caption = p.FileName
                photo.size    = p.FileSize
                photo.key     = p.Id
                photo.width   = "120px"
                photo.url     = "/equipment/deletephoto"
                photo.extra.photoName = p.FileName
                
                _initialPreview.push(p.RequestPath)
                _initialPreviewConfig.push(photo)
            })
        }

        this.selectHiddenPhototCheckbox(photos);
    }
    
    this.selectHiddenPhototCheckbox = function (photos){
        $.each(photos, function(i, photo){
            if(photo.IsHiddenAfterSold){
                $(".explorer-caption[title='" + photo.FileName + "']")
                .closest("tr")
                .find('input:checkbox[name="IsHiddenAfterSold"]:visible')
                .attr('checked', true)
            }
        })
        
    }

    this.SetHiddenPhototAfterSold = function(){
        var that = $(this)
        var photoName = that.closest("tr").find(".explorer-caption").attr("title")
        $.ajax({
            type: "post",
            url: "/equipment/sethiddenphototaftersold",
            data: { 
                photoName: photoName, 
                equipmentId: this.modelId,
                '__RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val() },
            success: function(result){
            }
        })
    }

    this.bindCheckbox = function(){
        $(document).on("click", 'input:checkbox[name="IsHiddenAfterSold"]:visible', this.SetHiddenPhototAfterSold)
    }

}