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
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔓 SuperAdmin – READ users of a selected company
        [HttpGet("company/{companyId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _mediator.Send(new GetUsersQuery
            {
                CompanyId = companyId
            });

            return Ok(result);
        }

        //// 🔓 SuperAdmin + CompanyAdmin – READ users (own scope)
        //[HttpGet]
        //[Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        //public async Task<IActionResult> Get()
        //{
        //    var result = await _mediator.Send(new GetUsersQuery());
        //    return Ok(result);
        //}

        // 🔒 CompanyAdmin – CREATE normal users (own company)
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //// 🔒 SuperAdmin – CREATE CompanyAdmin
        //[HttpPost("company-admin")]
        //[Authorize(Roles = "SuperAdmin")]
        //public async Task<IActionResult> CreateCompanyAdmin([FromBody] CreateUserCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        // 🔒 CompanyAdmin – UPDATE normal users
        // 🔒 SuperAdmin – UPDATE CompanyAdmins
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserCommand command)
        {
            command.UserId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        // 🔒 CompanyAdmin – DELETE normal users
        // 🔒 SuperAdmin – DELETE CompanyAdmins
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand { UserId = id });
            return NoContent();
        }

        // 🔒 CompanyAdmin only – BLOCK / UNBLOCK normal users
        [HttpPatch("{id}/block")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        public async Task<IActionResult> Block(string id, [FromQuery] bool isBlocked)
        {
            await _mediator.Send(new BlockUserCommand
            {
                UserId = id,
                IsBlocked = isBlocked
            });

            return NoContent();
        }
    }
}
