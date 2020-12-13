using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.ViewModel
{
    public class SimCardRegistrationDto: SimCardOpertorDto
    {
        [Required, StringLength(11, ErrorMessage = "Mobile Number must be 11 characters.")]
        public string MobileNumber { get; set; }
        [Required, StringLength(50, ErrorMessage = "Sim Number must be within 50 characters.")]
        public string SimNumber { get; set; }        
    }
    public class SimCardImportDto: SimCardOpertorDto
    {
        [Required, DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
    public class SimCardOpertorDto
    {
        [Required, Range(1, int.MaxValue)]
        public int OperatorId { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int OperatorPackageId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
    public class SimCardResponseDto: SimCardResponseShortDto
    {        
        public int AssetId { get; set; }
        public int OperatorId { get; set; }
        public string Operator { get; set; }
        public int OperatorPackageId { get; set; }
        public string OperatorPackage { get; set; }
        public DateTime CreationTime { get; set; }
        public string Creator { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Updator { get; set; }
    }
    public class SimCardResponseShortDto
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string SimNumber { get; set; }
    }
}
