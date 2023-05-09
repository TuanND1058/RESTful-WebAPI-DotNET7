using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaNumbercreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string SepcialDetails { get; set; }
    }
}
