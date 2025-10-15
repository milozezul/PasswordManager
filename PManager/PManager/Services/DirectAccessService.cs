using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Configs;
using SharedModels.APIs.Data.Outputs;
using SharedModels.APIs.DirectAccess.Input;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PManager.Services
{
    public class DirectAccessService : IDirectAccessService
    {
        PManagerDbContext _context;
        IDataService _dataService;
        IEncryptionService _encryptService;
        IOptions<JWTConfigs> _config;
        public DirectAccessService(PManagerDbContext context, IDataService dataService, IEncryptionService encryptService, IOptions<JWTConfigs> config)
        {
            _context = context;
            _dataService = dataService;
            _encryptService = encryptService;
            _config = config;
        }

        public async Task<DecryptedPasswordOutput?> GetDirectPassword(string password)
        {
            var record = await _dataService.GetRecordById(_dataService.GetRecordId());

            if (record == null) return null;

            var foundPassword = await _context.RecordPasswords
                .Where(r => r.RecordId == record.Id && r.PasswordId == _dataService.GetPasswordId())
                .Select(p => p.Password)
                .FirstAsync();

            var decryptedPassword = new DecryptedPasswordOutput()
            {
                Id = foundPassword.Id,
                Value = foundPassword.IsActive == true ? Encoding.UTF8.GetString(_encryptService.DecryptWithPassword(foundPassword.Value, password)) : "DIACTIVATED"
            };

            if (string.IsNullOrEmpty(decryptedPassword.Value))
            {
                decryptedPassword.Value = _dataService.GetFallback();
            }

            return decryptedPassword;
        }

        public string GetDirectLinkToken(LinkInput input)
        {
            int issuerId = _dataService.GetUserId();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, issuerId.ToString()),
                new Claim("rid", input.RecordId.ToString()),
                new Claim("pid", input.PasswordId.ToString()),
                new Claim("fallback", input.Fallback),
                new Claim(ClaimTypes.Role, "Link")
            };

            string strKey = _config.Value.Secret;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred
            );

            string bearer = new JwtSecurityTokenHandler().WriteToken(token);
            return bearer;
        }
    }
}
