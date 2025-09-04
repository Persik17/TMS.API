using Microsoft.AspNetCore.Http;

namespace TMS.Application.Dto.Task
{
    public class UploadTaskFileFormDto
    {
        public IFormFile File { get; set; }
        public Guid UserId { get; set; }
    }
}
