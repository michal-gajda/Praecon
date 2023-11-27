namespace Praecon.WinUI.Models.Services;

using Dapper;
using Microsoft.Data.SqlClient;
using Praecon.WinUI;
using Praecon.WinUI.Models.Entities;
using Praecon.WinUI.Models.Interfaces;

internal sealed class ArticleRepository : IArticleRepository
{
    private const string CREATE = "INSERT INTO [dbo].[Article]([Id], [Title], [Date], [Payload], [Published], [ThumbnailId], [MediaId]) VALUES (@Id, @Title, @Date, @Payload, @Published, @ThumbnailId, @MediaId)";

    private const string UPDATE =
        "UPDATE [dbo].[Article] SET [Title] = @Title, [Date] = @Date, [Payload] = @Payload, [Published] = @Published, [ThumbnailId] = @ThumbnailId, [MediaId]=@MediaId WHERE [Id] = @Id";
    private const string LIST = "SELECT [Id], [Title], [Date], [Payload], [Published], [ThumbnailId], [MediaId], '' AS Tags FROM [dbo].[Article]";

    private readonly ILogger<ArticleRepository> logger;
    private readonly SqlServerOptions options;

    public ArticleRepository(ILogger<ArticleRepository> logger, SqlServerOptions options)
        => (this.logger, this.options) = (logger, options);

    public async Task CreateAsync(ArticleEntity entity, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var parameters = new
        {
            entity.Id,
            entity.Date,
            entity.MediaId,
            entity.Payload,
            entity.Published,
            entity.ThumbnailId,
            entity.Title,
        };

        await connection.QueryAsync(CREATE, parameters);
    }

    public Task<ArticleEntity?> ReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(ArticleEntity entity, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var parameters = new
        {
            entity.Id,
            entity.Date,
            entity.MediaId,
            entity.Payload,
            entity.Published,
            entity.ThumbnailId,
            entity.Title,
        };

        await connection.QueryAsync(UPDATE, parameters);
    }

    public async Task<IEnumerable<ArticleEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        return await connection.QueryAsync<ArticleEntity>(LIST);
    }
}
