using System.Reflection.Metadata;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Net.Mail;

public class Validations
{
    public static LogData validateData(List<CSVHeaders> csvData, List<List<string>> dataFrame, string? path, LogData logData)
    {
        // This class is responsible for performung null checks and pattern checks on the data and removing the failed rows from the final 
        // Data Frame
        List<ErrorData> errorDataList = logData.errorDataList;
        int processedRows = logData.processedRows;
        int faultyRows = logData.faultyRows;
        List<int> faultyIndices = new List<int>();
        for(int i=0;i<csvData.Count;i++){
            CSVHeaders data = csvData[i];
            ErrorData error = new ErrorData();
            error = nullCheck(data, error);
            
            if(!error.DidValidationfail)error = patterncheck(data, error);

            if(error.DidValidationfail){
                faultyRows++;
                error.rowDataFromCSV = dataFrame[i];
                error.FilePath = path;
                errorDataList.Add(error);
                faultyIndices.Add(i);
            }else{
                processedRows++;
            }
        }
        logData.processedRows = processedRows;
        logData.faultyRows = faultyRows;
        logData.errorDataList = errorDataList;
        List<List<string>> finalDataFrame = removeFaultyData(faultyIndices,dataFrame);
        logData.dataFrame = finalDataFrame;
        return logData;
    }

    private static List<List<string>> removeFaultyData(List<int> faultyIndices, List<List<string>> dataFrame)
    {
        List<List<string>> finalDataFrame = new List<List<string>>();
        for(int i=0; i< dataFrame.Count;i++){
            if(!faultyIndices.Contains(i)){
                finalDataFrame.Add(dataFrame[i]);
            }
        }
        return finalDataFrame;
    }

    private static ErrorData patterncheck(CSVHeaders data, ErrorData errorData)
    {
        if(IsEmailInValid(data.EmailAddress)){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.INCORRECT_EMAIL_FORMAT;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_PATTERN_CHECK;
        }
        else if (!Regex.Match(data.FirstName,"^[A-Za-z\\s]+$").Success){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.ALPHA_NUMERIC_FIRST_NAME_FOUND;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_PATTERN_CHECK;
        }
        else if (!Regex.Match(data.LastName,"^[A-Za-z\\s]+$").Success){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.ALPHA_NUMERIC_LAST_NAME_FOUND;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_PATTERN_CHECK;
        }
        else if (!Regex.Match(data.Street,"^[^0-9\\p{P}\\p{S}]+$").Success){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.ALPHA_NUMERIC_STREET_NAME_FOUND;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_PATTERN_CHECK;
        }
        else if (!Regex.Match(data.StreetNumber,"^[0-9\\s]+$").Success){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.ALPHA_NUMERIC_STREET_NAME_FOUND;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_PATTERN_CHECK;
        }
        else if (!Regex.Match(data.PostalCode,"^[ABCEGHJKLMNPRSTVXY]\\d[A-Z][ -]?\\d[A-Z]\\d$").Success){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.INVALID_POSTAL_CODE_FOUND;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_PATTERN_CHECK;
        }
        return errorData;
    }

    private static ErrorData nullCheck(CSVHeaders data, ErrorData errorData)
    {
        
        if(data.FirstName is null || data.FirstName==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.FIRST_NAME_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.LastName is null || data.LastName==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.LAST_NAME_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.StreetNumber is null || data.StreetNumber==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.STREET_NUMBER_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.Street is null || data.Street==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.STREET_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.City is null || data.City==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.CITY_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.Province is null || data.Province==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.PROVINCE_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.PostalCode is null || data.PostalCode==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.POSTAL_CODE_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.Country is null || data.Country==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.COUNTRY_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.PhoneNumber is null || data.PhoneNumber==""){
            errorData.currentTime = DateTime.Now;
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.PHONE_NUMBER_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        else if(data.EmailAddress is null || data.EmailAddress==""){
            errorData.currentTime = DateTime.Now; 
            errorData.DidValidationfail=true;
            errorData.ErrorMessage = Messages.EMAIL_NULL;
            errorData.ValidationType = BusinessConstants.VALIDATION_TYPE_NULL_CHECK;
        }
        return errorData;
    }


    private static bool IsEmailInValid(string email)
    {
        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        return !regex.IsMatch(email);
    }


}