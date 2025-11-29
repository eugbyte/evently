using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Evently.Server.Common.Data.Migrations
{
	/// <inheritdoc />
	public partial class SQLServer : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(
						type: "nvarchar(256)",
						maxLength: 256,
						nullable: true
					),
					NormalizedName = table.Column<string>(
						type: "nvarchar(256)",
						maxLength: 256,
						nullable: true
					),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				}
			);

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(
						type: "nvarchar(100)",
						maxLength: 100,
						nullable: false
					),
					UserName = table.Column<string>(
						type: "nvarchar(256)",
						maxLength: 256,
						nullable: true
					),
					NormalizedUserName = table.Column<string>(
						type: "nvarchar(256)",
						maxLength: 256,
						nullable: true
					),
					Email = table.Column<string>(
						type: "nvarchar(256)",
						maxLength: 256,
						nullable: true
					),
					NormalizedEmail = table.Column<string>(
						type: "nvarchar(256)",
						maxLength: 256,
						nullable: true
					),
					EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(
						type: "datetimeoffset",
						nullable: true
					),
					LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
					AccessFailedCount = table.Column<int>(type: "int", nullable: false),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				}
			);

			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					CategoryId = table
						.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CategoryName = table.Column<string>(
						type: "nvarchar(100)",
						maxLength: 100,
						nullable: false
					),
					Approved = table.Column<bool>(type: "bit", nullable: false),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.CategoryId);
				}
			);

			migrationBuilder.CreateTable(
				name: "Gatherings",
				columns: table => new
				{
					GatheringId = table
						.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(
						type: "nvarchar(100)",
						maxLength: 100,
						nullable: false
					),
					Description = table.Column<string>(
						type: "nvarchar(max)",
						maxLength: 10000,
						nullable: false
					),
					Start = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
					End = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
					Location = table.Column<string>(
						type: "nvarchar(100)",
						maxLength: 100,
						nullable: false
					),
					CoverSrc = table.Column<string>(
						type: "nvarchar(1000)",
						maxLength: 1000,
						nullable: true
					),
					OrganiserId = table.Column<string>(
						type: "nvarchar(100)",
						maxLength: 100,
						nullable: false
					),
					CancellationDateTime = table.Column<DateTimeOffset>(
						type: "datetimeoffset",
						nullable: true
					),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Gatherings", x => x.GatheringId);
				}
			);

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderDisplayName = table.Column<string>(
						type: "nvarchar(max)",
						nullable: true
					),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				},
				constraints: table =>
				{
					table.PrimaryKey(
						"PK_AspNetUserLogins",
						x => new { x.LoginProvider, x.ProviderKey }
					);
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
				},
				constraints: table =>
				{
					table.PrimaryKey(
						"PK_AspNetUserTokens",
						x => new
						{
							x.UserId,
							x.LoginProvider,
							x.Name,
						}
					);
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "Bookings",
				columns: table => new
				{
					BookingId = table.Column<string>(
						type: "nvarchar(50)",
						maxLength: 50,
						nullable: false
					),
					AttendeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					GatheringId = table.Column<long>(type: "bigint", nullable: false),
					CreationDateTime = table.Column<DateTimeOffset>(
						type: "datetimeoffset",
						nullable: false
					),
					CheckInDateTime = table.Column<DateTimeOffset>(
						type: "datetimeoffset",
						nullable: true
					),
					CheckoutDateTime = table.Column<DateTimeOffset>(
						type: "datetimeoffset",
						nullable: true
					),
					CancellationDateTime = table.Column<DateTimeOffset>(
						type: "datetimeoffset",
						nullable: true
					),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Bookings", x => x.BookingId);
					table.ForeignKey(
						name: "FK_Bookings_AspNetUsers_AttendeeId",
						column: x => x.AttendeeId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
					table.ForeignKey(
						name: "FK_Bookings_Gatherings_GatheringId",
						column: x => x.GatheringId,
						principalTable: "Gatherings",
						principalColumn: "GatheringId",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "GatheringCategoryDetails",
				columns: table => new
				{
					GatheringId = table.Column<long>(type: "bigint", nullable: false),
					CategoryId = table.Column<long>(type: "bigint", nullable: false),
				},
				constraints: table =>
				{
					table.PrimaryKey(
						"PK_GatheringCategoryDetails",
						x => new { x.GatheringId, x.CategoryId }
					);
					table.ForeignKey(
						name: "FK_GatheringCategoryDetails_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "CategoryId",
						onDelete: ReferentialAction.Cascade
					);
					table.ForeignKey(
						name: "FK_GatheringCategoryDetails_Gatherings_GatheringId",
						column: x => x.GatheringId,
						principalTable: "Gatherings",
						principalColumn: "GatheringId",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[]
				{
					"Id",
					"AccessFailedCount",
					"ConcurrencyStamp",
					"Email",
					"EmailConfirmed",
					"LockoutEnabled",
					"LockoutEnd",
					"Name",
					"NormalizedEmail",
					"NormalizedUserName",
					"PasswordHash",
					"PhoneNumber",
					"PhoneNumberConfirmed",
					"SecurityStamp",
					"TwoFactorEnabled",
					"UserName",
				},
				values: new object[,]
				{
					{
						"empty-user-12345",
						0,
						"EMPTY-CONCURRENCY-STAMP-12345",
						"empty@example.com",
						false,
						true,
						null,
						"Empty User",
						"EMPTY@EXAMPLE.COM",
						"EMPTY_USER",
						null,
						null,
						false,
						"EMPTY-SECURITY-STAMP-12345",
						false,
						"empty_user",
					},
					{
						"guest-user-22222",
						0,
						"EMPTY-CONCURRENCY-STAMP-12345",
						"guest@example.com",
						false,
						true,
						null,
						"Guest User",
						"GUEST@EXAMPLE.COM",
						"GUEST_USER_2",
						null,
						null,
						false,
						"EMPTY-SECURITY-STAMP-12345",
						false,
						"guest_user2",
					},
				}
			);

			migrationBuilder.InsertData(
				table: "Categories",
				columns: new[] { "CategoryId", "Approved", "CategoryName" },
				values: new object[,]
				{
					{ 1L, false, "Information Technology" },
					{ 2L, false, "Business & Networking" },
					{ 3L, false, "Arts & Culture" },
				}
			);

			migrationBuilder.InsertData(
				table: "Gatherings",
				columns: new[]
				{
					"GatheringId",
					"CancellationDateTime",
					"CoverSrc",
					"Description",
					"End",
					"Location",
					"Name",
					"OrganiserId",
					"Start",
				},
				values: new object[,]
				{
					{
						1L,
						null,
						"",
						"A comprehensive summit exploring the latest in AI and machine learning",
						new DateTimeOffset(
							new DateTime(2025, 12, 5, 17, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Marina Bay Sands Convention Centre, Singapore",
						"Tech Innovation Summit",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2025, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						2L,
						null,
						"",
						"Connect with fellow entrepreneurs and investors",
						new DateTimeOffset(
							new DateTime(2025, 12, 10, 22, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Clarke Quay Central, Singapore",
						"Startup Networking Night",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2025, 12, 10, 18, 30, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						3L,
						null,
						"",
						"Showcasing contemporary digital art from emerging artists",
						new DateTimeOffset(
							new DateTime(2025, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"National Gallery Singapore",
						"Digital Art Exhibition",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2025, 12, 15, 10, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						4L,
						null,
						"",
						"Learn modern web development techniques and best practices",
						new DateTimeOffset(
							new DateTime(2025, 12, 8, 17, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Singapore Science Centre",
						"Web Development Workshop",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2025, 12, 8, 13, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						5L,
						null,
						"",
						"Advanced strategies for scaling your business",
						new DateTimeOffset(
							new DateTime(2025, 12, 20, 16, 30, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Raffles City Convention Centre, Singapore",
						"Business Strategy Seminar",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2025, 12, 20, 14, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						6L,
						null,
						"",
						"Professional photography techniques and portfolio building",
						new DateTimeOffset(
							new DateTime(2025, 12, 22, 12, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Gardens by the Bay, Singapore",
						"Photography Masterclass",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2025, 12, 22, 8, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						7L,
						null,
						"",
						"Intensive bootcamp covering iOS and Android development",
						new DateTimeOffset(
							new DateTime(2025, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"NUS School of Computing, Singapore",
						"Mobile App Development Bootcamp",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2025, 12, 12, 9, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						8L,
						null,
						"",
						"Learn about personal finance and investment strategies",
						new DateTimeOffset(
							new DateTime(2025, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Suntec Singapore Convention Centre",
						"Investment & Finance Forum",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2025, 12, 25, 14, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						9L,
						null,
						"",
						"Explore storytelling techniques and creative expression",
						new DateTimeOffset(
							new DateTime(2025, 12, 28, 15, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Esplanade Theatres, Singapore",
						"Creative Writing Workshop",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2025, 12, 28, 10, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						10L,
						null,
						"",
						"Latest trends in cloud architecture and DevOps",
						new DateTimeOffset(
							new DateTime(2025, 12, 30, 17, 30, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Singapore EXPO",
						"Cloud Computing Conference",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2025, 12, 30, 9, 30, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						11L,
						null,
						"",
						"Build and scale your online business effectively",
						new DateTimeOffset(
							new DateTime(2026, 1, 3, 18, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Marina Bay Financial Centre, Singapore",
						"E-commerce Mastery",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2026, 1, 3, 13, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						12L,
						null,
						"",
						"An evening of modern dance and artistic expression",
						new DateTimeOffset(
							new DateTime(2026, 1, 5, 22, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Victoria Theatre, Singapore",
						"Contemporary Dance Performance",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2026, 1, 5, 19, 30, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						13L,
						null,
						"",
						"Essential cybersecurity practices for businesses",
						new DateTimeOffset(
							new DateTime(2026, 1, 8, 16, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Singapore Management University",
						"Cybersecurity Awareness Training",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						14L,
						null,
						"",
						"Develop essential leadership skills for modern managers",
						new DateTimeOffset(
							new DateTime(2026, 1, 10, 17, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Orchard Hotel Singapore",
						"Leadership Excellence Workshop",
						"empty-user-12345",
						new DateTimeOffset(
							new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
					{
						15L,
						null,
						"",
						"Independent filmmakers present their latest works",
						new DateTimeOffset(
							new DateTime(2026, 1, 12, 23, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						"Singapore International Film Festival Venue",
						"Film & Media Production Showcase",
						"guest-user-22222",
						new DateTimeOffset(
							new DateTime(2026, 1, 12, 18, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
					},
				}
			);

			migrationBuilder.InsertData(
				table: "Bookings",
				columns: new[]
				{
					"BookingId",
					"AttendeeId",
					"CancellationDateTime",
					"CheckInDateTime",
					"CheckoutDateTime",
					"CreationDateTime",
					"GatheringId",
				},
				values: new object[,]
				{
					{
						"book_abc123456",
						"guest-user-22222",
						null,
						null,
						null,
						new DateTimeOffset(
							new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						1L,
					},
					{
						"book_def789012",
						"empty-user-12345",
						null,
						null,
						null,
						new DateTimeOffset(
							new DateTime(2024, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
							new TimeSpan(0, 0, 0, 0, 0)
						),
						2L,
					},
				}
			);

			migrationBuilder.InsertData(
				table: "GatheringCategoryDetails",
				columns: new[] { "CategoryId", "GatheringId" },
				values: new object[,]
				{
					{ 1L, 1L },
					{ 2L, 2L },
					{ 3L, 3L },
					{ 1L, 4L },
					{ 2L, 5L },
					{ 3L, 6L },
					{ 1L, 7L },
					{ 2L, 8L },
					{ 3L, 9L },
					{ 1L, 10L },
					{ 2L, 11L },
					{ 3L, 12L },
					{ 1L, 13L },
					{ 2L, 14L },
					{ 3L, 15L },
				}
			);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId"
			);

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL"
			);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId"
			);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId"
			);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId"
			);

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail"
			);

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL"
			);

			migrationBuilder.CreateIndex(
				name: "IX_Bookings_AttendeeId",
				table: "Bookings",
				column: "AttendeeId"
			);

			migrationBuilder.CreateIndex(
				name: "IX_Bookings_GatheringId",
				table: "Bookings",
				column: "GatheringId"
			);

			migrationBuilder.CreateIndex(
				name: "IX_GatheringCategoryDetails_CategoryId",
				table: "GatheringCategoryDetails",
				column: "CategoryId"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "AspNetRoleClaims");

			migrationBuilder.DropTable(name: "AspNetUserClaims");

			migrationBuilder.DropTable(name: "AspNetUserLogins");

			migrationBuilder.DropTable(name: "AspNetUserRoles");

			migrationBuilder.DropTable(name: "AspNetUserTokens");

			migrationBuilder.DropTable(name: "Bookings");

			migrationBuilder.DropTable(name: "GatheringCategoryDetails");

			migrationBuilder.DropTable(name: "AspNetRoles");

			migrationBuilder.DropTable(name: "AspNetUsers");

			migrationBuilder.DropTable(name: "Categories");

			migrationBuilder.DropTable(name: "Gatherings");
		}
	}
}
