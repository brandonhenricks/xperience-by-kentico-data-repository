using CMS.ContentEngine;
using CMS.Helpers;

using XperienceCommunity.DataRepository.Builders;

namespace XperienceCommunity.DataRepository.Tests.Builders
{
    [TestFixture]
    public class CacheDependencyBuilderTests
    {
        private CacheDependencyBuilder builder;

        [SetUp]
        public void SetUp() => builder = new CacheDependencyBuilder();

        [Test]
        public void Create_ShouldReturnNull_WhenItemsAreEmpty()
        {
            // Arrange
            var items = Enumerable.Empty<object>();

            // Act
            var result = builder.Create(items);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Create_ShouldReturnNull_WhenNoDependencyKeysAreFound()
        {
            // Arrange
            var items = new List<object> { new() };

            // Act
            var result = builder.Create(items);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Create_ShouldReturnCMSCacheDependency_WhenDependencyKeysAreFound()
        {
            // Arrange
            var items = new List<IContentItemFieldsSource>
            {
                new MockContentItemFieldsSource { SystemFields = new ContentItemFields { ContentItemID = 1 } }
            };

            // Act
            var result = builder.Create(items);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CMSCacheDependency>());
            Assert.That(1, Is.EqualTo(result.CacheKeys.Length));
            Assert.That("contentitem|byid|1", Is.EqualTo(result.CacheKeys[0]));
        }

        [Test]
        public void Create_ShouldHandleMultipleDependencyKeys()
        {
            // Arrange
            var items = new List<IContentItemFieldsSource>
            {
                new MockContentItemFieldsSource { SystemFields = new ContentItemFields { ContentItemID = 1 } },
                new MockContentItemFieldsSource { SystemFields = new ContentItemFields { ContentItemID = 2 } }
            };

            // Act
            var result = builder.Create(items);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CMSCacheDependency>());
            Assert.That(1, Is.EqualTo(result.CacheKeys.Length));
        }

        public class MockContentItemFieldsSource : IContentItemFieldsSource
        {
            public ContentItemFields SystemFields { get; set; } = new ContentItemFields();
        }
    }
}
