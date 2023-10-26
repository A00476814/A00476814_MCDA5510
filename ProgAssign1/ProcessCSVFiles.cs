using System.Globalization;
using System.Xml;
using CsvHelper;
using CsvHelper.Configuration;


public class ProcessCSVFiles
{
    public static LogData processFiles(List<FileMetadata> fileMetadataList)
    {
        LogData logData = new LogData();
        List<List<string>> masterDataFrame = InitializeHeaders();
        // This Block Reads the files performs validations and perpares a dataframe with all the valid data to easily write to the 
        // output file.
        // First all the data is loaded in the data frame and after that whichever row fails the validation is removed from the DataFrame
        // Here LogData object stores all the importtant imformation like skipped rows, proccessed rows and error data if an error occurs
        // or validation fails
        foreach(FileMetadata fileMetadata in fileMetadataList){

           Dictionary<string, object> csvReaderResults =  readCsvFile(fileMetadata.Path,fileMetadata.Date,logData);
           List<CSVHeaders> csvDataList = (List<CSVHeaders>)csvReaderResults[BusinessConstants.CSV_DATA_LIST];
           logData = (LogData)csvReaderResults[BusinessConstants.LOG_DATA];
           List<List<string>> dataFrame = (List<List<string>>)csvReaderResults[BusinessConstants.UPDATED_CSV_DATA];
           
           if(csvDataList is not null){
                logData = Validations.validateData(csvDataList,dataFrame,fileMetadata.Path,logData);               
           }
            dataFrame = logData.dataFrame;
            masterDataFrame.AddRange(dataFrame);
            //WriteCSV.UpdatedCsvData(dataFrame,fileMetadata.Path);

        }
        logData.dataFrame = masterDataFrame;
        return logData;
    }

    private static List<List<string>> InitializeHeaders()
    {
        List<List<string>> masterDataFrame = new List<List<string>>();
        List<string> headers = new List<string>();

        // Add the headers to the list
        headers.Add("First Name");
        headers.Add("Last Name");
        headers.Add("Street Number");
        headers.Add("Street");
        headers.Add("City");
        headers.Add("Province");
        headers.Add("Postal Code");
        headers.Add("Country");
        headers.Add("Phone Number");
        headers.Add("email Address");
        headers.Add("Date");
        masterDataFrame.Add(headers);

        return masterDataFrame;
    }

    private static Dictionary<string, object> readCsvFile(string? path, string? date, LogData logData)
    {

        if(path is not null || path==""){
            Dictionary<string, object> myDict = CsvReader(path,date,logData);
            return myDict;
        }
        return null;
    }


    public static Dictionary<string, object> CsvReader(string path, string? date, LogData logData)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        using (StreamReader sr = new StreamReader(@path))
        {

            string header = sr.ReadLine();
            string[] headers = header.Split(',');

            List<List<string>> dataFrame = new List<List<string>>();
            

            List<CSVHeaders> csvDataList = new List<CSVHeaders>();

            
            while (!sr.EndOfStream)
            {
             
                int faultyRows = logData.faultyRows;
                List<ErrorData> errorDataList = logData.errorDataList;
                string line = sr.ReadLine();
 
                string[] values = line.Split(',');

                List<string> csvRow = new List<string> (values);
                
                
                if(values.Length<10){
                    ErrorData errorData = new ErrorData();
                    errorData.DidValidationfail = true;
                    errorData.rowDataFromCSV = csvRow;
                    errorData.ErrorMessage = Messages.MISSING_DATA;
                    errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_MISSING_DATA;
                    errorData.FilePath = path;
                    errorData.currentTime = DateTime.Now;
                    faultyRows++;
                    errorDataList.Add(errorData);
                    logData.faultyRows = faultyRows;
                    logData.errorDataList = errorDataList;

                }else if(values.Length>10){
                    ErrorData errorData = new ErrorData();
                    errorData.DidValidationfail = true;
                    errorData.rowDataFromCSV = csvRow;
                    errorData.FilePath = path;
                    errorData.ErrorMessage = Messages.EXCESS_DATA;
                    errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_EXCESS_DATA;
                    errorData.currentTime = DateTime.Now;
                    faultyRows++;
                    errorDataList.Add(errorData);
                    logData.faultyRows = faultyRows;
                    logData.errorDataList = errorDataList;
                }else if(values.Length==10){
                    CSVHeaders csvRowData = mapCsvDataRawToCsvHeadersObject(values);
                    csvDataList.Add(csvRowData);
                    csvRow.Add(date);
                    dataFrame.Add(csvRow);  
                }
              
            }

            dict.Add(BusinessConstants.LOG_DATA,logData);
            dict.Add(BusinessConstants.CSV_DATA_LIST,csvDataList);
            dict.Add(BusinessConstants.UPDATED_CSV_DATA,dataFrame);
            return dict;
        }
    }

        private static CSVHeaders mapCsvDataRawToCsvHeadersObject(string[] csvDataRaw)
    {
            CSVHeaders csvData = new CSVHeaders();

            csvData.FirstName = csvDataRaw[0];
            csvData.LastName = csvDataRaw[1];
            csvData.StreetNumber = csvDataRaw[2];
            csvData.Street = csvDataRaw[3];
            csvData.City = csvDataRaw[4];
            csvData.Province = csvDataRaw[5];
            csvData.PostalCode = csvDataRaw[6];
            csvData.Country = csvDataRaw[7];
            csvData.PhoneNumber = csvDataRaw[8];
            csvData.EmailAddress = csvDataRaw[9];
         
            return csvData;
    }
}