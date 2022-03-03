using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hello_world.Controllers
{
    [ApiController]
    [Route("script")]
    public class ScriptController : ControllerBase
    {
        private readonly ILogger<ScriptController> _logger;

        public ScriptController(ILogger<ScriptController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            try
            {
               string results = "";
                using (PowerShell ps = PowerShell.Create())
                {
                    var cmds = System.IO.File.ReadAllLines("script.txt");
                    foreach (var cmd in cmds)
                        ps.AddScript(cmd);

                    var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);
                    foreach (var item in pipelineObjects)
                    {
                        results += item.BaseObject.ToString() + Environment.NewLine;
                    }
                }

                return new JsonResult(new {Code = "SUCCESS", Msg = results});
            }
            catch (Exception ex)
            {
                return new JsonResult(new {Code = "FAILED", Msg = ex.ToString()});
            }
        }
    }
}