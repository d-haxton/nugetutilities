using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NugetUtilities.Shared;

namespace NugetUtilities.UpdateReleaseNotes
{
    class Program : NugetProgram
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public Program() : base(@"Usage:
UpdateVersion Nuget\$nuspec.file releaseNotes
")
        {
        }

        protected override void Execute(string[] args)
        {
            var releaseNotes = args[0];
            var node = NuspecFile.Metadata.Elements().Single(x => x.Name.LocalName == "releaseNotes");
            node.Value = releaseNotes;
        }
    }
}
