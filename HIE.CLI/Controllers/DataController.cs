using HIE.CLI.Records;
using HIE.CLI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HIE.CLI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IEntryDataStore _store;

        public DataController(IEntryDataStore store) => _store = store;

        
        [HttpGet]
        public InventoryEntryDto[] Get()
        {
            return _store
                .GetValidEntries()
                .Select(entry => entry.ToDto())
                .ToArray();
        }
    }
}
