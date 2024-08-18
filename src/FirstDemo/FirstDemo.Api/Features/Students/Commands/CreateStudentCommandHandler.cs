using MediatR;

namespace FirstDemo.Api.Features.Students.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand>
    {
        public Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
