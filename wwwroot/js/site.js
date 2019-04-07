﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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


function selectPageSizeChange(obj) {
    $.ajax({
        type: "get",
        url: "/equipment/index",
        data: {
            PageSize: $(obj).val()
        },
        success: function (response) {
            $('#equipment—index-table').html(response);
            $('[data-toggle="table"]').bootstrapTable();
        }
    });
}


function UploadPhoto(uploadUrl, deleteUrl, setHiddenPhototUrl, modelId) {
    this.uploadUrl = uploadUrl
    this.deleteUrl = deleteUrl
    this.setHiddenPhototUrl = setHiddenPhototUrl
    this.modelId = modelId

    this.initInput = function (inputId, photoJson, photoType) {
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
                beforeSend: function (xhr) {
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
        }).on("filebatchselected", function (event, files) {
            $fileInput.fileinput("upload");
        }).on('filepreupload', function (event, data, previewId, index) {
            //data.form.append('IsHiddenAfterSold', $("input#" + previewId).is(":checked"))
        }).on("fileuploaded", function (event, data, previewId, index) {
            _addData([data.response])

            if (data.response.Message == "操作成功") {
                var caption = $("#" + previewId).find(".explorer-caption")
                caption.attr("title", data.response.Data.FileName)
                caption.text(data.response.Data.FileName)
                $('input:checkbox[name="IsHiddenAfterSold"]:visible').on("click", this.SetHiddenPhototAfterSold)
            }
            newPhotos.push({
                PreviewId: previewId,
                FileName: data.response.Data.FileName,
                PhotoId: data.response.Data.Id
            })
        }).on('filesorted', function (event, params) {}).on('filebatchpreupload', function (event, data, jqXHR) {}).on("filesuccessremove", function (event, id, fileindex) {
            var previewId = $("#" + id).data("previewid")
            $.each(newPhotos, function (i, p) {
                if (p.PreviewId == previewId) {
                    $.ajax({
                        type: "post",
                        url: "/equipment/deletephoto",
                        data: {
                            photoName: p.FileName
                        },
                        success: function (result) {
                            if (result.Message == "操作成功") {
                                delete newPhotos[i]
                            }
                        }
                    })
                }
            })
        });

        //初始化 initialPreview initialPreviewConfig
        function _addData(photos) {
            $.each(photos, function (i, p) {
                var photo = {
                    extra: {}
                }
                photo.caption = p.FileName
                photo.size = p.FileSize
                photo.key = p.Id
                photo.width = "120px"
                photo.url = "/equipment/deletephoto"
                photo.extra.photoName = p.FileName

                _initialPreview.push(p.RequestPath)
                _initialPreviewConfig.push(photo)
            })
        }

        this.selectHiddenPhototCheckbox(photos);
    }

    this.selectHiddenPhototCheckbox = function (photos) {
        $.each(photos, function (i, photo) {
            if (photo.IsHiddenAfterSold) {
                $(".explorer-caption[title='" + photo.FileName + "']")
                    .closest("tr")
                    .find('input:checkbox[name="IsHiddenAfterSold"]:visible')
                    .attr('checked', true)
            }
        })

    }

    this.SetHiddenPhototAfterSold = function () {
        var that = $(this)
        var photoName = that.closest("tr").find(".explorer-caption").attr("title")
        $.ajax({
            type: "post",
            url: "/equipment/sethiddenphototaftersold",
            data: {
                photoName: photoName,
                equipmentId: this.modelId,
                '__RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (result) {}
        })
    }

    this.bindCheckbox = function () {
        $(document).on("click", 'input:checkbox[name="IsHiddenAfterSold"]:visible', this.SetHiddenPhototAfterSold)
    }

}

$(document).on("click", ".order-ul .list-inline-item", function () {
    var className = "text-warning";
    var $orderBys = $(this).find(".order-by");
    if ($orderBys.length > 0) {
        if ($(this).find("p").hasClass(className)) {
            $orderBys.toggleClass("d-none");
        }
        var $showOrderBy = $(this).find(".order-by:visible");
        $("#Sort_Field").val($orderBys.first().data("order-by"));
        if ($showOrderBy.data("direction") == "desc") {
            $("#Sort_Direction").val("desc");
        } else {
            $("#Sort_Direction").val("asc");
        }
    } else {
        $("#Sort_Field").val("");
        $("#Sort_Direction").val("");
    }

    $(".order-ul .list-inline-item").find("p").removeClass(className);
    $(this).find("p").addClass(className);

    searchEquipments();
});

$("#toggle-menu").click(function () {
    $("#navbar").toggleClass("d-none ver-menu");
})

// .search是侧栏 .show是滑动主面板 需要touch-action: none;
if($('.info .show').get(0) && $('.info .search').get(0)){
    var slideout = new Slideout({
        'panel': $('.info .show').get(0),
        'menu': $('.info .search').get(0),
        'padding': 250,
        'tolerance': 70
    });
}


