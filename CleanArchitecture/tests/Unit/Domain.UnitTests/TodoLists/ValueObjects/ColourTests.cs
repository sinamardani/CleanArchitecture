using Domain.Commons.Exceptions;
using Domain.TodoLists.ValueObjects;
using FluentAssertions;

namespace Unit.Domain.TodoLists.ValueObjects;

public class ColourTests
{
    [Fact]
    public void From_WithValidCode_ShouldReturnColour()
    {
        var colour = Colour.From("#FFFFFF");

        colour.Should().NotBeNull();
        colour.Code.Should().Be("#FFFFFF");
    }

    [Fact]
    public void From_WithInvalidCode_ShouldThrowUnsupportedColourException()
    {
        var act = () => Colour.From("#INVALID");

        act.Should().Throw<UnsupportedColourException>()
            .WithMessage("Colour '#INVALID' is unsupported");
    }

    [Fact]
    public void White_ShouldReturnWhiteColour()
    {
        var colour = Colour.White;

        colour.Code.Should().Be("#FFFFFF");
    }

    [Fact]
    public void Red_ShouldReturnRedColour()
    {
        var colour = Colour.Red;

        colour.Code.Should().Be("#FF5733");
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnCode()
    {
        var colour = Colour.White;
        string code = colour;

        code.Should().Be("#FFFFFF");
    }

    [Fact]
    public void ExplicitOperator_FromString_ShouldReturnColour()
    {
        var colour = (Colour)"#FFFFFF";

        colour.Should().NotBeNull();
        colour.Code.Should().Be("#FFFFFF");
    }

    [Fact]
    public void ExplicitOperator_WithInvalidCode_ShouldThrowException()
    {
        var act = () => (Colour)"#INVALID";

        act.Should().Throw<UnsupportedColourException>();
    }

    [Fact]
    public void ToString_ShouldReturnCode()
    {
        var colour = Colour.White;

        colour.ToString().Should().Be("#FFFFFF");
    }

    [Fact]
    public void Equals_WithSameCode_ShouldReturnTrue()
    {
        var colour1 = Colour.White;
        var colour2 = Colour.White;

        colour1.Should().Be(colour2);
    }

    [Fact]
    public void Equals_WithDifferentCode_ShouldReturnFalse()
    {
        var colour1 = Colour.White;
        var colour2 = Colour.Red;

        colour1.Should().NotBe(colour2);
    }
}

