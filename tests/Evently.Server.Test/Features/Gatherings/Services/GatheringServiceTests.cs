using Evently.Server.Common.Data;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Gatherings.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Test.Features.Gatherings.Services;

public class GatheringServiceTests : IDisposable {
	private readonly SqliteConnection _conn;
	private readonly AppDbContext _dbContext;
	private readonly IGatheringService _gatheringService;

	public GatheringServiceTests() {
		_conn = new SqliteConnection("Filename=:memory:");
		_conn.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		DbContextOptions<AppDbContext> contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(_conn)
			.Options;

		// Create the schema and seed some data
		AppDbContext dbContext = new(contextOptions);

		dbContext.Database.EnsureCreated();
		_dbContext = dbContext;

		_gatheringService = new GatheringService(_dbContext, validator: new GatheringValidator());
	}

	public void Dispose() {
		_dbContext.Dispose();
		_conn.Dispose();
	}

	[Fact]
	public async Task CreateGathering_WithValidData_ShouldCreateGathering() {
		// Arrange
		GatheringReqDto gatheringReqDto = new(
			GatheringId: 0,
			"Test Gathering",
			"Test Description",
			Start: DateTimeOffset.UtcNow.AddDays(1),
			End: DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
			CancellationDateTime: null,
			"Test Location",
			"organizer123",
			"test-cover.jpg",
			GatheringCategoryDetails: []
		);

		// Act
		Gathering result = await _gatheringService.CreateGathering(gatheringReqDto);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(gatheringReqDto.Name, result.Name);
		Assert.Equal(gatheringReqDto.Description, result.Description);
		Assert.Equal(gatheringReqDto.Start, result.Start);
		Assert.Equal(gatheringReqDto.End, result.End);
		Assert.Equal(gatheringReqDto.Location, result.Location);
		Assert.Equal(gatheringReqDto.OrganiserId, result.OrganiserId);

		// Verify it was saved to database
		Gathering? savedGathering = await _dbContext.Gatherings.FirstOrDefaultAsync(g => g.GatheringId == result.GatheringId);
		Assert.NotNull(savedGathering);
	}

	[Fact]
	public async Task CreateGathering_WithInvalidData_ShouldThrowArgumentException() {
		// Arrange
		GatheringReqDto invalidGatheringReqDto = new(
			GatheringId: 0,
			"", // Invalid empty name
			"Test Description",
			Start: DateTimeOffset.UtcNow.AddDays(1),
			End: DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
			CancellationDateTime: null,
			"Test Location",
			"organizer123",
			CoverSrc: null,
			GatheringCategoryDetails: []
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() => _gatheringService.CreateGathering(invalidGatheringReqDto));
	}

	[Fact]
	public async Task GetGathering_WithExistingId_ShouldReturnGathering() {
		// Arrange
		Gathering gathering = new() {
			Name = "Test Gathering",
			Description = "Test Description",
			Start = DateTimeOffset.UtcNow.AddDays(1),
			End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
			Location = "Test Location",
			OrganiserId = "organizer123",
			Bookings = [],
			GatheringCategoryDetails = [],
		};

		_dbContext.Gatherings.Add(gathering);
		await _dbContext.SaveChangesAsync();

		// Act
		Gathering? result = await _gatheringService.GetGathering(gathering.GatheringId);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(gathering.GatheringId, result.GatheringId);
		Assert.Equal(gathering.Name, result.Name);
		Assert.Equal(gathering.Description, result.Description);
	}

