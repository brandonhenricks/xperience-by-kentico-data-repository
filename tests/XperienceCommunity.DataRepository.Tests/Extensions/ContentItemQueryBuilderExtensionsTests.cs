using CMS.ContentEngine;

using XperienceCommunity.DataRepository.Extensions;

namespace XperienceCommunity.DataRepository.Tests.Extensions
{
    [TestFixture]
    public class ContentItemQueryBuilderExtensionsTests
    {
        [Test]
        public void ForContentType_ShouldNotThrowException_WhenContentTypeIsValid()
        {
            // Arrange
            var builder = new ContentItemQueryBuilder();

            // Act & Assert
            Assert.DoesNotThrow(() => builder.ForContentType<MockContentItemFieldsSource>());
        }

        public class MockContentItemFieldsSource : IContentItemFieldsSource
        {
            public static string CONTENT_TYPE_NAME = "TestContentItemFieldsSource";
            // Mock implementation
            public ContentItemFields SystemFields { get; } = new();
        }
    }
}
