using System.Text;
using ConsiliumTempus.Infrastructure.Security.Authorization.Http;
using Microsoft.AspNetCore.Http;

namespace ConsiliumTempus.Infrastructure.UnitTests.Security.Authorization.Http;

public class HttpRequestReaderTest
{
    #region Setup

    private static HttpRequest CreateRequest(string? body = null)
    {
        var request = new DefaultHttpContext().Request;
        if (body is not null) request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
        return request;
    }

    private static async Task<string> ReadBody(HttpRequest request)
    {
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }

    #endregion

    [Fact]
    public void GetStringIdFromRoute_WhenIdIsPresent_ShouldReturnIt()
    {
        // Arrange
        var request = CreateRequest();
        request.RouteValues["id"] = "some id";

        // Act
        var outcome = HttpRequestReader.GetStringIdFromRoute(request);

        // Assert
        outcome.Should().Be("some id");
    }

    [Fact]
    public void GetStringIdFromRoute_WhenPropertyIsProvided_ShouldReturnThatProperty()
    {
        // Arrange
        var request = CreateRequest();
        request.RouteValues["id"] = "wrong id";
        request.RouteValues["projectId"] = "project id";

        // Act
        var outcome = HttpRequestReader.GetStringIdFromRoute(request, "projectId");

        // Assert
        outcome.Should().Be("project id");
    }

    [Fact]
    public void GetStringIdFromRoute_WhenPropertyCasingIsDifferent_ShouldReturnIt()
    {
        // Arrange
        var request = CreateRequest();
        request.RouteValues["ID"] = "some id";

        // Act
        var outcome = HttpRequestReader.GetStringIdFromRoute(request);

        // Assert
        outcome.Should().Be("some id");
    }

    [Fact]
    public void GetStringIdFromRoute_WhenValueIsNotString_ShouldReturnItsStringRepresentation()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = CreateRequest();
        request.RouteValues["id"] = id;

        // Act
        var outcome = HttpRequestReader.GetStringIdFromRoute(request);

        // Assert
        outcome.Should().Be(id.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetStringIdFromRoute_WhenIdIsMissingOrEmpty_ShouldReturnNull(string? id)
    {
        // Arrange
        var request = CreateRequest();
        if (id is not null) request.RouteValues["id"] = id;

        // Act
        var outcome = HttpRequestReader.GetStringIdFromRoute(request);

        // Assert
        outcome.Should().BeNull();
    }

    [Fact]
    public void GetStringIdFromQuery_WhenIdIsPresent_ShouldReturnIt()
    {
        // Arrange
        var request = CreateRequest();
        request.QueryString = new QueryString("?id=some%20id");

        // Act
        var outcome = HttpRequestReader.GetStringIdFromQuery(request);

        // Assert
        outcome.Should().Be("some id");
    }

    [Fact]
    public void GetStringIdFromQuery_WhenPropertyIsProvided_ShouldReturnThatProperty()
    {
        // Arrange
        var request = CreateRequest();
        request.QueryString = new QueryString("?id=wrong&workspaceId=workspace");

        // Act
        var outcome = HttpRequestReader.GetStringIdFromQuery(request, "workspaceId");

        // Assert
        outcome.Should().Be("workspace");
    }

    [Fact]
    public void GetStringIdFromQuery_WhenPropertyCasingIsDifferent_ShouldReturnIt()
    {
        // Arrange
        var request = CreateRequest();
        request.QueryString = new QueryString("?ID=some-id");

        // Act
        var outcome = HttpRequestReader.GetStringIdFromQuery(request);

        // Assert
        outcome.Should().Be("some-id");
    }

    [Theory]
    [InlineData("")]
    [InlineData("?id=")]
    [InlineData("?other=value")]
    public void GetStringIdFromQuery_WhenIdIsMissingOrEmpty_ShouldReturnNull(string query)
    {
        // Arrange
        var request = CreateRequest();
        request.QueryString = new QueryString(query);

        // Act
        var outcome = HttpRequestReader.GetStringIdFromQuery(request);

        // Assert
        outcome.Should().BeNull();
    }

    [Fact]
    public async Task GetStringIdFromBody_WhenIdIsPresent_ShouldReturnIt()
    {
        // Arrange
        var request = CreateRequest("""{ "name": "something", "id": "some id" }""");

        // Act
        var outcome = await HttpRequestReader.GetStringIdFromBody(request);

        // Assert
        outcome.Should().Be("some id");
    }

    [Fact]
    public async Task GetStringIdFromBody_WhenPropertyIsProvided_ShouldReturnThatProperty()
    {
        // Arrange
        var request = CreateRequest("""{ "id": "wrong id", "projectId": "project id" }""");

        // Act
        var outcome = await HttpRequestReader.GetStringIdFromBody(request, "projectId");

        // Assert
        outcome.Should().Be("project id");
    }

    [Theory]
    [InlineData("""{ "Id": "some id" }""")]
    [InlineData("""{ "ID": "some id" }""")]
    [InlineData("""{ "id": "first id", "Id": "some id" }""")]
    public async Task GetStringIdFromBody_WhenPropertyCasingIsDifferent_ShouldReturnIt(string body)
    {
        // Arrange
        // the model binding matches the property names case-insensitively, so the authorization has to do it too
        var request = CreateRequest(body);

        // Act
        var outcome = await HttpRequestReader.GetStringIdFromBody(request);

        // Assert
        outcome.Should().Be("some id");
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("""{ "name": "something" }""")]
    [InlineData("""{ "id": "" }""")]
    [InlineData("""{ "id": null }""")]
    [InlineData("""{ "id": 123 }""")]
    [InlineData("""{ "id": true }""")]
    [InlineData("""{ "id": ["some id"] }""")]
    [InlineData("""{ "id": { "value": "some id" } }""")]
    [InlineData("null")]
    [InlineData("[]")]
    [InlineData("\"some id\"")]
    public async Task GetStringIdFromBody_WhenThereIsNoStringId_ShouldReturnNull(string body)
    {
        // Arrange
        var request = CreateRequest(body);

        // Act
        var outcome = await HttpRequestReader.GetStringIdFromBody(request);

        // Assert
        outcome.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("not json")]
    [InlineData("""{ "id": """)]
    [InlineData("""{ "id": "some id" """)]
    public async Task GetStringIdFromBody_WhenBodyIsNotValidJson_ShouldReturnNull(string body)
    {
        // Arrange
        var request = CreateRequest(body);

        // Act
        var outcome = await HttpRequestReader.GetStringIdFromBody(request);

        // Assert
        outcome.Should().BeNull();
    }

    [Theory]
    [InlineData("""{ "id": "some id" }""")]
    [InlineData("""{ "name": "something" }""")]
    [InlineData("not json")]
    public async Task GetStringIdFromBody_WhenIsDone_ShouldLeaveTheBodyReadableFromTheStart(string body)
    {
        // Arrange
        var request = CreateRequest(body);

        // Act
        await HttpRequestReader.GetStringIdFromBody(request);

        // Assert
        request.Body.Position.Should().Be(0);
        (await ReadBody(request)).Should().Be(body);
    }
}
