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
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetUsersQuery());
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Update(string id,[FromBody] UpdateUserCommand command)
        {
            command.UserId = id;
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand { UserId = id });
            return NoContent();
        }


        [HttpPatch("{id}/block")]
        [Authorize(Roles = "CompanyAdmin")]
        public async Task<IActionResult> Block(string id,[FromQuery] bool isBlocked)
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
