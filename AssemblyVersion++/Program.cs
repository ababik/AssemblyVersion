using System;
using System.IO;
using System.Text;

namespace AssemblyVersion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = args[0];
            var now = DateTime.Now;
            var content = File.ReadAllText(path, Encoding.UTF8);
            var value = GetAttributeValue(content, "[assembly: AssemblyVersion(\"", "\")]");
            var version = Version.Parse(value);
            var build = Convert.ToInt32(string.Format("1{0}{1}", now.Month.ToString().PadLeft(2, '0'), now.Day.ToString().PadLeft(2, '0')));
            var revision = version.Revision + 1;
            version = new Version(version.Major, version.Minor, build, revision);
            value = version.ToString();
            content = ReplaceAttributeValue(content, "[assembly: AssemblyVersion(\"", "\")]", value);
            content = ReplaceAttributeValue(content, "[assembly: AssemblyFileVersion(\"", "\")]", value);
            File.WriteAllText(path, content);
        }

        private static string GetAttributeValue(string content, string start, string end)
        {
            var index1 = content.IndexOf(start) + start.Length;
            var index2 = content.IndexOf(end, index1);
            return content.Substring(index1, index2 - index1);
        }

        private static string ReplaceAttributeValue(string content, string start, string end, string version)
        {
            var index1 = content.IndexOf(start) + start.Length;
            var index2 = content.IndexOf(end, index1);
            content = content.Remove(index1, index2 - index1);
            content = content.Insert(index1, version);
            return content;
        }
    }
}
