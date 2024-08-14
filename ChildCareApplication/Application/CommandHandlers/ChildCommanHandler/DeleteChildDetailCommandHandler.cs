using ChildCareApplication.Infrastructure.Repositories;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler
{
    public class DeleteChildDetailCommandHandler : IRequestHandler<DeleteChildDetailCommand, bool>
    {
        private readonly ChildDetailRepository _childDetailRepository;

        public DeleteChildDetailCommandHandler(ChildDetailRepository childDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
        }
        public async Task<bool> Handle(DeleteChildDetailCommand request, CancellationToken cancellationToken)
        {
          
            var existingChildDetail = await _childDetailRepository.GetChildDetailByIdAsync(request.Id);
            if (existingChildDetail == null)
            {
                throw new InvalidOperationException("Child detail not found.");
            }

           
            await _childDetailRepository.DeleteChildDetail(request.Id);
            return true;
        }
    }
}
