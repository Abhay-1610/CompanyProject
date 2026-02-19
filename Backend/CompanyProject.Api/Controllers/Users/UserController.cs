using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyProject.Application.Users.CreateUser;
using CompanyProject.Application.Users.UpdateUser;
using CompanyProject.Application.Users.DeleteUser;
using CompanyProject.Application.Users.GetUsers;
using CompanyProject.Application.Users.BlockUser;

namespace CompanyProject.Api.Controllers.Users
{
    // ======================================================
    // Users API Controller
    // SuperAdmin & CompanyAdmin access
    // ======================================================
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        // ==================================================
        // Constructor (MediatR)
        // ==================================================
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==================================================
        // GET: api/users/company/{companyId}
        // Get users by company
        // ==================================================
        [HttpGet("company/{companyId}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _mediator.Send(
                new GetUsersQuery
                {
                    CompanyId = companyId
                }
            );

            return Ok(result);
        }

        // ==================================================
        // POST: api/users
        // Create new user
        // ==================================================
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result); // returns UserDto
        }

        // ==================================================
        // PUT: api/users/{id}
        // Update user
        // ==================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Update(
            string id,
            [FromBody] UpdateUserCommand command)
        {
            command.UserId = id; // route id → command
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // ==================================================
        // DELETE: api/users/{id}
        // Delete user
        // ==================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(
                new DeleteUserCommand { UserId = id }
            );

            return NoContent();
        }

        // ==================================================
        // POST: api/users/{id}
        // Block user
        // ==================================================
        [HttpPost("{id}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Block(string id)
        {
            await _mediator.Send(
                new BlockUserCommand
                {
                    UserId = id
                }
            );

            return NoContent();
        }
    }
}
