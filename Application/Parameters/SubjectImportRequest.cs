using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Parameters
{
    public class SubjectImportRequest
    {
        [Required]
        public IFormFile? File { get; set; }
    }

}
