using Template;

Console.Title = "Template Method Pattern";

Console.WriteLine("--- Mining CSV File ---");
DataMiner csvMiner = new CsvDataMiner();
csvMiner.Mine("employees.csv");

Console.WriteLine("\n--- Mining PDF File ---");
DataMiner pdfMiner = new PdfDataMiner();
pdfMiner.Mine("financial_report.pdf");
