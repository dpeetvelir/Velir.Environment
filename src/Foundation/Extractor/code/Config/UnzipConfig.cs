using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velir.Environment.Foundation.Extractor
{
    public class UnzipConfig
    {
        public string SourceZipPath { get; set; }
        public string DestinationRootPath { get; set; }
        public List<ZipContentMappings>  PathsToCopy { get; set; }
    }
}
