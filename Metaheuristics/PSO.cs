namespace Optimizer.Metaheuristics
{
    using System.Collections.Generic;

    using Core;
    using Decorators;
    using Math;

    /// <summary>
    /// Particle Swarm Optimization (PSO) simulates social behavior of bird flocks or fish school.
    /// References:
    /// J. Kennedy, R. C. Eberhart and Y. Shi. Swarm intelligence. Artificial Intelligence (2001).
    /// </summary>
    public class PSO : Metaheuristic
    {

        /// <summary>
        /// Particle agents.
        /// </summary>
        public List<Particle> Particles { get; set; }

        /// <summary>
        /// Inertia weight.
        /// </summary>
        public double W { get; set; }

        /// <summary>
        /// Cognitive constant.
        /// </summary>
        public double C1 { get; set; }

        /// <summary>
        /// Social constant.
        /// </summary>
        public double C2 { get; set; }

        /// <summary>
        /// This constructor initializes the PSO.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public PSO(Dictionary<string, double> hyperparams = null, string algorithm = "PSO") : base(algorithm)
        {
            W = 0.7;
            C1 = 1.7;
            C2 = 1.7;

            Particles = new List<Particle>();

            // Now, we need to build this class up
            Build(hyperparams);
        }

        /// <summary>
        /// It decorates the agent.
        /// <param name="agents">List of agents.</param>
        /// </summary>
        public override void Decorate(List<Agent> agents)
        {
            foreach (Agent agent in agents)
            {
                this.Particles.Add(new Particle(agent));
            }
        }

        /// <summary>
        /// Updates a particle velocity (p. 295).
        /// <param name="particle">Particle agent.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// </summary>
        private void UpdateVelocity(Particle particle, IAgent bestAgent)
        {
            // Generating first random number
            double r1 = Stochastic.GenerateUniformRandomNumber();

            // Generating second random number
            double r2 = Stochastic.GenerateUniformRandomNumber();

            // Calculates new velocity
            for (int j = 0; j < particle.NumberVariables; j++)
            {
                particle.Velocity[j] = this.W * particle.Velocity[j] +
                                        this.C1 * r1 * (particle.BestPosition[j] - particle.Position[j]) +
                                        this.C2 * r2 * (bestAgent.Position[j] - particle.Position[j]);
            }
        }

        /// <summary>
        /// Updates a particle position (p. 294).
        /// <param name="particle">Particle agent.</param>
        /// </summary>
        private void UpdatePosition(Particle particle)
        {
            // Calculates new position
            for (int j = 0; j < particle.NumberVariables; j++)
            {
                particle.Position[j] = particle.Position[j] + particle.Velocity[j];
            }
        }

        /// <summary>
        /// Evaluates the search space according to the objective function.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object that will be used as the objective function.</param>
        /// <param name="localPosition">Array of local best positions.</param>
        /// </summary>
        public override Agent Evaluate(Agent bestAgent, Function function)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                double fit = function.Calculate(Particles[i].Position);

                if (fit < Particles[i].FitnessValue)
                {
                    Particles[i].FitnessValue = fit;

                    for (int j = 0; j < Particles[i].NumberVariables; j++)
                    {
                        Particles[i].BestPosition[j] = Particles[i].Position[j];
                    }
                }

                if (Particles[i].FitnessValue < bestAgent.FitnessValue)
                {
                    for (int j = 0; j < Particles[i].NumberVariables; j++)
                    {
                        bestAgent.Position[j] = Particles[i].BestPosition[j];
                    }

                    bestAgent.FitnessValue = Particles[i].FitnessValue;
                }
            }

            return bestAgent;
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            foreach (Particle particle in Particles)
            {
                // Updates current particle velocity
                UpdateVelocity(particle, space.BestAgent);

                // Updates current particle position
                UpdatePosition(particle);
            }

            // Checking if agents meet the bounds limits
            foreach (Particle particle in Particles)
            {
                particle.ClipLimits();
            }

            // After the update, we need to re-evaluate the search space
            space.BestAgent = Evaluate(space.BestAgent, function);
        }
    }
}