using System.ComponentModel.DataAnnotations;

namespace ApiTemplate.Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;  
        public int Count { get; set; }
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "Nome do produto é obrigatório")]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Count { get; set; }
    }
}
