namespace Optimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Decorators;
    using Math;

    /// <summary>
    /// Bat Algorithm - 
    /// References:
    /// X.-S. Yang. A new metaheuristic bat-inspired algorithm.
    /// Nature inspired cooperative strategies for optimization (2010).
    /// </summary>
    public class BA : Metaheuristic
    {
        /// <summary>
        /// List of agents.
        /// </summary>
        public List<Bat> Bats { get; set; }

        // Minimum frequency range
        public double f_min { get; set; }

        // Maximum frequency range
        public double f_max { get; set; }

        // Loudness parameter
        public double A { get; set; }

        // Pulse rate
        public double r { get; set; }

        // Array of frequencies
        public double[] frequency { get; set; }

        // Array of loudness
        public double[] loudness { get; set; }

        // Array of pulse rate
        public double[] pulse_rate { get; set; }

        /// <summary>
        /// This constructor initializes the BA.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>        
        public BA(Dictionary<string, double> hyperparams = null, string algorithm = "BA") : base(algorithm)
        {
            f_min = 0;
            f_max = 2;
            A = 0.5;
            r = 0.5;

            Bats = new List<Bat>();

            // Now, we need to build this class up
            Build(hyperparams);
        }

        /// <summary>
        /// It decorates the agent.
        /// <param name="agents">List of agents.</param>
        /// </summary>
        public override void Decorate(List<Agent> agents)
        {
            foreach (IAgent agent in agents)
            {
                this.Bats.Add(new Bat(agent));
            }

            frequency = new double[Bats.Count];

            loudness = new double[Bats.Count];

            pulse_rate = new double[Bats.Count];

            for (int i = 0; i < Bats.Count; i++)
            {
                // to generate random numbers for agents frequency
                frequency[i] = Stochastic.GenerateUniformRandomNumber(this.f_min, this.f_max);

                // random uniform [0, A]                
                loudness[i] = Stochastic.GenerateUniformRandomNumber(0, this.A);

                // random uniform [0, r]
                pulse_rate[i] = Stochastic.GenerateUniformRandomNumber(0, this.r);
            }
        }

        /// <summary>
        /// Updates an agent frequency (eq. 2).
        /// <param name="f_min">Minimum frequency range.</param>
        /// <param name="f_max">Maximum frequency range.</param>
        /// <returns>A new frequency.</returns>
        /// </summary>
        private double UpdateFrequency(double f_min, double f_max)
        {
            double new_frequency;

            // Generating beta random number
            double beta = Stochastic.GenerateUniformRandomNumber();

            // Calculating new frequency
            // Note that we have to apply (min - max) instead of (max - min) or it will not converge
            new_frequency = f_min + (f_min - f_max) * beta;

            return new_frequency;
        }

        /// <summary>
        /// Updates a bat velocity (eq. 3).
        /// <param name="bat">Bat agent.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="frequency">Agent's frequency.</param>
        /// </summary>
        private void UpdateVelocity(Bat bat, Agent bestAgent, double frequency)
        {
            // Calculates new velocity
            for (int j = 0; j < bat.NumberVariables; j++)
            {
                bat.Velocity[j] = bat.Velocity[j] + (bat.Position[j] - bestAgent.Position[j]) * frequency;
            }
        }

        /// <summary>
        /// Updates a particle position (p. 294).
        /// <param name="bat">Bat agent.</param>
        /// </summary>
        private void UpdatePosition(Bat bat)
        {
            // Calculates new position
            for (int j = 0; j < bat.NumberVariables; j++)
            {
                bat.Position[j] = bat.Position[j] + bat.Velocity[j];
            }
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Declaring alpha constant
            double alpha = 0.9;

            // Iterate through all agents
            for (int i = 0; i < Bats.Count; i++)
            {
                // Updating frequency
                frequency[i] = UpdateFrequency(f_min, f_max);

                // Updating velocity
                UpdateVelocity(Bats[i], space.BestAgent, frequency[i]);

                // Updating agent's position
                UpdatePosition(Bats[i]);

                // Generating a random probability
                double p = Stochastic.GenerateIntegerRandomNumber();

                // Creates a random uniform number
                double r1 = Stochastic.GenerateUniformRandomNumber();

                // Creates a random uniform number
                double r2 = Stochastic.GenerateUniformRandomNumber();

                // Generating a random number
                double e = Math.Sqrt(-2.0 * Math.Log(1.0 - r1)) * Math.Sin(2.0 * Math.PI * (1.0 - r2));

                // # Check if probability is bigger than current pulse rate
                if (p > pulse_rate[i])
                {
                    // Performing a local random walk (eq. 5)
                    // We apply 0.001 to limit the step size
                    for (int j = 0; j < Bats[i].NumberVariables; j++)
                    {
                        Bats[i].Position[j] = space.BestAgent.Position[j] + 0.001 * e * loudness.Average();
                    }
                }

                // Checks agent limits
                Bats[i].ClipLimits();

                // Evaluates agent
                Bats[i].FitnessValue = function.Calculate(Bats[i].Position);

                // Checks if probability is smaller than loudness and if fit is better
                if ((p < loudness[i]) && (Bats[i].FitnessValue < space.BestAgent.FitnessValue))
                {
                    // Copying the new solution to space's best agent
                    space.BestAgent = (Agent)Bats[i].Clone();

                    // Increasing pulse rate (eq. 6)
                    pulse_rate[i] = r * (1 - Math.Exp(-alpha * iteration));

                    // Decreasing loudness (eq. 6)
                    loudness[i] = this.A * alpha;
                }
            }
        }
    }
}