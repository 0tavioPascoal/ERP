using ERP.Services.Report;
using ERP.Services.Reports;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

public class ReportController : Controller {
    private readonly ReportService _reportService;
    private readonly IWebHostEnvironment _webHostEnv;

    public ReportController(IWebHostEnvironment webHostEnv, ReportService reportService) {
        _webHostEnv = webHostEnv;
        _reportService = reportService;
    }

    public IActionResult Index() {
        return View();
    }
 
    public async Task<IActionResult> GerarReport() {
        var webReport = new WebReport();
        var mssqlDataConnection = new MsSqlDataConnection();

        webReport.Report.Dictionary.AddChild(mssqlDataConnection);

        webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports",
                                           "SaleReport.frx"));

        var sale = HelperFastReport.GetTable(await _reportService.GetSaleReport(), "SaleReports");
        var prod = HelperFastReport.GetTable(await _reportService.GetProductReport(), "ProdReport");
        var client = HelperFastReport.GetTable(await _reportService.GetClientsReport(), "ClientReports");
        var saleItem = HelperFastReport.GetTable(await _reportService.GetSaleItemsReport(), "SaleItemReport");


        webReport.Report.RegisterData(sale, "SaleReports");
        webReport.Report.RegisterData(prod, "ProdReport");
        webReport.Report.RegisterData(client, "ClientReports");
        webReport.Report.RegisterData(saleItem, "SaleItemReport");
        return View(webReport);
    }

    [Route("SalePDF")]
    public async Task<IActionResult> SalesPDF() {
        var webReport = new WebReport();
        var mssqlDataConnection = new MsSqlDataConnection();

        webReport.Report.Dictionary.AddChild(mssqlDataConnection);

        webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports",
                                           "SaleReport.frx"));

        var sale = HelperFastReport.GetTable(await _reportService.GetSaleReport(), "SaleReports");
        var prod = HelperFastReport.GetTable(await _reportService.GetProductReport(), "ProdReport");
        var client = HelperFastReport.GetTable(await _reportService.GetClientsReport(), "ClientReports");
        var saleItem = HelperFastReport.GetTable(await _reportService.GetSaleItemsReport(), "SaleItemReport");


        webReport.Report.RegisterData(sale, "SaleReports");
        webReport.Report.RegisterData(prod, "ProdReport");
        webReport.Report.RegisterData(client, "ClientReports");
        webReport.Report.RegisterData(saleItem, "SaleItemReport");

        webReport.Report.Prepare();

        Stream stream = new MemoryStream();

        webReport.Report.Export(new PDFSimpleExport(), stream);
        stream.Position = 0;

        //return File(stream, "application/zip", "SalesReport.pdf");
        return new FileStreamResult(stream, "application/pdf");
    }

}
