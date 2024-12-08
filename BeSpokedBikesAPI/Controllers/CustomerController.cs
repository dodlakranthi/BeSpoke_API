using BeSpokedBikesAPI.DataAccess;
using BeSpokedBikesAPI.Interfaces;
using BeSpokedBikesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeSpokedBikesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IBeSpokeBikesRepo _repo;
        public CustomerController(IBeSpokeBikesRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            var response = await _repo.GetAllCustomers();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
