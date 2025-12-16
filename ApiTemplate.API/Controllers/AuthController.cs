using ApiTemplate.API.Extensions;
using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        ///     Registra um novo usuário
        /// </summary>
        /// <remarks>
        ///     Registra um novo usuário com as 
        ///     informações fornecidas no corpo da requisição.
        /// </remarks>        
        /// <returns>Um produto</returns>
        /// <response code="200">Retorna o usuario criado</response>        
        /// <response code="500">Erro interno.</response>
        [ProducesResponseType(typeof(Response<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new Response<IEnumerable<string>>(ModelState.GetErrors()));

                var response = await authService.RegisterAsync(registerDto);
                return Ok(new Response<AuthResponseDto>(response));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new Response<string>(ex.Message));
            }
        }

        /// <summary>
        ///     Realiza o login de um usuário
        /// </summary>
        /// <remarks>
        ///    Autentica o usuário
        /// </remarks>        
        /// <returns>Um produto</returns>
        /// <response code="200">Retorna o token do usuário</response>    
        /// <response code="401">Credenciais Inválidas</response>    
        /// <response code="500">Erro interno.</response>
        [ProducesResponseType(typeof(Response<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new Response<IEnumerable<string>>(ModelState.GetErrors()));

                var response = await authService.LoginAsync(loginDto);

                return Ok(new Response<AuthResponseDto>(response));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new Response<string>(ex.Message));
            }
        }
    }
}
