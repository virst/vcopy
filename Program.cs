using System;
using System.Collections.Generic;
using System.IO;


namespace vcopy
{
    class Program
    {
        private static readonly string BasePath;
        private static bool _f;

        static Program()
        {
            BasePath = new DirectoryInfo(".").FullName;
        }

        static bool ChBool(string name, string[] args)
        {
            for (int i = 2; i < args.Length; i++)
                if (string.Equals(args[i].TrimStart('-'), name, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            return false;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            bool x = ChBool("X", args);
            bool r = ChBool("R", args);
            _f = ChBool("F", args);

            DirectoryInfo d1, d2;

            if (args.Length == 1)
            {
                d1 = new DirectoryInfo(".");
                d2 = new DirectoryInfo(args[0]);
            }
            else
            {
                d1 = new DirectoryInfo(args[0]);
                d2 = new DirectoryInfo(args[1]);
            }

            if (File.Exists(args[0]))
            {
                if (d2.Exists)
                    Copy(new FilePair(args[0], Path.Combine(args[1], Path.GetFileName(args[0]))));
                else
                    Copy(new FilePair(args[0], args[1]));
                return;
            }

            List<DirectoryInfo> dis = new List<DirectoryInfo>();
            if (!x)
                dis.Add(d1);
            if (r)
                dis.AddRange(d1.GetDirectories("*", SearchOption.AllDirectories));

            List<FilePair> files = new List<FilePair>();
            foreach (var d in dis)
            {
                foreach (var fi in d.GetFiles())
                    files.Add(new FilePair(fi.FullName, Path.Combine(args[1], fi.Name)));
            }

            Console.WriteLine("Files to be copied - " + files.Count);

            foreach (var fp in files)
                Copy(fp);
        }

        static void Copy(FilePair fp)
        {
            try
            {
                Console.Write("copy {0} - ", fp.Fi1.FullName.Substring(BasePath.Length));
                File.Copy(fp.Fi1.FullName, fp.Fi2.FullName, _f);
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        class FilePair
        {
            public FilePair(string f1, string f2)
            {
                Fi1 = new FileInfo(f1);
                Fi2 = new FileInfo(f2);
            }
            public readonly FileInfo Fi1;
            public readonly FileInfo Fi2;
        }
    }
}




