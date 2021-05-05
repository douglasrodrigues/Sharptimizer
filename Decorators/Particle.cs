namespace Sharptimizer.Decorators
{
    using Core;

    public class Particle : Decorator
    {
        /// <summary>
        /// Particle's best position.
        /// </summary>
        public double[] BestPosition { get; set; }

        /// <summary>
        /// Particle's velocity.
        /// </summary>
        public double[] Velocity { get; set; }

        public Particle(IAgent agent) : base(agent)
        {
            BestPosition = new double[NumberVariables];

            Velocity = new double[NumberVariables];
        }
    }
}