	[Fact]
	public async Task GetGathering_WithNonExistentId_ShouldReturnNull() {
		// Arrange
		const long nonExistentId = 999;

		// Act
		Gathering? result = await _gatheringService.GetGathering(nonExistentId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetGatherings_WithNameFilter_ShouldReturnFilteredResults() {
		// Arrange
		List<Gathering> gatherings = [
			new() {
				Name = "XYZ Conference",
				Description = "Description 1",
				Start = DateTimeOffset.UtcNow.AddDays(1),
				End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
				Location = "Location 1",
				OrganiserId = "organizer1",
				Bookings = [],
				GatheringCategoryDetails = [],
			},

			new() {
				Name = "Art Workshop",
				Description = "Description 2",
				Start = DateTimeOffset.UtcNow.AddDays(2),
				End = DateTimeOffset.UtcNow.AddDays(2).AddHours(2),
				Location = "Location 2",
				OrganiserId = "organizer2",
				Bookings = [],
				GatheringCategoryDetails = [],
			},

		];

		_dbContext.Gatherings.AddRange(gatherings);
		await _dbContext.SaveChangesAsync();

		// Act
		PageResult<Gathering> result = await _gatheringService.GetGatherings(attendeeId: null,
			organiserId: null,
			"XYZ",
			startDateBefore: null,
			startDateAfter: null,
			endDateBefore: null,
			endDateAfter: null,
			isCancelled: null,
			categoryIds: [],
			offset: null,
			limit: null);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(expected: 1, result.TotalCount);
		Assert.Equal("XYZ Conference", result.Items.First().Name);
	}

	[Fact]
	public async Task UpdateGathering_WithValidData_ShouldUpdateGathering() {
		// Arrange
		Gathering gathering = new() {
			Name = "Original Name",
			Description = "Original Description",
			Start = DateTimeOffset.UtcNow.AddDays(1),
			End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
			Location = "Original Location",
			OrganiserId = "organizer123",
			GatheringCategoryDetails = [],
		};

		_dbContext.Gatherings.Add(gathering);
		await _dbContext.SaveChangesAsync();

		GatheringReqDto updateDto = new(
			gathering.GatheringId,
			"Updated Name",
			"Updated Description",
			Start: DateTimeOffset.UtcNow.AddDays(2),
			End: DateTimeOffset.UtcNow.AddDays(2).AddHours(3),
			CancellationDateTime: null,
			"Updated Location",
			"organizer123",
			"updated-cover.jpg",
			GatheringCategoryDetails: []
		);

		// Act
		Gathering result = await _gatheringService.UpdateGathering(gathering.GatheringId, updateDto);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Updated Name", result.Name);
		Assert.Equal("Updated Description", result.Description);
		Assert.Equal(updateDto.Start, result.Start);
		Assert.Equal(updateDto.End, result.End);
		Assert.Equal("Updated Location", result.Location);
		Assert.Equal("updated-cover.jpg", result.CoverSrc);
	}

	[Fact]
	public async Task UpdateGathering_WithNonExistentId_ShouldThrowKeyNotFoundException() {
		// Arrange
		GatheringReqDto updateDto = new(
			GatheringId: 999,
			"Updated Name",
			"Updated Description",
			Start: DateTimeOffset.UtcNow.AddDays(2),
			End: DateTimeOffset.UtcNow.AddDays(2).AddHours(3),
			CancellationDateTime: null,
			"Updated Location",
			"organizer123",
			CoverSrc: null,
			GatheringCategoryDetails: []
		);

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() => _gatheringService.UpdateGathering(gatheringId: 999, updateDto));
	}

	[Fact]
	public async Task DeleteGathering_WithExistingId_ShouldDeleteGathering() {
		// Arrange
		Gathering gathering = new() {
			Name = "Test Gathering",
			Description = "Test Description",
			Start = DateTimeOffset.UtcNow.AddDays(1),
			End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
			Location = "Test Location",
			OrganiserId = "organizer123",
			GatheringCategoryDetails = [],
		};

		_dbContext.Gatherings.Add(gathering);
		await _dbContext.SaveChangesAsync();
		long gatheringId = gathering.GatheringId;

		// Act
		await _gatheringService.DeleteGathering(gatheringId);

		// Assert
		Gathering? deletedGathering = await _dbContext.Gatherings.FirstOrDefaultAsync(g => g.GatheringId == gatheringId);
		Assert.Null(deletedGathering);
	}

	[Fact]
	public async Task DeleteGathering_WithNonExistentId_ShouldThrowInvalidOperationException() {
		// Arrange
		const long nonExistentId = 999;

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() => _gatheringService.DeleteGathering(nonExistentId));
	}
}