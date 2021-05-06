namespace Sharptimizer.Tests.Spaces
{
    using Sharptimizer.Spaces;
    using Xunit;

    public class SearchTest
    {
        [Fact]
        public void Test_Search_InitializeAgents()
        {
            var newSearchSpace = new SearchSpace();

            Assert.True(newSearchSpace.Agents[0].Position[0] != 0);
        }

        [Fact]
        public void Test_Search_ClipLimits()
        {
            var newSearchSpace = new SearchSpace();

            newSearchSpace.Agents[0].Position[0] = 20;

            newSearchSpace.ClipLimits();

            Assert.True(newSearchSpace.Agents[0].Position[0] != 20);
        }
    }
}