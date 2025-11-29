using Evently.Server.Common.Data;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Features.Categories.Services;

public sealed class CategoryService(AppDbContext db) : ICategoryService
{
	public async Task<Category> CreateCategory(Category category)
	{
		await db.Categories.AddAsync(category);
		await db.SaveChangesAsync();
		return category;
	}

	public async Task<PageResult<Category>> GetCategories(long? gatheringId, bool? approved)
	{
		IQueryable<Category> query = db
			.Categories.Include((category) => category.GatheringCategoryDetails)
			.Where(
				(category) =>
					gatheringId == null
					|| category.GatheringCategoryDetails.Any(
						(detail) => detail.GatheringId == gatheringId
					)
			)
			.Where((category) => approved == null || category.Approved == approved);

		int totalCount = await query.CountAsync();
		List<Category> topics = await query.ToListAsync();

		return new PageResult<Category> { Items = topics, TotalCount = totalCount };
	}
}
