using AutoMapper;
using System;
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
        public async Task<IHttpActionResult> Get(bool includeTalks = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetAllCampsAsync(includeTalks);
                var mappedResult = _mapper.Map<CampModel[]>(result);
                return Ok(mappedResult);
            }
            return await ExecuteAsync(Function);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetCampAsync(moniker, includeTalks);
                if (result == null) {
                    return NotFound();
                }
                var mappedResult = _mapper.Map<CampModel>(result);
                return Ok(mappedResult);
            }
            return await ExecuteAsync(Function);
        }

        [Route("searchByDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);
                var mappedResult = _mapper.Map<CampModel[]>(result);
                return Ok(mappedResult);
            }
            return await ExecuteAsync(Function);
        }
    }
}
