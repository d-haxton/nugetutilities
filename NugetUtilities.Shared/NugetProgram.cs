using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NugetUtilities.Shared
{
    public abstract class NugetProgram
    {
        public NuspecFile NuspecFile { get; private set; }
        public string UsageMessage { get; private set; }

        protected abstract void Execute(string[] args);

        protected NugetProgram(string usageMessage)
        {
            UsageMessage = usageMessage;
        }

        public void Run(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(UsageMessage);
                return;
            }
            var path = args[0];
            NuspecFile = NuspecFile.Load(new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), path)));

            var remainingArgs = args.Skip(1).ToArray();
            try
            {
                Execute(remainingArgs);

                NuspecFile.Save();
                Console.WriteLine("Successfully updated: " + NuspecFile.File.FullName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}