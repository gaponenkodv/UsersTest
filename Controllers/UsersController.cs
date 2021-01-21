using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AppTest.Queries;
using AppTest.Db.DTO;

namespace AppTest.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly UserQuery _userQuery;

        public UsersController(UserQuery userQuery)
        {
            _userQuery = userQuery;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _userQuery.GetUsersAsync();

            return users != null && users.Any()
                ? Ok(users)
                : NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _userQuery.FindByIdAsync(id);

            return user != null
                ? Ok(user)
                : NotFound();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserResponse>> PutUser(int id, UserRequest user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var result = _userQuery.UpdateUserAsync(user);

            return result != null
                ? await GetUser(id)
                : NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserResponse>> PostUser(UserRequest user)
        {
            var result = await _userQuery.AddUserAsync(user);

            return result > 0
                ? await GetUser(result)
                : BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id.ToString() == User.Identity.Name)
                return BadRequest();

            var result = await _userQuery.RemoveUserAsync(id);

            return result
                ? Ok()
                : NotFound();
        }
    }
}
