using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildQueryHandler
{
    public class GetAllChildDetailsQueryHandler : IRequestHandler<GetAllChildDetailsQuery, List<ChildInformation>>
    {
        public readonly ChildDetailRepository _childDetailRepository;

        public GetAllChildDetailsQueryHandler(ChildDetailRepository childDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
        }
        public async Task<List<ChildInformation>> Handle(GetAllChildDetailsQuery request, CancellationToken cancellationToken)
        {
            var childDetail= await _childDetailRepository.GetAllChildDetails();
            return childDetail.ToList();
        }
    }
}
