using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Data.Migrations {
	/// <inheritdoc />
	public partial class UpdateSeededDates : Migration {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.UpdateData(
				table: "Bookings",
				keyColumn: "BookingId",
				keyValue: "book_abc123456",
				column: "CreationDateTime",
				value: new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
					new TimeSpan(0, 0, 0, 0, 0)));

			migrationBuilder.UpdateData(
				table: "Bookings",
				keyColumn: "BookingId",
				keyValue: "book_def789012",
				column: "CreationDateTime",
				value: new DateTimeOffset(new DateTime(2025, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
					new TimeSpan(0, 0, 0, 0, 0)));

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 1L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 5, 17, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 2L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 10, 22, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 10, 18, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 3L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 15, 10, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 4L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 8, 17, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 8, 13, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 5L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 20, 16, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 20, 14, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 6L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 22, 12, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 22, 8, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 7L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 12, 9, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 8L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 25, 14, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 9L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 28, 15, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 28, 10, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 10L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 12, 30, 17, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 12, 30, 9, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 11L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2027, 1, 3, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2027, 1, 3, 13, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 12L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2027, 1, 5, 22, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2027, 1, 5, 19, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 13L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2027, 1, 8, 16, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2027, 1, 8, 10, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 14L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2027, 1, 10, 17, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2027, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 15L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2027, 1, 12, 23, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2027, 1, 12, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.UpdateData(
				table: "Bookings",
				keyColumn: "BookingId",
				keyValue: "book_abc123456",
				column: "CreationDateTime",
				value: new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
					new TimeSpan(0, 0, 0, 0, 0)));

			migrationBuilder.UpdateData(
				table: "Bookings",
				keyColumn: "BookingId",
				keyValue: "book_def789012",
				column: "CreationDateTime",
				value: new DateTimeOffset(new DateTime(2024, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
					new TimeSpan(0, 0, 0, 0, 0)));

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 1L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 5, 17, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 2L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 10, 22, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 10, 18, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 3L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 15, 10, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 4L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 8, 17, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 8, 13, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 5L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 20, 16, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 20, 14, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 6L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 22, 12, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 22, 8, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 7L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 12, 9, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 8L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 25, 14, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 9L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 28, 15, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 28, 10, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 10L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2025, 12, 30, 17, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2025, 12, 30, 9, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 11L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 1, 3, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 1, 3, 13, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 12L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 1, 5, 22, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 1, 5, 19, 30, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 13L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 1, 8, 16, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 14L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 1, 10, 17, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});

			migrationBuilder.UpdateData(
				table: "Gatherings",
				keyColumn: "GatheringId",
				keyValue: 15L,
				columns: new[] { "End", "Start" },
				values: new object[] {
					new DateTimeOffset(new DateTime(2026, 1, 12, 23, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0)),
					new DateTimeOffset(new DateTime(2026, 1, 12, 18, 0, 0, 0, DateTimeKind.Unspecified),
						new TimeSpan(0, 0, 0, 0, 0))
				});
		}
	}
}