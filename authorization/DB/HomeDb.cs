using authorization.contracts;
using authorization.DB.Models;
using authorization.persistance;
using System;
using System.Threading.Tasks;

namespace persistence
{
    public class HomeDb : IHomeDb
    {
        private readonly AuthContext _context;

        public HomeDb(AuthContext context)
        {
            _context = context;
        }
        public async Task UpsertCode(string client_id, int code)
        {
            var dbcodeEntity = await _context.Codes.FindAsync(client_id);
            if (dbcodeEntity is null)
                _context.Codes.Add(new Code
                {
                    code = code,
                    client_id = client_id
                });

            else
                dbcodeEntity.code = code;

            await _context.SaveChangesAsync();
        }

        public async Task<Code> GetCodeDbEntity(string client_id)
        {
             return await _context.Codes.FindAsync(client_id);
        }

        public async Task UpdateToken(string client_id, TokenUpdateDbEntity tokenEntity)
        {
            var codeEntity = await _context.Codes.FindAsync(client_id);
            codeEntity.AccessToken = tokenEntity.access_token;
            codeEntity.RefreshToken = tokenEntity.refresh_token;

            _context.Codes.Update(codeEntity);
            await _context.SaveChangesAsync();
        }
    }
}
