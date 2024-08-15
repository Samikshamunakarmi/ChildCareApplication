using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildQueryHandler
{
    public class GetAllChildDetailsQueryHandler : IRequestHandler<GetAllChildDetailsQuery, List<ChildInformation>>
    {
        public readonly IChildDetail _childDetailRepository;

        public GetAllChildDetailsQueryHandler(IChildDetail childDetailRepository)
        {
            _childDetailRepository = childDetailRepository ?? throw new ArgumentNullException(nameof(childDetailRepository));
        }
        public async Task<List<ChildInformation>> Handle(GetAllChildDetailsQuery request, CancellationToken cancellationToken)
        {
            var childDetail= await _childDetailRepository.GetAllChildDetails();
            return childDetail;
        }
    }
}
