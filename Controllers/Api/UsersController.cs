using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Equipment>> Get()
        {
             var Equipments = new List<Equipment>();
            Console.WriteLine(_context.Equipments.Count());
            if(_context.Equipments.Count() > 0){
                try {
                    Equipments = _context.Equipments
                                .Include(e=> e.EquipmentPhotos)
                                .ThenInclude(eq => eq.Photo)
                                .ToList();
                }
                catch (Exception e){
                    Console.WriteLine(e);
                }
                
            }else{
                Equipments = null;
            }
            // .Include(e => e.EquipmentPhotos)
            // .ThenInclude(ep => ep.Photo)
            return Equipments;
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