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
    public class JobController : ControllerBase
    {
        private readonly IJobService _iJobService;
        public JobController(IJobService JobService)
        {
            _iJobService = JobService;
        }

        [HttpGet("{id}")]
        public OutputJobDto? Find(Guid id)
        {
            return _iJobService.Get(id);
        }

        [HttpGet("/search")]
        public async Task<List<JobElasticDTO>> SearchJobs([FromQuery] string text)
        {
            return await _iJobService.SearchJobs(text);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InputJobDTO Job)
        {
            OutputJobDto outputJobDto = await _iJobService.Create(Job);
            return Ok(outputJobDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InputJobDTO Job)
        {
            OutputJobDto outputJobDto = await _iJobService.Update(Job);
            return Ok(outputJobDto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _iJobService.Delete(id);
            return Ok();
        }
    }
}
