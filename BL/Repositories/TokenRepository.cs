using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Security;

namespace BL.Repositories
{
    public interface ITokenRepository
    {
        public string GetToken(string secureKey);
    }

    public class TokenRepository : ITokenRepository
    {
        public string GetToken(string secureKey)
        {
            try
            {
                string serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);
                return serializedToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
