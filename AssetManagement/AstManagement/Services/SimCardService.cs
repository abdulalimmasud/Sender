using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;

namespace AstManagement.Services
{
    public interface ISimCardService
    {
        Task<SimCardResponseDto> GetAsync(int id);
        Task<SimCardResponseDto> GetAsync(string mobileNumber);
        Task<List<SimCardResponseShortDto>> GetInactiveSimAsync(int page, int pageSize, string search);
        Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize);
        Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize, int type, string search);
        Task<SimCard> InsertAsync(SimCardRegistrationDto dto);
        Task<ResponseResult> InsertFromFileAsync(SimCardImportDto dto);
        Task<SimCard> UpdateAsync(SimCard model);
        Task<int> DeleteAsync(int id, int userId, string username);
    }
    public class SimCardService:ISimCardService
    {
        private readonly ISimCardRepository _repository;
        public SimCardService(ISimCardRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteAsync(int id, int userId, string username)
        {
            var usesCount = await _repository.UsesCountAsync(id);
            if (usesCount == 0)
            {
                return await _repository.DeleteAsync(id, userId, username);
            }
            return 0;
        }

        public async Task<SimCardResponseDto> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<SimCardResponseDto> GetAsync(string mobileNumber)
        {
            return await _repository.GetAsync(mobileNumber);
        }
        public async Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize, int type, string search)
        {
            return await _repository.GetAsync(page, pageSize, type, search);
        }

        public async Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize)
        {
            return await _repository.GetAsync(page, pageSize);
        }

        public async Task<List<SimCardResponseShortDto>> GetInactiveSimAsync(int page, int pageSize, string search)
        {
            return await _repository.GetInactiveSimAsync(page, pageSize, search);
        }

        public async Task<SimCard> InsertAsync(SimCardRegistrationDto dto)
        {
            var exist = await _repository.GetAsync(dto.MobileNumber);
            if (exist == null)
            {
                return await _repository.InsertAsync(dto.MobileNumber, dto.SimNumber, dto.OperatorId, dto.OperatorPackageId, dto.UserId, dto.UserName);
            }
            return null;
        }

        public async Task<ResponseResult> InsertFromFileAsync(SimCardImportDto dto)
        {
            using(var stream = new MemoryStream())
            {
                await dto.File.CopyToAsync(stream);
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    SharedStringTablePart shared = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringItem[] items = shared.SharedStringTable.Elements<SharedStringItem>().ToArray();
                    string key, text = "";
                    var list = new List<SimCard>();
                    var wrongInputs = new List<WrongInput>();
                    long r;
                    var rows = sheetData.Elements<Row>().ToList();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        if(rows[i].RowIndex.Value > 1)
                        {
                            var sim = new SimCard
                            {
                                OperatorId = dto.OperatorId,
                                OperatorPackageId = dto.OperatorPackageId,
                                UserName = dto.UserName,
                                UserId = dto.UserId,
                                UpdateTime = DateTime.Now,
                                UpdateBy = dto.UserId
                            };
                            var cells = rows[i].Elements<Cell>().ToList();
                            string mobileNo = "", simNo = "";
                            if (cells.Count == 2)
                            {
                                bool isMobileCorrect = false, isSimCorrect = false;
                                if(cells[0].DataType != null && cells[0].DataType.Value == CellValues.SharedString)
                                {
                                    mobileNo = items[int.Parse(cells[0].CellValue.Text)].InnerText;
                                    if (mobileNo.Length == 11)
                                    {
                                        long.TryParse(mobileNo, out r);
                                        if (r > 1000000000)
                                        {
                                            sim.MobileNumber = mobileNo;
                                            isMobileCorrect = true;
                                        }
                                        else
                                        {
                                            text = $"MobileNo have wrong character.";
                                        }
                                    }
                                    else
                                    {
                                        text = $"MobileNo {mobileNo.Length} digit.";
                                    }
                                }
                                else
                                {
                                    text = "MobileNo DataType Not Text.";
                                }
                                if (isMobileCorrect)
                                {
                                    if(cells[1].DataType != null && cells[1].DataType.Value == CellValues.SharedString)
                                    {
                                        simNo = items[int.Parse(cells[1].CellValue.Text)].InnerText;
                                        if(simNo.Length > 11 && simNo.Length < 50)
                                        {
                                            sim.SimNumber = simNo;
                                            isSimCorrect = true;
                                        }
                                        else
                                        {
                                            text = "SimNo not should between 12 and 50";
                                        }                                      
                                    }
                                    else
                                    {
                                        text = "SimNo DataType Not Text.";
                                    }
                                }
                                if(!isSimCorrect)
                                {
                                    key = !string.IsNullOrEmpty(mobileNo) ? mobileNo : !string.IsNullOrEmpty(simNo) ? simNo : "";
                                    if (string.IsNullOrEmpty(key))
                                    {
                                        try
                                        {
                                            key = items[int.Parse(cells[0].CellValue.Text)].InnerText;
                                        }
                                        catch (Exception)
                                        {
                                            key = cells[0].InnerText;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        wrongInputs.Add(new WrongInput { Key = key, Cause = text });
                                    }
                                }
                                else
                                {
                                    list.Add(sim);
                                }
                            }
                        }
                    }
                    var response = _repository.BulkInsert(list);
                    wrongInputs.AddRange(response);
                    var result = new ResponseResult
                    {
                        StatusCode = response.Count > 0 ? 409 : wrongInputs.Count > 0 ? 411: 200,
                        Message = wrongInputs
                    };
                    return await Task.FromResult(result);
                }
            }
        }
        public async Task<SimCard> UpdateAsync(SimCard model)
        {
            var exist = await _repository.GetAsync(model.Id, model.MobileNumber);
            if (exist == null)
            {
                return await _repository.UpdateAsync(model.Id, model.MobileNumber, model.SimNumber, model.OperatorId, model.OperatorPackageId, model.UserId, model.UserName);
            }
            return null;
        }
    }
}
