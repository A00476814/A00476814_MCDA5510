using Newtonsoft.Json;
using System.Globalization;
using CsvHelper;
public class PrintOutputs
{
    public static void PrintLogsToFile(LogData logData)
    {
        string currentDir = Environment.CurrentDirectory;
        string logFilePath = Path.GetRelativePath(currentDir, @"..\..\..\logs\logs.txt");
        using (StreamWriter writer = File.AppendText(@logFilePath))
        {
            // Write the Logs to the file
            foreach(var errorLog in logData.errorDataList ){
                LoggerModel loggerModel = new LoggerModel();
                loggerModel.dateTime = errorLog.currentTime;
                loggerModel.logLevel = "Error";
                loggerModel.message = "Row with data: " + JsonConvert.SerializeObject(errorLog.rowDataFromCSV, Formatting.None) +" "+ errorLog.ErrorMessage + " " +  errorLog.FilePath;
                string json = JsonConvert.SerializeObject(loggerModel, Formatting.None);

                writer.WriteLine(json);
            }
        }

    }


    public static void PrintPerformanceMetric(LogData logData,TimeSpan elapsed)
    {
        // This Method Write the performance metrics at the end of the log file 
        string currentDir = Environment.CurrentDirectory;
        string logFilePath = Path.GetRelativePath(currentDir, @"..\..\..\logs\logs.txt");
        using (StreamWriter writer = File.AppendText(@logFilePath))
        {
            writer.WriteLine("Total execution time:  " + elapsed);
            writer.WriteLine("Total number of valid rows: " + logData.processedRows);
            writer.WriteLine("Total number of skipped rows: " + logData.faultyRows);  
           
        }

    }

    public static void PrepareOutputFile(List<List<string>> dataFrame)
    {
        // This method Writes the CLean CSV records to the output file
        string currentDir = Environment.CurrentDirectory;
        string outputFilePath = Path.GetRelativePath(currentDir, @"..\..\..\Output\output.csv");
        try{
            using (var writer = new StreamWriter(outputFilePath))
            {
                // Create a CsvWriter object and pass it the StreamWriter object
                using (var csv = new CsvWriter (writer, CultureInfo.InvariantCulture))
                {
                    for (int i = 0; i < dataFrame.Count; i++)
                    {
                        for (int j = 0; j < dataFrame[i].Count; j++)
                        {
                            // Write each element of the array as a field
                            csv.WriteField(dataFrame[i][j]);
                            
                        }
                        // Write a new line after each row
                        csv.NextRecord();
                    }
                }
            }
        }catch (FileNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maxium supported path length.");
            }
    }
}