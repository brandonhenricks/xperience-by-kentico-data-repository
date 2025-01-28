using CMS.ContentEngine;

using NSubstitute;

using XperienceCommunity.DataRepository.Extensions;

#pragma warning disable IDE0001
#pragma warning disable IDE0090

namespace XperienceCommunity.DataRepository.Tests.Extensions
{
    [TestFixture]
    public class IContentItemFieldsSourceExtensionsTests
    {
        [Test]
        public void GetCacheDependencyKey_ReturnsCorrectKey()
        {
            var source = Substitute.For<IContentItemFieldsSource>();
            var systemFields = new ContentItemFields { ContentItemID = 1, ContentItemIsSecured = true };
            source.SystemFields.Returns(systemFields);

            string[] result = source.GetCacheDependencyKey();

            Assert.That(result, Is.EqualTo(new[] { "contentitem|byid|1" }));
        }

        [Test]
        public void GetCacheDependencyKeys_ReturnsCorrectKeys()
        {
            var source = new List<IContentItemFieldsSource>
            {
                Substitute.For<IContentItemFieldsSource>(), Substitute.For<IContentItemFieldsSource>()
            };
            var systemFields1 = new ContentItemFields { ContentItemID = 1, ContentItemIsSecured = false };
            var systemFields2 = new ContentItemFields { ContentItemID = 2, ContentItemIsSecured = false };

            source[0].SystemFields.Returns(systemFields1);
            source[1].SystemFields.Returns(systemFields2);

            string[] result = source.GetCacheDependencyKeys();

            Assert.That(result, Is.EqualTo(new[] { "contentitem|byid|1", "contentitem|byid|2" }));
        }

        [Test]
        public void GetContentItemIds_ReturnsCorrectIds()
        {
            var source = new List<IContentItemFieldsSource>
            {
                Substitute.For<IContentItemFieldsSource>(), Substitute.For<IContentItemFieldsSource>()
            };

            var systemFields1 = new ContentItemFields { ContentItemID = 1, ContentItemIsSecured = false };
            var systemFields2 = new ContentItemFields { ContentItemID = 2, ContentItemIsSecured = false };

            source[0].SystemFields.Returns(systemFields1);
            source[1].SystemFields.Returns(systemFields2);

            var result = source.GetContentItemIds();

            Assert.That(result, Is.EqualTo(new[] { 1, 2 }));
        }

        [Test]
        public void GetContentTypes_ReturnsCorrectTypes()
        {
            var source = new List<TestContentItemFieldsSource>
            {
                Substitute.For<TestContentItemFieldsSource>(), Substitute.For<TestContentItemFieldsSource>()
            };

            var result = source.GetContentTypes();

            Assert.That(result, Is.EqualTo(new[] { "TestContentItemFieldsSource", "TestContentItemFieldsSource" }));
        }

        [Test]
        public void HasSecureItems_ReturnsFalse_WhenNoItemsAreSecure()
        {
            var source = new List<IContentItemFieldsSource>
            {
                Substitute.For<IContentItemFieldsSource>(), Substitute.For<IContentItemFieldsSource>()
            };
            var systemFields1 = new ContentItemFields { ContentItemIsSecured = false };
            var systemFields2 = new ContentItemFields { ContentItemIsSecured = false };

            source[0].SystemFields.Returns(systemFields1);
            source[1].SystemFields.Returns(systemFields2);

            bool result = source.HasSecureItems();

            Assert.That(result, Is.False);
        }

        [Test]
        public void HasSecureItems_ReturnsTrue_WhenAnyItemIsSecure()
        {
            var source = new List<IContentItemFieldsSource>
            {
                Substitute.For<IContentItemFieldsSource>(), Substitute.For<IContentItemFieldsSource>()
            };
            var systemFields1 = new ContentItemFields { ContentItemIsSecured = false };
            var systemFields2 = new ContentItemFields { ContentItemIsSecured = true };

            source[0].SystemFields.Returns(systemFields1);
            source[1].SystemFields.Returns(systemFields2);

            bool result = source.HasSecureItems();

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsSecureItem_ReturnsFalse_WhenItemIsNotSecure()
        {
            var source = Substitute.For<IContentItemFieldsSource>();
            var systemFields = new ContentItemFields { ContentItemIsSecured = false };
            source.SystemFields.Returns(systemFields);

            bool result = source.IsSecureItem();

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsSecureItem_ReturnsTrue_WhenItemIsSecure()
        {
            var source = Substitute.For<IContentItemFieldsSource>();
            var systemFields = new ContentItemFields { ContentItemIsSecured = true };
            source.SystemFields.Returns(systemFields);

            bool result = source.IsSecureItem();

            Assert.That(result, Is.True);
        }

        [Test]
        public void ToTypedList_ReturnsCorrectList()
        {
            var source = new List<TestContentItemFieldsSource>
            {
                Substitute.For<TestContentItemFieldsSource>(), Substitute.For<TestContentItemFieldsSource>()
            };

            var result = source.ToTypedList<TestContentItemFieldsSource>();

            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetContentItemGUIDs_ReturnsCorrectGUIDs()
        {
            var source = new List<IContentItemFieldsSource>
            {
                Substitute.For<IContentItemFieldsSource>(), Substitute.For<IContentItemFieldsSource>()
            };

            var systemFields1 = new ContentItemFields { ContentItemGUID = Guid.NewGuid() };
            var systemFields2 = new ContentItemFields { ContentItemGUID = Guid.NewGuid() };

            source[0].SystemFields.Returns(systemFields1);
            source[1].SystemFields.Returns(systemFields2);

            var result = source.GetContentItemGUIDs();

            Assert.That(result, Is.EqualTo(new[] { systemFields1.ContentItemGUID, systemFields2.ContentItemGUID }));
        }

        public class TestContentItemFieldsSource : IContentItemFieldsSource
        {
            public const string CONTENT_TYPE_NAME = "TestContentItemFieldsSource";
            public ContentItemFields SystemFields => new ContentItemFields();
        }
    }
}
