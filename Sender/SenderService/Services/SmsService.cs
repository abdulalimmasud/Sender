using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationDb.Repositories;
using SenderService.Configuration;
using SenderService.Utility;
using SenderService.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SenderService.Services
{
    public interface ISmsService
    {
        Task<int> Send(SmsDto dto);
    }
    public class SmsService : ISmsService
    {
        private readonly SmsCredential _credential;
        private readonly ISmsRepository _repository;
        public SmsService(IOptions<SmsCredential> smsOption, ISmsRepository repository)
        {
            _credential = smsOption.Value;
            _repository = repository;
        }

        public async Task<int> Send(SmsDto dto)
        {
            int statusCode = 400;
            try
            {
                string url = $"{_credential.ApiUrl}?Username={_credential.Username}&Password={_credential.Password}&From={_credential.From}&To=88{dto.MobileNumber}&Message={dto.Message}";
                using (var response = await HttpHelper.SendRequest(HttpMethod.Get, url))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var serializer = new XmlSerializer(typeof(ArrayOfServiceClass));
                        using (TextReader reader = new StringReader(result))
                        {
                            var res = (ArrayOfServiceClass)serializer.Deserialize(reader);
                            if (res.ServiceClass.StatusText == "success")
                            {
                                statusCode = 200;
                            }
                            string json = JsonConvert.SerializeObject(res);
                            await _repository.Insert(dto.MobileNumber, dto.Message, (int)response.StatusCode, dto.AppId, responseJson: json);
                        }
                    }
                    else
                    {
                        await _repository.Insert(dto.MobileNumber, dto.Message, (int)response.StatusCode, dto.AppId, result);
                    }
                }
            }
            catch(Exception ex)
            {
                await _repository.Insert(dto.MobileNumber, dto.Message, statusCode, dto.AppId, ex.ToString());
            }
            return await Task.FromResult(statusCode);
        }
    }
}
