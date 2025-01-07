using CMS.Websites;

using NSubstitute;

using XperienceCommunity.DataRepository.Extensions;

namespace XperienceCommunity.DataRepository.Tests.Extensions;

[TestFixture]
public class IWebPageFieldsSourceExtensionsTests
{
    public class TestWebPageFieldsSource : IWebPageFieldsSource
    {
        public static string CONTENT_TYPE_NAME = "TestWebPageFieldsSource";
        public WebPageFields SystemFields => new();
    }


    [Test]
    public void IsSecureItem_ShouldReturnTrue_WhenItemIsSecure()
    {
        var source = Substitute.For<IWebPageFieldsSource>();
        var systemFields = new WebPageFields { ContentItemIsSecured = true };
        source.SystemFields.Returns(systemFields);

        bool result = source.IsSecureItem();

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSecureItem_ReturnsFalse_WhenItemIsNotSecure()
    {
        var source = Substitute.For<IWebPageFieldsSource>();
        var systemFields = new WebPageFields { ContentItemIsSecured = false };
        source.SystemFields.Returns(systemFields);

        bool result = source.IsSecureItem();

        Assert.That(result, Is.False);
    }

    [Test]
    public void HasSecureItems_ShouldReturnTrue_WhenCollectionContainsSecureItems()
    {
        var source = new List<IWebPageFieldsSource>
        {
            Substitute.For<IWebPageFieldsSource>(), Substitute.For<IWebPageFieldsSource>()
        };
        var systemFields1 = new WebPageFields { ContentItemIsSecured = false };
        var systemFields2 = new WebPageFields { ContentItemIsSecured = true };

        source[0].SystemFields.Returns(systemFields1);
        source[1].SystemFields.Returns(systemFields2);

        bool result = source.HasSecureItems();

        Assert.That(result, Is.True);
    }

    [Test]
    public void HasSecureItems_ReturnsFalse_WhenNoItemsAreSecure()
    {
        var source = new List<IWebPageFieldsSource>
        {
            Substitute.For<IWebPageFieldsSource>(), Substitute.For<IWebPageFieldsSource>()
        };
        var systemFields1 = new WebPageFields { ContentItemIsSecured = false };
        var systemFields2 = new WebPageFields { ContentItemIsSecured = false };

        source[0].SystemFields.Returns(systemFields1);
        source[1].SystemFields.Returns(systemFields2);

        bool result = source.HasSecureItems();

        Assert.That(result, Is.False);
    }

    [Test]
    public void GetCacheDependencyKey_ShouldReturnCorrectKey_WhenSourceIsNotNull()
    {
        var source = Substitute.For<IWebPageFieldsSource>();
        var systemFields = new WebPageFields() { WebPageItemID = 1, ContentItemIsSecured = true };
        source.SystemFields.Returns(systemFields);

        string[] result = source.GetCacheDependencyKey();
        Assert.That(result, Is.EqualTo(new[] { "webpageitem|byid|1" }));
    }

    [Test]
    public void GetCacheDependencyKeys_ShouldReturnCorrectKeys_WhenSourcesAreNotNull()
    {
        var source = new List<IWebPageFieldsSource>
        {
            Substitute.For<IWebPageFieldsSource>(), Substitute.For<IWebPageFieldsSource>()
        };
        var systemFields1 = new WebPageFields() { WebPageItemID = 1, ContentItemIsSecured = false };
        var systemFields2 = new WebPageFields() { WebPageItemID = 2, ContentItemIsSecured = false };

        source[0].SystemFields.Returns(systemFields1);
        source[1].SystemFields.Returns(systemFields2);

        string[] result = source.GetCacheDependencyKeys();
        Assert.That(result, Is.EqualTo(new[] { "webpageitem|byid|1", "webpageitem|byid|2" }));
    }

    [Test]
    public void GetWebPageItemIds_ShouldReturnCorrectIds_WhenSourcesAreNotNull()
    {
        var sources = new List<IWebPageFieldsSource>
        {
            Substitute.For<IWebPageFieldsSource>(), Substitute.For<IWebPageFieldsSource>()
        };

        var systemFields1 = new WebPageFields() { WebPageItemID = 1, ContentItemIsSecured = false };
        var systemFields2 = new WebPageFields() { WebPageItemID = 2, ContentItemIsSecured = false };

        sources[0].SystemFields.Returns(systemFields1);
        sources[1].SystemFields.Returns(systemFields2);

        var result = sources.GetWebPageItemIds();
        Assert.That(result, Is.EqualTo(new[] { 1, 2 }));
    }

    [Test]
    public void GetContentTypes_ShouldReturnCorrectContentTypes_WhenSourcesAreNotNull()
    {
        var source = new List<TestWebPageFieldsSource>
        {
            Substitute.For<TestWebPageFieldsSource>(), Substitute.For<TestWebPageFieldsSource>()
        };

        var result = source.GetContentTypes();

        Assert.That(result, Is.EqualTo(new[] { "TestWebPageFieldsSource", "TestWebPageFieldsSource" }));
        Assert.That(result, Is.EqualTo(new[] { "TestWebPageFieldsSource", "TestWebPageFieldsSource" }));
    }
}
