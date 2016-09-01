using System;
using System.Diagnostics;

namespace Mirapp
{
    public class GameResultCalculation
    {
        public static Stopwatch ElapsedStropWatch=new Stopwatch();

        public static int TryCount { get; set; }
        public static int SuccessCount { get; set; }

        public static decimal SuccessPercentage
        {
            get
            {
                try
                {
                    if (TryCount == 0)
                    {
                        return 0;
                    }
                    return 100 * Math.Round(Convert.ToDecimal(SuccessCount) / Convert.ToDecimal(TryCount), 2);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static string Result => $"Try : {TryCount} Second:{ElapsedStropWatch.ElapsedMilliseconds/1000}  Sucess: %{SuccessPercentage} ";

        public static string ResultInGame (int ListCount) => String.Format("%{0} Success  {1} Words Remained", SuccessPercentage, ListCount);
    }
}