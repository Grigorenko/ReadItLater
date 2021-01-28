using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Http;
using ReadItLater.Web.Client.Services.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class TagsSelector : IDisposable,
        IContext,
        IFolderChanged,
        IStateChanged
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public int refsAllCount { get; set; }
        //ToDo: think about getting this value from query for all tags by folder

        private Shared.Breadcrumbs breadcrumbsComponent;
        private TagListItemProjection[] tags;
        private Guid? folderId;
        private Guid? tagId;
        private SortItems sortItems;

        protected override void OnInitialized()
        {
            Context.Subscribe(this);
            folderId = null;
            tagId = null;
            sortItems = new SortItems(SortTags);
        }

        private void TagChosen(Guid? id)
        {
            Context.TagChosen(id);
            this.tagId = id;
        }

        async Task IFolderChanged.Handle(Guid folderId, Guid? tagId)
        {
            tags = await httpService.GetListOfTagsAsync(folderId);
            this.folderId = folderId;
            this.tagId = tagId;

            StateHasChanged();

            if (breadcrumbsComponent is null)
                throw new NullReferenceException($"{nameof(breadcrumbsComponent)} is null.");

            await breadcrumbsComponent?.FolderChangedEventHandler(folderId, tagId);
        }

        async Task IStateChanged.Handle()
        {
            StateHasChanged();

            await Task.CompletedTask;
        }

        private void SortTags(string sortProperty, Direction direction)
        {
            var source = tags.AsQueryable();
            var type = typeof(TagListItemProjection);
            var property = type.GetProperty(sortProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = direction == Direction.Descending ? "OrderByDescending" : "OrderBy";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));

            tags = source.Provider.CreateQuery<TagListItemProjection>(resultExp).ToArray();
        }

        private string GetSelectedTagClassName(Guid? tagId = null) =>
            !this.tagId.HasValue && tagId is null
                ? "selected"
                : this.tagId.HasValue && tagId == this.tagId.Value
                    ? "selected"
                    : string.Empty;

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }

    //public class SortItems
    //{
    //    private const string name = "Name";
    //    private const string count = "RefsCount";

    //    private string sortProperty;
    //    private Direction direction;

    //    private Action<string, Direction> sortChangedHandler;

    //    public SortItems(Action<string, Direction> sortChangedHandler)
    //    {
    //        this.sortChangedHandler = sortChangedHandler;
    //        sortProperty = count;
    //        direction = Direction.Descending;
    //    }

    //    public void SortByNameAsc() => AppleSorting(name, Direction.Ascending);
    //    public void SortByNameDesc() => AppleSorting(name, Direction.Descending);
    //    public void SortByCountAsc() => AppleSorting(count, Direction.Ascending);
    //    public void SortByCountDesc() => AppleSorting(count, Direction.Descending);

    //    public string SortByNameAscClassName => string.Equals(sortProperty, name) && direction == Direction.Ascending ? "active" : NameAvailable ? string.Empty : "inactive";
    //    public string SortByNameDescClassName => string.Equals(sortProperty, name) && direction == Direction.Descending ? "active" : "inactive";
    //    public string SortByCountAscClassName => string.Equals(sortProperty, count) && direction == Direction.Ascending ? "active" : "inactive";
    //    public string SortByCountDescClassName => string.Equals(sortProperty, count) && direction == Direction.Descending ? "active" : CountAvailable ? string.Empty : "inactive";

    //    private bool CountAvailable => !string.Equals(sortProperty, count);
    //    private bool NameAvailable => !string.Equals(sortProperty, name);

    //    private void AppleSorting(string sortProperty, Direction direction)
    //    {
    //        this.sortProperty = sortProperty;
    //        this.direction = direction;

    //        sortChangedHandler.Invoke(this.sortProperty, this.direction);
    //    }
    //}
}
