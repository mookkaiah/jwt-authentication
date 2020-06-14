using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mooks.authenticationservice.Services;

namespace mooks.authenticationservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IStorageService _storeageService;

        public AdminController(IStorageService storeageService)
        {
            _storeageService = storeageService;
        }

        [HttpGet("getstorageobject")]
        public IActionResult GetStorageObject()
        {
            return Ok(_storeageService.GetAllUsers());
        }
    }
}