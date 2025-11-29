using Evently.Server.Domains.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace Evently.Server.Common.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Account>(options) {
	public DbSet<Account> Accounts { get; set; }
	public DbSet<Booking> Bookings { get; set; }
	public DbSet<Gathering> Gatherings { get; set; }
	public DbSet<GatheringCategoryDetail> GatheringCategoryDetails { get; set; }
	public DbSet<Category> Categories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		// for unit testing, sqlite is used
		if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite") {
			ConfigureSqlite(modelBuilder);
		}

		// Postgres identity configuration
		modelBuilder.Entity<Gathering>().Property(g => g.GatheringId)
			.HasIdentityOptions(startValue: 20);

		modelBuilder.Entity<Category>().Property(c => c.CategoryId)
			.HasIdentityOptions(startValue: 20);

		SeedData(modelBuilder);
	}

	private static void SeedData(ModelBuilder modelBuilder) {
		// Fixed Account IDs for referencing
		const string hostUserId = "empty-user-12345";
		const string guestUserId = "guest-user-22222";

		// Seed Accounts (without proper password hashes - they won't be able to login)
		modelBuilder.Entity<Account>().HasData(
			new Account {
				Id = hostUserId, // Fixed constant ID
				UserName = "empty_user",
				NormalizedUserName = "EMPTY_USER",
				Email = "empty@example.com",
				NormalizedEmail = "EMPTY@EXAMPLE.COM",
				Name = "Empty User",
				EmailConfirmed = false,
				PasswordHash = null, // No password - unusable account
				SecurityStamp = "EMPTY-SECURITY-STAMP-12345", // Fixed constant
				ConcurrencyStamp = "EMPTY-CONCURRENCY-STAMP-12345", // Fixed constant
				PhoneNumber = null,
				PhoneNumberConfirmed = false,
				TwoFactorEnabled = false,
				LockoutEnabled = true,
				AccessFailedCount = 0,
			},
			new Account {
				Id = guestUserId, // Fixed constant ID
				UserName = "guest_user2",
				NormalizedUserName = "GUEST_USER_2",
				Email = "guest@example.com",
				NormalizedEmail = "GUEST@EXAMPLE.COM",
				Name = "Guest User",
				EmailConfirmed = false,
				PasswordHash = null, // No password - unusable account
				SecurityStamp = "EMPTY-SECURITY-STAMP-12345", // Fixed constant
				ConcurrencyStamp = "EMPTY-CONCURRENCY-STAMP-12345", // Fixed constant
				PhoneNumber = null,
				PhoneNumberConfirmed = false,
				TwoFactorEnabled = false,
				LockoutEnabled = true,
				AccessFailedCount = 0,
			}
		);

		// Seed Categories
		modelBuilder.Entity<Category>().HasData(
			new Category { CategoryId = 1, CategoryName = "Information Technology" },
			new Category { CategoryId = 2, CategoryName = "Business & Networking" },
			new Category { CategoryId = 3, CategoryName = "Arts & Culture" }
		);

		// Singapore timezone offset
		TimeSpan singaporeOffset = TimeSpan.Zero;

		// Seed Gatherings
		modelBuilder.Entity<Gathering>().HasData(
			new Gathering {
				GatheringId = 1,
				Name = "Tech Innovation Summit",
				Description = "A comprehensive summit exploring the latest in AI and machine learning",
				Location = "Marina Bay Sands Convention Centre, Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 5, hour: 9, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 5, hour: 17, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 2,
				Name = "Startup Networking Night",
				Description = "Connect with fellow entrepreneurs and investors",
				Location = "Clarke Quay Central, Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 10, hour: 18, minute: 30, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 10, hour: 22, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 3,
				Name = "Digital Art Exhibition",
				Description = "Showcasing contemporary digital art from emerging artists",
				Location = "National Gallery Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 15, hour: 10, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 15, hour: 18, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 4,
				Name = "Web Development Workshop",
				Description = "Learn modern web development techniques and best practices",
				Location = "Singapore Science Centre",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 8, hour: 13, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 8, hour: 17, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = guestUserId,
			},
			new Gathering {
				GatheringId = 5,
				Name = "Business Strategy Seminar",
				Description = "Advanced strategies for scaling your business",
				Location = "Raffles City Convention Centre, Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 20, hour: 14, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 20, hour: 16, minute: 30, second: 0,
					singaporeOffset),
				OrganiserId = guestUserId,
			},
			new Gathering {
				GatheringId = 6,
				Name = "Photography Masterclass",
				Description = "Professional photography techniques and portfolio building",
				Location = "Gardens by the Bay, Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 22, hour: 8, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 22, hour: 12, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = guestUserId,
			},
			new Gathering {
				GatheringId = 7,
				Name = "Mobile App Development Bootcamp",
				Description = "Intensive bootcamp covering iOS and Android development",
				Location = "NUS School of Computing, Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 12, hour: 9, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 12, hour: 18, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 8,
				Name = "Investment & Finance Forum",
				Description = "Learn about personal finance and investment strategies",
				Location = "Suntec Singapore Convention Centre",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 25, hour: 14, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 25, hour: 17, minute: 30, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 9,
				Name = "Creative Writing Workshop",
				Description = "Explore storytelling techniques and creative expression",
				Location = "Esplanade Theatres, Singapore",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 28, hour: 10, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 28, hour: 15, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = guestUserId,
			},
			new Gathering {
				GatheringId = 10,
				Name = "Cloud Computing Conference",
				Description = "Latest trends in cloud architecture and DevOps",
				Location = "Singapore EXPO",
				Start = new DateTimeOffset(year: 2026, month: 12, day: 30, hour: 9, minute: 30, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2026, month: 12, day: 30, hour: 17, minute: 30, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 11,
				Name = "E-commerce Mastery",
				Description = "Build and scale your online business effectively",
				Location = "Marina Bay Financial Centre, Singapore",
				Start = new DateTimeOffset(year: 2027, month: 1, day: 3, hour: 13, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2027, month: 1, day: 3, hour: 18, minute: 0, second: 0, singaporeOffset),
				OrganiserId = guestUserId,
			},
			new Gathering {
				GatheringId = 12,
				Name = "Contemporary Dance Performance",
				Description = "An evening of modern dance and artistic expression",
				Location = "Victoria Theatre, Singapore",
				Start = new DateTimeOffset(year: 2027, month: 1, day: 5, hour: 19, minute: 30, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2027, month: 1, day: 5, hour: 22, minute: 0, second: 0, singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 13,
				Name = "Cybersecurity Awareness Training",
				Description = "Essential cybersecurity practices for businesses",
				Location = "Singapore Management University",
				Start = new DateTimeOffset(year: 2027, month: 1, day: 8, hour: 10, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2027, month: 1, day: 8, hour: 16, minute: 0, second: 0, singaporeOffset),
				OrganiserId = guestUserId,
			},
			new Gathering {
				GatheringId = 14,
				Name = "Leadership Excellence Workshop",
				Description = "Develop essential leadership skills for modern managers",
				Location = "Orchard Hotel Singapore",
				Start = new DateTimeOffset(year: 2027, month: 1, day: 10, hour: 9, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2027, month: 1, day: 10, hour: 17, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = hostUserId,
			},
			new Gathering {
				GatheringId = 15,
				Name = "Film & Media Production Showcase",
				Description = "Independent filmmakers present their latest works",
				Location = "Singapore International Film Festival Venue",
				Start = new DateTimeOffset(year: 2027, month: 1, day: 12, hour: 18, minute: 0, second: 0,
					singaporeOffset),
				End = new DateTimeOffset(year: 2027, month: 1, day: 12, hour: 23, minute: 0, second: 0,
					singaporeOffset),
				OrganiserId = guestUserId,
			}
		);

		// Seed GatheringCategoryDetails
		modelBuilder.Entity<GatheringCategoryDetail>().HasData(
			new GatheringCategoryDetail { GatheringId = 1, CategoryId = 1 }, // Tech Innovation Summit -> IT
			new GatheringCategoryDetail { GatheringId = 2, CategoryId = 2 }, // Startup Networking Night -> Business
			new GatheringCategoryDetail { GatheringId = 3, CategoryId = 3 }, // Digital Art Exhibition -> Arts
			new GatheringCategoryDetail { GatheringId = 4, CategoryId = 1 }, // Web Development Workshop -> IT
			new GatheringCategoryDetail { GatheringId = 5, CategoryId = 2 }, // Business Strategy Seminar -> Business
			new GatheringCategoryDetail { GatheringId = 6, CategoryId = 3 }, // Photography Masterclass -> Arts
			new GatheringCategoryDetail { GatheringId = 7, CategoryId = 1 }, // Mobile App Development Bootcamp -> IT
			new GatheringCategoryDetail { GatheringId = 8, CategoryId = 2 }, // Investment & Finance Forum -> Business
			new GatheringCategoryDetail { GatheringId = 9, CategoryId = 3 }, // Creative Writing Workshop -> Arts
			new GatheringCategoryDetail { GatheringId = 10, CategoryId = 1 }, // Cloud Computing Conference -> IT
			new GatheringCategoryDetail { GatheringId = 11, CategoryId = 2 }, // E-commerce Mastery -> Business
			new GatheringCategoryDetail { GatheringId = 12, CategoryId = 3 }, // Contemporary Dance Performance -> Arts
			new GatheringCategoryDetail { GatheringId = 13, CategoryId = 1 }, // Cybersecurity Awareness Training -> IT
			new GatheringCategoryDetail
				{ GatheringId = 14, CategoryId = 2 }, // Leadership Excellence Workshop -> Business
			new GatheringCategoryDetail { GatheringId = 15, CategoryId = 3 } // Film & Media Production Showcase -> Arts
		);

		// Seed Bookings with fixed DateTimeOffset values
		// Fixed DateTimeOffset for seeding (static value)
		DateTimeOffset fixedCreationTime =
			new(year: 2025, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.Zero);

		modelBuilder.Entity<Booking>().HasData(
			new Booking {
				BookingId = "book_abc123456",
				GatheringId = 1,
				AttendeeId = guestUserId,
				CreationDateTime = fixedCreationTime,
				CheckInDateTime = null,
				CheckoutDateTime = null,
				CancellationDateTime = null,
			},
			new Booking {
				BookingId = "book_def789012",
				GatheringId = 2,
				AttendeeId = hostUserId,
				CreationDateTime = fixedCreationTime.AddHours(1), // Slightly different time
				CheckInDateTime = null,
				CheckoutDateTime = null,
				CancellationDateTime = null,
			}
		);
	}

	// https://stackoverflow.com/a/76152994/6514532
	private static void ConfigureSqlite(ModelBuilder modelBuilder) {
		// SQLite does not have proper support for DateTimeOffset via Entity Framework Core,
		// see the limitations here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations.
		// Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754.
		foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()) {
			IEnumerable<PropertyInfo> properties = entityType.ClrType
				.GetProperties()
				.Where(p => p.PropertyType == typeof(DateTimeOffset)
				            || p.PropertyType == typeof(DateTimeOffset?));
			foreach (PropertyInfo property in properties) {
				modelBuilder
					.Entity(entityType.Name)
					.Property(property.Name)
					.HasConversion(new DateTimeOffsetToBinaryConverter());
			}
		}
	}
}