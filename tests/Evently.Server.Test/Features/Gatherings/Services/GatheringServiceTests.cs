using Evently.Server.Common.Data;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Gatherings.Services;
using Evently.Server.Test.Common.Setup;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Test.Features.Gatherings.Services;

public class GatheringServiceTests(DatabaseFixture dbFixture) : IClassFixture<DatabaseFixture>
{

    [Fact]
    public async Task CreateGathering_WithValidData_ShouldCreateGathering() {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());
        
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
        Gathering result = await gatheringService.CreateGathering(gatheringReqDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gatheringReqDto.Name, result.Name);
        Assert.Equal(gatheringReqDto.Description, result.Description);
        Assert.Equal(gatheringReqDto.Start, result.Start);
        Assert.Equal(gatheringReqDto.End, result.End);
        Assert.Equal(gatheringReqDto.Location, result.Location);
        Assert.Equal(gatheringReqDto.OrganiserId, result.OrganiserId);

        // Verify it was saved to database
        Gathering? savedGathering = await dbContext.Gatherings.FirstOrDefaultAsync(g =>
            g.GatheringId == result.GatheringId
        );
        Assert.NotNull(savedGathering);
    }

    [Fact]
    public async Task CreateGathering_WithInvalidData_ShouldThrowArgumentException()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

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
        await Assert.ThrowsAsync<ArgumentException>(() =>
            gatheringService.CreateGathering(invalidGatheringReqDto)
        );
    }

    [Fact]
    public async Task GetGathering_WithExistingId_ShouldReturnGathering()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

        // Arrange
        Gathering gathering = new()
        {
            Name = "Test Gathering",
            Description = "Test Description",
            Start = DateTimeOffset.UtcNow.AddDays(1),
            End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
            Location = "Test Location",
            OrganiserId = "organizer123",
            Bookings = [],
            GatheringCategoryDetails = [],
        };

        dbContext.Gatherings.Add(gathering);
        await dbContext.SaveChangesAsync();

        // Act
        Gathering? result = await gatheringService.GetGathering(gathering.GatheringId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gathering.GatheringId, result.GatheringId);
        Assert.Equal(gathering.Name, result.Name);
        Assert.Equal(gathering.Description, result.Description);
    }

    [Fact]
    public async Task GetGathering_WithNonExistentId_ShouldReturnNull()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

        // Arrange
        const long nonExistentId = 999;

        // Act
        Gathering? result = await gatheringService.GetGathering(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetGatherings_WithNameFilter_ShouldReturnFilteredResults()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

        // Arrange
        List<Gathering> gatherings =
        [
            new()
            {
                Name = "XYZ Conference",
                Description = "Description 1",
                Start = DateTimeOffset.UtcNow.AddDays(1),
                End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
                Location = "Location 1",
                OrganiserId = "organizer1",
                Bookings = [],
                GatheringCategoryDetails = [],
            },
            new()
            {
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

        dbContext.Gatherings.AddRange(gatherings);
        await dbContext.SaveChangesAsync();

        // Act
        PageResult<Gathering> result = await gatheringService.GetGatherings(
            attendeeId: null,
            organiserId: null,
            "XYZ",
            startDateBefore: null,
            startDateAfter: null,
            endDateBefore: null,
            endDateAfter: null,
            isCancelled: null,
            categoryIds: [],
            offset: null,
            limit: null
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected: 1, result.TotalCount);
        Assert.Equal("XYZ Conference", result.Items.First().Name);
    }

    [Fact]
    public async Task UpdateGathering_WithValidData_ShouldUpdateGathering()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

        // Arrange
        Gathering gathering = new()
        {
            Name = "Original Name",
            Description = "Original Description",
            Start = DateTimeOffset.UtcNow.AddDays(1),
            End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
            Location = "Original Location",
            OrganiserId = "organizer123",
            GatheringCategoryDetails = [],
        };

        dbContext.Gatherings.Add(gathering);
        await dbContext.SaveChangesAsync();

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
        Gathering result = await gatheringService.UpdateGathering(
            gathering.GatheringId,
            updateDto
        );

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
    public async Task UpdateGathering_WithNonExistentId_ShouldThrowKeyNotFoundException()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

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
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            gatheringService.UpdateGathering(gatheringId: 999, updateDto)
        );
    }

    [Fact]
    public async Task DeleteGathering_WithExistingId_ShouldDeleteGathering()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

        // Arrange
        Gathering gathering = new()
        {
            Name = "Test Gathering",
            Description = "Test Description",
            Start = DateTimeOffset.UtcNow.AddDays(1),
            End = DateTimeOffset.UtcNow.AddDays(1).AddHours(2),
            Location = "Test Location",
            OrganiserId = "organizer123",
            GatheringCategoryDetails = [],
        };

        dbContext.Gatherings.Add(gathering);
        await dbContext.SaveChangesAsync();
        long gatheringId = gathering.GatheringId;

        // Act
        await gatheringService.DeleteGathering(gatheringId);

        // Assert
        Gathering? deletedGathering = await dbContext.Gatherings.FirstOrDefaultAsync(g =>
            g.GatheringId == gatheringId
        );
        Assert.Null(deletedGathering);
    }

    [Fact]
    public async Task DeleteGathering_WithNonExistentId_ShouldThrowInvalidOperationException()
    {
        AppDbContext dbContext = await dbFixture.GetDbContext();
        GatheringService gatheringService = new(dbContext, validator: new GatheringValidator());

        // Arrange
        const long nonExistentId = 999;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            gatheringService.DeleteGathering(nonExistentId)
        );
    }
}
