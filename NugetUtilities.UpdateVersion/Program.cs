using System;
using System.Linq;
using NugetUtilities.Shared;

namespace NugetUtilities.UpdateVersion
{
    class Program : NugetProgram
    {
        public static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public Program() : base(@"Usage:

UpdateVersion Nuget\$nuspec.file [versionDigitToIncrement = 0]

Where versionDigitToIncrement is an optional numeric value that defaults 
to zero indicating which digit to increment for a version string such as
1.2.3.  0 will increment 3, 1 will increment 2, etc.  Example:

UpdateVersion Nuget\MyProject.nuspec 1

Will increment the version element from 1.2.3 to 1.3.3
")
        {
        }

        protected override void Execute(string[] args)
        {
            var mode = args[0];
            var remainingArgs = args.Skip(1).ToArray();
            switch (mode)
            {
                case "-Increment":
                    IncrementVersion(remainingArgs);
                    break;
                case "-SetVersion":
                    SetVersion(remainingArgs);
                    break;
                default:
                    Console.WriteLine("Unexpected mode: " + mode);
                    break;
            }
        }

        private void IncrementVersion(string[] args)
        {
            int versionDigitToIncrement;
            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out versionDigitToIncrement))
                {
                    throw new Exception("The second argument should be an integer representing the digit to increment.");
                }
            }
            else
            {
                versionDigitToIncrement = 0;
            }

            Console.WriteLine("Incrementing version in: " + NuspecFile.File.FullName);

            var version = NuspecFile.Metadata.Element("version");
            var value = Version.Parse(version.Value);
            Console.WriteLine("Found version: " + value);
            value[value.Length - 1 - versionDigitToIncrement]++;
            Console.WriteLine("Changed version to: " + value);
            version.Value = value.ToString();
        }

        private void SetVersion(string[] args)
        {
            string newVersion = args[0];

            Console.WriteLine("Incrementing version in: " + NuspecFile.File.FullName);

            var version = NuspecFile.Metadata.Element("version");
            Console.WriteLine("Changed version to: " + newVersion);
            version.Value = newVersion;
        }
    }
}