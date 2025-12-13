namespace ApiTemplate.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;  
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;

        public User User { get; set; } = new();
        public Guid UserId { get; set; }
    }
}
