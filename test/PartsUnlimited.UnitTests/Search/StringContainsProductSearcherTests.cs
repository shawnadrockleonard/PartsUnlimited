// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PartsUnlimited.Models;
using PartsUnlimited.UnitTests.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PartsUnlimited.Search
{
    public class StringContainsProductSearcherTests
    {
        private static readonly IEnumerable<string> s_productTitles = new[] { "word in the middle", "something", "something outside", "inside where outside" };

        [Fact]
        public async Task SearchSuccess()
        {
            var productList = s_productTitles.Select(o => new Product { Title = o });

            using var context = DbHelper.GetFakeDbContext();
            context.Products.AddRange(productList);
            context.SaveChanges();


            var searcher = new StringContainsProductSearch(context);
            var thing = await searcher.Search("thing");

            Assert.Equal(new string[] { "something", "something outside" }, thing.Select(o => o.Title));
        }
    }
}