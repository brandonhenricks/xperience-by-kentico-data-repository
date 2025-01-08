using CMS.ContentEngine;
using CMS.Websites;

using NSubstitute;

using XperienceCommunity.DataRepository.Extensions;

#pragma warning disable S1144
#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace XperienceCommunity.DataRepository.Tests.Extensions
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        public interface ISchemaTest
        {
            // ReSharper disable once ArrangeTypeMemberModifiers
            public const string REUSABLE_FIELD_SCHEMA_NAME = "TestSchema";
        }

        [Test]
        public void GetContentTypeName_ShouldReturnContentTypeName_WhenTypeInheritsFromIWebPageFieldsSource()
        {
            // Arrange
            var type = typeof(TestWebPageFieldsSource);

            // Act
            string? result = type.GetContentTypeName();

            // Assert
            Assert.That(result, Is.EqualTo("TestWebPageFieldsSource"));
        }

        [Test]
        public void GetContentTypeName_ShouldReturnNull_WhenTypeDoesNotInheritFromIWebPageFieldsSourceOrIContentItemFieldsSource()
        {
            // Arrange
            var type = typeof(string);

            // Act
            string? result = type.GetContentTypeName();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetReusableFieldSchemaName_ShouldReturnNull_WhenTypeIsNotInterface()
        {
            // Arrange
            var value = new TestWebPageFieldsSource();

            // Act
            string? result = value.GetReusableFieldSchemaName();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetReusableFieldSchemaName_ShouldReturnSchemaName_WhenTypeIsInterface()
        {
            // Arrange
            var value = Substitute.For<ISchemaTest>();

            // Act
            string? result = value.GetReusableFieldSchemaName();

            // Assert
            Assert.That(result, Is.EqualTo("TestSchema"));
        }

        [Test]
        public void GetStaticString_ShouldReturnFieldValue_WhenFieldExists()
        {
            // Arrange
            var type = typeof(TestWebPageFieldsSource);

            // Act
            string? result = type.GetStaticString("CONTENT_TYPE_NAME");

            // Assert
            Assert.That(result, Is.EqualTo("TestWebPageFieldsSource"));
        }

        [Test]
        public void GetStaticString_ShouldReturnNull_WhenFieldDoesNotExist()
        {
            // Arrange
            var type = typeof(TestWebPageFieldsSource);

            // Act
            string? result = type.GetStaticString("NON_EXISTENT_FIELD");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void InheritsFromIContentItemFieldsSource_ShouldReturnFalse_WhenTypeDoesNotInherit()
        {
            // Arrange
            var type = typeof(string);

            // Act
            bool result = type.InheritsFromIContentItemFieldsSource();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InheritsFromIContentItemFieldsSource_ShouldReturnTrue_WhenTypeInherits()
        {
            // Arrange
            var type = typeof(TestContentItemFieldsSource);

            // Act
            bool result = type.InheritsFromIContentItemFieldsSource();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void InheritsFromIWebPageFieldsSource_ShouldReturnFalse_WhenTypeDoesNotInherit()
        {
            // Arrange
            var type = typeof(string);

            // Act
            bool result = type.InheritsFromIWebPageFieldsSource();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InheritsFromIWebPageFieldsSource_ShouldReturnTrue_WhenTypeInherits()
        {
            // Arrange
            var type = typeof(TestWebPageFieldsSource);

            // Act
            bool result = type.InheritsFromIWebPageFieldsSource();

            // Assert
            Assert.That(result, Is.True);
        }


        [Test]
        public void GetRelatedWebPageGuids_SingleItem_ReturnsGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var relatedItem = new WebPageRelatedItem { WebPageGuid = guid };

            var source = new TestContentItemFieldsSource() { RelatedItem = relatedItem };

            // Act
            var result = source.GetRelatedWebPageGuids(x => x.RelatedItem);

            // Assert
            Assert.That(result, Is.EquivalentTo([guid]));
        }

        [Test]
        public void GetRelatedWebPageGuids_MultipleItems_ReturnsGuids()
        {
            // Arrange
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var relatedItems = new List<WebPageRelatedItem>
            {
                new() { WebPageGuid = guid1 },
                new() { WebPageGuid = guid2 }
            };

            var source = new TestWebPageFieldsSource() { RelatedItems = relatedItems };


            // Act
            var result = source.GetRelatedWebPageGuids(x => x.RelatedItems);

            // Assert
            Assert.That(result, Is.EquivalentTo(new[] { guid1, guid2 }));
        }

        [Test]
        public void GetRelatedWebPageGuids_NoItems_ReturnsEmpty()
        {
            // Arrange
            var source = new TestWebPageFieldsSource();

            // Act
            var result = source.GetRelatedWebPageGuids(x => x.RelatedItems);

            // Assert
            Assert.That(result, Is.Empty);
        }
        public class TestContentItemFieldsSource : IContentItemFieldsSource
        {
            public static string CONTENT_TYPE_NAME = "TestContentItemFieldsSource";

            public WebPageRelatedItem RelatedItem { get; set; } = new WebPageRelatedItem();

            public ContentItemFields SystemFields => throw new NotImplementedException();
        }

        public class TestWebPageFieldsSource : IWebPageFieldsSource
        {
            public static string CONTENT_TYPE_NAME = "TestWebPageFieldsSource";

            public IEnumerable<WebPageRelatedItem> RelatedItems { get; set; } = [];

            public WebPageFields SystemFields => throw new NotImplementedException();
        }
    }
}
