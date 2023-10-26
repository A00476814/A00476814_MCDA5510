using aplha;
using System;
using System.Diagnostics;
class Program
{
    static void Main(string[] args)
    {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string currentDir = Environment.CurrentDirectory;
            string rootPath = Path.GetRelativePath(currentDir, @"..\..\..\SampleData2");
            //string rootPath = @"C:\Users\kheva\OneDrive\MCDA5510\ProgAssign1\SampleData2";
            string[] filePathList = DirWalker.getFilePathList(rootPath);
            List<FileMetadata> fileMetadataList = new List<FileMetadata>();
            fileMetadataList = Utilities.getDatesFromPath(filePathList);
            LogData logData = ProcessCSVFiles.processFiles(fileMetadataList);

            PrintOutputs.PrepareOutputFile(logData.dataFrame);
            PrintOutputs.PrintLogsToFile(logData);
            TimeSpan elapsed = stopwatch.Elapsed;
            stopwatch.Stop();
            PrintOutputs.PrintPerformanceMetric(logData,elapsed);       
            
            Console.WriteLine("Total number of valid rows: " + logData.processedRows);
            Console.WriteLine("Total number of skipped rows: " + logData.faultyRows);                       
            Console.WriteLine("Total execution time: " + elapsed);
    }
}