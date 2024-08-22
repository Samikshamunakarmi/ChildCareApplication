using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler

{
    public class CreateChildDetailsCommandHandler : IRequestHandler<CreateChildDetailCommand, bool>
    {
       
        public readonly IChildDetail _childDetailRepository;
        public readonly IParent _parentDetailRepository;

        public CreateChildDetailsCommandHandler(IChildDetail childDetailRepository, IParent parentDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
            _parentDetailRepository = parentDetailRepository;
            
        }
        public async Task<bool> Handle(CreateChildDetailCommand request, CancellationToken cancellationToken)
        {
            // Validation logic
            if (string.IsNullOrEmpty(request.ChildInformation.FirstName) || string.IsNullOrEmpty(request.ChildInformation.LastName) ||
                !request.ChildInformation.Address.Any() || !request.ChildInformation.Parents.Any())
            {
                throw new InvalidOperationException("Please fill all the details.");
            }

            if (request.ChildInformation.DateOfBirth > DateTime.Now)
            {
                throw new InvalidOperationException("Date of Birth cannot be in the future.");
            }

            try
                {
                    // Create child detail
                    var childDetail = new ChildInformation
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        DateAdded = DateTime.Now,
                        FirstName = request.ChildInformation.FirstName,
                        LastName = request.ChildInformation.LastName,
                        Address = request.ChildInformation.Address,
                        Allergies = request.ChildInformation.Allergies,
                        DateOfBirth = request.ChildInformation.DateOfBirth
                    };

                    await _childDetailRepository.CreateChildDetail(childDetail);

                    // Create parent details
                    var parentDetails = request.ChildInformation.Parents.Select(parent => new ParentDetail
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        FirstName = parent.FirstName,
                        LastName = parent.LastName,
                        ContactNumber = parent.ContactNumber,
                        Address = parent.Address,
                        Relationship = parent.Relationship,
                        DateOfBirth = parent.DateOfBirth,
                        ChildId = childDetail.Id,
                        ChildFullName = $"{childDetail.FirstName} {childDetail.LastName}",
                        DateAdded = DateTime.Now
                    }).ToList();

                    // Batch insert parent details
                    await _parentDetailRepository.CreateParentDetailsBatch(parentDetails);

                    // Optionally update child detail with parent references
                    childDetail.Parents = parentDetails;
                    await _childDetailRepository.UpdateChildDetail(childDetail);

                  
                }
                catch (Exception ex)
                {
                    // Log and handle the exception
                    throw;
                }



            return true;
        }



    }
}
