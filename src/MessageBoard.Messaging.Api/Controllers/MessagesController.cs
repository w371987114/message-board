﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MessageBoard.Messaging.Api.Models;
using MessageBoard.Messaging.Core;
using MessageBoard.Messaging.Core.Commands;
using MessageBoard.Messaging.Core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoard.Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Message>> Get([FromQuery]uint? from)
        {
            var result = await _mediator.Send(new PaginatedMessagesQuery(from));
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateMessageModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateMessageCommand(model.Text));
            return Ok(result);
        }
    }
}
