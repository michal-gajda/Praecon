namespace Praecon.WinUI.Models.Profiles;

using AutoMapper;
using CreateCommand = Praecon.WinUI.Models.Commands.CreateArticle;
using Entity = Praecon.WinUI.Models.Entities.ArticleEntity;
using UpdateCommand = Praecon.WinUI.Models.Commands.UpdateArticle;
using ViewModel = Praecon.WinUI.Models.ViewModels.Article;

internal sealed class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        this.CreateMap<Entity, ViewModel>()
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.Title, options => options.MapFrom(source => source.Title))
            .ForMember(target => target.Date, options => options.MapFrom(source => new DateTime(source.Date.Year, source.Date.Month, source.Date.Day)))
            .ForMember(target => target.Payload, options => options.MapFrom(source => source.Payload))
            .ForMember(target => target.Published, options => options.MapFrom(source => source.Published))
            .ForMember(target => target.ThumbnailId, options => options.MapFrom(source => source.ThumbnailId))
            .ForMember(target => target.MediaId, options => options.MapFrom(source => source.MediaId))
            .ForMember(target => target.Tags, options => options.MapFrom(source => source.Tags))
            ;

        this.CreateMap<ViewModel, CreateCommand>()
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.Title, options => options.MapFrom(source => source.Title))
            .ForMember(target => target.Date, options => options.MapFrom(source => DateOnly.FromDateTime(source.Date)))
            .ForMember(target => target.Payload, options => options.MapFrom(source => source.Payload))
            .ForMember(target => target.Published, options => options.MapFrom(source => source.Published))
            .ForMember(target => target.ThumbnailId, options => options.MapFrom(source => source.ThumbnailId))
            .ForMember(target => target.MediaId, options => options.MapFrom(source => source.MediaId))
            .ForMember(target => target.Tags, options => options.MapFrom(source => source.Tags))
            ;

        this.CreateMap<ViewModel, UpdateCommand>()
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.Title, options => options.MapFrom(source => source.Title))
            .ForMember(target => target.Date, options => options.MapFrom(source => DateOnly.FromDateTime(source.Date)))
            .ForMember(target => target.Payload, options => options.MapFrom(source => source.Payload))
            .ForMember(target => target.Published, options => options.MapFrom(source => source.Published))
            .ForMember(target => target.ThumbnailId, options => options.MapFrom(source => source.ThumbnailId))
            .ForMember(target => target.MediaId, options => options.MapFrom(source => source.MediaId))
            .ForMember(target => target.Tags, options => options.MapFrom(source => source.Tags))
            ;

        this.CreateMap<IEnumerable<Entity>, IEnumerable<ViewModel>>()
            .ConvertUsing((source, _, context) =>
                source.Select(item => context.Mapper.Map<ViewModel>(item)))
            ;
    }
}
