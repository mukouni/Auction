using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Auction.Entities;
using Auction.Identity.Entities;
using Auction.Models.EquipmentViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using Auction.Data;
using Auction.Entities.Filters;
using Auction.Extensions;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Microsoft.Net.Http.Headers;
using Auction.Extensions.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Auction.Extensions.Alerts;
using Auction.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Auction.Models;
using Auction.Extensions.AuthContext;
using Auction.Entities.Enums;
using Microsoft.EntityFrameworkCore.Internal;
using Castle.Core.Internal;
using X.PagedList;
using AutoMapper.QueryableExtensions;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Auction.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EquipmentController : Controller
    {

        private readonly AuctionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AuctionSettings _appSettings { get; }

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public EquipmentController(AuctionDbContext context,
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    IMapper mapper,
                    ILoggerFactory loggerFactory,
                    IOptions<AuctionSettings> appSettings)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<EquipmentController>();
            _appSettings = appSettings.Value;
        }

        [HttpGet("[action]")]
        [HttpGet("")]
        [HttpPost("[action]")]
        [HttpPost("")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public IActionResult Index(SearchEquipmentViewModel searchEquipment)
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "设备列表" },
                { "href", "javascript: void(0)" }
            });
            ViewData["breadcrumb"] = breadcrumb;
            searchEquipment.PageSizeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "20", Text = "每页20条" },
                new SelectListItem { Value = "50", Text = "每页50条" },
                new SelectListItem { Value = "100", Text = "每页100"  },
                new SelectListItem { Value = "1000", Text = "每页1000"  },
            };

            var query = _context.Equipments.AsQueryable<Equipment>();
            if (!string.IsNullOrEmpty(searchEquipment.KeyWord))
            {
                query = query.Where(x => x.Name.Contains(searchEquipment.KeyWord.Trim()) || x.Code.Contains(searchEquipment.KeyWord.Trim()));
            }
            query = query.OrderByDescending(e => e.LastUpdatedAt).ThenByDescending(e => e.CreatedAt).ThenBy(e => e.Name);

            var list = query.Paged(searchEquipment.CurrentPage, searchEquipment.PageSize)
                            // .Select(equipment =>  _mapper.Map<EquipmentViewModel>(equipment)) //因为设置了延迟加载会报错
                            .ProjectTo<EquipmentViewModel>();
                            // .Project().To<EquipmentViewModel>()

            var totalCount = query.Count();

            searchEquipment.Equipments = new StaticPagedList<EquipmentViewModel>(
                    list,
                    searchEquipment.CurrentPage,
                    searchEquipment.PageSize,
                    totalCount);
            searchEquipment.Count = totalCount;

            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(searchEquipment, totalCount);

            // var myViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { { "SearchEquipmentViewModel", searchEquipment } };
            // myViewData.Model = searchEquipment;

            // PartialViewResult result = new PartialViewResult()
            // {
            //     ViewName = "searchEquipmentPartial",
            //     ViewData = myViewData,
            // };
            if (Request.Headers["Referer"].ToString().Contains("manage/system"))
            {
                return (IActionResult)PartialView("_IndexBodyPartial", searchEquipment);
            }
            return Request.IsAjaxRequest()
                ? (IActionResult)PartialView("_IndexTablePartial", searchEquipment)
                : View(searchEquipment);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public IActionResult New()
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "设备列表" },
                { "href", "/equipment/index" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "新建设备" },
                { "href", "/equipment/new" }
            });
            ViewData["breadcrumb"] = breadcrumb;

            var equipment = new EquipmentViewModel();
            equipment.Code = "A" + GenerateCode();
            var group1 = new SelectListGroup() { Name = "Group 1" };
            equipment.Currencies = _context.Currencies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            equipment.Currencies.First().Selected = true;
            return View(equipment);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public async Task<IActionResult> Create(EquipmentViewModel EquipmentVM)
        {

            Equipment equipment = _mapper.Map<Equipment>(EquipmentVM);
            equipment.CoverPhoto = null;
            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)).WithSuccess("创建成功", "");
        }

        [HttpGet("{id:Guid}/[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        [GenerateAntiforgeryTokenCookieForAjax]
        [CustomAuthorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var breadcrumb = new List<IDictionary<string, string>>();
            breadcrumb.Add(new Dictionary<string, string>()
            {
                { "text", "系统管理" },
                { "href", "javascript: void(0)" }
            });
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "设备列表" },
                { "href", "/equipment/index" }
            });
            ViewData["breadcrumb"] = breadcrumb;
            breadcrumb.Add(new Dictionary<string, string>
            {
                { "text", "修改设备"},
                { "href", "/equipment/"+id+"/edit"}
            });
            ViewData["breadcrumb"] = breadcrumb;

            if (id == null)
            {
                return NotFound();
            }
            var equipment = await _context.Equipments.FirstOrDefaultAsync(e => e.Id == id);
            var equipmentVM = _mapper.Map<EquipmentViewModel>(equipment);
            equipmentVM.Currencies = _context.Currencies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(equipmentVM);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments.FindAsync(id);
            equipment.IsDeleted = CommonEnum.IsDeleted.Yes;
            _context.Equipments.Attach(equipment);
            _context.Entry(equipment).Property(x => x.IsDeleted).IsModified = true;
            await _context.SaveChangesAsync();
            response.SetData(new { id = id });
            return Ok(response);
        }

        [HttpPost("{id:Guid}/[action]")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public async Task<IActionResult> Update(EquipmentViewModel equipmentVM, Guid id)
        {
            if (id != equipmentVM.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var changedEquipment = new Equipment();
                    if (equipmentVM.CoverPhoto.EquipmentId == new Guid())
                    {
                        changedEquipment = _mapper.Map<EquipmentViewModel, Equipment>(equipmentVM);
                        changedEquipment.CoverPhoto = null;
                    }

                    _context.Equipments.Update(changedEquipment);
                    // var list = new List<Equipment>();
                    // for (int i = 9; i < 20; i++)
                    // {
                    //     var enew = new Equipment();
                    //     enew = _mapper.Map<EquipmentViewModel, Equipment>(equipmentVM);
                    //     enew.Id = Guid.NewGuid();
                    //     enew.Name = enew.Name.Split("新设备")[0] + ((int.Parse(enew.Name.Split("新设备")[1]) + 100+ i));
                    //     enew.Code = "Code" + i;
                    //     enew.ProductionDate = ((DateTime)(enew.ProductionDate)).AddMonths(i);
                    //     list.Add(enew);
                    // }
                    // await _context.AddRangeeAsync(list);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                    Console.WriteLine(ex.Message);
                    _logger.LogError(ex.Message);
                }
                var equipment = await _context.Equipments.FirstOrDefaultAsync(e => e.Id == id);
            }
            return RedirectToAction(nameof(Edit), equipmentVM);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Show(Guid id, string type)
        {
            var equipment = _mapper.Map<EquipmentViewModel>(await _context.Equipments.FindAsync(id));
             if (equipment == null)
            {
                return NotFound();
            }
            ViewData["Type"] = type;
            return View(equipment);
        }

        [HttpGet("[action]/{id:Guid}")]
        public async Task<IActionResult> ShowAll(Guid id, string type)
        {
            var equipment = _mapper.Map<EquipmentViewModel>(await _context.Equipments.FindAsync(id));
             if (equipment == null)
            {
                return NotFound();
            }
            ViewData["Type"] = type;
            return View(equipment);
        }

        // [HttpPost("[action]")]
        // [DisableFormValueModelBinding]
        // // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> UploadPhoto()
        // {
        //     if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        //     {
        //         return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
        //     }

        //     // Used to accumulate all the form url encoded key value pairs in the 
        //     // request.
        //     var formAccumulator = new KeyValueAccumulator();
        //     string targetFilePath = null;

        //     var boundary = MultipartRequestHelper.GetBoundary(
        //         MediaTypeHeaderValue.Parse(Request.ContentType),
        //         _defaultFormOptions.MultipartBoundaryLengthLimit);
        //     var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        //     var section = await reader.ReadNextSectionAsync();
        //     while (section != null)
        //     {
        //         ContentDispositionHeaderValue contentDisposition;
        //         var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

        //         if (hasContentDispositionHeader)
        //         {
        //             if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
        //             {
        //                 string photoDir = _appSettings.FilesRootDir;
        //                 string fileExt = Path.GetExtension(HttpContext.Request.Form.Files[0].FileName);
        //                 string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt;
        //                 string uri = Path.Combine("equipment", newFileName);
        //                 string pathString = Path.Combine(photoDir, uri);
        //                 Directory.CreateDirectory(pathString);
        //                 if (!File.Exists(pathString))
        //                 {
        //                     using (var targetStream = File.Create(pathString))
        //                     {
        //                         await section.Body.CopyToAsync(targetStream);
        //                         _logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
        //                     }
        //                 }

        //                 targetFilePath = Path.GetTempFileName();
        //                 using (var targetStream = File.Create(targetFilePath))
        //                 {
        //                     await section.Body.CopyToAsync(targetStream);

        //                     _logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
        //                 }

        //             }
        //             else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
        //             {
        //                 // Content-Disposition: form-data; name="key"
        //                 //
        //                 // value

        //                 // Do not limit the key name length here because the 
        //                 // multipart headers length limit is already in effect.
        //                 var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
        //                 var encoding = GetEncoding(section);
        //                 using (var streamReader = new StreamReader(
        //                     section.Body,
        //                     encoding,
        //                     detectEncodingFromByteOrderMarks: true,
        //                     bufferSize: 1024,
        //                     leaveOpen: true))
        //                 {
        //                     // The value length limit is enforced by MultipartBodyLengthLimit
        //                     var value = await streamReader.ReadToEndAsync();
        //                     if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
        //                     {
        //                         value = string.Empty;
        //                     }
        //                     formAccumulator.Append(key, value);

        //                     if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
        //                     {
        //                         throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
        //                     }
        //                 }
        //             }
        //         }

        //         // Drains any remaining section body that has not been consumed and
        //         // reads the headers for the next section.
        //         section = await reader.ReadNextSectionAsync();
        //     }

        //     var photo = new Photo();
        //     photo.RequestPath = targetFilePath;

        //     // List<Photo> eqphotos = new List<Photo>();
        //     // photos.Add(eqphotos);

        //     // await _context.AddRangeeAsync(photos);

        //     var formValueProvider = new FormValueProvider(
        //             BindingSource.Form,
        //             new FormCollection(formAccumulator.GetResults()),
        //             CultureInfo.CurrentCulture);

        //     var equipmentViewModel = new CreateEquipmentViewModel()
        //     {
        //         // FilePath = targetFilePath
        //     };
        //     return Json(equipmentViewModel);
        // }


        // 上传
        [Authorize(Roles = "Admin, Staff, Develpment")]
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadPhotoAsync(EquipmentPhotoViewModel equipmentPhoto)
        {
            var response = ResponseModelFactory.CreateInstance;
            var photo = equipmentPhoto.photo;
            if (photo == null)
            {
                response.SetNotFound("没有可上传的文件");
                return Ok(response);
            }

            var equipment = await _context.Equipments.FirstOrDefaultAsync(e => e.Id == equipmentPhoto.EquipmentId);
            if (equipment == null)
            {
                response.SetNotFound("没有找到设备");
                return Ok(response);
            }

            string FilesRootDir = _appSettings.FilesRootDir;
            string pathString = Path.Combine(FilesRootDir, "images", typeof(Equipment).Name);
            if (!Directory.Exists(pathString))
            {
                try
                {
                    Directory.CreateDirectory(pathString);
                }
                catch (System.IO.IOException e)
                {
                    response.SetFailed("创建目录失败");
                    _logger.LogError(e.Message);
                    return Ok(response);
                }
            }

            string fileExt = Path.GetExtension(equipmentPhoto.photo?.FileName);
            string newFileName = System.Guid.NewGuid().ToString() + fileExt;

            string filePathString = Path.Combine(pathString, newFileName);

            var newPhoto = _mapper.Map<Photo>(equipmentPhoto);
            using (var stream = System.IO.File.Create(filePathString))
            {
                await photo.CopyToAsync(stream);
                newPhoto = newPhoto.MapFrom(photo,
                                            newFileName,
                                            RequestPath: "/images/" + typeof(Equipment).Name + "/" + newFileName,
                                            SavePath: "\\images\\" + typeof(Equipment).Name + "\\" + newFileName);
                newPhoto.EquipmentId = equipment.Id;

                if (equipmentPhoto.PhotoType == "Cover")
                {
                    equipment.CoverPhoto = newPhoto;
                }
                else if (equipmentPhoto.PhotoType == "Exterior")
                {
                    equipment.ExteriorPhotos.Add(newPhoto);
                }
                else if (equipmentPhoto.PhotoType == "TrackedChassis")
                {
                    equipment.TrackedChassisPhotos.Add(newPhoto);
                }
                else if (equipmentPhoto.PhotoType == "Cab")
                {
                    equipment.CabPhotos.Add(newPhoto);
                }
                else if (equipmentPhoto.PhotoType == "Boom")
                {
                    equipment.BoomPhotos.Add(newPhoto);
                }
                else if (equipmentPhoto.PhotoType == "Engine")
                {
                    equipment.EnginePhotos.Add(newPhoto);
                }

                await _context.SaveChangesAsync();
            }

            var pho = _mapper.Map<PhotoViewModel>(await _context.Photos.FirstOrDefaultAsync(p => p.FileName == newPhoto.FileName));
            response.SetData(Json(pho).Value);
            return Ok(response);
        }

        /// <summary>
        /// <param name="photoName">包含图片名字和后缀</param>
        /// </summary>
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public async Task<IActionResult> DeletePhoto(string photoName, Guid key)
        {
            var response = ResponseModelFactory.CreateInstance;

            var photo = _context.Photos.FirstOrDefault(p => p.FileName == photoName);
            if (photo == null)
            {
                response.SetNotFound("没有找到图片");
                return Ok(response);
            }

            string sourceFile = System.IO.Path.Combine(_appSettings.FilesRootDir + photo.SavePath);
            if (System.IO.File.Exists(sourceFile))
            {
                try
                {
                    System.IO.File.Delete(sourceFile);
                }
                catch (System.IO.IOException e)
                {
                    response.SetFailed("删除图片失败");
                    _logger.LogError(e.Message);
                    return Ok(response);
                }

                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();
                return Ok(response);
            }
            response.SetNotFound("删除时磁盘上没有找到图片");
            return Ok(response);
        }

        /// <summary>
        /// <param name="photoName">包含图片名字和后缀</param>
        /// </summary>
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public async Task<IActionResult> SetHiddenPhototAfterSold(string photoName, Guid equipmentId)
        {
            var response = ResponseModelFactory.CreateInstance;
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.FileName == photoName);
            if (photo == null)
            {
                response.SetNotFound("没有找到图片");
                return Ok(response);
            }

            // var originHiddenPhototAfterSold = await _context.Photos.FirstOrDefaultAsync(p => p.EquipmentId == equipmentId && p.IsHiddenAfterSold == true);
            // if (originHiddenPhototAfterSold != null)
            // {
            //     originHiddenPhototAfterSold.IsHiddenAfterSold = false;
            //     _context.Entry(originHiddenPhototAfterSold).State = EntityState.Modified;
            // }

            photo.IsHiddenAfterSold = !photo.IsHiddenAfterSold;
            _context.Entry(photo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(response);
        }


        /// <summary>
        /// 未拍卖设备
        /// </summary>
        [HttpGet("[action]")]
        [HttpPost("[action]")]
        public IActionResult NoAuction(SearchEquipmentViewModel searchEquipment)
        {
            ViewData["Title"] = "未拍卖设备";
            ViewData["Action"] = "NoAuction";
            ViewData["ShowSoltAtSort"] = true;
            ViewData["ShowPriceSort"] = true;
            using (_context)
            {
                var query = _context.Equipments.AsQueryable<Equipment>();
                if (!string.IsNullOrEmpty(searchEquipment.KeyWord))
                {
                    query = query.Where(x => 
                        x.Name.Contains(searchEquipment.KeyWord.Trim()) || 
                        x.Code.Contains(searchEquipment.KeyWord.Trim()) ||
                        x.Model.Contains(searchEquipment.KeyWord.Trim()));
                }
                query = query.Where(x => x.AuctionHouse != null && x.DealPrice == null);
                query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No || x.IsDeleted == null);

                searchEquipment = SliderMaxMin(searchEquipment, query);
                searchEquipment = SearchConditionRange(searchEquipment, query);

                // 全匹配查询，不过滤所有条件
                query = SearchCondition(query, searchEquipment);

                query = query.AuctionSort(searchEquipment);

                var totalCount = query.Count();
                var list = query.Paged(searchEquipment.CurrentPage, searchEquipment.PageSize)
                               // .Select(equipment =>  _mapper.Map<EquipmentViewModel>(equipment)) //因为设置了延迟加载会报错
                               .ProjectTo<EquipmentViewModel>().ToList();
                // .Project().To<EquipmentViewModel>()

                searchEquipment.Equipments = new StaticPagedList<EquipmentViewModel>(
                    list,
                    searchEquipment.CurrentPage,
                    searchEquipment.PageSize,
                    totalCount);
                searchEquipment.Count = totalCount;

                if (Request.IsAjaxGetRequest())
                {
                    // 搜索
                    return (IActionResult)PartialView("_AuctionInfoPartial", searchEquipment);
                }
                if (Request.IsAjaxPostRequest())
                {
                    // 翻页
                     if(list.Count == 0){
                        var response = ResponseModelFactory.CreateResultInstance;
                        response.SetData("<p class=\"text-center\">没有更多数据了！</p>", list.Count);
                        return Ok(response);
                    }
                    return (IActionResult)PartialView("_AuctionListPartial", searchEquipment);
                }
                // 刷新
                return View(searchEquipment);
            }
        }

        /// <summary>
        /// 已拍卖设备
        /// </summary>
        [HttpGet("[action]")]
        [HttpPost("[action]")]
        public IActionResult Auctioned(SearchEquipmentViewModel searchEquipment)
        {
            ViewData["Title"] = "已拍卖设备";
            ViewData["Action"] = "Auctioned";
            ViewData["ShowSoltAtSort"] = true;
            ViewData["ShowPriceSort"] = true;
            using (_context)
            {
                var query = _context.Equipments.AsQueryable<Equipment>();
                if (!string.IsNullOrEmpty(searchEquipment.KeyWord))
                {
                    query = query.Where(x => 
                        x.Name.Contains(searchEquipment.KeyWord.Trim()) || 
                        x.Code.Contains(searchEquipment.KeyWord.Trim()) ||
                        x.Model.Contains(searchEquipment.KeyWord.Trim()));
                }
                query = query.Where(x => x.AuctionHouse != null && x.DealPrice != null);
                query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No || x.IsDeleted == null);

                searchEquipment = SliderMaxMin(searchEquipment, query);
                searchEquipment = SearchConditionRange(searchEquipment, query);

                // 全匹配查询，不过滤所有条件
                query = SearchCondition(query, searchEquipment);

                query = query.AuctionSort(searchEquipment);

                var totalCount = query.Count();
                var list = query.Paged(searchEquipment.CurrentPage, searchEquipment.PageSize)
                               // .Select(equipment =>  _mapper.Map<EquipmentViewModel>(equipment)) //因为设置了延迟加载会报错
                               .ProjectTo<EquipmentViewModel>().ToList();
                // .Project().To<EquipmentViewModel>()

                searchEquipment.Equipments = new StaticPagedList<EquipmentViewModel>(list,
                                                                                    searchEquipment.CurrentPage,
                                                                                    searchEquipment.PageSize,
                                                                                    totalCount);
                searchEquipment.Count = totalCount;

                if (Request.IsAjaxGetRequest())
                {
                    // 搜索
                    return (IActionResult)PartialView("_AuctionInfoPartial", searchEquipment);
                }
                if (Request.IsAjaxPostRequest())
                {
                    // 翻页
                    if(list.Count == 0){
                        var response = ResponseModelFactory.CreateResultInstance;
                        response.SetData("<p class=\"text-center\">没有更多数据了！</p>", list.Count);
                        return Ok(response);
                    }
                    return (IActionResult)PartialView("_AuctionListPartial", searchEquipment);
                }
                // 刷新
                return View(searchEquipment);
            }
        }

        /// <summary>
        /// 代采购设备
        /// </summary>
        [HttpGet("[action]")]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, Development, Staff, Member")]
        public IActionResult InsteadAuction(SearchEquipmentViewModel searchEquipment)
        {
            ViewData["Title"] = "代购拍卖设备";
            ViewData["Action"] = "InsteadAuction";
            ViewData["ShowSoltAtSort"] = true;
            ViewData["ShowPriceSort"] = true;
            using (_context)
            {
                var query = _context.Equipments.AsQueryable<Equipment>();
                if (!string.IsNullOrEmpty(searchEquipment.KeyWord))
                {
                    query = query.Where(x => 
                        x.Name.Contains(searchEquipment.KeyWord.Trim()) || 
                        x.Code.Contains(searchEquipment.KeyWord.Trim()) ||
                        x.Model.Contains(searchEquipment.KeyWord.Trim()));
                }
                query = query.Where(x => x.AuctionHouse == null);
                query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No || x.IsDeleted == null);

                searchEquipment = SliderMaxMin(searchEquipment, query);
                searchEquipment = SearchConditionRange(searchEquipment, query);

                // 不允许搜索
                searchEquipment.PriceMin = null;
                searchEquipment.PriceMax = null;
                // searchEquipment.SoldAt = null;


                // 全匹配查询，不过滤所有条件
                query = SearchCondition(query, searchEquipment);

                query = query.AuctionSort(searchEquipment);

                var totalCount = query.Count();
                var list = query.Paged(searchEquipment.CurrentPage, searchEquipment.PageSize)
                               // .Select(equipment =>  _mapper.Map<EquipmentViewModel>(equipment)) //因为设置了延迟加载会报错
                               .ProjectTo<EquipmentViewModel>().ToList();
                // .Project().To<EquipmentViewModel>()

                searchEquipment.Equipments = new StaticPagedList<EquipmentViewModel>(
                    list,
                    searchEquipment.CurrentPage,
                    searchEquipment.PageSize,
                    totalCount);
                searchEquipment.Count = totalCount;

                if (Request.IsAjaxGetRequest())
                {
                    // 搜索
                    return (IActionResult)PartialView("_AuctionInfoPartial", searchEquipment);
                }
                if (Request.IsAjaxPostRequest())
                {
                    // 翻页
                    if(list.Count == 0){
                        var response = ResponseModelFactory.CreateResultInstance;
                        response.SetData("<p class=\"text-center\">没有更多数据了！</p>", list.Count);
                        return Ok(response);
                    }
                    return (IActionResult)PartialView("_AuctionListPartial", searchEquipment);
                }
                // 刷新
                return View(searchEquipment);
            }
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }

        private IQueryable<Equipment> SearchCondition(IQueryable<Equipment> query, SearchEquipmentViewModel searchEquipment, string removeCondition = "")
        {

            if (removeCondition != "Name" && searchEquipment.Names != null && searchEquipment.Names.Any(n => n.Selected == true))
            {
                var names = searchEquipment.Names.Where(n => n.Selected == true).Select(n => n.Name);
                query = query.Where(x => names.Contains(x.Name));
            }
            if (removeCondition != "Manufacturer" && searchEquipment.Manufacturers != null && searchEquipment.Manufacturers.Any(n => n.Selected == true))
            {
                var manufacturers = searchEquipment.Manufacturers.Where(n => n.Selected == true).Select(n => n.Name);
                query = query.Where(x => manufacturers.Contains(x.Manufacturer));
            }
            if (removeCondition != "Model" && searchEquipment.Models != null && searchEquipment.Models.Any(n => n.Selected == true))
            {
                var models = searchEquipment.Models.Where(n => n.Selected == true).Select(n => n.Name);
                query = query.Where(x => models.Contains(x.Model));
            }
            if (removeCondition != "AuctionHouse" && searchEquipment.AuctionHouses != null && searchEquipment.AuctionHouses.Any(n => n.Selected == true))
            {
                var auctionHouses = searchEquipment.AuctionHouses.Where(n => n.Selected == true).Select(n => n.Name);
                query = query.Where(x => auctionHouses.Contains(x.AuctionHouse));
            }
            if (removeCondition != "Country" && searchEquipment.Countries != null && searchEquipment.Countries.Any(n => n.Selected == true))
            {
                var countries = searchEquipment.Countries.Where(n => n.Selected == true).Select(n => n.Name);
                query = query.Where(x => countries.Contains(x.Country));
            }
            if (removeCondition != "City" && searchEquipment.Cities != null && searchEquipment.Cities.Any(n => n.Selected == true))
            {
                var cities = searchEquipment.Cities.Where(n => n.Selected == true).Select(n => n.Name);
                query = query.Where(x => cities.Contains(x.City));
            }

            // if (removeCondition != "ProductionDate" &&
            //     searchEquipment.ProductionDateRangeMin > 0 && searchEquipment.ProductionDateRangeMax == 0)
            // {
            //     query = query.Where(x => ((DateTime)x.ProductionDate).Year >= searchEquipment.ProductionDateRangeMin);

            // }
            // if (removeCondition != "ProductionDate" &&
            //     searchEquipment.ProductionDateRangeMin == 0 && searchEquipment.ProductionDateRangeMax > 0)
            // {
            //     query = query.Where(x => ((DateTime)x.ProductionDate).Year <= searchEquipment.ProductionDateRangeMax);
            // }
            // if (removeCondition != "ProductionDate" &&
            //     searchEquipment.ProductionDateRangeMin > 0 && searchEquipment.ProductionDateRangeMax > 0)
            // {
            //     query = query.Where(x => ((DateTime)x.ProductionDate).Year >= searchEquipment.ProductionDateRangeMin && ((DateTime)x.ProductionDate).Year <= searchEquipment.ProductionDateRangeMax);

            // }
            var ProductionDateMin = searchEquipment.ProductionDateMin ?? ApplicationConstant.intDefaultValue;
            var ProductionDateMax = searchEquipment.ProductionDateMax ?? ApplicationConstant.intMaxValue;
            if (removeCondition != "ProductionDate" &&
                searchEquipment.ProductionDateRangeMin != null && searchEquipment.ProductionDateRangeMax != null &&
                // searchEquipment.ProductionDateRangeMin >= ProductionDateMin && searchEquipment.ProductionDateRangeMax <= ProductionDateMax &&
                searchEquipment.ProductionDateRangeMin <= searchEquipment.ProductionDateRangeMax)
            {
                query = query.Where(x =>
                    ((DateTime)x.ProductionDate).Year >= searchEquipment.ProductionDateRangeMin &&
                    ((DateTime)x.ProductionDate).Year <= searchEquipment.ProductionDateRangeMax);
            }
            else
            {
                // if (removeCondition == "ProductionDate")
                // {
                //     if (searchEquipment.ProductionDateRangeMin != null)
                //         searchEquipment.ProductionDateRangeMin = ProductionDateMin;
                //     if (searchEquipment.ProductionDateRangeMax != null)
                //         searchEquipment.ProductionDateRangeMax = ProductionDateMax;
                // }
            }

            // if (removeCondition != "WorkingTime" &&
            //     searchEquipment?.WorkingTimeRangeMin >= 0 &&
            //     searchEquipment?.WorkingTimeRangeMax == 0)
            // {
            //     query = query.Where(x => x.WorkingTime >= searchEquipment.WorkingTimeRangeMin);

            // }
            // if (removeCondition != "WorkingTime" &&
            //     searchEquipment.WorkingTimeRangeMin < 0 && 
            //     searchEquipment.WorkingTimeRangeMax >= 0)
            // {
            //     query = query.Where(x => x.WorkingTime <= searchEquipment.WorkingTimeRangeMax);
            // }
            // if (removeCondition != "WorkingTime" &&
            //     searchEquipment.WorkingTimeRangeMin > 0 && 
            //     searchEquipment.WorkingTimeRangeMax > 0)
            // {
            //     query = query.Where(x => 
            //         x.WorkingTime >= searchEquipment.WorkingTimeRangeMin && x.WorkingTime <= searchEquipment.WorkingTimeRangeMax);

            // }
            var WorkingTimeMin = searchEquipment.WorkingTimeMin ?? ApplicationConstant.longDefaultValue;
            var WorkingTimeMax = searchEquipment.WorkingTimeMax ?? ApplicationConstant.longMaxValue;
            if (removeCondition != "WorkingTime" &&
                searchEquipment.WorkingTimeRangeMin != null && searchEquipment.WorkingTimeRangeMax != null &&
                // searchEquipment.WorkingTimeRangeMin >= WorkingTimeMin && searchEquipment.WorkingTimeRangeMax <= WorkingTimeMax &&
                searchEquipment.WorkingTimeRangeMin <= searchEquipment.WorkingTimeRangeMax)
            {
                query = query.Where(x =>
                    x.WorkingTime >= searchEquipment.WorkingTimeRangeMin && x.WorkingTime <= searchEquipment.WorkingTimeRangeMax);
            }
            else
            {
                // if (searchEquipment.WorkingTimeRangeMin != null)
                //     searchEquipment.WorkingTimeRangeMin = WorkingTimeMin;
                // if (searchEquipment.WorkingTimeRangeMax != null)
                //     searchEquipment.WorkingTimeRangeMax = WorkingTimeMax;
            }

            // if (removeCondition != "DealPrice" &&
            //     searchEquipment.DealPriceRangeMin >= 0 && searchEquipment.DealPriceRangeMax == 0)
            // {
            //     query = query.Where(x => x.DealPrice >= searchEquipment.DealPriceRangeMin);

            // }
            // if (removeCondition != "DealPrice" &&
            //     searchEquipment.DealPriceRangeMin < 0 && searchEquipment.DealPriceRangeMax >= 0)
            // {
            //     query = query.Where(x => x.DealPrice <= searchEquipment.DealPriceRangeMax);
            // }
            // if (removeCondition != "DealPrice" &&
            //     searchEquipment.DealPriceRangeMin > 0 && searchEquipment.DealPriceRangeMax > 0)
            // {
            //     query = query.Where(x => x.DealPrice >= searchEquipment.DealPriceRangeMin && x.DealPrice <= searchEquipment.DealPriceRangeMax);
            // }
            var DealPriceMin = searchEquipment.DealPriceMin ?? ApplicationConstant.decimalDefaultValue;
            var DealPriceMax = searchEquipment.DealPriceMax ?? ApplicationConstant.decimalMaxValue;
            if (removeCondition != "DealPrice" &&
                searchEquipment.DealPriceRangeMin != null && searchEquipment.DealPriceRangeMax != null &&
                // searchEquipment.DealPriceRangeMin >= DealPriceMin && searchEquipment.DealPriceRangeMax <= DealPriceMax &&
                searchEquipment.DealPriceRangeMin <= searchEquipment.DealPriceRangeMax)
            {
                query = query.Where(x =>
                    x.DealPrice >= searchEquipment.DealPriceRangeMin && x.DealPrice <= searchEquipment.DealPriceRangeMax);
            }
            else
            {
                // if (searchEquipment.DealPriceRangeMin != null)
                //     searchEquipment.DealPriceRangeMin = DealPriceMin;
                // if (searchEquipment.DealPriceRangeMax != null)
                //     searchEquipment.DealPriceRangeMax = DealPriceMax;
            }

            var PriceMin = searchEquipment.PriceMin ?? ApplicationConstant.decimalDefaultValue;
            var PriceMax = searchEquipment.PriceMax ?? ApplicationConstant.decimalMaxValue;
            if (removeCondition != "Price" &&
                searchEquipment.PriceRangeMin >= PriceMin && searchEquipment.PriceRangeMax <= PriceMax &&
                // searchEquipment.PriceRangeMin != null && searchEquipment.PriceRangeMax != null &&
                searchEquipment.PriceRangeMin <= searchEquipment.PriceRangeMax)
            {
                query = query.Where(x =>
                    x.Price >= searchEquipment.PriceRangeMin && x.Price <= searchEquipment.PriceRangeMax);
            }
            else
            {
                // if (searchEquipment.PriceRangeMin != null)
                //     searchEquipment.PriceRangeMin = PriceMin;
                // if (searchEquipment.PriceRangeMax != null)
                //     searchEquipment.PriceRangeMax = PriceMax;
            }

            // if (searchEquipment?.SoldAtRangeMin < searchEquipment?.SoldAtMin ||
            //     searchEquipment?.SoldAtRangeMin > searchEquipment?.SoldAtMax)
            // {
            //     searchEquipment.SoldAtRangeMin = searchEquipment.SoldAtMin;
            // }
            // if (searchEquipment?.SoldAtRangeMax > searchEquipment?.SoldAtMax ||
            //     searchEquipment?.SoldAtRangeMax < searchEquipment?.SoldAtMin)
            // {
            //     searchEquipment.SoldAtRangeMax = searchEquipment.SoldAtMax;
            // }
            // if (searchEquipment?.SoldAtRangeMin > searchEquipment?.SoldAtRangeMax)
            // {
            //     searchEquipment.SoldAtRangeMin = searchEquipment.SoldAtRangeMax;
            // }
            // if (searchEquipment?.SoldAtRangeMax < searchEquipment.SoldAtRangeMin)
            // {
            //     searchEquipment.SoldAtRangeMax = searchEquipment.SoldAtRangeMin;
            // }
            var SoldAtMin = searchEquipment.SoldAtMin ?? ApplicationConstant.DateTimeMinValue;
            var SoldAtMax = searchEquipment.SoldAtMax ?? ApplicationConstant.DateTimeMaxValue;
            if (removeCondition != "SoldAt" &&
                searchEquipment.SoldAtRangeMin != null && searchEquipment.SoldAtRangeMax != null &&
                // searchEquipment.SoldAtRangeMin >= SoldAtMin && searchEquipment.SoldAtRangeMax <= SoldAtMax &&
                searchEquipment.SoldAtRangeMin <= searchEquipment.SoldAtRangeMax)
            {
                query = query.Where(x =>
                    ((DateTime)x.SoldAt) >= searchEquipment.SoldAtRangeMin && ((DateTime)x.SoldAt) <= searchEquipment.SoldAtRangeMax);
            }
            else
            {
                // if (searchEquipment.SoldAtRangeMin != null)
                //     searchEquipment.SoldAtRangeMin = SoldAtMin;
                // if (searchEquipment.SoldAtRangeMax != null)
                //     searchEquipment.SoldAtRangeMax = SoldAtMax;
            }

            return query;
        }

        private string GenerateCode()
        {
            var query = _context.Equipments.AsQueryable<Equipment>();


            int DayCount = query.Where(e =>
                DateTime.Now.Year == ((DateTime)e.CreatedAt).Year &&
                DateTime.Now.Month == ((DateTime)e.CreatedAt).Month &&
                DateTime.Now.Day == ((DateTime)e.CreatedAt).Day).Count() + 1;

            //   DayCount ""+ id.ToString().Split("-").;
            var s = new StringBuilder();
            s.Append(DateTime.Now.Year.ToString().Substring(2, 2));
            s.Append(DateTime.Now.Month.ToString().PadLeft(2, '0'));
            s.Append(DateTime.Now.Day.ToString().PadLeft(2, '0'));
            s.Append(DayCount.ToString().PadLeft(3, '0'));

            return s.ToString();
        }


        private SearchEquipmentViewModel SearchConditionRange(SearchEquipmentViewModel searchEquipment, IQueryable<Equipment> query)
        {
            var selectedNames = new string[] { };
            if (searchEquipment.Names != null)
                selectedNames = searchEquipment.Names?.Where(e => e.Selected).Select(e => e.Name).ToArray();

            var selectedManufacturers = new string[] { };
            if (searchEquipment.Manufacturers != null)
                selectedManufacturers = searchEquipment.Manufacturers?.Where(e => e.Selected).Select(e => e.Name).ToArray();

            var selectedAuctionHouses = new string[] { };
            if (searchEquipment.Manufacturers != null)
                selectedAuctionHouses = searchEquipment.AuctionHouses?.Where(e => e.Selected).Select(e => e.Name).ToArray();

            var selectedCountries = new string[] { };
            if (searchEquipment.Manufacturers != null)
                selectedCountries = searchEquipment.Countries?.Where(e => e.Selected).Select(e => e.Name).ToArray();

            var selectedCities = new string[] { };
            if (searchEquipment.Manufacturers != null)
                selectedCities = searchEquipment.Cities?.Where(e => e.Selected).Select(e => e.Name).ToArray();


            var selectedModels = new string[] { };
            if (searchEquipment.Models != null)
                selectedModels = searchEquipment.Models.Where(n => n.Selected == true).Select(n => n.Name).ToArray();

            searchEquipment.Models = SearchCondition(query, searchEquipment, "Model")
                                            .GroupBy(e => e.Model)
                                            .Select(grp => new Filter()
                                            {
                                                Name = grp.Key,
                                                Count = grp.Count()
                                            }).ToArray();

            if (searchEquipment.Models != null && selectedModels.Length > 0)
            {
                foreach (var model in searchEquipment.Models)
                {
                    if (selectedModels.Contains(model.Name))
                    {
                        model.Selected = true;
                    }
                }
            }

            searchEquipment.Models.OrderByDescending(m => m.SortNumber);

            searchEquipment.Names = SearchCondition(query, searchEquipment, "Name")
                                            .GroupBy(e => e.Name)
                                            .Select(grp => new Filter()
                                            {
                                                Name = grp.Key,
                                                Count = grp.Count()
                                            })
                                            .ToArray();

            foreach (var m in searchEquipment.Names)
            {
                if (m.Name == "液压挖掘机")
                {
                    m.SortNumber = 0;
                }
                else
                {
                    m.SortNumber = 100;
                }
                if (selectedNames != null)
                {
                    foreach (var selecteName in selectedNames)
                    {
                        if (selecteName == m.Name) m.Selected = true;
                    }
                }
            }
            searchEquipment.Names = searchEquipment.Names.OrderBy(e => e.SortNumber).ToArray();

            searchEquipment.Manufacturers = SearchCondition(query, searchEquipment, "Manufacturer")
                                                .GroupBy(e => e.Manufacturer)
                                                .Select(grp => new Filter()
                                                {
                                                    Name = grp.Key,
                                                    Count = grp.Count(),
                                                })
                                                .ToArray();
            if (selectedManufacturers.Length > 0)
            {
                foreach (var m in searchEquipment.Manufacturers)
                {
                    foreach (var selecteManufacturer in selectedManufacturers)
                    {
                        if (selecteManufacturer == m.Name) m.Selected = true;
                    }
                }
            }

            searchEquipment.AuctionHouses = SearchCondition(query, searchEquipment, "AuctionHouse")
                                                .GroupBy(e => e.AuctionHouse)
                                                .Select(grp => new Filter()
                                                {
                                                    Name = grp.Key,
                                                    Count = grp.Count(),
                                                })
                                                .ToArray();
            if (selectedAuctionHouses.Length > 0)
            {
                foreach (var m in searchEquipment.AuctionHouses)
                {
                    foreach (var selecteAuctionHouse in selectedAuctionHouses)
                    {
                        if (selecteAuctionHouse == m.Name) m.Selected = true;
                    }
                }
            }

            searchEquipment.Countries = SearchCondition(query, searchEquipment, "Country")
                                            .GroupBy(e => e.Country)
                                            .Select(grp => new Filter()
                                            {
                                                Name = grp.Key,
                                                Count = grp.Count(),
                                            })
                                            .ToArray();
            if (selectedCountries.Length > 0)
            {
                foreach (var m in searchEquipment.Countries)
                {
                    foreach (var selecteCountry in selectedCountries)
                    {
                        if (selecteCountry == m.Name) m.Selected = true;
                    }
                }
            }

            searchEquipment.Cities = SearchCondition(query, searchEquipment, "City")
                                        .GroupBy(e => e.City)
                                        .Select(grp => new Filter()
                                        {
                                            Name = grp.Key,
                                            Count = grp.Count(),
                                        })
                                        .ToArray();
            if (selectedCities.Length > 0)
            {
                foreach (var m in searchEquipment.Cities)
                {
                    foreach (var selecteCity in selectedCities)
                    {
                        if (selecteCity == m.Name) m.Selected = true;
                    }
                }
            }

            return searchEquipment;
        }


        private SearchEquipmentViewModel SliderMaxMin(SearchEquipmentViewModel searchEquipment, IQueryable<Equipment> query)
        {
            var WorkingTimeQuery = SearchCondition(query, searchEquipment, "WorkingTime");
            if (WorkingTimeQuery.Max(e => e.WorkingTime) != null && WorkingTimeQuery.Max(e => e.WorkingTime) > 0)
            {
                searchEquipment.WorkingTimeMin = WorkingTimeQuery.Min(e => e.WorkingTime);
                searchEquipment.WorkingTimeMax = WorkingTimeQuery.Max(e => e.WorkingTime);
                // 防止搜索超出范围
                if (searchEquipment.WorkingTimeRangeMin != null &&
                    searchEquipment.WorkingTimeMin > searchEquipment.WorkingTimeRangeMin)
                    searchEquipment.WorkingTimeRangeMin = searchEquipment.WorkingTimeMin;
                if (searchEquipment.WorkingTimeRangeMax != null &&
                    searchEquipment.WorkingTimeMax < searchEquipment.WorkingTimeRangeMax)
                    searchEquipment.WorkingTimeRangeMax = searchEquipment.WorkingTimeMax;
                if (searchEquipment.WorkingTimeRangeMin != null && searchEquipment.WorkingTimeRangeMax != null &&
                    searchEquipment.WorkingTimeRangeMin > searchEquipment.WorkingTimeRangeMax)
                {
                    searchEquipment.WorkingTimeRangeMin = searchEquipment.WorkingTimeMin;
                    searchEquipment.WorkingTimeRangeMax = searchEquipment.WorkingTimeMax;
                }

                if (searchEquipment.WorkingTimeRangeMin != null)
                {
                    searchEquipment.WorkingTimeRangeDefaultMin = searchEquipment.WorkingTimeRangeMin;
                }
                else
                {
                    searchEquipment.WorkingTimeRangeDefaultMin = searchEquipment.WorkingTimeMin;
                }
                if (searchEquipment.WorkingTimeRangeMax != null)
                {
                    searchEquipment.WorkingTimeRangeDefaultMax = searchEquipment.WorkingTimeRangeMax;
                }
                else
                {
                    searchEquipment.WorkingTimeRangeDefaultMax = searchEquipment.WorkingTimeMax;
                }
            }
            else
            {
                // 数据库搜索不到值，前台不做显示。重置所有数据为0
                searchEquipment.WorkingTimeRangeMin = null;
                searchEquipment.WorkingTimeRangeMax = null;
                searchEquipment.WorkingTimeRangeDefaultMin = 0;
                searchEquipment.WorkingTimeRangeDefaultMax = 0;
                searchEquipment.WorkingTimeMin = 0;
                searchEquipment.WorkingTimeMax = 0;
            }

            var ProductionDateQuery = SearchCondition(query, searchEquipment, "ProductionDate");
            if (ProductionDateQuery.Max(e => e.ProductionDate) != null && ProductionDateQuery.Max(e => e.ProductionDate) > new DateTime(1900, 1, 1))
            {
                searchEquipment.ProductionDateMin = ((DateTime)ProductionDateQuery.Min(e => e.ProductionDate)).Year;
                searchEquipment.ProductionDateMax = ((DateTime)ProductionDateQuery.Max(e => e.ProductionDate)).Year;
                // 防止搜索超出范围       
                if (searchEquipment.ProductionDateRangeMin != null &&
                    searchEquipment.ProductionDateMin > searchEquipment.ProductionDateRangeMin)
                    searchEquipment.ProductionDateRangeMin = searchEquipment.ProductionDateMin;
                if (searchEquipment.ProductionDateRangeMax != null &&
                    searchEquipment.ProductionDateMax < searchEquipment.ProductionDateRangeMax)
                    searchEquipment.ProductionDateRangeMax = searchEquipment.ProductionDateMax;
                if (searchEquipment.ProductionDateRangeMin != null && searchEquipment.ProductionDateRangeMax != null &&
                    searchEquipment.ProductionDateRangeMin > searchEquipment.ProductionDateRangeMax)
                {
                    searchEquipment.ProductionDateRangeMin = searchEquipment.ProductionDateMin;
                    searchEquipment.ProductionDateRangeMax = searchEquipment.ProductionDateMax;
                }

                if (searchEquipment.ProductionDateRangeMin != null)
                {
                    searchEquipment.ProductionDateRangeDefaultMin = searchEquipment.ProductionDateRangeMin;
                }
                else
                {
                    searchEquipment.ProductionDateRangeDefaultMin = searchEquipment.ProductionDateMin;
                }
                if (searchEquipment.ProductionDateRangeMax != null)
                {
                    searchEquipment.ProductionDateRangeDefaultMax = searchEquipment.ProductionDateRangeMax;
                }
                else
                {
                    searchEquipment.ProductionDateRangeDefaultMax = searchEquipment.ProductionDateMax;
                }
            }
            else
            {
                // 数据库搜索不到值，前台不做显示。重置所有数据为0
                searchEquipment.ProductionDateRangeMin = null;
                searchEquipment.ProductionDateRangeMax = null;
                searchEquipment.ProductionDateRangeDefaultMin = 0;
                searchEquipment.ProductionDateRangeDefaultMax = 0;
                searchEquipment.ProductionDateMin = 0;
                searchEquipment.ProductionDateMax = 0;
            }

            var DealPriceQuery = SearchCondition(query, searchEquipment, "DealPrice");
            if (DealPriceQuery.Max(e => e.DealPrice) != null && DealPriceQuery.Max(e => e.DealPrice) > 0)
            {
                searchEquipment.DealPriceMin = ((decimal)DealPriceQuery.Min(e => e.DealPrice));
                searchEquipment.DealPriceMax = ((decimal)DealPriceQuery.Max(e => e.DealPrice));
                // 防止搜索超出范围       
                if (searchEquipment.DealPriceRangeMin != null &&
                    searchEquipment.DealPriceMin > searchEquipment.DealPriceRangeMin)
                    searchEquipment.DealPriceRangeMin = searchEquipment.DealPriceMin;
                if (searchEquipment.DealPriceRangeMax != null &&
                    searchEquipment.DealPriceMax < searchEquipment.DealPriceRangeMax)
                    searchEquipment.DealPriceRangeMax = searchEquipment.DealPriceMax;
                if (searchEquipment.DealPriceRangeMin != null && searchEquipment.DealPriceRangeMax != null &&
                    searchEquipment.DealPriceRangeMin > searchEquipment.DealPriceRangeMax)
                {
                    searchEquipment.DealPriceRangeMin = searchEquipment.DealPriceMin;
                    searchEquipment.DealPriceRangeMax = searchEquipment.DealPriceMax;
                }

                if (searchEquipment.DealPriceRangeMin != null)
                {
                    searchEquipment.DealPriceRangeDefaultMin = searchEquipment.DealPriceRangeMin;
                }
                else
                {
                    searchEquipment.DealPriceRangeDefaultMin = searchEquipment.DealPriceMin;
                }
                if (searchEquipment.DealPriceRangeMax != null)
                {
                    searchEquipment.DealPriceRangeDefaultMax = searchEquipment.DealPriceRangeMax;
                }
                else
                {
                    searchEquipment.DealPriceRangeDefaultMax = searchEquipment.DealPriceMax;
                }
            }
            else
            {
                // 数据库搜索不到值，前台不做显示。重置所有数据为0
                searchEquipment.DealPriceRangeMin = null;
                searchEquipment.DealPriceRangeMax = null;
                searchEquipment.DealPriceRangeDefaultMin = 0;
                searchEquipment.DealPriceRangeDefaultMax = 0;
                searchEquipment.DealPriceMin = 0;
                searchEquipment.DealPriceMax = 0;
            }

            var PriceQuery = SearchCondition(query, searchEquipment, "Price");
            if (PriceQuery.Max(e => e.Price) != null && PriceQuery.Max(e => e.Price) > 0)
            {
                searchEquipment.PriceMin = ((decimal)PriceQuery.Min(e => e.Price));
                searchEquipment.PriceMax = ((decimal)PriceQuery.Max(e => e.Price));
                // 防止搜索超出范围       
                if (searchEquipment.PriceRangeMin != null &&
                    searchEquipment.PriceMin > searchEquipment.PriceRangeMin)
                    searchEquipment.PriceRangeMin = searchEquipment.PriceMin;
                if (searchEquipment.PriceRangeMax != null &&
                    searchEquipment.PriceMax < searchEquipment.PriceRangeMax)
                    searchEquipment.PriceRangeMax = searchEquipment.PriceMax;
                if (searchEquipment.PriceRangeMin != null && searchEquipment.PriceRangeMax != null &&
                    searchEquipment.PriceRangeMin > searchEquipment.PriceRangeMax)
                {
                    searchEquipment.PriceRangeMin = searchEquipment.PriceMin;
                    searchEquipment.PriceRangeMax = searchEquipment.PriceMax;
                }

                if (searchEquipment.PriceRangeMin != null)
                {
                    searchEquipment.PriceRangeDefaultMin = searchEquipment.PriceRangeMin;
                }
                else
                {
                    searchEquipment.PriceRangeDefaultMin = searchEquipment.PriceMin;
                }
                if (searchEquipment.PriceRangeMax != null)
                {
                    searchEquipment.PriceRangeDefaultMax = searchEquipment.PriceRangeMax;
                }
                else
                {
                    searchEquipment.PriceRangeDefaultMax = searchEquipment.PriceMax;
                }
            }
            else
            {
                // 数据库搜索不到值，前台不做显示。重置所有数据为0
                searchEquipment.DealPriceRangeMin = null;
                searchEquipment.DealPriceRangeMax = null;
                searchEquipment.DealPriceRangeDefaultMin = 0;
                searchEquipment.DealPriceRangeDefaultMax = 0;
                searchEquipment.DealPriceMin = 0;
                searchEquipment.DealPriceMax = 0;
            }


            var SoldAtQuery = SearchCondition(query, searchEquipment, "SoldAt");
            if (SoldAtQuery.Max(e => e.SoldAt) != null && SoldAtQuery.Max(e => e.SoldAt) > new DateTime(1900, 1, 1))
            {
                searchEquipment.SoldAtMin = SoldAtQuery.Min(e => e.SoldAt);
                searchEquipment.SoldAtMax = SoldAtQuery.Max(e => e.SoldAt);
                searchEquipment.SoldAtRangeDefaultMin = searchEquipment.SoldAtMin;
                searchEquipment.SoldAtRangeDefaultMax = searchEquipment.SoldAtMax;
                // 防止搜索超出范围, 当其他搜索条件影响到这个输入框的值的时候，要缩小用户的值  
                if (searchEquipment.SoldAtRangeMin != null &&
                    searchEquipment.SoldAtMin > searchEquipment.SoldAtRangeMin)
                    searchEquipment.SoldAtRangeMin = searchEquipment.SoldAtMin;
                if (searchEquipment.SoldAtRangeMax != null &&
                    searchEquipment.SoldAtMax < searchEquipment.SoldAtRangeMax)
                    searchEquipment.SoldAtRangeMax = searchEquipment.SoldAtMax;
                if (searchEquipment.SoldAtRangeMin != null && searchEquipment.SoldAtRangeMax != null &&
                    searchEquipment.SoldAtRangeMin > searchEquipment.SoldAtRangeMax)
                {
                    searchEquipment.SoldAtRangeMin = searchEquipment.SoldAtMin;
                    searchEquipment.SoldAtRangeMax = searchEquipment.SoldAtMax;
                }

                if (searchEquipment.SoldAtRangeMin != null)
                {
                    searchEquipment.SoldAtRangeDefaultMin = searchEquipment.SoldAtRangeMin;
                }
                else
                {
                    searchEquipment.SoldAtRangeDefaultMin = searchEquipment.SoldAtMin;
                }
                if (searchEquipment.SoldAtRangeMax != null)
                {
                    searchEquipment.SoldAtRangeDefaultMax = searchEquipment.SoldAtRangeMax;
                }
                else
                {
                    searchEquipment.SoldAtRangeDefaultMax = searchEquipment.SoldAtMax;
                }
            }
            else
            {
                // 数据库搜索不到值，前台不做显示。重置所有数据为 1900-1-1
                searchEquipment.SoldAtRangeMin = null;
                searchEquipment.SoldAtRangeMax = null;
                searchEquipment.SoldAtRangeDefaultMin = ApplicationConstant.DateTimeMinValue;
                searchEquipment.SoldAtRangeDefaultMax = ApplicationConstant.DateTimeMinValue;
                searchEquipment.SoldAtMin = ApplicationConstant.DateTimeMinValue;
                searchEquipment.SoldAtMax = ApplicationConstant.DateTimeMinValue;
            }

            return searchEquipment;
        }

    }
}