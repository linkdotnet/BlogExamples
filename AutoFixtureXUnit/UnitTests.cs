using System.Diagnostics;
using AutoFixture.Xunit2;

namespace AutoFixtureXUnit;

public class UnitTest1
{
    [Theory]
    [AutoData]
    public void TestOne(string name)
    {
        var sut = new MyFooService();

        var result = sut.MyCall(name);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineAutoData]
    [InlineAutoData(31)]
    [InlineAutoData(31, "Steven")]
    public void TestTwo(int age, string name)
    {
        var sut = new MyFooService();

        var result = sut.MyCall(name, age);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineAutoData]
    public void TestThree(string name, MyFooService sut)
    {
        var result = sut.MyCall(name);

        Assert.NotNull(result);
    }

}

public class MyFooService
{
    public string MyCall(string name) => name;
    public string MyCall(string name, int age) => name + age;
}