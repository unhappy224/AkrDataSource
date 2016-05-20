using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AkrDataSource
{
    public abstract class PagingColletcionDataSourceBase<T, TParam> : ObservableCollection<T>,
        IPagingColletcionDataSource<TParam>
    {
        protected int CurrentPageImpt;

        protected TParam CurrentParam { get; set; }

        public bool IsNoMoreData { get; set; }

        public async Task Reload(TParam param)
        {
            IsNoMoreData = false;

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsNoMoreData)));
            CurrentPageImpt = 0;
            CurrentParam = param;
            await SetPage(0, true);
        }

        public int CurrentPage => CurrentPageImpt;

        public abstract int CountInPage(int page);

        public abstract PagingType PagingType { get; }

        public async Task Next()
        {
            CurrentPageImpt += 1;
            await SetPage(CurrentPageImpt, false);
        }


        public async Task SetPage(int page, bool needClear)
        {
            IEnumerable<T> data;
            try
            {
                data = await LoadDataImpt(page, CurrentParam);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                if (needClear || PagingType == PagingType.Paging && Count > 0)
                {
                    Clear();
                }
            }

            if (data == null || !data.Any())
            {
                IsNoMoreData = true;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsNoMoreData)));
            }
            else
            {
                AddRange(data);
            }
        }


        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var i in collection)
            {
                Items.Add(i);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                collection.ToList()));
        }

        protected abstract Task<IEnumerable<T>> LoadDataImpt(int page, TParam param);
    }
}