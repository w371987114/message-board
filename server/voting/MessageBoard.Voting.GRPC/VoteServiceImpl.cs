using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using MessageBoard.Voting.Core;
using MessageBoard.Voting.Core.Commands;
using MessageBoard.Voting.Core.Events;
using MessageBoard.Voting.Core.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBoard.Voting.GRPC
{
    public class VoteServiceImpl : VoteService.VoteServiceBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public VoteServiceImpl(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public override async Task<VoteResponse> Add(AddRequest request, ServerCallContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                //TODO: refactor this command
                //It needs to be only one command doing the same
                var vote = await mediator.Send(new AddVoteCommand(request.SubjectId, request.OptionName));
                await mediator.Publish(VoteCreated.From(vote));

                return new VoteResponse
                {
                    Count = vote.Count,
                    OptionName = vote.OptionName,
                };
            }
        }

        public override async Task<LoadResponse> Load(LoadRequest request, ServerCallContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var votes = await mediator.Send(new VoteCountQuery(request.SubjectId));

                var response = new LoadResponse();
                response.Votes.AddRange(votes.Select(ToResponse));
                return response;
            }
        }

        public override async Task<LoadBatchResponse> LoadBatch(LoadBatchRequest request, ServerCallContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var votes = await mediator.Send(new VoteCountBatchQuery(request.SubjectId));

                var votesBySubject = votes
                    .GroupBy(v => v.SubjectId)
                    .Select(group =>
                    {
                        var votesResponse = group.Select(vote => new VoteResponse
                        {
                            Count = vote.Count,
                            OptionName = vote.OptionName
                        });

                        var list = new LoadBatchResponse.Types.SubjectListResponse();
                        list.SubjectId = group.Key;
                        list.Votes.Add(votesResponse);
                        return list;
                    });

                var response = new LoadBatchResponse();
                response.Votes.Add(votesBySubject);
                return response;
            }
        }

        private VoteResponse ToResponse(Vote vote) => new VoteResponse
        {
            Count = vote.Count,
            OptionName = vote.OptionName,
        };
    }
}