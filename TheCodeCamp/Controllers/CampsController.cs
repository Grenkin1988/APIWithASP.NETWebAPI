using AutoMapper;
using Microsoft.Web.Http;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers {
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [RoutePrefix("api/v{version:apiVersion}/camps")]
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
            return await SafeExecuteAsync(Function);
        }

        [MapToApiVersion("1.0")]
        [Route("{moniker}", Name = "GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetCampAsync(moniker, includeTalks);
                if (result == null) {
                    return NotFound();
                }
                var mappedResult = _mapper.Map<CampModel>(result);
                return Ok(mappedResult);
            }
            return await SafeExecuteAsync(Function);
        }

        [MapToApiVersion("1.1")]
        [Route("{moniker}", Name = "GetCamp11")]
        public async Task<IHttpActionResult> Get(string moniker) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetCampAsync(moniker, true);
                if (result == null) {
                    return NotFound();
                }
                var mappedResult = _mapper.Map<CampModel>(result);
                return Ok(mappedResult);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route("searchByDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);
                var mappedResult = _mapper.Map<CampModel[]>(result);
                return Ok(mappedResult);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route()]
        public async Task<IHttpActionResult> Post(CampModel model) {
            async Task<IHttpActionResult> Function() {
                if (await _repository.GetCampAsync(model.Moniker) != null) {
                    ModelState.AddModelError(nameof(model.Moniker), "Moniker in use");
                }

                if (ModelState.IsValid) {
                    var camp = _mapper.Map<Camp>(model);
                    _repository.AddCamp(camp);

                    if (await _repository.SaveChangesAsync()) {
                        var newModel = _mapper.Map<CampModel>(camp);
                        return CreatedAtRoute(
                            "GetCamp",
                            new { moniker = newModel.Moniker },
                            newModel);
                    }
                }
                return BadRequest(ModelState);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel model) {
            async Task<IHttpActionResult> Function() {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) {
                    return NotFound();
                }
                if (ModelState.IsValid) {
                    _mapper.Map(model, camp);

                    if (await _repository.SaveChangesAsync()) {
                        var mappedResult = _mapper.Map<CampModel>(camp);
                        return Ok(mappedResult);
                    } else {
                        return InternalServerError();
                    }
                }
                return BadRequest(ModelState);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker) {
            async Task<IHttpActionResult> Function() {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) {
                    return NotFound();
                }

                _repository.DeleteCamp(camp);

                if (await _repository.SaveChangesAsync()) {
                    return Ok();
                } else {
                    return InternalServerError();
                }
            }
            return await SafeExecuteAsync(Function);
        }
    }
}
