namespace Template
{
    /// <summary>
    /// Abstract Class defining the Template Method skeleton
    /// </summary>
    public abstract class DataMiner
    {
        /// <summary>
        /// Template Method: declares the skeleton of the algorithm.
        /// Marked as sealed/non-virtual so subclasses cannot alter the overall flow.
        /// </summary>
        public void Mine(string path)
        {
            OpenFile(path);
            var rawData = ExtractRawData();
            var parsedData = ParseData(rawData);
            AnalyzeData(parsedData);
            SendReport();
            CloseFile();
            HookAfterComplete();
        }

        protected abstract void OpenFile(string path);
        protected abstract string ExtractRawData();
        protected abstract List<string> ParseData(string rawData);

        protected virtual void AnalyzeData(List<string> data)
        {
            Console.WriteLine($"Analyzing {data.Count} data records...");
        }

        protected virtual void SendReport()
        {
            Console.WriteLine("Generating standard summary report...");
        }

        protected abstract void CloseFile();

        /// <summary>
        /// Optional hook that subclasses can optionally override
        /// </summary>
        protected virtual void HookAfterComplete() { }
    }

    /// <summary>
    /// Concrete Class 1
    /// </summary>
    public class CsvDataMiner : DataMiner
    {
        protected override void OpenFile(string path)
        {
            Console.WriteLine($"[CSV] Opening CSV stream at '{path}'...");
        }

        protected override string ExtractRawData()
        {
            Console.WriteLine("[CSV] Reading comma-separated rows...");
            return "id,name,role\n1,Alice,Dev\n2,Bob,QA";
        }

        protected override List<string> ParseData(string rawData)
        {
            Console.WriteLine("[CSV] Parsing CSV lines into entity list...");
            return rawData.Split('\n').Skip(1).ToList();
        }

        protected override void CloseFile()
        {
            Console.WriteLine("[CSV] Closing CSV stream.");
        }
    }

    /// <summary>
    /// Concrete Class 2
    /// </summary>
    public class PdfDataMiner : DataMiner
    {
        protected override void OpenFile(string path)
        {
            Console.WriteLine($"[PDF] Opening PDF document at '{path}'...");
        }

        protected override string ExtractRawData()
        {
            Console.WriteLine("[PDF] Extracting text streams and tables from PDF pages...");
            return "Document Page 1 Content | Page 2 Tables";
        }

        protected override List<string> ParseData(string rawData)
        {
            Console.WriteLine("[PDF] Tokenizing PDF layout structure...");
            return rawData.Split('|').Select(s => s.Trim()).ToList();
        }

        protected override void CloseFile()
        {
            Console.WriteLine("[PDF] Releasing PDF handle.");
        }

        protected override void HookAfterComplete()
        {
            Console.WriteLine("[PDF Hook] Sending audit notification for PDF parsing completion.");
        }
    }
}
