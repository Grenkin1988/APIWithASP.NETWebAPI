using AutoMapper;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers {
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : BaseApiController {
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

        [Route("{id:int}", Name = "GetTalk")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeSpeakers = false) {
            async Task<IHttpActionResult> Function() {
                var result = await _repository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);
                var mappedResult = _mapper.Map<TalkModel>(result);
                return Ok(mappedResult);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalkModel model) {
            async Task<IHttpActionResult> Function() {
                if (ModelState.IsValid) {
                    var camp = await _repository.GetCampAsync(moniker);
                    if (camp != null) {
                        var talk = _mapper.Map<Talk>(model);
                        talk.Camp = camp;

                        // Map the speaker if necessary
                        if (model.Speaker != null) {
                            var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                            talk.Speaker = speaker;
                        }

                        _repository.AddTalk(talk);

                        if (await _repository.SaveChangesAsync()) {
                            var newModel = _mapper.Map<TalkModel>(talk);
                            return CreatedAtRoute(
                                "GetTalk",
                                new { moniker = moniker, id = newModel.TalkId },
                                newModel);
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(string moniker, int id, TalkModel model) {
            async Task<IHttpActionResult> Function() {
                if (ModelState.IsValid) {
                    var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                    if (talk == null) {
                        return NotFound();
                    }

                    // It's going to ignore speker
                    _mapper.Map(model, talk);

                    // Change speaker if needed
                    if (model.Speaker != null && talk?.Speaker?.SpeakerId != model.Speaker.SpeakerId) {
                        var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                        if (speaker != null) {
                            talk.Speaker = speaker;
                        }
                    }

                    if (await _repository.SaveChangesAsync()) {
                        var newModel = _mapper.Map<TalkModel>(talk);
                        return Ok();
                    } else {
                        return InternalServerError();
                    }
                }
                return BadRequest(ModelState);
            }
            return await SafeExecuteAsync(Function);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(string moniker, int id) {
            async Task<IHttpActionResult> Function() {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) {
                    return NotFound();
                }

                _repository.DeleteTalk(talk);

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
