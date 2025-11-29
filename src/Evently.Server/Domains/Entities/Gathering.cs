using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class Gathering {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long GatheringId { get; set; }

	[StringLength(100)] public string Name { get; set; } = string.Empty;

	[StringLength(10_000)] public string Description { get; set; } = string.Empty;

	public DateTimeOffset Start { get; set; } = DateTimeOffset.UtcNow;
	public DateTimeOffset End { get; set; } = DateTimeOffset.UtcNow;

	[StringLength(100)] public string Location { get; set; } = string.Empty;

	[StringLength(1000)] public string? CoverSrc { get; set; } = string.Empty;

	// convenience field that acts as a readonly field for Account that created the Gathering
	[ForeignKey("Account")]
	[StringLength(100)]
	public string OrganiserId { get; set; } = string.Empty;

	public DateTimeOffset? CancellationDateTime { get; set; }

	public List<Booking> Bookings { get; set; } = [];
	public List<GatheringCategoryDetail> GatheringCategoryDetails { get; set; } = [];
}