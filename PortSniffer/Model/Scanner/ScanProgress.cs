using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSniffer.Model.Scanner
{
    public class ScanProgress
    {
        private long totalScans;
        private long completedScans;
        private double progressPercentage;

        public ScanProgress(long totalScans, long completedScans, double progressPercentage)
        {
            this.totalScans = totalScans;
            this.completedScans = completedScans;
            this.progressPercentage = progressPercentage;
        }

        public override string? ToString()
        {
            return $"{progressPercentage:F2}%  ({completedScans}/{totalScans})"; //F = fixed-point format, 2 = two decimal places
        }
    }
}
