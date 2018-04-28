using System;
using Documents.iOS.Enums;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Common;
namespace Documents.iOS.Managers
{
    public class ArchiveManager
    {

        public bool CheckForArchiveFilesExists(string filePath, UnarchiveLocationEnum location, string folderToSave = "")
        {
            var collision = false;
            var extractLocation = DetermineExtractLocation(filePath, location, folderToSave);
            collision = CheckForGenericFilesExists(filePath, extractLocation);
            return collision;
        }

        private bool CheckForGenericFilesExists(string filePath, string extractLocation)
        {
            var collision = false;
            using (var archive = SharpCompress.Archives.ArchiveFactory.Open(filePath))
            {
                var reader = archive.ExtractAllEntries();
                while (reader.MoveToNextEntry())
                {
                    var extractFilePath = Path.Combine(extractLocation, reader.Entry.Key);
                    if ((Directory.Exists(extractFilePath) || File.Exists(extractFilePath)))
                    {
                        collision = true;
                    }
                }
            }
            return collision;
        }

        public void ArchiveFiles(IEnumerable<string> files, ArchiveTypeEnum type, string archiveFilePath)
        {
            switch (type)
            {
                case ArchiveTypeEnum.Zip:
                    using (var zip = File.OpenWrite(archiveFilePath))
                    using (var zipWriter = WriterFactory.Open(zip, ArchiveType.Zip, CompressionType.Deflate))
                    {
                        foreach (var filePath in files)
                        {
                            zipWriter.Write(Path.GetFileName(filePath), filePath);
                        }
                    }
                    break;
                case ArchiveTypeEnum.GZip:
                    using (Stream stream = File.OpenWrite(archiveFilePath))
                    using (var writer = WriterFactory.Open(stream, ArchiveType.GZip, CompressionType.GZip))
                    {
                        foreach (var filePath in files)
                        {
                            writer.Write(Path.GetFileName(filePath), filePath);
                        }
                    }
                    break;
                case ArchiveTypeEnum.Tar:
                    using (Stream stream = File.OpenWrite(archiveFilePath))
                    using (var writer = WriterFactory.Open(stream, ArchiveType.Tar, CompressionType.GZip))
                    {
                        foreach (var filePath in files)
                        {
                            writer.Write(Path.GetFileName(filePath), filePath);
                        }
                    }
                    break;
                default:
                    throw new Exception("Unsupported archive type");
            }
        }

        public void UnarchiveFile(string filePath, UnarchiveLocationEnum location, UnarchiveActionEnum action, string folderToSave = "")
        {
            var extractLocation = DetermineExtractLocation(filePath, location, folderToSave);
            CreateExtractLocation(location, extractLocation);
            Unarchive(filePath, extractLocation, action);
        }

        private void Unarchive(string filePath, string extractLocation, UnarchiveActionEnum action)
        {
            var overwrite = false;
            switch (action)
            {
                case UnarchiveActionEnum.MergeWithOverwrite:
                case UnarchiveActionEnum.Overwrite:
                    overwrite = true;
                    break;
                case UnarchiveActionEnum.MergeWithoutOverwrite:
                    overwrite = false;
                    break;

            }
            using (var archive = SharpCompress.Archives.ArchiveFactory.Open(filePath))
            {
                var reader = archive.ExtractAllEntries();
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                        reader.WriteEntryToDirectory(extractLocation, new ExtractionOptions() { ExtractFullPath = true, Overwrite = overwrite, PreserveFileTime = true });
                }
            }
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
    }
}
