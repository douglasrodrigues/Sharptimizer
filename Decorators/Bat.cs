namespace Optimizer.Decorators
{
    using Core;

    public class Bat : Decorator
    {
        /// <summary>
        /// Bat's velocity.
        /// </summary>
        public double[] Velocity { get; set; }

        public Bat(IAgent agent)
        : base(agent)
        {
            Velocity = new double[NumberVariables];
        }
    }
}