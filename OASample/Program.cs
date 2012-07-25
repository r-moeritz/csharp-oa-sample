using System;
using System.Globalization;
using OASample.DynamicProxy;

namespace OASample
{
    // Initial object algebra interface for expressions: integers and addition
    interface IExpAlg<E>
    {
        E Lit(int x);
        E Add(E e1, E e2);
    }

    // ----------------------------------------------------------------------
    //       An object algebra implementing that interface (evaluation)
    // ----------------------------------------------------------------------

    // The evaluation interface
    public interface IEval
    {
        int Eval();
    }

    // The object algebra
    class EvalExpAlg : IExpAlg<IEval>
    {
        public IEval Lit(int x)
        {
            Func<int> f = () => x;
            return DelegateWrapper.WrapAs<IEval>(f);
        }

        public IEval Add(IEval e1, IEval e2)
        {
            Func<int> f = () => e1.Eval() + e2.Eval();
            return DelegateWrapper.WrapAs<IEval>(f);
        }
    }

    // ----------------------------------------------------------------------
    //                    Evolution 1: Adding subtraction
    // ----------------------------------------------------------------------

    interface ISubExpAlg<E> : IExpAlg<E>
    {
        E Sub(E e1, E e2);
    }

    // Updating evaluation:
    class EvalSubExpAlg : EvalExpAlg, ISubExpAlg<IEval>
    {
        public IEval Sub(IEval e1, IEval e2)
        {
            Func<int> f = () => e1.Eval() - e2.Eval();
            return DelegateWrapper.WrapAs<IEval>(f);
        }
    }

    // ----------------------------------------------------------------------
    //                  Evolution 2: Adding pretty printing
    // ----------------------------------------------------------------------

    public interface IPPrint
    {
        string Print();
    }

    class PrintExpAlg : ISubExpAlg<IPPrint>
    {
        public IPPrint Lit(int x)
        {
            Func<string> f = () => x.ToString(CultureInfo.InvariantCulture);
            return DelegateWrapper.WrapAs<IPPrint>(f);
        }

        public IPPrint Add(IPPrint e1, IPPrint e2)
        {
            Func<string> f = () => e1.Print() + " + " + e2.Print();
            return DelegateWrapper.WrapAs<IPPrint>(f);
        }

        public IPPrint Sub(IPPrint e1, IPPrint e2)
        {
            Func<string> f = () => e1.Print() + " - " + e2.Print();
            return DelegateWrapper.WrapAs<IPPrint>(f);
        }
    }

    // An alternative object algebra for pretty printing:

    // Often, when precise control over the invocation of 
    // methods is not needed, we can simplify object algebras. 
    // For example, here's an alternative implementation 
    // of pretty printing that directly computes a string:
    class PrintExpAlg2 : ISubExpAlg<string>
    {
        public string Lit(int x)
        {
            return x.ToString(CultureInfo.InvariantCulture);
        }

        public string Add(string e1, string e2)
        {
            return e1 + " + " + e2;
        }

        public string Sub(string e1, string e2)
        {
            return e1 + " - " + e2;
        }
    }

    // ----------------------------------------------------------------------
    //                                Testing
    // ----------------------------------------------------------------------

    class Program
    {
        // An expression using the basic ExpAlg
        private static E Exp1<E>(IExpAlg<E> alg)
        {
            return alg.Add(alg.Lit(3), alg.Lit(4));
        }

        // An expression using subtraction too
        private static E Exp2<E>(ISubExpAlg<E> alg)
        {
            return alg.Sub(Exp1(alg), alg.Lit(4));
        }

        private static void Main()
        {
            // Some object algebras:
            var ea = new EvalExpAlg();
            var esa = new EvalSubExpAlg();
            var pa = new PrintExpAlg();
            var pa2 = new PrintExpAlg2();

            // We can call esa with Exp1
            var ev = Exp1(esa);

            // But calling ea with Exp2 is an error
            // var ev_bad = Exp2(ea);

            // Testing the actual algebras
            Console.WriteLine("Evaluation of Exp1 '{0}' is: {1}", Exp1(pa).Print(), ev.Eval());
            Console.WriteLine("Evaluation of Exp2 '{0}' is: {1}", Exp2(pa).Print(), Exp2(esa).Eval());
            Console.WriteLine("The alternative pretty printer works nicely too!{0}Exp1: {1}{0}Exp2: {2}",
                              Environment.NewLine, Exp1(pa2), Exp2(pa2));
        }
    }
}