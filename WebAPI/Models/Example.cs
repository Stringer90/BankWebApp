using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Example
    {
        //[Required(ErrorMessage = "The 'id' field is required.")]
        public int? id { get; set; }
        //[Required(ErrorMessage = "The 'name' field is required.")]
        public string? name { get; set; }
    }
}
