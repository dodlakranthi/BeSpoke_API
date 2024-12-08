using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeSpokedBikesAPI.DataAccess;
using BeSpokedBikesAPI.Models;
using BeSpokedBikesAPI.Interfaces;

namespace BeSpokedBikesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IBeSpokeBikesRepo _repo;
        public SalesController(IBeSpokeBikesRepo repo)
        {
            _repo = repo;
        }

        //// GET: api/Sales
        [HttpGet]
        public async Task<ActionResult> GetSales()
        {
            var response = await _repo.GetAllSales();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<ActionResult<UpdateEntityResponse>> PostSale(AddNewSaleRequest sale)
        {
            if (sale == null) 
            {
                return BadRequest();
            }
            var response = await _repo.AddNewSale(sale);
            if (response != null && response.IsSuccess)
            {
                return Created();
            }
            return BadRequest(response);
        }
    }
}
