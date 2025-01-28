using MediatR;
using Telegram.Bot.Types;

namespace Application.Features.GetPosts;

public record GetPostsQuery(int Offset) : IRequest<List<Update>>;