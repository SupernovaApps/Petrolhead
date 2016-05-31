using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;

namespace Petrolhead.Backend
{
    public class SimpleMappedEntityDomainManager<TData, TModel>
   : MappedEntityDomainManager<TData, TModel>
        where TData : class, ITableData
        where TModel : class, ITableData
    {
        
        public SimpleMappedEntityDomainManager(DbContext context,
            HttpRequestMessage request)
            : base(context, request)
        {
           
        }
        public override SingleResult<TData> Lookup(string id)
        {
            return this.LookupEntity(p => p.Id == id);
        }
        public override Task<TData> UpdateAsync(string id, Delta<TData> patch)
        {
            return this.UpdateEntityAsync(patch, id);
        }
        public override Task<bool> DeleteAsync(string id)
        {
            return this.DeleteItemAsync(id);
        }
    }
}