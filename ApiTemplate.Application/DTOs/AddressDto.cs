using System.ComponentModel.DataAnnotations;

namespace ApiTemplate.Application.DTOs
{
    public class AddressRegisterDto
    {
        [MaxLength(8)]
        [Required]
        public string Cep { get; set; } = string.Empty;
        [Required]
        public string Logradouro { get; set; } = string.Empty;
        [Required]
        public string Localidade { get; set; } = string.Empty;
        [Required]
        public string Complemento { get; set; } = string.Empty;
        [MaxLength(2)]
        [Required]
        public string Uf { get; set; } = string.Empty;
    }
}
