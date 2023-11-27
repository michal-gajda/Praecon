namespace Praecon.WinUI;

internal sealed record SqlServerOptions
{
    public required string ConnectionString { get; init; }
}