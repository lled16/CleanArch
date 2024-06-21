using CleanArchMvc.API.Models;
using CleanArchMvc.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authenticate;
        private readonly IConfiguration _configuration;

        public TokenController(IAuthenticate authenticate, IConfiguration configuration)
        {
            _authenticate = authenticate ?? throw new ArgumentNullException(nameof(authenticate));
            _configuration = configuration;
        }

        [HttpPost("CreateUser")]
        [ApiExplorerSettings(IgnoreApi = true)] //usado para não exibir no swagger este endpoint
        public async Task<ActionResult> CreateUser([FromBody] LoginModel loginModel)
        {
            var result = await _authenticate.RegisterUser(loginModel.Email, loginModel.Password);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, " Invalid Login attempt.");
                return BadRequest();
            }
        }


        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel loginModel)
        {
            var result = await _authenticate.Authenticate(loginModel.Email, loginModel.Password);

            if (result)
            {
                return GenerateToken(loginModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                return BadRequest(ModelState);
            }

        }

        private ActionResult<UserToken> GenerateToken(LoginModel loginModel)
        {
            //declarações do usuário
            var claims = new[]
            {
                new Claim("email", loginModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gerar chave privada para o token
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            //gerar a assinatura

            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            //define o tempo de expiração

            var expiration = DateTime.UtcNow.AddMinutes(10);

            //ggerar o token

            JwtSecurityToken token = new JwtSecurityToken
                (

                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["JwtSecurityToken: Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials

                );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
