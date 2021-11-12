using Auth.Core.Models;
using Auth.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Net.Http;
using System.Net;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using Microsoft.AspNetCore.Cors;

namespace Auth_API.Controllers
{
    //[EnableCors("SSOTemplate")]
    [EnableCors]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ITokenSecurity _tokenSecurity;
        public AuthController(ITokenSecurity tokenSecurity)
        {
            _tokenSecurity = tokenSecurity;
        }
        
        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDetails loginDetails)
        {
            if (!(_tokenSecurity.IsLatestRequest(loginDetails.TokenDetails.Timestamp).Result))
            {
                return BadRequest();
            }
            var status = await _tokenSecurity.ValidateToken(loginDetails);
            // create new token with scopes
            // insert into database
            if(!status)
            {
                return Unauthorized(); 
            }
            return Ok() ;
        }
    }
}
