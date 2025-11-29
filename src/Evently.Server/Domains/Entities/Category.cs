using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Category
{
	[Key]
	public long CategoryId { get; set; }

	[StringLength(100)]
	public string CategoryName { get; set; } = string.Empty;
	public bool Approved { get; set; }

	public List<GatheringCategoryDetail> GatheringCategoryDetails { get; set; } = [];
}
