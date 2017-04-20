using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velir.Environment.Foundation.Extractor
{
    public static class Runner
    {
        public static void Run(UnzipConfig c)
        {
            Extractor e = new Extractor(c);
            e.Extract();
        }
    }
}
