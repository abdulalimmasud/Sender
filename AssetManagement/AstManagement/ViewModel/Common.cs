using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.ViewModel
{
    public class EntityList<T> where T : class
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
    public class ResponseResult
    {
        public int StatusCode { get; set; }
        public dynamic Message { get; set; }
    }
    public class WrongInput
    {
        public string Key { get; set; }
        public string Cause { get; set; }
    }
    public class Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
