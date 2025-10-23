using CloudCalculusCalculator.Math;
namespace CloudCalculusCalculator.Tests;

public class MultiplicationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Is3Times4EqualTo12()
    {
    
        string testJSON = "[\"Equal\", \"x\", [\"Multiply\", 3, 4]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "12");
    }

    [Test]
    public void Is3Times0EqualTo0()
    {

        string testJSON = "[\"Equal\", \"x\", [\"Multiply\", 3, 0]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "0");
    }

    [Test]
    public void Is3TimesNegative4EqualToNegative12()
    {

        string testJSON = "[\"Equal\", \"x\", [\"Multiply\", 3, -4]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "-12");
    }

    [Test]
    public void IsNegative3TimesNegative4EqualTo12()
    {

        string testJSON = "[\"Equal\", \"x\", [\"Multiply\", -3, -4]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "12");
    }

    [Test]
    public void Is3TimesHalfEqualTo1Point5()
    {

        string testJSON = "[\"Equal\", \"x\", [\"Multiply\", 3, 0.5]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "1.5");
    }

    [Test]
    public void Is3Times1AndAHalfEqualTo4Point5()
    {

        string testJSON = "[\"Equal\", \"x\", [\"Multiply\", 3, 1.5]]";
        Assert.That(ParsedMathFactory.Create(testJSON).GetDisplaySolution() == "4.5");
    }



}
