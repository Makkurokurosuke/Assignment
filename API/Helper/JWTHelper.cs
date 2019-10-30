using Microsoft.IdentityModel.Tokens;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Common;

namespace API
{
    public class JWTHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<Claim> Authorize(string userName)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));

            string groupId = "piyo";
            claims.Add(new Claim(ClaimTypes.GroupSid, groupId));

            return claims;
        }

        /// <summary>
        /// Issue JWT token
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="expiresIn">Expiration</param>
        public static string GenerateToken(string userName)
        {
            //Container of token
            List<Claim> claims = Authorize(userName);

            var now = DateTime.UtcNow;
            long epochTime = (long)Math.Round((now.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            string jwtId = Guid.NewGuid().ToString();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, epochTime.ToString(), ClaimValueTypes.Integer64)); //Issued at

            DateTime expireDate = now + TimeSpan.FromSeconds(AuthConfig.ApiJwtExpirationSec);

            var jwt = new JwtSecurityToken(
               AuthConfig.ApiJwtIssuer, //issuer
               AuthConfig.ApiJwtAudience, //\
               claims, //\
               now, //beginning time
               expireDate, //
               new SigningCredentials(AuthConfig.ApiJwtSigningKey, SecurityAlgorithms.HmacSha256) //Credential for token
               );
            //create a token (token is encoded in base 64 plus signature
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

    }

}
