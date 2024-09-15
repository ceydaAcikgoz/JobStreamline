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
    public class BlackwordController : ControllerBase
    {
        private readonly IBlackwordService _iBlackwordService;
        public BlackwordController(IBlackwordService BlackwordService)
        {
            _iBlackwordService = BlackwordService;
        }

        [HttpGet("{Word}")]
        public bool Find(string Word)
        {
            return _iBlackwordService.IsWordBlacklisted(Word);
        }

        [HttpPost("{Word}")]
        public IActionResult Create(string Word)
        {
           _iBlackwordService.AddBlackword(Word);
            return Ok();
        }

        [HttpDelete("{Word}")]
        public IActionResult Delete(string Word)
        {
            _iBlackwordService.RemoveWordFromBlackword(Word);
            return Ok();
        }
    }
}
