using CMS.ContentEngine;
using CMS.Websites;

using NSubstitute;

using XperienceCommunity.DataRepository.Extensions;

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

        private class TestWebPageFieldsSource : IWebPageFieldsSource
        {
            public static string CONTENT_TYPE_NAME = "TestWebPageFieldsSource";

            public WebPageFields SystemFields => throw new NotImplementedException();
        }

        private class TestContentItemFieldsSource : IContentItemFieldsSource
        {
            public static string CONTENT_TYPE_NAME = "TestContentItemFieldsSource";
            public ContentItemFields SystemFields => throw new NotImplementedException();
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
    }
}
