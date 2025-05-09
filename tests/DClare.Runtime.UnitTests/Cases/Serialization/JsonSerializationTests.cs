using DClare.Runtime.Integration.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Neuroglia.Serialization;
using Neuroglia.Serialization.Json;

namespace DClare.Runtime.UnitTests.Cases.Serialization;

public class JsonSerializationTests
{

    public JsonSerializationTests()
    {
        Serializer = new JsonSerializer(Options.Create(JsonSerializer.DefaultOptions));
    }

    protected IJsonSerializer Serializer { get; }

    [Fact]
    public void SerializeDeserialize_MessageFragment_Should_Work()
    {
        //arrange
        var toSerialize = new MessageFragment()
        {
            Parts =
            [
                new AnnotationFragmentPart
                {
                    Quote = "Fake Quote"
                },
                new BinaryFragmentPart
                {
                    Uri = new("https://fake.com")
                },
                new TextFragmentPart
                {
                    Text = "Fake Text"
                }
            ]
        };

        //act
        var serialized = Serializer.SerializeToText(toSerialize);
        var deserialized = Serializer.Deserialize<MessageFragment>(serialized);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void SerializeDeserialize_Message_Should_Work()
    {
        //arrange
        var toSerialize = new Message()
        {
            Role = "assistant",
            Parts =
            [
               new AnnotationPart
                {
                    Quote = "Fake Quote"
                },
                new BinaryPart
                {
                    Uri = new("https://fake.com")
                },
                new TextPart
                {
                    Text = "Fake Text"
                }
            ]
        };

        //act
        var serialized = Serializer.SerializeToText(toSerialize);
        var deserialized = Serializer.Deserialize<Message>(serialized);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

}
