using System;
using Documents.iOS.Enums;
using System.IO;
using System.IO.Compression;
using SharpCompress.Readers;
namespace Documents.iOS.Managers
{
    public class ArchiveManager
    {
        public ArchiveManager()
        {
        }

        public bool CheckForArchiveFilesExists(string filePath, UnarchiveLocationEnum location, string folderToSave = "")
        {
            //var archiveType = DetermineArchiveType(filePath);

            var collision = false;
            var extractLocation = DetermineExtractLocation(filePath, location, folderToSave);
            collision = CheckForGenericFilesExists(filePath, extractLocation);

            //switch (archiveType)
            //{
            //    case ArchiveTypeEnum.Zip:
            //        collision = CheckForZipArchiveFilesExists(filePath, extractLocation);
            //        break;
            //    case ArchiveTypeEnum.Rar:
            //    case ArchiveTypeEnum.SevenZip:
            //    case ArchiveTypeEnum.Tar:
            //    case ArchiveTypeEnum.GZip:
            //        collision = CheckForGenericFilesExists(filePath, extractLocation);
            //        break;
            //}
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

        private static bool CheckForZipArchiveFilesExists(string filePath, string extractLocation)
        {
            var collision = false;
            using (var zip = ZipFile.OpenRead(filePath))
            {
                foreach (var fe in zip.Entries)
                {

                    var extractFilePath = Path.Combine(extractLocation, fe.FullName);
                    if ((Directory.Exists(extractFilePath) || File.Exists(extractFilePath)))
                    {
                        collision = true;
                    }

                }
            }

            return collision;
        }

        public void UnarchiveFile(string filePath, UnarchiveLocationEnum location, UnarchiveActionEnum action, string folderToSave = "")
        {
            //var archiveType = DetermineArchiveType(filePath);
            var extractLocation = DetermineExtractLocation(filePath, location, folderToSave);

            CreateExtractLocation(location, extractLocation);
            Unarchive(filePath, extractLocation, action);

            //switch (archiveType)
            //{
            //    case ArchiveTypeEnum.Zip:
            //        UnarchiveZip(filePath, extractLocation, action);
            //        break;
            //    case ArchiveTypeEnum.Rar:
            //    case ArchiveTypeEnum.SevenZip:
            //    case ArchiveTypeEnum.Tar:
            //    case ArchiveTypeEnum.GZip:
            //        Unarchive(filePath, extractLocation, action);
            //        break;
            //}

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
            //var fileExt = Path.GetExtension(filePath);
            //if (string.Equals(fileExt, ".7z", StringComparison.InvariantCultureIgnoreCase))
            //{

                using (var archive = SharpCompress.Archives.ArchiveFactory.Open(filePath))
                {
                    var reader = archive.ExtractAllEntries();
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                            reader.WriteEntryToDirectory(extractLocation, new ExtractionOptions() { ExtractFullPath = false, Overwrite = true });
                    }
                }
            //}
            //else
            //{
            //    using (Stream stream = File.OpenRead(filePath))
            //    {
            //        var reader = ReaderFactory.Open(stream);
            //        while (reader.MoveToNextEntry())
            //        {
            //            if (!reader.Entry.IsDirectory)
            //            {
            //                reader.WriteEntryToDirectory(extractLocation, new ExtractionOptions() { ExtractFullPath = true, Overwrite = overwrite });
            //            }
            //        }
            //    }
            //}
        }

        private void UnarchiveZip(string filePath, string extractLocation, UnarchiveActionEnum action)
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
            using (var zip = ZipFile.OpenRead(filePath))
            {
                foreach (var fe in zip.Entries)
                {
                    try
                    {
                        var extractFilePath = Path.Combine(extractLocation, fe.FullName);
                        fe.ExtractToFile(extractFilePath, overwrite);
                    }
                    catch (IOException ex)
                    {
                        var a = ex;
                        //Catching as dont care :)   
                    }
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

        //private ArchiveTypeEnum DetermineArchiveType(string filePath)
        //{
        //    var fileExt = System.IO.Path.GetExtension(filePath);
        //    switch (fileExt.ToLower())
        //    {
        //        case ".zip":
        //            return ArchiveTypeEnum.Zip;
        //        case ".7z":
        //            return ArchiveTypeEnum.SevenZip;
        //        case ".rar":
        //            return ArchiveTypeEnum.Rar;
        //        case ".tar":
        //            return ArchiveTypeEnum.Tar;
        //        case ".gz":
        //        case ".bz2":
        //            return ArchiveTypeEnum.GZip;
        //        default:
        //            throw new Exception("Unsupported file type");
        //    }
        //}
    }
}
