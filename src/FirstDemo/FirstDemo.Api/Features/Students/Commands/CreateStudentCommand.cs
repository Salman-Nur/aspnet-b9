
using FirstDemo.Application.Features;
using MediatR;

namespace FirstDemo.Api.Features.Students.Commands
{
    public class CreateStudentCommand : IRequest
    {
        public string Name { get; set; }
        public double CGPA { get; set; }
    }
}
