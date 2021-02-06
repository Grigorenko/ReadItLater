using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Data.EF.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ReadItLater.Web.Server.Controllers
{
    public class RefsController : BaseController
    {
        private readonly IDapperContext dapperContext;
        private readonly IDapperContext<Ref> refContext;

        public RefsController(IDapperContext dapperContext, IDapperContext<Ref> refContext)
        {
            this.dapperContext = dapperContext;
            this.refContext = refContext;
        }

        [HttpGet("{id:guid}")]
        public async Task<Ref> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await ExecStoredProcedureSingleWithMapping("SelectRefById", new { id }, cancellationToken);
        }

        [HttpGet]
        public async Task<IEnumerable<Ref>> Get(Guid? folderId, Guid? tagId, int offset, int limit, string sort, CancellationToken cancellationToken)
        {
            return await ExecStoredProcedureWithMapping(
                "SelectRefs",
                new
                {
                    folderId,
                    tagId,
                    offset,
                    limit,
                    sort = sort.ToSortUdt()
                },
                cancellationToken);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Ref>> Search(Guid? folderId, Guid? tagId, string searchTerm, int offset, int limit, string sort, CancellationToken cancellationToken)
        {
            return await ExecStoredProcedureWithMapping(
                "SearchRefs",
                new
                {
                    folderId,
                    tagId,
                    searchTerm,
                    offset,
                    limit,
                    sort = sort.ToSortUdt()
                },
                cancellationToken);
        }

        [HttpPost]
        public async Task Create([FromBody] Ref @ref, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("CreateRef", new { @ref = @ref.ToUdt(), tags = @ref.Tags.ToUdt() }, cancellationToken);
        }

        [HttpPut]
        public async Task Update([FromBody] Ref @ref, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("UpdateRef", new { @ref = @ref.ToUdt(), tags = @ref.Tags.ToUdt() }, cancellationToken);
        }

        [HttpPatch("{refId:guid}")]
        public async Task UpdateCountOfView([FromRoute] Guid refId, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("UpdateCountOfView", new { refId }, cancellationToken);
        }

        [HttpDelete("{id:guid}")]
        public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await dapperContext.ExecuteProcedureAsync("DeleteRef", new { id }, cancellationToken);
        }

        private async Task<Ref> ExecStoredProcedureSingleWithMapping(string command, object param, CancellationToken cancellationToken)
        {
            var results = await ExecStoredProcedureWithMapping(command, param, cancellationToken);

            return results.FirstOrDefault();
        }

        private async Task<IEnumerable<Ref>> ExecStoredProcedureWithMapping(string command, object param, CancellationToken cancellationToken)
        {
            var lookup = new Dictionary<Guid, Ref>();

            await refContext.SelectAsync<Tag>(
                command,
                map: Map(),
                param: param,
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            return lookup.Values;

            Func<Ref, Tag, Ref> Map() => (r, t) =>
            {
                Ref current = null;

                if (!lookup.TryGetValue(r.Id, out current))
                    lookup.Add(r.Id, current = r);

                if (t != null)
                {
                    if (current.Tags is null)
                        current.Tags = new List<Tag>();

                    current.Tags.Add(t);
                }

                return r;
            };
        }
    }
}
