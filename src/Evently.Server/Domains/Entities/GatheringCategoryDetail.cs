using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Entities;

[PrimaryKey(propertyName: nameof(GatheringId), nameof(CategoryId))]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class GatheringCategoryDetail {
	public long GatheringId { get; set; }
	public Gathering? Gathering { get; set; }
	public long CategoryId { get; set; }
	public Category? Category { get; set; }
}