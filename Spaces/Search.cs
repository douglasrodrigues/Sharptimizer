namespace Optimizer.Spaces
{
    using System;
    using System.Linq;
    using Core;

    public class SearchSpace : Space
    {
        public Random Rand;
        public SearchSpace(int numberAgents = 1,
                           int numberVariables = 1,
                           int numberIterations = 10,
                           double[] lowerBound = null,
                           double[] upperBound = null) : base(numberAgents,
                                                           numberVariables,
                                                           numberIterations)
        {
            Rand = new Random();
            Build(lowerBound, upperBound);
            InitializeAgents();
        }

        public override void InitializeAgents()
        {
            foreach (Agent agent in Agents)
            {
                var bounds = LowerBound.Zip(UpperBound, (n, w) => new { Lower = n, Upper = w });
                foreach (var (bound, index) in bounds.Select((v, i) => (v, i)))
                {
                    agent.Position[index] = (bound.Upper - bound.Lower) * Rand.NextDouble() + bound.Lower;
                    agent.LowerBound[index] = bound.Lower;
                    agent.UpperBound[index] = bound.Upper;
                }
            }
        }

        public override void ClipLimits()
        {
            foreach (Agent agent in Agents)
            {
                var bounds = LowerBound.Zip(UpperBound, (n, w) => new { Lower = n, Upper = w });
                foreach (var (bound, index) in bounds.Select((v, i) => (v, i)))
                {
                    agent.Position[index] = Math.Clamp(agent.Position[index], bound.Lower, bound.Upper);
                }
            }
        }
    }
}