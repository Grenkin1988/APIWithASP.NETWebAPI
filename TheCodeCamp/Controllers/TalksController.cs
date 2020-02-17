using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : BaseApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetTalksByMonikerAsync(moniker, includeSpeakers);
                var mappedResult = _mapper.Map<TalkModel[]>(result);
                return Ok(mappedResult);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeSpeakers = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);
                var mappedResult = _mapper.Map<TalkModel>(result);
                return Ok(mappedResult);
            }
            return await SafeExecuteAsync(Function);
        }
    }
}
