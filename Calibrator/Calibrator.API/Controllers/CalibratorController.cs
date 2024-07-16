using Calibrator.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Calibrator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalibratorController : Controller
    {
        private ExperimentManager _manager;

        public CalibratorController(ExperimentManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public void ActuatorFlag(bool flag)
        {
            _manager.IsActuatorReady = flag;
        }
    }
}
