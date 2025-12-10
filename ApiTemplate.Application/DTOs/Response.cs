using System.Text.Json.Serialization;

namespace ApiTemplate.Application.DTOs
{
    public class Response<TEntity>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TEntity? Data { get; protected set; } = default;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string>? Erros { get; private set; } = default;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public bool IsSuccess { get; set; } = true;

        public Response(TEntity data, List<string> erros)
        {
            Data = data;
            Erros = erros;
        }

        public Response(List<string> erros)
            => Erros = erros;
        public Response(TEntity data)
            => Data = data;
        public Response(string erro)
            => Erros = [erro];

        public static SuccessResponse ReturnSucess(string message)
            => new(message);
        public class SuccessResponse(string message)
        {
            public string Message { get; set; } = message;
        }

        public static Response<string> NotFound() => new("A consulta não retornou dados.");
    }
}
