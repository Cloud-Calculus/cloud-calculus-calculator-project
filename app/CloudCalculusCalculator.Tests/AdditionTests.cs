using CloudCalculusCalculator.Math;
namespace CloudCalculusCalculator.Tests;

public class AdditionTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Is1Plus1EqualTo2()
    {
        string testJSON = "[\"Equal\", \"x\", [\"Add\", 1, 1]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "2");
    }

    [Test]
    public void IsNegative1Plus1EqualTo0()
    {
        string testJSON = "[\"Equal\", \"x\", [\"Add\", -1, 1]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "0");
    }
}
