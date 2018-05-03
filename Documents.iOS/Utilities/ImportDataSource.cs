using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Documents.iOS.Utilities
{
    public class ImportDataSource
    {
        public ImportDataSource()
        {
        }


        public List<string> GetDirectories() 
        {
            var directories = Directory.GetDirectories(Environment.SpecialFolder.MyDocuments.ToString(), "*", SearchOption.AllDirectories).ToList();

            return directories;
        }
    }
}
