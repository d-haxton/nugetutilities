using System.Linq;

namespace NugetUtilities.UpdateVersion
{
    public class Version
    {
        private int[] digits;

        public Version(params int[] digits)
        {
            this.digits = digits.ToArray();
        }

        public static Version Parse(string s)
        {
            var digits = s.Split('.').Select(x => int.Parse(x)).ToArray();
            return new Version(digits);
        }

        public override string ToString()
        {
            return string.Join(".", digits);
        }

        public int this[int index]
        {
            get { return digits[index]; }
            set { digits[index] = value; }
        }

        public int Length
        {
            get { return digits.Length; }
        }
    }
}
