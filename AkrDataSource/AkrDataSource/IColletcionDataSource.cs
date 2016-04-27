using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace AkrDataSource
{
    public interface IColletcionDataSource<in TParam> : INotifyCollectionChanged
    {
        Task Reload(TParam param);
    }
}