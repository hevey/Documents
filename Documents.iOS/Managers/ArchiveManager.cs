using System;
using Documents.iOS.Enums;
using System.IO;
using System.IO.Compression;

namespace Documents.iOS.Managers
{
    public class ArchiveManager
    {
        public ArchiveManager()
        {
        }

        public void UnarchiveFile(string filePath, UnarchiveLocationEnum location, string folderToSave = "")
        {
            var archiveType = DetermineArchiveType(filePath);
            var extractLocation = DetermineExtractLocation(filePath, location, folderToSave);

            CreateExtractLocation(location, extractLocation);

            switch (archiveType)
            {
                case ArchiveTypeEnum.Zip:
                    UnarchiveZip(filePath, extractLocation);
                    break;
            }

        }

        private void UnarchiveZip(string filePath, string extractLocation)
        {
            ZipFile.ExtractToDirectory(filePath, extractLocation);
        }

        public string DetermineExtractLocation(string filePath, UnarchiveLocationEnum location, string folderToSave)
        {
            var folderName = Path.GetFileNameWithoutExtension(filePath);
            var folderPath = Path.GetDirectoryName(filePath);
            switch (location)
            {
                case UnarchiveLocationEnum.CurrentDirectory:
                    return folderPath;
                case UnarchiveLocationEnum.SubDirectoryWithArchiveName:
                    return Path.Combine(folderPath, folderName);
                case UnarchiveLocationEnum.SubDirectoryWithName:
                    return Path.Combine(folderPath, folderToSave);
                default:
                    throw new Exception("Unsupported folder location");
            }
        }

        private void CreateExtractLocation(UnarchiveLocationEnum location, string extractLocation)
        {
            if (location != UnarchiveLocationEnum.CurrentDirectory)
            {
                if (!Directory.Exists(extractLocation))
                {
                    Directory.CreateDirectory(extractLocation);
                }
            }
        }

        private ArchiveTypeEnum DetermineArchiveType(string filePath)
        {
            var fileExt = System.IO.Path.GetExtension(filePath);
            switch (fileExt.ToLower())
            {
                case ".zip":
                    return ArchiveTypeEnum.Zip;
                    break;
                default:
                    throw new Exception("Unsupported file type");
            }
        }
    }
}
