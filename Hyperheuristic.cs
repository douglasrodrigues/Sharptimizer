namespace Optimizer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Core;
    using Utils;

    public class Hyperheuristic
    {
        public Space Space { get; set; }
        public List<Metaheuristic> Metaheuristics { get; set; }
        public Function Function { get; set; }
        public Hyperheuristic(Space space, List<Metaheuristic> metaheuristics, Function function)
        {
            Space = space;
            Metaheuristics = metaheuristics;
            Function = function;
        }
        public History Start()
        {
            Stopwatch Watch = new Stopwatch();

            Watch.Start();

            dynamic history = Space.Run(Metaheuristics, Function);

            Watch.Stop();

            history.ElapsedTime = Watch.ElapsedMilliseconds;

            Console.WriteLine("Optimization task ended. It took {0} milliseconds.", Watch.ElapsedMilliseconds);

            return history;
        }
    }
}