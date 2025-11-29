using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Evently.Server.Features.Categories.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class CategoriesController(ICategoryService categoryService) : ControllerBase {
	[HttpGet(Name = "GetCategories")]
	public async Task<ActionResult<List<Category>>> GetCategories(long? memberId, bool? approved) {
		PageResult<Category> result = await categoryService.GetCategories(memberId, approved);
		List<Category> topics = result.Items;
		int total = result.TotalCount;
		HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
		HttpContext.Response.Headers.Append("X-Total-Count", value: total.ToString(CultureInfo.InvariantCulture));
		return Ok(topics);
	}

	[HttpPost("", Name = "CreateCategory")]
	public async Task<ActionResult<Category>> CreateCategory(Category category) {
		category = await categoryService.CreateCategory(category);
		return Ok(category);
	}
}