function initPhotoSwipeFromDOM(gallerySelector) {
    // parse slide data (url, title, size ...) from DOM elements
    // (children of gallerySelector)
    var parseThumbnailElements = function(el) {
        var thumbElements = el.childNodes,
            numNodes = thumbElements.length,
            items = [],
            figureEl,
            linkEl,
            size,
            item;
        for (var i = 0; i < numNodes; i++) {
            figureEl = thumbElements[i]; // <figure> element
            // include only element nodes
            if (figureEl.nodeType !== 1) {
                continue;
            }
            linkEl = figureEl.children[0]; // <a> element
            if(linkEl.getAttribute('data-size')){
                size = linkEl.getAttribute('data-size').split('x');
                // create slide object
                item = {
                    src: linkEl.getAttribute('href'),
                    w: parseInt(size[0], 10),
                    h: parseInt(size[1], 10)
                };
            }
            if (figureEl.children.length > 1) {
                // <figcaption> content
                item.title = figureEl.children[1].innerHTML;
            }
            if (linkEl.children.length > 0) {
                // <img> thumbnail element, retrieving thumbnail url
                item.msrc = linkEl.children[0].getAttribute('src');
            }
            item.el = figureEl; // save link to element for getThumbBoundsFn
            items.push(item);
        }
        return items;
    };
    // find nearest parent element
    var closest = function closest(el, fn) {
        return el && (fn(el) ? el : closest(el.parentNode, fn));
    };
    // triggers when user clicks on thumbnail
    var onThumbnailsClick = function(e) {
        e = e || window.event;
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
        var eTarget = e.target || e.srcElement;
        // find root element of slide
        var clickedListItem = closest(eTarget, function(el) {
            return (el.tagName && el.tagName.toUpperCase() === 'FIGURE');
        });
        if (!clickedListItem) {
            return;
        }
        // find index of clicked item by looping through all child nodes
        // alternatively, you may define index via data- attribute
        var clickedGallery = clickedListItem.parentNode,
            childNodes = clickedListItem.parentNode.childNodes,
            numChildNodes = childNodes.length,
            nodeIndex = 0,
            index;
        for (var i = 0; i < numChildNodes; i++) {
            if (childNodes[i].nodeType !== 1) {
                continue;
            }
            if (childNodes[i] === clickedListItem) {
                index = nodeIndex;
                break;
            }
            nodeIndex++;
        }
        if (index >= 0) {
            // open PhotoSwipe if valid index found
            openPhotoSwipe(index, clickedGallery);
        }
        return false;
    };
    // parse picture index and gallery index from URL (#&pid=1&gid=2)
    var photoswipeParseHash = function() {
        var hash = window.location.hash.substring(1),
            params = {};
        if (hash.length < 5) {
            return params;
        }
        var vars = hash.split('&');
        for (var i = 0; i < vars.length; i++) {
            if (!vars[i]) {
                continue;
            }
            var pair = vars[i].split('=');
            if (pair.length < 2) {
                continue;
            }
            params[pair[0]] = pair[1];
        }
        if (params.gid) {
            params.gid = parseInt(params.gid, 10);
        }
        return params;
    };
    var openPhotoSwipe = function(index, galleryElement, disableAnimation, fromURL) {
        var pswpElement = document.querySelectorAll('.pswp')[0],
            gallery,
            options,
            items;
        items = parseThumbnailElements(galleryElement);
        // define options (if needed)
        options = {
            // define gallery index (for URL)
            galleryUID: galleryElement.getAttribute('data-pswp-uid'),
            // getThumbBoundsFn: function(index) {
            //     // See Options -> getThumbBoundsFn section of documentation for more info
            //     var thumbnail = items[index].el.getElementsByTagName('img')[0], // find thumbnail
            //         pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
            //         rect = thumbnail.getBoundingClientRect();
            //     return { x: rect.left, y: rect.top + pageYScroll, w: rect.width };
            // }
        };
        // PhotoSwipe opened from URL
        if (fromURL) {
            if (options.galleryPIDs) {
                // parse real index when custom PIDs are used
                // http://photoswipe.com/documentation/faq.html#custom-pid-in-url
                for (var j = 0; j < items.length; j++) {
                    if (items[j].pid == index) {
                        options.index = j;
                        break;
                    }
                }
            } else {
                // in URL indexes start from 1
                options.index = parseInt(index, 10) - 1;
            }
        } else {
            options.index = parseInt(index, 10);
        }
        // exit if index not found
        if (isNaN(options.index)) {
            return;
        }
        if (disableAnimation) {
            options.showAnimationDuration = 0;
        }
        // Pass data to PhotoSwipe and initialize it
        gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
        gallery.listen('imageLoadComplete', function(index, item) {
            var linkEl = item.el.children[0];
            var img = item.container.children[0];
            if (linkEl.getAttribute('data-size').split('x')[0] == '0' && linkEl.getAttribute('data-size').split('x')[1] == '0') {
                linkEl.setAttribute('data-size', img.naturalWidth + 'x' + img.naturalHeight);
                item.w = img.naturalWidth;
                item.h = img.naturalHeight;
                gallery.invalidateCurrItems();
                gallery.updateSize(true);
            }
        });
      
        gallery.init();
    };
    // loop through all gallery elements and bind events
    var galleryElements = document.querySelectorAll(gallerySelector);
    for (var i = 0, l = galleryElements.length; i < l; i++) {
        galleryElements[i].setAttribute('data-pswp-uid', i + 1);
        galleryElements[i].onclick = onThumbnailsClick;
    }
    // Parse URL and open gallery if it contains #&pid=3&gid=1
    var hashData = photoswipeParseHash();
    if (hashData.pid && hashData.gid) {
        openPhotoSwipe(hashData.pid, galleryElements[hashData.gid - 1], true, true);
    }
};