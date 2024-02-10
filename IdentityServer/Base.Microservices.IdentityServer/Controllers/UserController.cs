using Base.Microservices.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;
using System.Linq;
using System.Threading.Tasks;
using Base.Microservices.IdentityServer.Dtos;
using FreeCourse.Shared.Dtos;

namespace Base.Microservices.IdentityServer.Controllers {
    [Authorize(LocalApi.PolicyName)] //Authorize işleminin tanımlanması
    //Gelen token içerisinde scope olarak IdentityServerApi bekler, bunu da startupda yazılan AddLocalApiAuthentication ile sağlamış oluyoruz
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto) {
            var user = new ApplicationUser {
                UserName = signupDto.UserName,
                Email = signupDto.Email,
                City = signupDto.City
            };

            //Password hashleneceği için burada belirtilir. 
            var result = await _userManager.CreateAsync(user, signupDto.Password);

            if (!result.Succeeded) {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            }

            return NoContent();
        }

    }
}
