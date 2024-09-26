using JobStreamline.Entity;
using JobStreamline.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobStreamline.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _iCompanyService;
        public CompanyController(ICompanyService CompanyService)
        {
            _iCompanyService = CompanyService;
        }

        [HttpGet("{id}")]
        public OutputCompanyDto? Find(Guid id)
        {
            return _iCompanyService.Get(id);
        }

        [HttpPost]
        public IActionResult Create([FromBody] InputCompanyDto Company)
        {
            OutputCompanyDto outputCompanyDto = _iCompanyService.Create(Company);
            return Ok(outputCompanyDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] InputCompanyDto Company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Geçersiz veri durumunda 400 BadRequest döner
            }

            OutputCompanyDto outputCompanyDto = _iCompanyService.Update(id, Company);
            return Ok(outputCompanyDto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _iCompanyService.Delete(id);
            return Ok();
        }
    }
}
