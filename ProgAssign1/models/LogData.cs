public class LogData{
        public List<ErrorData> errorDataList { get; set; }      
        public int processedRows { get; set; }   
        public int faultyRows { get; set; }    

        public List<List<string>> dataFrame {get; set;}

        public LogData()
        {
            errorDataList = new List<ErrorData>();
            processedRows = 0;
            faultyRows = 0;
        }
} 