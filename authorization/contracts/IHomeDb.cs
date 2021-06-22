using authorization.DB.Models;
using authorization.persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization.contracts
{
    public interface IHomeDb
    {
        Task UpsertCode(string client_id, int code);
        Task<Code> GetCodeDbEntity(string client_id);
        Task UpdateToken(string client_id, TokenUpdateDbEntity tokenEntity);
    }
}
