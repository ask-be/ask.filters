using ASK.Filters.Operations;
using FluentAssertions;

namespace ASK.Filters.Tests;

public record User(string FirstName, string LastName);

public enum TestEnum
{
    Val1,Val2,Val3
}

public class FilterParserTests
{
    [Fact]
    public void CanParseAnd()
    {
        var o = new FilterOptions([
            new FilterProperty<string>("firstname"),
            new FilterProperty<string>("lastname")
        ]);

        var andQuery = "and eq firstname John eq lastname Doe";

        var filter = new FilterPolishNotationParser(o).Parse(andQuery);

        filter.Operation.Should().BeAssignableTo<AndOperation>();


        var expression = FilterEvaluator<User>.Default.GetExpression(filter);
    }

    [Fact]
    public void CanParseWithCustomConverter()
    {
        var o = new FilterOptions([
            new FilterProperty<TestEnum>("enu")
        ]);
        o.AddConverter(Enum.Parse<TestEnum>);

        var filter = new FilterPolishNotationParser(o).Parse("eq enu Val1");
        filter.Operation.Should().BeAssignableTo<EqualOperation>();

        ((EqualOperation)filter.Operation).Value.Should().Be(TestEnum.Val1);

    }
}