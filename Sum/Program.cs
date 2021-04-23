using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sum
{
    class Program
    {
        static void Main(string[] args)
        {
            SumFinding sumFinding = new SumFinding(76318276, new int[] { 14545, 18182, 10909, 16364, 22727, 24545, 23636, 21818 }, new float[] { 1500, 2800, 500, 500, 500, 350, 450, 300 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            Console.WriteLine("================================================================================");
            Console.WriteLine("Sum:" + sumFinding.Sum);
            for (int i = 0; i < sumFinding.A.Length; i++)
            {
                Console.WriteLine("A:" + sumFinding.A[i]);
                Console.WriteLine("MaxA:" + sumFinding.MaxA[i]);
                Console.WriteLine("MinA:" + sumFinding.MinA[i]);
            }

            Console.WriteLine("ucln:" + sumFinding.ucln);
            Console.WriteLine("sum:" + sumFinding.sum);
            for (int i = 0; i < sumFinding.A.Length; i++)
            {
                Console.WriteLine("a:" + sumFinding.a[i]);
                Console.WriteLine("maxA:" + sumFinding.maxA[i]);
                Console.WriteLine("minA:" + sumFinding.minA[i]);
            }

            Console.WriteLine("================================================================================");
            sumFinding.PrintResult();
            Console.WriteLine("================================================================================");
        }
    }
}

public class SumFinding
{
    public int Sum { get; private set; }
    public int[] A { get; private set; }
    public float[] MaxA { get; private set; }
    public float[] MinA { get; private set; }

    public int sum { get; private set; }
    public int[] a { get; private set; }
    public float[] maxA { get; private set; }
    public float[] minA { get; private set; }
    public int ucln { get; private set; }
    public List<int[]> results { get; private set; }

    private int[,] F;

    public SumFinding(int sum_, int[] a_, float[] maxA_, float[] minA_)
    {
        Sum = sum_;
        A = a_;
        MaxA = maxA_;
        MinA = minA_;

        int[] AA = new int[A.Length + 1];
        for (int i = 0; i < A.Length; i++)
        {
            AA[i] = A[i];
        }
        AA[A.Length] = Sum;

        ucln = FindUcln(AA);
        a = new int[A.Length];
        maxA = new float[A.Length];
        minA = new float[A.Length];
        for (int i = 0; i < A.Length; i++)
        {
            a[i] = AA[i];
            maxA[i] = MaxA[i] / ucln;
            minA[i] = MinA[i] / ucln;
        }
        sum = AA[A.Length];

        FindResult();
    }

    public int FindUcln(int[] n_)
    {
        int min = 0;
        for (int i = 1; i < n_.Length; i++)
        {
            min = n_[i] < n_[min] ? i : min;
        }

        int ucln = 1;
        for (int a = 2; a <= n_[min];)
        {
            bool uc = true;
            foreach (int x in n_)
            {
                if (x % a != 0)
                {
                    uc = false;
                    break;
                }
            }
            if (uc)
            {
                for (int i = 0; i < n_.Length; i++)
                {
                    n_[i] /= a;
                }
                ucln *= a;
            }
            else
            {
                a++;
            }
        }
        return ucln;
    }

    private void FindResult()
    {
        //F[n,m] la so cach chon Sum(A i-> n) = Sum
        //Khoi tao =========================================
        F = new int[A.Length, sum + 1];
        for (int n = 0; n < A.Length; n++)
        {
            if (0 >= minA[n])
            {
                F[n, 0] = 1;
            }
            else
            {
                break;
            }
        }
        for (int m = 1; m <= sum; m++)
        {
            if (m % a[0] == 0 && m / a[0] >= minA[0] && m / a[0] <= maxA[0])
            {
                F[0, m] = 1;
            }
            else
            {
                F[0, m] = 0;
            }
        }
        //Quy hoach ==========================================
        for (int n = 1; n < A.Length; n++)
        {
            for (int m = 1; m <= sum; m++)
            {
                int s = 0;
                for (int k = 0; m - k * a[n] >= 0; k++)
                {
                    if (k >= minA[n] && k <= maxA[n])
                    {
                        s += F[n - 1, m - k * a[n]];
                    }
                }
                F[n, m] = s;
            }
        }
        Console.WriteLine("to hop:" + F[A.Length - 1, sum]);
    }

    public void PrintResult()
    {
        results = new List<int[]>();
        int[] result = new int[A.Length];
        FindTrace(A.Length - 1, sum, result);
        Console.WriteLine("count trace:" + results.Count);
        foreach(int[] re in results)
        {
            string s = string.Empty;
            for(int i = 0; i < re.Length; i++)
            {
                s += re[i]*ucln + "*" + a[i]*ucln + " ";
            }
            s += "= " + sum * ucln;
            Console.WriteLine(s);
        }
    }

    private void FindTrace(int n, int m, int[] result)
    {
        if (n == 0 || m == 0)
        {
            if (n == 0)
            {
                result[n] = m / a[n];
            }
            int[] re = new int[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                re[i] = result[i];
            }
            results.Add(re);
            result[n] = 0;
        }
        else
        {
            for (int k = 0; m - k * a[n] >= 0; k++)
            {
                if (k >= minA[n] && k <= maxA[n] && F[n - 1, m - k * a[n]] > 0)
                {
                    result[n] = k;
                    FindTrace(n - 1, m - k * a[n], result);
                    result[n] = 0;
                }
            }
        }
    }
}
