namespace Praecon.WinUI.Models.Entities;

public sealed class ArticleEntity
{
    public DateOnly Date { get; private set; } = new(year: 2000, month: 1, day: 1);
    public Guid Id { get; private set; }
    public Guid? MediaId { get; private set; } = default;
    public string Payload { get; private set; } = string.Empty;
    public bool Published { get; private set; } = false;
    public string Tags { get; private set; } = string.Empty;
    public Guid? ThumbnailId { get; private set; } = default;
    public string Title { get; private set; } = string.Empty;

    public ArticleEntity(Guid id, DateOnly date, string title, string payload)
    {
        this.Id = id;
        this.SetDate(date);
        this.SetTitle(title);
        this.SetPayload(payload);
    }

    public ArticleEntity(Guid id, string title, DateOnly date, string payload, bool published = default, Guid? thumbnailId = default, Guid? mediaId = default, string tags = "")
    {
        this.Id = id;
        this.SetTitle(title);
        this.SetDate(date);
        this.SetPayload(payload);
        this.Published = published;
        this.SetThumbnailId(thumbnailId);
        this.SetMediaId(mediaId);
        this.SetTags(tags);
    }

    public void Publish()
    {
        this.Published = true;
    }

    public void SetDate(DateOnly date)
    {
        this.Date = date;
    }

    public void SetMediaId(Guid? mediaId)
    {
        this.MediaId = mediaId;
    }

    public void SetPayload(string payload)
    {
        this.Payload = payload;
    }

    public void SetTags(string tags)
    {
        this.Tags = tags;
    }

    public void SetThumbnailId(Guid? thumbnailId)
    {
        this.ThumbnailId = thumbnailId;
    }

    public void SetTitle(string title)
    {
        this.Title = title;
    }

    public void Unpublish()
    {
        this.Published = false;
    }
}
