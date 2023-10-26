
using System.Reflection.Metadata;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

public class Utilities
{
    private const string  pattern  = @"^\d+\/\d+\/\d+$";
    public static List<FileMetadata> getDatesFromPath(string[] filePathList)
    {
        List<FileMetadata> fileMetadataList = new List<FileMetadata>();
        foreach(var filePath in filePathList){
            string[] parts = filePath.Split('\\');

            string year = parts[parts.Length - 4];
            string month = parts[parts.Length - 3];
            string day = parts[parts.Length - 2];

            string dateAsString = year+"/"+month+"/"+day;

            if (Regex.IsMatch(dateAsString, pattern)){
                FileMetadata fileMetadata = new FileMetadata();
                fileMetadata.Date = dateAsString;
                fileMetadata.Path = filePath;
                fileMetadataList.Add(fileMetadata);
            }
        }      
        return fileMetadataList;
    }
}