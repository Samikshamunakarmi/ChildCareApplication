using ChildCareApplication.Domain;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers.ChildQueryHandler
{
    public class GetAllChildDetailsQueryByID:IRequest<ChildInformation>
    {
        public string Id { get; set; }

        public GetAllChildDetailsQueryByID(string id)
        {
            Id = id;
        }
    }
}
