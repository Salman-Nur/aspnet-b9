using Autofac;
using AutoMapper;
using FirstDemo.Api.Features.Students.Commands;
using FirstDemo.Api.Features.Students.Queries;
using FirstDemo.Api.Models;
using FirstDemo.Api.RequestHandlers;
using FirstDemo.Domain.Entities;
using FirstDemo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FirstDemo.API.Controllers
{
    [ApiController]
    [Route("v3/[controller]")]
    [EnableCors("AllowSites")]
    public class StudentController : ControllerBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<CourseController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public StudentController(ILogger<CourseController> logger, ILifetimeScope scope,
            IMediator mediator)
        {
            _logger = logger;
            _scope = scope;
            _mediator = mediator;
        }


        [HttpPost, Authorize(Policy = "CourseViewRequirementPolicy")]
        public ActionResult Post(CreateStudentModel model)
        {
            var command = _mapper.Map<CreateStudentModel, CreateStudentCommand>(model);
            _mediator.Send(command);

            return Ok();
        }



        [HttpGet("{id}")]
        public Student Get(Guid id)
        {
            var query = new GetStudentByIdQuery(id);
            var handler = new GetStudentByIdQueryHandler();

            return handler.Handle(query);
        }
    }
}