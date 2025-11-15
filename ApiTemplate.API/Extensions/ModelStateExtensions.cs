using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiTemplate.API.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<string> GetErrors(this ModelStateDictionary modelstate)
        => modelstate.Values.SelectMany(values => values.Errors).Select(error => error.ErrorMessage);
    }
}
