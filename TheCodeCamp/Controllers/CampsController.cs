using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers {
    [RoutePrefix("api/camps")]
    public class CampsController : BaseApiController {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get() {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetAllCampsAsync();
                var mappedResult = _mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(mappedResult);
            }
            return await ExecuteAsync(Function);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetCampAsync(moniker);
                if (result == null) {
                    return NotFound();
                }
                var mappedResult = _mapper.Map<CampModel>(result);
                return Ok(mappedResult);
            }
            return await ExecuteAsync(Function);
        }
    }
}
