using System.Data;
using ConverterAPI.Services;
using ConverterClassLib;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ConverterAPI.Controllers
{
    [Route("api/etudiants")]
    [ApiController]
    public class EtudiantsController : ControllerBase
    {
        private readonly IEtudiantService _etudiantService;
        //both dependencies are injected correctly when the controller is instantiated.
        public EtudiantsController(IEtudiantService etudiantService)
        {
            _etudiantService = etudiantService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllEtudiants()
        {
            var etds = await _etudiantService.GetEtdAsync();
            if (etds == null)
                return BadRequest();
            return Ok(etds);

        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateEtudiant(Etudiant etd)
        {
            if (etd != null)
            {
                var result = await _etudiantService.InsertAsync(etd);
                if (result)
                    return Ok(etd);
                else
                    return StatusCode(500);
            }
            else
                return BadRequest();
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetEtudiantById(int id)
        {
            var result = await _etudiantService.GetEtudiantByIdAsync(id);
            if (result == null)
                return BadRequest("Not Found");
            return Ok(result);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateEtudiant(Etudiant etd)
        {
            if (etd != null && etd.id > 0)
            {
                var etdDB = await _etudiantService.PutEtudiantAsync(etd);
                if (etdDB)
                    return Ok(etd);
                else
                    return StatusCode(500);
            }
            else
                return BadRequest("Error");
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteEtudiantById(int id)
        {
            var result = await _etudiantService.DeleteAsync(id);
            if (result)
                return Ok(result);
            else
                return BadRequest("Etudiant Not Found");
        }

        [HttpGet("export/xlsx"), Authorize]
        public async Task<IActionResult> ExportXlsx([FromQuery] int[] selectedDataColIds)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                SpreadsheetDocument SSD = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);

                WorkbookPart WBP = SSD.AddWorkbookPart();
                WBP.Workbook = new Workbook();

                WorksheetPart WSP = WBP.AddNewPart<WorksheetPart>();
                WSP.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = SSD.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = SSD.WorkbookPart.GetIdOfPart(WSP),
                    SheetId = 1,
                    Name = "Page 1"
                };

                sheets.Append(sheet);
                Worksheet WS = WSP.Worksheet;
                SheetData sheetData = WS.GetFirstChild<SheetData>();


                // Load etudiant async
                List<Etudiant> etds = await _etudiantService.GetEtdAsync();
                if (etds == null)
                    return BadRequest();

                //filter list to show according to the selected data
                List<string> customDataCols = new List<string>();
                List<List<string>> customData = new List<List<string>>();

                //fetch needed (requested) col names and save it to customDataCols
                var properties = etds[0].GetType().GetProperties();

                foreach (int i in selectedDataColIds)
                {
                    for (int j = 0; j < properties.Length; j++)
                    {
                        if (j == i)
                        {
                            customDataCols.Add(properties[j].Name);
                        }
                    }
                }
                foreach (Etudiant etd in etds)
                {
                    List<string> customDataRow = new List<string>();
                    foreach (int i in selectedDataColIds)
                    {
                        string x = properties[i].GetValue(etd).ToString();
                        customDataRow.Add(x);
                    }
                    customData.Add(customDataRow);
                }
                
                // Add headings as the first row in the sheet
                Row headingRow = new Row();
                foreach (string col in customDataCols)
                {
                    Cell headingCell = new Cell()
                    {
                        CellValue = new CellValue(col),
                        DataType = CellValues.String
                    };
                    headingRow.Append(headingCell);
                }
                sheetData.Append(headingRow);

                foreach (List<string> singleEtdRow in customData)
                {
                    Row r = new Row();
                    foreach (string prop in singleEtdRow)
                    {
                        // Create new cell 
                        Cell c = new Cell()
                        {
                            CellValue = new CellValue(prop),
                            DataType = CellValues.String
                        };
                        r.Append(c);
                    }
                    sheetData.Append(r);
                }

                WSP.Worksheet.Save();
                SSD.Close();

                // Set the position of the memory stream to the beginning
                memoryStream.Position = 0;

                // Return the file as a FileContentResult
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "etudiants.xlsx");

            }
        }

        [HttpGet("export/pdf"),Authorize]
        public async Task<IActionResult> ExportPdf([FromQuery] int[] selectedDataColIds)
        {
            var doc = new PdfDocument();

            //get full etudiants list (all data)
            List<Etudiant> etds = await _etudiantService.GetEtdAsync();
            if (etds == null)
                return BadRequest();
            
            //filter list to show according to the selected data
            List<string> customDataCols= new List<string>();
            List<List<string>> customData= new List<List<string>>();

            //fetch needed (requested) col names and save it to customDataCols
            var properties = etds[0].GetType().GetProperties();

            foreach (int i in selectedDataColIds)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    if (j == i)
                    {
                        customDataCols.Add(properties[j].Name);
                    }
                }
            }
            foreach (Etudiant etd in etds)
            {
                List<string> customDataRow = new List<string>();
                foreach (int i in selectedDataColIds)
                {
                    string x = properties[i].GetValue(etd).ToString();
                    customDataRow.Add(x);
                }
                customData.Add(customDataRow);
            }

            string htmlcontent = "<h1>Liste des Etudiants</h1>";
            htmlcontent += "<style>";
            htmlcontent += "h1 { text-align: center; font-weight: bold; font-size: 3.6em; line-height: 1.7em; margin-bottom: 15px; color: #4285F4;}";
            htmlcontent += " th, td {border: 1px solid black;border-collapse: collapse;} ";
            htmlcontent += " table {border: 1px solid black;border-collapse: collapse;width : 100%;} ";
            htmlcontent += "</style>";
            htmlcontent += "<table>";
            htmlcontent += "<thead>";
            htmlcontent += "<tr>";

            //make it dynamic according to the choosen data
            foreach (string col in customDataCols)
            {
                htmlcontent += "<td>"+ col + "</td>";
            }

            htmlcontent += "</tr>";
            htmlcontent += "</thead >";
            htmlcontent += "<tbody>";
            foreach (List<string> singleEtdRow in customData)
            {
                htmlcontent += "<tr>";
                foreach (string prop in singleEtdRow)
                {
                    htmlcontent += "<td>" + prop + "</td>";
                }
                htmlcontent += "</tr>";
            }          
            htmlcontent += "</tbody>";
            htmlcontent += "</table>";

            PdfGenerator.AddPdfPages(doc, htmlcontent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                response = ms.ToArray();
            }
            return File(response, "application/pdf");
        }
    }
}
