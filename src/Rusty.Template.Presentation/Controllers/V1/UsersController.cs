#region

using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rusty.Template.Application.Repositories;
using Rusty.Template.Contracts.Dtos.User;
using Rusty.Template.Contracts.Requests;
using Rusty.Template.Contracts.Responses;
using Rusty.Template.Domain;
using Rusty.Template.Domain.Exceptions.Entity;
using Rusty.Template.Infrastructure.Attributes;
using Swashbuckle.AspNetCore.Annotations;

#endregion

namespace Rusty.Template.Presentation.Controllers.V1;

[ApiVersion("1.0", Deprecated = false)]
// [AuthorizeRoles("Admin")]
public class UsersController : BaseApiController
{
	private readonly IUserRepo _userRepo;

	public UsersController(IUserRepo userRepo)
	{
		_userRepo = userRepo;
	}

	[SwaggerOperation(
		Summary = "Get all users",
		Description = "Returns trader list"
	)]
	[SwaggerResponse(
		StatusCodes.Status200OK,
		"Users retrieved successfully",
		typeof(List<UserDto>)
	)]
	[HttpGet]
	public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
	{
		return Ok(await _userRepo.GetAll().ProjectToType<UserDto>().ToListAsync(cancellationToken));
	}

	[SwaggerOperation(
		Summary = "Get paged users",
		Description = "Returns paged list"
	)]
	[SwaggerResponse(
		StatusCodes.Status200OK,
		"Users retrieved successfully",
		typeof(PagedResponse<UserDto>)
	)]
	[HttpPost("paged")]
	public async Task<IActionResult> GetPagedUsersAsync(OrderedPagedRequest request,
														CancellationToken cancellationToken)
	{
		return Ok(await _userRepo.PaginateAsync<UserDto>(request, cancellationToken));
	}

	[SwaggerOperation(
		Summary = "Get user by id",
		Description = "Returns paged list"
	)]
	[SwaggerResponse(
		StatusCodes.Status200OK,
		"User retrieved successfully",
		typeof(UserDto)
	)]
	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetUserByIdAsync(int id, CancellationToken cancellationToken)
	{
		var user = await _userRepo.GetByIdAsync(id, cancellationToken) ??
				   throw new EntityNotFoundByIdException<User>(id);
		return Ok(user.Adapt<UserDto>());
	}

	[SwaggerOperation(
		Summary = "Get user by username",
		Description = "Returns user"
	)]
	[SwaggerResponse(
		StatusCodes.Status200OK,
		"User retrieved successfully",
		typeof(UserDto)
	)]
	[HttpGet("{username}")]
	public async Task<IActionResult> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
	{
		var user = await _userRepo.GetByUsernameAsync(username, cancellationToken) ??
				   throw new EntityNotFoundByNameException<User>(username);
		return Ok(user.Adapt<UserDto>());
	}

	[SwaggerOperation(
		Summary = "Get user to update by id",
		Description = "Returns user dto for update"
	)]
	[SwaggerResponse(
		StatusCodes.Status200OK,
		"User retrieved successfully",
		typeof(UserUpdateDto)
	)]
	[HttpGet("update/{id:int}")]
	public async Task<IActionResult> GetUserToUpdateByIdAsync(int id, CancellationToken cancellationToken)
	{
		var user = await _userRepo.GetByIdAsync(id, cancellationToken) ??
				   throw new EntityNotFoundByIdException<User>(id);
		return Ok(user.Adapt<UserUpdateDto>());
	}

	[SwaggerOperation(
		Summary = "Get user to update by username",
		Description = "Returns user dto for update"
	)]
	[SwaggerResponse(
		StatusCodes.Status200OK,
		"User retrieved successfully",
		typeof(UserUpdateDto)
	)]
	[HttpGet("update/{username}")]
	public async Task<IActionResult> GetUserToUpdateByNameAsync(string username, CancellationToken cancellationToken)
	{
		var user = await _userRepo.GetByUsernameAsync(username, cancellationToken) ??
				   throw new EntityNotFoundByNameException<User>(username);
		return Ok(user.Adapt<UserUpdateDto>());
	}

	[SwaggerOperation(
		Summary = "Create new user",
		Description = "Creates new user"
	)]
	[SwaggerResponse(
		StatusCodes.Status201Created, "User created successfully"
	)]
	[HttpPost]
	public async Task<IActionResult> CreateUserAsync(UserCreateDto dto)
	{
		var user = await _userRepo.CreateAsync(dto.Adapt<User>());
		return CreatedAtAction("GetUserById", new { id = user.Id }, user.Adapt<UserDto>());
	}

	[SwaggerOperation(
		Summary = "Update user",
		Description = "Updates existing user"
	)]
	[SwaggerResponse(
		StatusCodes.Status204NoContent, "User updated successfully"
	)]
	[HttpPut("{id:int}")]
	[HttpPutIdCompare]
	public async Task<IActionResult> UpdateUserAsync(int id, UserUpdateDto dto)
	{
		await _userRepo.UpdateAsync(dto.Adapt<User>());
		return NoContent();
	}

	[SwaggerOperation(
		Summary = "Delete user",
		Description = "Deletes existing user"
	)]
	[SwaggerResponse(
		StatusCodes.Status204NoContent, "User deleted successfully"
	)]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteUserAsync(object id)
	{
		await _userRepo.DeleteAsync(id);
		return NoContent();
	}
}