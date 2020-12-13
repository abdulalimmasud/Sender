using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.ViewModel
{
    public class AttachmentInsertionDto
    {
        [Required, Range(1, Int32.MaxValue)]
        public int SimCardId { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int AssetId { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int AppId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int UserId { get; set; }
    }
    public class AttachmentDto
    {
        public int SimCardId { get; set; }
        public string MobileNumber { get; set; }
        public int AssetId { get; set; }
        public int AppId { get; set; }
        public string AppName { get; set; }
    }
    public class AttachmentHistoryDto : AttachmentDto
    {
        public string SimNumber { get; set; }
        public DateTime LogTime { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public int LogType { get; set; }
        public string Status
        {
            get
            {
                return LogType == 1 ? "SimDeviceAttach " : LogType == 2 ? "SimDeviceDetach " : "";
            }
        }
    }
    public class AttachmentResponseShortDto: AttachmentDto
    {
        public string SimNumber { get; set; }
        public int IsActive { get; set; }
    }
}
