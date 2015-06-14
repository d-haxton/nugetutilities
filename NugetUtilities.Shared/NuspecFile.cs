using System.IO;
using System.Xml.Linq;

namespace NugetUtilities.Shared
{
    public class NuspecFile
    {
        private FileInfo file;
        private XDocument document;
        private XElement package;
        private XElement metadata;

        private NuspecFile(FileInfo file)
        {
            this.file = file;
            document = XDocument.Parse(System.IO.File.ReadAllText(file.FullName), LoadOptions.PreserveWhitespace);
            package = document.Root;
            metadata = package.Element("metadata");
        }

        public FileInfo File
        {
            get { return file; }
        }

        public XDocument Document
        {
            get { return document; }
        }

        public XElement Package
        {
            get { return package; }
        }

        public XElement Metadata
        {
            get { return metadata; }
        }

        public static NuspecFile Load(FileInfo file)
        {
            return new NuspecFile(file);
        }

        public void Save()
        {
            System.IO.File.WriteAllText(file.FullName, document.ToString(SaveOptions.DisableFormatting));            
        }
    }
}