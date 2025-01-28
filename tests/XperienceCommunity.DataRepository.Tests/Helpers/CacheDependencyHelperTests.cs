using CMS.ContentEngine;
using CMS.Websites;

using XperienceCommunity.DataRepository.Helpers;

namespace XperienceCommunity.DataRepository.Tests.Helpers
{
    [TestFixture]
    public class CacheDependencyHelperTests
    {
        [Test]
        public void CreateContentItemGUIDKeys_ShouldReturnEmptyArray_WhenItemGuidsIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateContentItemGUIDKeys(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateContentItemGUIDKeys_ShouldReturnCacheKeys_WhenItemGuidsIsNotNull()
        {
            // Arrange
            var guids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            // Act
            string[] result = CacheDependencyHelper.CreateContentItemGUIDKeys(guids);

            // Assert
            Assert.That(result.Length, Is.EqualTo(guids.Count));
            Assert.That(result[0], Is.EqualTo($"contentitem|byguid|{guids[0]}"));
            Assert.That(result[1], Is.EqualTo($"contentitem|byguid|{guids[1]}"));
        }

        [Test]
        public void CreateWebPageItemGUIDKeys_ShouldReturnEmptyArray_WhenItemGuidsIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemGUIDKeys(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateWebPageItemGUIDKeys_ShouldReturnCacheKeys_WhenItemGuidsIsNotNull()
        {
            // Arrange
            var guids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemGUIDKeys(guids);

            // Assert
            Assert.That(result.Length, Is.EqualTo(guids.Count));
            Assert.That(result[0], Is.EqualTo($"webpageitem|byguid|{guids[0]}"));
            Assert.That(result[1], Is.EqualTo($"webpageitem|byguid|{guids[1]}"));
        }

        [Test]
        public void CreateContentItemIdKeys_ShouldReturnEmptyArray_WhenItemIdListIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateContentItemIdKeys(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateContentItemIdKeys_ShouldReturnCacheKeys_WhenItemIdListIsNotNull()
        {
            // Arrange
            var ids = new List<int> { 1, 2 };

            // Act
            string[] result = CacheDependencyHelper.CreateContentItemIdKeys(ids);

            // Assert
            Assert.That(result.Length, Is.EqualTo(ids.Count));
            Assert.That(result[0], Is.EqualTo("contentitem|byid|1"));
            Assert.That(result[1], Is.EqualTo("contentitem|byid|2"));
        }

        [Test]
        public void CreateWebPageItemIdKeys_ShouldReturnEmptyArray_WhenItemIdListIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemIdKeys(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateWebPageItemIdKeys_ShouldReturnCacheKeys_WhenItemIdListIsNotNull()
        {
            // Arrange
            var ids = new List<int> { 1, 2 };

            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemIdKeys(ids);

            // Assert
            Assert.That(result.Length, Is.EqualTo(ids.Count));
            Assert.That(result[0], Is.EqualTo("webpageitem|byid|1"));
            Assert.That(result[1], Is.EqualTo("webpageitem|byid|2"));
        }

        [Test]
        public void CreateWebPageItemTypeKeys_ShouldReturnEmptyArray_WhenContentTypesIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemTypeKeys(null, "channel");

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateWebPageItemTypeKeys_ShouldReturnCacheKeys_WhenContentTypesIsNotNull()
        {
            // Arrange
            var contentTypes = new List<string> { "type1", "type2" };

            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemTypeKeys(contentTypes, "channel");

            // Assert
            Assert.That(result.Length, Is.EqualTo(contentTypes.Count));
            Assert.That(result[0], Is.EqualTo("webpageitem|bychannel|channel|bycontenttype|type1"));
            Assert.That(result[1], Is.EqualTo("webpageitem|bychannel|channel|bycontenttype|type2"));
        }

        [Test]
        public void CreateContentItemTypeKeys_ShouldReturnEmptyArray_WhenContentTypesIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateContentItemTypeKeys(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateContentItemTypeKeys_ShouldReturnCacheKeys_WhenContentTypesIsNotNull()
        {
            // Arrange
            var contentTypes = new List<string> { "type1", "type2" };

            // Act
            string[] result = CacheDependencyHelper.CreateContentItemTypeKeys(contentTypes);

            // Assert
            Assert.That(result.Length, Is.EqualTo(contentTypes.Count));
            Assert.That(result[0], Is.EqualTo("contentitem|bycontenttype|type1"));
            Assert.That(result[1], Is.EqualTo("contentitem|bycontenttype|type2"));
        }

        [Test]
        public void CreateContentItemKeys_ShouldReturnEmptyArray_WhenItemsIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateContentItemKeys<IContentItemFieldsSource>(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateContentItemKeys_ShouldReturnCacheKeys_WhenItemsIsNotNull()
        {
            // Arrange
            var items = new List<IContentItemFieldsSource>
            {
                new MockContentItemFieldsSource { SystemFields = new ContentItemFields { ContentItemGUID = Guid.NewGuid() } },
                new MockContentItemFieldsSource { SystemFields = new ContentItemFields { ContentItemGUID = Guid.NewGuid() } }
            };

            // Act
            string[] result = CacheDependencyHelper.CreateContentItemKeys(items);

            // Assert
            Assert.That(result.Length, Is.EqualTo(items.Count));
            Assert.That(result[0], Is.EqualTo($"contentitem|byid|{items[0].SystemFields.ContentItemGUID}"));
            Assert.That(result[1], Is.EqualTo($"contentitem|byid|{items[1].SystemFields.ContentItemGUID}"));
        }

        [Test]
        public void CreateWebPageItemKeys_ShouldReturnEmptyArray_WhenItemsIsNull()
        {
            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemKeys<IWebPageFieldsSource>(null);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CreateWebPageItemKeys_ShouldReturnCacheKeys_WhenItemsIsNotNull()
        {
            // Arrange
            var items = new List<IWebPageFieldsSource>
            {
                new MockWebPageFieldsSource { SystemFields = new WebPageFields { WebPageItemID = 1 } },
                new MockWebPageFieldsSource { SystemFields = new WebPageFields { WebPageItemID = 2 } }
            };

            // Act
            string[] result = CacheDependencyHelper.CreateWebPageItemKeys(items);

            // Assert
            Assert.That(result.Length, Is.EqualTo(items.Count));
            Assert.That(result[0], Is.EqualTo("webpageitem|byid|1"));
            Assert.That(result[1], Is.EqualTo("webpageitem|byid|2"));
        }

        public class MockContentItemFieldsSource : IContentItemFieldsSource
        {
            public ContentItemFields SystemFields { get; set; } = new ContentItemFields();
        }

        public class MockWebPageFieldsSource : IWebPageFieldsSource
        {
            public WebPageFields SystemFields { get; set; } = new WebPageFields();
        }
    }
}
