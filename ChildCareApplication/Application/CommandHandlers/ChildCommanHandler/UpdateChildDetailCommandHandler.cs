using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;


namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler
{
    public class UpdateChildDetailCommandHandler : IRequestHandler<ChildInformation, bool>
    {
        public readonly ChildDetailRepository _childDetailRepository;
        private readonly ParentDetailRepository _parentDetailRepository;

        public UpdateChildDetailCommandHandler(ChildDetailRepository childDetailRepository, ParentDetailRepository parentDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
            _parentDetailRepository = parentDetailRepository;
        }
        public async Task<bool> Handle(ChildInformation request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) ||
             !request.Address.Any() || !request.Parents.Any())
            {
                throw new InvalidOperationException("Please fill all the details.");
            }

            if (request.DateOfBirth > DateTime.Now)
            {
                throw new InvalidOperationException("Date of Birth cannot be in the future.");
            }

            // Retrieve the existing child detail
            var existingChildDetail = await _childDetailRepository.GetChildDetailByIdAsync(request.Id);
            if (existingChildDetail == null)
            {
                throw new InvalidOperationException("Child detail not found.");
            }

            // Update the child detail with the new information
            existingChildDetail.FirstName = request.FirstName;
            existingChildDetail.LastName = request.LastName;
            existingChildDetail.Address = request.Address;
            existingChildDetail.Parents = request.Parents;
            existingChildDetail.Allergies = request.Allergies;
            existingChildDetail.DateOfBirth = request.DateOfBirth;

            // Update parent details if necessary
            foreach (var parent in request.Parents)
            {
                parent.ChildId = existingChildDetail.Id; 
                parent.ChildFullName = $"{request.FirstName} {request.LastName}";
                await _parentDetailRepository.UpdateParentDetail(parent);
            }

            // Update the child detail in the repository
            await _childDetailRepository.UpdateChildDetail(existingChildDetail);
            return true;
        }
        }
}
