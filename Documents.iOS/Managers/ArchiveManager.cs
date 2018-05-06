using System;
using Documents.iOS.Enums;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SharpCompress.Readers;
// ReSharper disable AssignNullToNotNullAttribute

namespace Documents.iOS.Managers
{
    public class ArchiveManager
    {

        public bool CheckForArchiveFilesExists(string filePath, UnarchiveLocationEnum location, string folderToSave = "")
        {
            var extractLocation = DetermineExtractLocation(filePath, location, folderToSave);
            var collision = CheckForGenericFilesExists(filePath, extractLocation);
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
                    var enumerable = files as string[] ?? files.ToArray();
                    var fp = enumerable.First();
                    if (File.Exists(fp))
                    {
                        using (FileStream zipToOpen = new FileStream(archiveFilePath, FileMode.Create))
                        {
                            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                            {
                                foreach (var file in enumerable)
                                {
                                    if (File.Exists(file))
                                    {
                                        var fileToCompress = new FileInfo(file);
                                        using (FileStream originalFileStream = fileToCompress.OpenRead())
                                        {
                                            ZipArchiveEntry readmeEntry = archive.CreateEntry(fileToCompress.Name);
                                            originalFileStream.CopyTo(readmeEntry.Open());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if(Directory.Exists(fp))
                    {
                        ZipFile.CreateFromDirectory(fp, archiveFilePath, CompressionLevel.Optimal,true);
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
