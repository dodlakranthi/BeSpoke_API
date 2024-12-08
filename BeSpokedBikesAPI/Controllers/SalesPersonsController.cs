using BeSpokedBikesAPI.Interfaces;
using BeSpokedBikesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeSpokedBikesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPersonsController : ControllerBase
    {
        private readonly IBeSpokeBikesRepo _repo;
        public SalesPersonsController(IBeSpokeBikesRepo repo)
        {
            _repo = repo;
        }

        // GET: api/SalesPersons
        [HttpGet]
        public async Task<ActionResult> GetSalesPersons()
        {
            var response = await _repo.GetAllSalesPersons();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // GET: api/Discounts/2024/2
        [HttpGet("{year}/{quarter}")]
        public async Task<ActionResult> GetQuarterlyBonus(int year, int quarter)
        {
            if (quarter == 0 || year == 0)
            {
                return BadRequest();
            }
            var response = await _repo.GetQuarterlyBonus(year, quarter);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // PUT: api/SalesPersons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesPerson(int id, UpdateSalesPersonRequest salesPerson)
        {
            if (salesPerson == null || id != salesPerson.SalesPersonId)
            {
                return BadRequest();
            }

            var response = await _repo.UpdateSalesPerson(salesPerson);
            if (response != null && response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
