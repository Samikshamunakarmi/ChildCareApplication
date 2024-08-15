using ChildCareApplication.Domain;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler
{
    public class UpdateChildDetailCommand:IRequest<bool>
    {
        public string Id { get;}

        public ChildInformation childInformation { get; }

        public UpdateChildDetailCommand(string _Id, ChildInformation _childInformation)
        {
            Id = _Id;
            childInformation = _childInformation;
        }
    }
}
