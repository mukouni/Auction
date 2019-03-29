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
            if (!string.IsNullOrEmpty(searchEquipment.Kw))
            {
                query = query.Where(x => x.Name.Contains(searchEquipment.Kw.Trim()) || x.Code.Contains(searchEquipment.Kw.Trim()));
            }
            query.OrderBy(e => e.LastUpdatedAt).ThenBy(e => e.CreatedAt).ThenBy(e => e.Name);

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

            return View(equipment);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, Staff, Develpment")]
        public async Task<IActionResult> Create(EquipmentViewModel EquipmentVM)
        {

            Equipment equipment = _mapper.Map<Equipment>(EquipmentVM);

            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)).WithSuccess("创建成功", "");
        }

        [HttpGet("{id:Guid}")]
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
                    _context.Update(_mapper.Map<EquipmentViewModel, Equipment>(equipmentVM));
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
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                var equipment = await _context.Equipments.FirstOrDefaultAsync(e => e.Id == id);
            }
            return View(equipmentVM);
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
                equipment.Photos.Add(newPhoto);
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
        public async Task<IActionResult> SetCoverPhoto(string photoName, Guid equipmentId)
        {
            var response = ResponseModelFactory.CreateInstance;
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.FileName == photoName && p.IsCover != true);
            if (photo == null)
            {
                response.SetNotFound("没有找到图片");
                return Ok(response);
            }

            var originCoverPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.EquipmentId == equipmentId && p.IsCover == true);
            if (originCoverPhoto != null)
            {
                originCoverPhoto.IsCover = false;
                _context.Entry(originCoverPhoto).State = EntityState.Modified;
            }

            photo.IsCover = true;
            _context.Entry(photo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(response);
        }


        /// <summary>
        /// 为拍卖设备
        /// </summary>
        [HttpGet("[action]")]
        [HttpPost("[action]")]
        public IActionResult NoAuction(SearchEquipmentViewModel searchEquipment)
        {
            using (_context)
            {
                var query = _context.Equipments.AsQueryable<Equipment>();
                query = query.Where(x => x.SoldAt != null);
                query = query.Where(x => x.IsDeleted == CommonEnum.IsDeleted.No || x.IsDeleted == null);

                query = SearchCondition(query, searchEquipment);
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

                searchEquipment.WorkingTimeMin = query.Min(e => e.WorkingTime);
                searchEquipment.WorkingTimeMax = query.Max(e => e.WorkingTime);
                if (searchEquipment.WorkingTimeRange == null)
                {
                    searchEquipment.WorkingTimeRange = new long[2];
                    searchEquipment.WorkingTimeRange[0] = searchEquipment.WorkingTimeMin ?? 0;
                    searchEquipment.WorkingTimeRange[1] = searchEquipment.WorkingTimeMax ?? 0;
                }

                searchEquipment.DealPriceMin = query.Min(e => e.DealPrice);
                searchEquipment.DealPriceMax = query.Max(e => e.DealPrice);
                if (searchEquipment.DealPriceRange == null)
                {
                    searchEquipment.DealPriceRange = new decimal[2];
                    searchEquipment.DealPriceRange[0] = searchEquipment.DealPriceMin ?? 0;
                    searchEquipment.DealPriceRange[1] = searchEquipment.DealPriceMax ?? 0;
                }

                searchEquipment.SoldAtMin = query.Min(e => e.SoldAt) == null ? 0 : ((DateTime)query.Min(e => e.SoldAt)).Year;
                searchEquipment.SoldAtMax = query.Max(e => e.SoldAt) == null ? 0 : ((DateTime)query.Max(e => e.SoldAt)).Year;
                if (searchEquipment.SoldAtRange == null)
                {
                    searchEquipment.SoldAtRange = new int[2];
                    searchEquipment.SoldAtRange[0] = searchEquipment.SoldAtMin ?? 0;
                    searchEquipment.SoldAtRange[1] = searchEquipment.SoldAtMax ?? 0;
                }

                //  long?[] WorkingTimeRangeTemp = query.GroupBy(r => 1)
                //                                     .Select(grp => new[]
                //                                     {
                //                                        grp.Min(t => t.WorkingTime),
                //                                        grp.Max(t => t.WorkingTime)
                //                                     }).First();
                // searchEquipment.WorkingTimeMin = WorkingTimeRangeTemp[0];
                // searchEquipment.WorkingTimeMax = WorkingTimeRangeTemp[1];


                // decimal?[] DealPriceRangeTemp = query.GroupBy(r => 1)
                //                                         .Select(grp => new[]
                //                                         {
                //                                             grp.Min(t => t.DealPrice),
                //                                             grp.Max(t => t.DealPrice)
                //                                         }).First();
                // searchEquipment.DealPriceMin = DealPriceRangeTemp[0];
                // searchEquipment.DealPriceMax = DealPriceRangeTemp[1];

                if (searchEquipment.Sort?.Field != null && searchEquipment.Sort?.Direction != null)
                {
                    if (searchEquipment.Sort.Direction == "desc")
                    {
                        query.OrderByDescending(e => searchEquipment.Sort.Field);
                    }
                    else
                    {
                        query.OrderBy(e => searchEquipment.Sort.Field);
                    }
                }
                else
                {
                    query.OrderBy(e => e.CreatedAt).ThenBy(e => e.LastUpdatedAt);
                }

                searchEquipment.Models = query.GroupBy(e => e.Model).Select(grp => new Filter()
                {
                    Name = grp.Key,
                    Count = grp.Count(),
                }).ToArray();
                foreach (var m in searchEquipment.Models)
                {
                    if (m.Name == "挖掘机")
                    {
                        m.SortNumber = 999;
                    }
                };
                searchEquipment.Models.OrderByDescending(m => m.SortNumber);

                searchEquipment.Names = query.GroupBy(e => e.Name)
                                             .Select(grp => new Filter()
                                             {
                                                 Name = grp.Key,
                                                 Count = grp.Count(),
                                             })
                                             .ToArray();

                searchEquipment.Manufacturers = query.GroupBy(e => e.Manufacturer)
                                                     .Select(grp => new Filter()
                                                     {
                                                         Name = grp.Key,
                                                         Count = grp.Count(),
                                                     })
                                                     .ToArray();

                searchEquipment.AuctionHouses = query.GroupBy(e => e.AuctionHouse)
                                                     .Select(grp => new Filter()
                                                     {
                                                         Name = grp.Key,
                                                         Count = grp.Count(),
                                                     })
                                                     .ToArray();

                searchEquipment.Countries = query.GroupBy(e => e.Country)
                                                 .Select(grp => new Filter()
                                                 {
                                                     Name = grp.Key,
                                                     Count = grp.Count(),
                                                 })
                                                 .ToArray();


                searchEquipment.Cities = query.GroupBy(e => e.City)
                                              .Select(grp => new Filter()
                                              {
                                                  Name = grp.Key,
                                                  Count = grp.Count(),
                                              })
                                              .ToArray();


                return Request.IsAjaxRequest()
                    ? (IActionResult)PartialView("_NoAuctionListPartial", searchEquipment)
                    : View(searchEquipment);
            }
        }

        /// <summary>
        /// 已拍卖设备
        /// </summary>
        [HttpGet("[action]")]
        public IActionResult Auctioned()
        {
            return View(new EquipmentViewModel());
        }

        /// <summary>
        /// 代采购设备
        /// </summary>
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin, Development, Staff, Member")]
        public IActionResult InsteadAuction()
        {
            return View(new EquipmentViewModel());
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

        private IQueryable<Equipment> SearchCondition(IQueryable<Equipment> query, SearchEquipmentViewModel searchEquipment)
        {
            if (!string.IsNullOrEmpty(searchEquipment.Kw))
            {
                query = query.Where(x => x.Name.Contains(searchEquipment.Kw.Trim()) || x.Code.Contains(searchEquipment.Kw.Trim()));
            }
            if (searchEquipment.Names != null && searchEquipment.Names.Any(n => n.Selected == true))
            {
                query = query.Where(x => searchEquipment.Names.Any(f => x.Name == f.Name));
            }
            if (searchEquipment.Manufacturers != null && searchEquipment.Manufacturers.Any(n => n.Selected == true))
            {
                query = query.Where(x => searchEquipment.Manufacturers.Any(f => x.Manufacturer == f.Name));
            }
            if (searchEquipment.Models != null && searchEquipment.Models.Any(n => n.Selected == true))
            {
                query = query.Where(x => searchEquipment.Models.Any(f => x.Model == f.Name));
            }
            if (searchEquipment.AuctionHouses != null && searchEquipment.AuctionHouses.Any(n => n.Selected == true))
            {
                query = query.Where(x => searchEquipment.AuctionHouses.Any(f => x.AuctionHouse == f.Name));
            }
            if (searchEquipment.Countries != null && searchEquipment.Countries.Any(n => n.Selected == true))
            {
                query = query.Where(x => searchEquipment.Countries.Any(f => x.Country == f.Name));
            }
            if (searchEquipment.Cities != null && searchEquipment.Cities.Any(n => n.Selected == true))
            {
                query = query.Where(x => searchEquipment.Cities.Any(f => x.City == f.Name));
            }

            if (searchEquipment.ProductionDateRange != null && searchEquipment.ProductionDateRange.Length == 2 &&
                searchEquipment.ProductionDateRange[0] > DateTime.MinValue && searchEquipment.ProductionDateRange[1] == DateTime.MinValue)
            {
                query = query.Where(x => x.ProductionDate >= searchEquipment.ProductionDateRange[0]);

            }
            if (searchEquipment.ProductionDateRange != null && searchEquipment.ProductionDateRange.Length == 2 &&
                searchEquipment.ProductionDateRange[0] == DateTime.MinValue && searchEquipment.ProductionDateRange[1] > DateTime.MinValue)
            {
                query = query.Where(x => x.ProductionDate <= searchEquipment.ProductionDateRange[1]);
            }
            if (searchEquipment.ProductionDateRange != null && searchEquipment.ProductionDateRange.Length == 2 &&
                searchEquipment.ProductionDateRange[0] > DateTime.MinValue && searchEquipment.ProductionDateRange[1] < DateTime.MinValue)
            {
                query = query.Where(x => x.ProductionDate >= searchEquipment.ProductionDateRange[0] && x.ProductionDate <= searchEquipment.ProductionDateRange[1]);

            }

            if (searchEquipment.WorkingTimeRange != null && searchEquipment.WorkingTimeRange.Length == 2 &&
                searchEquipment?.WorkingTimeRange[0] >= long.MinValue && searchEquipment?.WorkingTimeRange[1] == long.MinValue)
            {
                query = query.Where(x => x.WorkingTime >= searchEquipment.WorkingTimeRange[0]);

            }
            if (searchEquipment.WorkingTimeRange != null && searchEquipment.WorkingTimeRange.Length == 2 &&
                searchEquipment.WorkingTimeRange[0] < long.MinValue && searchEquipment.WorkingTimeRange[1] >= long.MinValue)
            {
                query = query.Where(x => x.WorkingTime <= searchEquipment.WorkingTimeRange[1]);
            }
            if (searchEquipment.WorkingTimeRange != null && searchEquipment.WorkingTimeRange.Length == 2 &&
                searchEquipment.WorkingTimeRange[0] > long.MinValue && searchEquipment.WorkingTimeRange[1] > long.MinValue)
            {
                query = query.Where(x => x.WorkingTime >= searchEquipment.WorkingTimeRange[0] && x.WorkingTime <= searchEquipment.WorkingTimeRange[1]);

            }

            if (searchEquipment.DealPriceRange != null && searchEquipment.DealPriceRange.Length == 2 &&
                searchEquipment.DealPriceRange[0] >= 0 && searchEquipment.DealPriceRange[1] == 0)
            {
                query = query.Where(x => x.DealPrice >= searchEquipment.DealPriceRange[0]);

            }
            if (searchEquipment.DealPriceRange != null && searchEquipment.DealPriceRange.Length == 2 &&
                searchEquipment.DealPriceRange[0] < 0 && searchEquipment.DealPriceRange[1] >= 0)
            {
                query = query.Where(x => x.DealPrice <= searchEquipment.DealPriceRange[1]);
            }
            if (searchEquipment.DealPriceRange != null && searchEquipment.DealPriceRange.Length == 2 &&
                searchEquipment.DealPriceRange[0] > 0 && searchEquipment.DealPriceRange[1] > 0)
            {
                query = query.Where(x => x.DealPrice >= searchEquipment.DealPriceRange[0] && x.DealPrice <= searchEquipment.DealPriceRange[1]);

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
    }
}