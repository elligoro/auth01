using authorization.persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorization.contracts
{
    public interface IHomeDb
    {
        Task UpsertCode(int code,string client_id);
        Task<Code> GetCodeDbEntity(string client_id);
    }
}
