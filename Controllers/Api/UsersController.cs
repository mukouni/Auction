using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Auction.Entities;
using System;

namespace Auction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            // var Equipments = new List<Equipment>();

            var equi_list = new List<Equipment> {
                new Equipment { Code = "It" },
                new Equipment { Code = "Carrie" },
                new Equipment { Code = "Misery" },
                new Equipment { Code = "dmzsz" }
            };
            _context.Equipments.AddRange(equi_list);
            _context.SaveChanges();


            var photo1_list = new List<Photo>();
            foreach (Equipment equipment in _context.Equipments)
            {
                Photo photo = new Photo
                {
                    FileName = ".avator"
                };
                photo1_list.Add(photo);
                // user.Avator = new Photo();
                // user.Avator.FileName = user.LoginName + ".avator";
                // Photo photo = new Photo {AttachmentId = user.Guid, AttachmentType = typeof(User).FullName, FileName = user.LoginName+".avator"};
            }
            _context.Photos.AddRange(photo1_list);
            _context.SaveChanges();


            // var user_list = new List<User> {
            //     new User { LoginName = "It", RealName = "It", Email = "It@sina.com", Phone = "11111111111"},
            //     new User { LoginName = "Carrie", RealName = "Carrie", Email = "Carrie@sina.com", Phone = "22222222222"},
            //     new User { LoginName = "Misery", RealName = "Misery", Email = "Misery@sina.com", Phone = "33333333333"},
            //     new User { LoginName = "dmzsz", RealName = "dmzsz", Email = "dmzsz@sina.com", Phone = "44444444444"}
            // };
            // _context.Users.AddRange(user_list);
            // _context.SaveChanges();

            // var photo_list = new List<Photo>();
            // foreach (User user in _context.Users)
            // {
            //     Photo photo = new Photo
            //     {
            //         OwnerType = user.AvatorAttachType,
            //         OwnerGuId = user.Guid,
            //         FileName = user.LoginName + ".avator"
            //     };
            //     photo_list.Add(photo);

            //     // user.Avator = new Photo();
            //     // user.Avator.FileName = user.LoginName + ".avator";
            //     // Photo photo = new Photo {AttachmentId = user.Guid, AttachmentType = typeof(User).FullName, FileName = user.LoginName+".avator"};
            // }
            // _context.Photos.AddRange(photo_list);
            // _context.SaveChanges();

            // if (_context.Equipments.Count() > 0)
            // {
            //     try
            //     {
            //         Equipments = _context.Equipments

            //     }
            //     catch (Exception e)
            //     {
            //         Console.WriteLine(e);
            //     }

            // }
            // else
            // {
            //     Equipments = null;
            // }
            return _context.Users;
        }

        [HttpGet("photolist")]
        public ICollection<Photo> Photolist()
        {
            var user = _context.Equipments
                                .Include(e => e.Photos)
                                .Where(e => e.Guid.ToString() == "B84DAF06-8A1D-464D-BF5E-9CCD5423963D").First();
            var id = user.Photos;
            return id;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}