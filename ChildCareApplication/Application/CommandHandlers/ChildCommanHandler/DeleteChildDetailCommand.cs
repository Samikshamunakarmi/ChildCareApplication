using Amazon.Runtime.Internal;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler
{
    public class DeleteChildDetailCommand :IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteChildDetailCommand(string id )
        {
            id = Id;
        }
    }
}
