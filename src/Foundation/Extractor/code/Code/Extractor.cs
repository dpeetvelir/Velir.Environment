using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Velir.Environment.Foundation.Extractor
{
    internal class Extractor
    {
        private UnzipConfig _config;

        public Extractor(UnzipConfig config)
        {
            _config = config;
        }

        public bool Extract()
        {
            string tempPath = Path.GetTempPath();
            string fullTempFolder = Path.Combine(tempPath, "VelirExtractor");

            try
            {
                if (!Directory.Exists(fullTempFolder))
                {
                    Directory.CreateDirectory(fullTempFolder);
                }

                FileInfo sourceFileInfo = new FileInfo(_config.SourceZipPath);
                if (!sourceFileInfo.Exists)
                {
                    return false;
                }

                string localZipPath = Path.Combine(fullTempFolder, sourceFileInfo.Name);
                string extractToPath = Path.Combine(fullTempFolder, "tmp");

                Console.WriteLine($"copying source: {localZipPath}");

                File.Copy(_config.SourceZipPath, localZipPath);

                Console.WriteLine($"copying finished");

                Console.WriteLine($"unzipping");

                using (ZipArchive archive = ZipFile.Open(localZipPath, ZipArchiveMode.Read))
                {
                    foreach (var archiveEntry in archive.Entries)
                    {
                        FileInfo entryDestination = new FileInfo(Path.Combine(extractToPath, archiveEntry.FullName));
                        if (entryDestination.Directory != null && !entryDestination.Directory.Exists)
                        {
                            entryDestination.Directory.Create();
                        }

                        if (entryDestination.Name == string.Empty)
                        {
                            // this was a folder NOT a file
                            continue;
                        }

                        string entryDestiationFullPath = Path.Combine(extractToPath, archiveEntry.FullName);
                        try
                        {
                            archiveEntry.ExtractToFile(entryDestiationFullPath, false);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"error {entryDestiationFullPath} : {e}");
                        }
                    }
                }

                foreach (var p2c in _config.PathsToCopy)
                {
                    string fullPathSource = Path.Combine(extractToPath, p2c.RelativeSourcePath);
                    string fullPathDestination = Path.Combine(_config.DestinationRootPath, p2c.RelativeDestinationPath);

                    CopyFolder(new DirectoryInfo(fullPathSource), new DirectoryInfo(fullPathDestination));
                }

                Console.WriteLine($"successfully finished unzipping");
            }
            catch (Exception expUnExpected)
            {
                Console.WriteLine($"Unexpected error: {expUnExpected}");
            }
            finally
            {
                Console.WriteLine($"cleaning up: {fullTempFolder}");
                Directory.Delete(fullTempFolder, true);
            }

            return true;
        }

        private void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                DirectoryInfo targetDirectory = target.GetDirectories(dir.Name).FirstOrDefault();
                // check if directory exists before creating
                targetDirectory = targetDirectory ?? target.CreateSubdirectory(dir.Name);

                CopyFolder(dir, targetDirectory);
            }

            foreach (FileInfo file in source.GetFiles())
            {
                string destinationPath = Path.Combine(target.FullName, file.Name);
                if (!File.Exists(destinationPath))
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name));
                }
            }
        }
    }
}
