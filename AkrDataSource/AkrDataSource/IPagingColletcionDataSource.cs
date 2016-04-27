using System.Threading.Tasks;

namespace AkrDataSource
{
    public interface IPagingColletcionDataSource<in TParam> : IColletcionDataSource<TParam>
    {
        int CurrentPage { get; }

        int CountInPage(int page);

        PagingType PagingType { get; }

        Task Next();
        Task SetPage(int page);
    }
}