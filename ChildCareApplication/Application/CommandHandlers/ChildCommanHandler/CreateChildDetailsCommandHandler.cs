﻿using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler

{
    public class CreateChildDetailsCommandHandler : IRequestHandler<ChildInformation, bool>
    {
        private readonly IMongoClient _mongoClient;
        public readonly IChildDetail _childDetailRepository;
        public readonly IParent _parentDetailRepository;

        public CreateChildDetailsCommandHandler(IChildDetail childDetailRepository, IParent parentDetailRepository, IMongoClient mongoClient)
        {
            _childDetailRepository = childDetailRepository;
            _parentDetailRepository = parentDetailRepository;
            _mongoClient = mongoClient;
        }
        public async Task<bool> Handle(ChildInformation request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.FirstName) || String.IsNullOrEmpty(request.LastName) ||
            request.Address.Count()<0 || request.Parents.Count() < 0)

            {
                throw new InvalidOperationException("Please fill all the details");
            }

            if (request.DateOfBirth > DateTime.Now)
            {
                throw new InvalidOperationException("Date of Birth cannot be in the future.");
            }

            using (var session = await _mongoClient.StartSessionAsync())
            {

                session.StartTransaction();

                try
                {
                   
                    var childDetail = new ChildInformation
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        DateAdded = DateTime.Now,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Address = request.Address,
                        Allergies = request.Allergies,
                        DateOfBirth = request.DateOfBirth
                    };

                   
                    await _childDetailRepository.CreateChildDetail(childDetail);

                    
                    var parentDetails = request.Parents.Select(parent => new ParentDetail
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

                    // Update the child detail with parent references if needed (depends on your data model)
                    childDetail.Parents = parentDetails;

                    await _childDetailRepository.UpdateChildDetail(childDetail);

                    // Commit transaction
                    await session.CommitTransactionAsync();
                }
                catch
                {
                    // Abort transaction in case of an error
                    await session.AbortTransactionAsync();
                    throw;
                }
            }

            return true;
        }

            
        
    }
}
