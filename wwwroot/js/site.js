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


function selectPageSizeChange(obj, url) {
    $.ajax({
        type: "get",
        url: url,
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
                    caption.attr("title", data.response.data.fileName)
                    caption.text(data.response.data.fileName)
                    $('input:checkbox[name="IsHiddenAfterSold"]:visible').on("click", this.SetHiddenPhototAfterSold)
                }
                newPhotos.push({
                    PreviewId: previewId,
                    FileName: data.response.data.fileName,
                    PhotoId: data.response.data.id
                });
            }).on('filesorted', function (event, params) {})
            .on('filebatchpreupload', function (event, data, jqXHR) {})
            .on("filesuccessremove", function (event, id, fileindex) {
                var previewId = $("#" + id).data("previewid")
                $.each(newPhotos, function (i, p) {
                    if (p.PreviewId == previewId) {
                        $.ajax({
                            url: "/equipment/deletephoto",
                            type: "post",
                            dataType: 'json',
                            contentType: "application/json",
                            data: JSON.stringify({
                                photoName: p.FileName,
                                id: p.PhotoId
                            }),
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
if ($('.info .show').get(0) && $('.info .search').get(0)) {
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
    var parseThumbnailElements = function (el) {
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
            if (linkEl.getAttribute('data-size')) {
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
    var onThumbnailsClick = function (e) {
        e = e || window.event;
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
        var eTarget = e.target || e.srcElement;
        // find root element of slide
        var clickedListItem = closest(eTarget, function (el) {
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
    var photoswipeParseHash = function () {
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
    var openPhotoSwipe = function (index, galleryElement, disableAnimation, fromURL) {
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
            tapToClose: true
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
        gallery.listen('imageLoadComplete', function (index, item) {
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

function ScrollEndGet() {
    this.hasMore = false; // 是否有下一页
    this.lastKnownScrollPosition = 0; // 最后滚动的位置，滚动条上沿
    this.windowHeight = 0; // viewpoint的高度
    this.documentHeight = 0; // 窗口内容总高度
    this.ticking = false; //是否正进行复杂操作
    this.currentPage = 1;

    this.scrollListening = function () {
        var scrollEndGet = this;
        $(window).scroll(function () {
            if (scrollEndGet.hasMore) {
                scrollEndGet.lastKnownScrollPosition = Math.ceil($(this).scrollTop()); // 不太准，有很长的小数位
                scrollEndGet.documentHeight = $(document).height();
                scrollEndGet.windowHeight = $(this).height();
                var isScrollEnd = scrollEndGet.lastKnownScrollPosition + scrollEndGet.windowHeight >= scrollEndGet.documentHeight;

                if (!scrollEndGet.ticking && isScrollEnd) {
                    window.requestAnimationFrame(function () {
                        scrollEndGet.getNextPage(scrollEndGet.currentPage);
                        scrollEndGet.ticking = false;
                    });
                }
                if (scrollEndGet.ticking && isScrollEnd) {
                    scrollEndGet.ticking = true;
                }

            }
        });
    };

    this.getNextPage = function (currentPage) {
        searchEquipments("post", currentPage + 1)
        // do something with the scroll position
    }
};
var scrollEndGet = new ScrollEndGet();
scrollEndGet.scrollListening();

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


function addRemoveElement(parentNode, chipElementId, resetValue) {
    var span = document.createElement("span");
    span.className = "closebtn";
    span.innerHTML = "&times;";
    span.onclick = function (e) {
        var elem = document.getElementById(chipElementId);
        elem.parentNode.removeChild(elem);
        if (resetValue.length == 0) {
            resetValue();
        } else {
            resetValue(chipElementId.split("-chip")[0])
        }
        searchEquipments();
    }
    parentNode.appendChild(span);
}

function addOrUpdateChip(resetValue, ...args) {
    var existChip = document.getElementById(args[0]);

    if (existChip) {
        if (args.length == 3) {
            existChip.innerText = args[1] + "—" + args[2];
        } else {
            existChip.innerText = args[1];
        }
        addRemoveElement(existChip, args[0], resetValue);
    } else {
        var chip = document.createElement("div");
        chip.className = "chip";
        chip.id = args[0].toString();
        if (args.length == 3) {
            chip.innerHTML = args[1] + "—" + args[2];
        } else {
            chip.innerHTML = args[1];
        }
        addRemoveElement(chip, args[0], resetValue);
        document.getElementsByClassName("condition")[0].appendChild(chip);
    }
}

var bindCheckBox = function () {
    $(document).on('click', 'input[type="checkbox"]', function () {
        var chipId = $(this).attr("id") + "-chip";
        if ($(this).is(':checked')) {
            searchEquipments();
        } else {
            $("#" + chipId).remove();
            searchEquipments();
        }
    });
    //$("#models-form-group").find("input[type='checkbox']:visible").each(function(i, ele){
    // $(".search input[type='checkbox']:visible").each(function (i, ele) {
    //     $(document).on("change click touch touchstart touchend", "#" + $(ele).attr("id"), function () {
    //         var chipId = $(this).attr("id") + "-chip";
    //         if ($(this).is(':checked')) {
    //             // var checkboxArrayId = $(this).attr("id").split("_").slice(0, 2).join("_");
    //             // var hiddenInputId = checkboxArrayId + "__Name";
    //             // var valueInput = $("#" + hiddenInputId);

    //             //addOrUpdateChip(resetCheckboxStatus, chipId, valueInput.val());
    //             searchEquipments();
    //         } else {
    //             $("#" + chipId).remove();
    //             searchEquipments();
    //         }

    //     })
    // });
};

function deleteUnCheckedEle(formData) {
    for (var key in formData) {
        let match = key.match(/(\w*\[[0-9]*\])\.(\w*)/);
        if (match && match[2] == 'Name' && formData[match[1] + '.' + 'Selected'] == undefined) {
            delete formData[match[1] + '.' + 'Name'];
        }
    }
    let names = {
        Names: 0,
        Manufacturers: 0,
        Models: 0,
        Countries: 0,
        Cities: 0,
        AuctionHouses: 0
    };
    for (var key in formData) {
        let match = key.match(/((\w*)\[([0-9]*)\])\.Select/);
        if (match) {
            if (match[3] != 0) {
                let orginNameKey = match[1] + '.' + 'Name';
                let newNameKey = match[2] + '[' + names[match[2]] + ']' + '.' + 'Name';
                if (orginNameKey != newNameKey) {
                    let orginSelectKey = match[1] + '.' + 'Selected';
                    let newSelectKey = match[2] + '[' + names[match[2]] + ']' + '.' + 'Selected';

                    formData[newNameKey] = formData[orginNameKey];
                    formData[newSelectKey] = 'true';
                    delete formData[orginNameKey];
                    delete formData[orginSelectKey];
                }
            }

            names[match[2]] = names[match[2]] + 1;
        }
    }
    return formData;
}

function resetCheckboxStatus(id) {
    $("#" + id.toString()).prop('checked', false);
}
bindCheckBox();


function deleteItemSuccessCallback(form) {
    $(form).closest("tr").find(".isDeleted").text("Yes");
}

function becomMemberItemSuccessCallback(form) {
    $(form).closest("tr").find(".role").text("Member");
}


function applicationMembers(obj, id) {
    $.ajax({
        type: "post",
        url: "/account/applyformember",
        headers: {
            "X-XSRF-Token": $('#RequestVerificationToken').val(),
            "id": id
        },
        data: {
            "id": id
        },
        success: function (response) {
            if (response.code == 200) {
                $(obj).prop('disabled', true);
            }
        }
    });
}
var showSearchItem = { Names: true, Manufacturers: true, Models: true, Cities: true, AuctionHouse: true }
function SearchItemDisplay() {
    $('.search').on('click', '.show-more', function (){
        let type = $(this).closest('.form-group ').find('.control-label').first().attr('for');
        showSearchItem[type] = false;
        $(this).nextAll().show();
        $(this).hide();
        $(this).prevAll('.hidden-more').show();
    });
    $('.search').on('click', '.hidden-more', function (){
        let type = $(this).closest('.form-group ').find('.control-label').first().attr('for');
        showSearchItem[type] = true;
        var allFormCheck = $(this).closest(".form-group").find(".form-check");
        allFormCheck.eq(5).nextAll(".form-check").hide();
        $(this).prevAll('.show-more').show();
        $(this).hide();
    });

    let group = $('.search .form-group');
    $.each(group, function(){
        let length = $(this).find('.form-check').length;
        if(length > 6){

            let type = $(this).find('.control-label').first().attr('for');
            
            if(!$(this).find('.show-more').length > 0)
                $(this).find('.form-check').eq(5).after("<span class='show-more'><span>");
            if(!$(this).find('.hidden-more').length > 0)
                $(this).find('.form-check:last').after("<span class='hidden-more'><span>");
            if(!showSearchItem[type]){
                $(this).find('.show-more').hide();
                $(this).find('.show-more').nextAll().show();
                // $(this).find('.hidden-more').show();
            }else{
                $(this).find('.show-more').show();
                $(this).find('.hidden-more').hide();
            }
        }
    })
}
// SearchItemDisplay();

