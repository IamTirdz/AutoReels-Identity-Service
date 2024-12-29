using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoReels.Studio.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        private IMapper _mapper = null!;

        public ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        public IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}
