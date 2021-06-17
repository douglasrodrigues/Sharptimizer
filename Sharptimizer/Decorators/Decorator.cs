namespace Sharptimizer.Decorators
{
    using Core;

    //Base Decorator
    public abstract class Decorator : IAgent
    {
        private readonly IAgent _agent;

        public Decorator(IAgent agent)
        {
            _agent = agent;
        }

        #region Getters and Setters

        /// <summary>
        /// Number of decision variables.
        /// </summary>
        public virtual int NumberVariables
        {
            get { return _agent.NumberVariables; }
            set { _agent.NumberVariables = value; }
        }

        /// <summary>
        /// Value of the fitness function.
        /// </summary>
        public virtual double FitnessValue
        {
            get { return _agent.FitnessValue; }
            set { _agent.FitnessValue = value; }
        }

        /// <summary>
        /// Agent's position in the search space.
        /// </summary>
        public virtual double[] Position
        {
            get { return _agent.Position; }
            set { _agent.Position = value; }
        }

        /// <summary>
        /// Decision variables lower bound.
        /// </summary>
        public virtual double[] LowerBound
        {
            get { return _agent.LowerBound; }
            set { _agent.LowerBound = value; }
        }

        /// <summary>
        /// Decision variables upper bound.
        /// </summary>
        public virtual double[] UpperBound
        {
            get { return _agent.UpperBound; }
            set { _agent.UpperBound = value; }
        }

        #endregion

        public void ClipByBounds()
        {
            _agent.ClipByBounds();
        }

        public object Clone()
        {
            return _agent.Clone();
        }
    }
}