using System.ComponentModel.DataAnnotations;

namespace Erpinfo.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}