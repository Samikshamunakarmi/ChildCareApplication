using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildQueryHandler
{
    public class GetChildDetailByIdQueryHandler : IRequestHandler<GetAllChildDetailsQueryByID, ChildInformation>
    {
        public readonly IChildDetail _childDetailRepository;

        public GetChildDetailByIdQueryHandler(IChildDetail childDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
        }
        public async Task<ChildInformation> Handle(GetAllChildDetailsQueryByID request, CancellationToken cancellationToken)
        {
            var childDetail = await _childDetailRepository.GetChildDetailByIdAsync(request.Id);
            return childDetail;
        }
    }
}
