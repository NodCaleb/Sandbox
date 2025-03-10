using MathNet.Numerics.RootFinding;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Sandbox.ProbabilityDistribution
{
    internal class Program
    {
        static double _targetRtp = 0.97;

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var games = JsonSerializer.Deserialize<double[][]>(config["Games"]);

            Console.WriteLine($"Target RTP: {_targetRtp.ToString("P2")}");

            //Calculate the weights configuration for each game setup by adjusting the probability distribution steepness
            foreach (var game in games)
            {
                Console.WriteLine();
                CalculateConfig(game, _targetRtp);
            }
        }

        static void CalculateConfig(double[] buckets, double targetRtp)
        {
            //Find the maximum steepness that will result in an RTP lesser than the target RTP
            double maxSteepness = 0;
            while (CalculateRtp(buckets, maxSteepness) > targetRtp) maxSteepness++;

            //Find the steepness that will result in an RTP equal to the target RTP using Brent's method
            Func<double, double> f = x => CalculateRtp(buckets, x);
            var targetSteepness = Brent.FindRoot(x => f(x) - targetRtp, 0, maxSteepness);

            //Calcualte the weights using the target steepness
            var weights = CalculateWeights(buckets, targetSteepness);

            //Make the weights integers and adjust them to match the target RTP
            var refinedWeights = RefineWeights(weights);
            var adjustedWeights = AdjustWeights(buckets, refinedWeights, targetRtp);

            //Print the results
            Console.WriteLine($"Buckets: {string.Join(", ", buckets)}");
            Console.WriteLine($"Weights: {string.Join(", ", adjustedWeights)}");
            Console.WriteLine($"Expected RTP: {CalculateRtp(buckets, adjustedWeights).ToString("P2")}");
        }

        static double[] CalculateArguments(double[] multipliers)
        {
            var arguments = new double[multipliers.Length];

            var maxIndex = multipliers.Length - 1;
            var midIndex = maxIndex / 2;

            for (int i = 0; i < multipliers.Length; i++)
            {
                arguments[i] = (double)(i - midIndex - (i > midIndex ? maxIndex % 2 : 0)) / midIndex;
            }

            return arguments;
        }

        static double[] CalculateWeights(double[] multipliers, double steepness)
        {
            var weights = new double[multipliers.Length];
            var arguments = CalculateArguments(multipliers);
            for (int i = 0; i < multipliers.Length; i++)
            {
                weights[i] = Math.Pow(Math.E, -Math.Pow(arguments[i], 2) * steepness);
            }
            return weights;
        }

        static double CalculateRtp(double[] multipliers, double steepness)
        {
            var weights = CalculateWeights(multipliers, steepness);

            return CalculateRtp(multipliers, weights);
        }

        static double CalculateRtp(double[] multipliers, double[] weights)
        {
            double expectation = 0;

            for (int i = 0; i < multipliers.Length; i++)
            {
                expectation += multipliers[i] * weights[i];
            }

            return expectation / weights.Sum();
        }

        static double[] RefineWeights(double[] weights)
        {
            var refinedWeights = new double[weights.Length];
            var minWeight = weights.Min();
            for (int i = 0; i < weights.Length; i++)
            {
                refinedWeights[i] = Math.Round(weights[i] / minWeight);
            }
            return refinedWeights;
        }

        static double[] AdjustWeights(double[] multipliers, double[] weights, double targetRtp)
        {
            var maxIndex = multipliers.Length - 1;
            var midIndex = maxIndex / 2;

            while (CalculateRtp(multipliers, weights) > targetRtp) weights[midIndex]++;

            while (CalculateRtp(multipliers, weights) < targetRtp) weights[midIndex]--;

            var weight0 = weights[midIndex];
            var rtp0 = CalculateRtp(multipliers, weights);

            weights[midIndex]++;

            var weight1 = weights[midIndex];
            var rtp1 = CalculateRtp(multipliers, weights);

            var midWeight = LinearInterpolation(rtp0, weight0, rtp1, weight1, targetRtp);

            if (multipliers.Length % 2 == 0)
            {
                midWeight = (midWeight + weights[midIndex + 1]) / 2;

                weights[midIndex] = midWeight;
                weights[midIndex + 1] = midWeight;
            }
            else
            {
                weights[midIndex] = midWeight;
            }

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = Math.Round(weights[i] * 100);
            }

            var gcd = weights.Select(w => (long)w).Aggregate(GCD);

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] /= gcd;
            }

            return weights;
        }

        static double LinearInterpolation(double x0, double y0, double x1, double y1, double x)
        {
            return y0 + (y1 - y0) / (x1 - x0) * (x - x0);
        }

        static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a); // Ensure positive GCD
        }
    }
}
