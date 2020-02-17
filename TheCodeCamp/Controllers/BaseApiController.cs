﻿using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace TheCodeCamp.Controllers {
    public abstract class BaseApiController : ApiController {
        protected async Task<IHttpActionResult> ExecuteAsync(Func<Task<IHttpActionResult>> executeAsync) {
            try {
                IHttpActionResult responce = await executeAsync();
                return responce;
            } catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}