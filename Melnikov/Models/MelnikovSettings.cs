using Nestor.Db.Models;

namespace Melnikov.Models;

[SourceEntity(nameof(Id))]
public partial class MelnikovSettings
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
}
