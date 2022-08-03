using LiveSplit.Options;
using System;
using System.Linq;

namespace LiveSplit.Model.Comparisons
{
    public class SimpleDistributedComparisonGenerator : IComparisonGenerator
    {
        public IRun Run { get; set; }
        public const string ComparisonName = "Simple Distributed PB";
        public const string ShortComparisonName = "Simple Distributed";
        public string Name => ComparisonName;

        public SimpleDistributedComparisonGenerator(IRun run)
        {
            Run = run;
        }

        protected TimeSpan TimeSpanDivide(TimeSpan one, TimeSpan two)
        {
            double ticks1 = one.TotalSeconds;
            double ticks2 = two.TotalSeconds;

            double count = ticks1 / ticks2;

            return TimeSpan.FromSeconds(count);
        }

        protected TimeSpan TimeSpanMultiply(TimeSpan one, TimeSpan two)
        {
            double ticks1 = one.TotalSeconds;
            double ticks2 = two.TotalSeconds;

            double count = ticks1 * ticks2;

            return TimeSpan.FromSeconds(count);
        }

        protected TimeSpan MathStuff(TimeSpan segmentGold, TimeSpan personalBest, TimeSpan sumOfBest)
        {
            TimeSpan pbminussob = personalBest - sumOfBest;
            TimeSpan timesave = TimeSpanMultiply(TimeSpanDivide(segmentGold, sumOfBest), pbminussob);

            return segmentGold + timesave;
        }

        public void Generate(TimingMethod timingMethod)
        {
            TimeSpan? PersonalBest = Run.Last().Comparisons["Personal Best"][timingMethod];

            TimeSpan? SumOfBest = Run.Last().Comparisons["Best Segments"][timingMethod];

            TimeSpan LastValue = TimeSpan.Zero;

            if (PersonalBest.HasValue && SumOfBest.HasValue)
            {
                foreach (var segment in Run)
                {
                    TimeSpan? bestSegment = timingMethod == TimingMethod.RealTime ? segment.BestSegmentTime.RealTime : segment.BestSegmentTime.GameTime;
                    if (bestSegment.HasValue)
                    {
                        TimeSpan Calculated = MathStuff(bestSegment.Value, PersonalBest.Value, SumOfBest.Value);

                        LastValue += Calculated;

                        segment.Comparisons[Name] = timingMethod == TimingMethod.RealTime ? new Time(realTime: LastValue) : new Time(gameTime: LastValue);
                    }
                }
            }
        }

        public void Generate(ISettings settings)
        {
            Generate(TimingMethod.RealTime);
            Generate(TimingMethod.GameTime);
        }
    }
}
