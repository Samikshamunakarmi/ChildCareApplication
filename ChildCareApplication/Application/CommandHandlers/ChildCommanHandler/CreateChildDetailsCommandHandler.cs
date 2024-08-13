using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;
using MongoDB.Bson;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler

{
    public class CreateChildDetailsCommandHandler : IRequestHandler<ChildInformation, bool>
    {
        public readonly ChildDetailRepository _childDetailRepository;

        public CreateChildDetailsCommandHandler(ChildDetailRepository childDetailRepository)
        {
            _childDetailRepository = childDetailRepository;
        }
        public async Task<bool> Handle(ChildInformation request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.FirstName) || String.IsNullOrEmpty(request.LastName) ||
            request.Address.Count()>0 || request.Parents.Count() >0)

            {
                throw new InvalidOperationException("Please fill all the details");
            }

            if (request.DateOfBirth > DateTime.Now)
            {
                throw new InvalidOperationException("Date of Birth cannot be in the future.");
            }
            var childDetail = new ChildInformation
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DateAdded = DateTime.Now,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Address = request.Address,
                    Parents = request.Parents,
                    Allergies = request.Allergies,
                    DateOfBirth = request.DateOfBirth
            };

                await _childDetailRepository.CreateChildDetail(childDetail);
                return true;
            
        }
    }
}
