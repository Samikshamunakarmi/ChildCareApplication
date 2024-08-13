using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildQueryHandler
{
    public class GetAllChildDetailsQuery : IRequest<List<ChildInformation>>
    {

    }
}
