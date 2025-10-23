using CloudCalculusCalculator.Math;
namespace CloudCalculusCalculator.Tests;

public class SubtractionTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Is2Minus1EqualTo1()
    {
        string testJSON = "[\"Equal\", \"x\", [\"Add\", -1, 2]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "1");
    }

    [Test]
    public void Is1Minus1EqualTo0()
    {
        string testJSON = "[\"Equal\", \"x\", [\"Add\", -1, 1]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "0");
    }

    [Test]
    public void Is1Minus0Point5EqualTo0Point5()
    {
        string testJSON = "[\"Equal\", \"x\", [\"Add\", -0.5, 1]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "0.5");
    }

}
