using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AkrDataSource.UnitTest
{
    [TestFixture]
    public class PagingColletcionDataSourceBase
    {
        [Test]
        public async Task PagingLoad()
        {
            var source = new SimpleCollectionDataSource();
            await source.Reload(10);

            Assert.AreEqual(source.Count, 10);

            await source.Next();

            Assert.AreEqual(source.Count, 10);
        }

        [Test]
        public async Task InfiniteLoad()
        {
            var source = new SimpleCollectionDataSource();
            source.PagingTypeImpt = PagingType.Infinite;
            await source.Reload(10);

            Assert.AreEqual(source.Count, 10);

            await source.Next();

            Assert.AreEqual(source.Count, 20);
        }

        [Test]
        public async Task InfiniteReLoad()
        {
            var source = new SimpleCollectionDataSource();
            source.PagingTypeImpt = PagingType.Infinite;
            await source.Reload(10);

            Assert.AreEqual(source.Count, 10);

            await source.Reload(12);

            Assert.AreEqual(source.Count, 10);
        }

        [Test]
        public async Task LoadEventRaise()
        {
            var source = new SimpleCollectionDataSource();
            int changedCount = 0;
            source.CollectionChanged += (_, e) =>
            {
                Console.WriteLine(e);
                changedCount++;
            };
            await source.Reload(10);
            Assert.AreEqual(source.Count, 10);
        }
    }

    public class SimpleCollectionDataSource : PagingColletcionDataSourceBase<string, int>
    {
        public override int CountInPage(int page)
        {
            return CurrentParam;
        }

        public PagingType PagingTypeImpt { get; set; } = PagingType.Paging;

        public override PagingType PagingType => PagingTypeImpt;


        protected override async Task<IEnumerable<string>> LoadDataImpt(int page, int param)
        {
            if (page == 2) return new List<string>();
            await Task.Delay(500);
            return Enumerable.Range(0, param).Select(i => $"page = {page} ,param = {param}");
        }
    }
}
