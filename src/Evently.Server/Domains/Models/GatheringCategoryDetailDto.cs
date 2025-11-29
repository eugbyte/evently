using JetBrains.Annotations;

namespace Evently.Server.Domains.Models;

[UsedImplicitly]
public sealed record GatheringCategoryDetailDto(long GatheringId, long CategoryId);