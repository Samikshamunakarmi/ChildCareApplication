using ChildCareApplication.Domain;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildCommanHandler
{
    public class CreateChildDetailCommand:IRequest<bool>
    {
        public ChildInformation ChildInformation { get; set; }

        public CreateChildDetailCommand(ChildInformation _ChildInformation)
        {
            ChildInformation = _ChildInformation;
        }
    }
}
