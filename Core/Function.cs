namespace Sharptimizer.Core
{
    using System;

    /// <summary>
    /// A Function class for using with objective functions that will be further evaluated.
    /// It serves as the basis class for holding in-code related objective functions.
    /// </summary>
    public class Function
    {
        public Func<double[], double> Calculate { get; set; }

        public string Name
        {
            get { return Calculate.Method.Name; }
        }

        public bool Built { get; set; }

        public Function(Func<double[], double> pointer)
        {
            Calculate = pointer;
        }
    }
}