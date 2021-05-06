namespace Sharptimizer
{
    using System;
    using System.Diagnostics;
    using Core;
    using Utils;

    public class Optimizer
    {
        public Space Space { get; set; }
        public Metaheuristic Metaheuristic { get; set; }
        public Function Function { get; set; }
        public Optimizer(Space space, Metaheuristic metaheuristic, Function function)
        {
            Space = space;
            Metaheuristic = metaheuristic;
            Function = function;
        }
        public History Start()
        {
            Stopwatch Watch = new Stopwatch();

            Watch.Start();

            dynamic history = Space.Run(Metaheuristic, Function);

            Watch.Stop();

            history.ElapsedTime = Watch.ElapsedMilliseconds;

            Console.WriteLine("Optimization task ended. It took {0} milliseconds.", Watch.ElapsedMilliseconds);

            return history;
        }
    }
}