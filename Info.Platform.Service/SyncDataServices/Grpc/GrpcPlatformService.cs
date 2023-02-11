using AutoMapper;
using Grpc.Core;
using Info.PlatformService.Data.PlatformRepository;

namespace Info.PlatformService.SyncDataServices.Grps;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository repository, IMapper mapper) 
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = _repository.GetAll();

        foreach(var platform in platforms)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }

        return Task.FromResult(response);
    }
}