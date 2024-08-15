using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;


namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler
{
    public class UpdateChildDetailCommandHandler : IRequestHandler<UpdateChildDetailCommand, bool>
    {
        public readonly IChildDetail _childDetailRepository;
        private readonly IParent _parentDetailRepository;

        public UpdateChildDetailCommandHandler(IChildDetail childDetailRepository, IParent parentDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
            _parentDetailRepository = parentDetailRepository;
        }
        public async Task<bool> Handle(UpdateChildDetailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.childInformation.FirstName) || string.IsNullOrEmpty(request.childInformation.LastName) ||
             !request.childInformation.Address.Any() || !request.childInformation.Parents.Any())
            {
                throw new InvalidOperationException("Please fill all the details.");
            }

            if (request.childInformation.DateOfBirth > DateTime.Now)
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
            existingChildDetail.FirstName = request.childInformation.FirstName;
            existingChildDetail.LastName = request.childInformation.LastName;
            existingChildDetail.Address = request.childInformation.Address;
            existingChildDetail.Parents = request.childInformation.Parents;
            existingChildDetail.Allergies = request.childInformation.Allergies;
            existingChildDetail.DateOfBirth = request.childInformation.DateOfBirth;

            // Update parent details if necessary
            foreach (var parent in request.childInformation.Parents)
            {
                parent.ChildId = existingChildDetail.Id; 
                parent.ChildFullName = $"{request.childInformation.FirstName} {request.childInformation.LastName}";
                await _parentDetailRepository.UpdateParentDetail(parent);
            }

            // Update the child detail in the repository
            await _childDetailRepository.UpdateChildDetail(existingChildDetail);
            return true;
        }
        }
}
