using Common.Constants;
using Common.ViewModels;
using Dapper;
using DataAccessLayer.Persistance.Dapper;
using DataAccessLayer.Repositories.Interface;
using Entities;
using System.Data;

namespace DataAccessLayer.Repository.DapperRepository
{
    public class DapperDiscussionRepository : IDiscussionRepository
    {
        public readonly DapperContext _dapperContext;

        public DapperDiscussionRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task AddDiscussion(Discussion discussion)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", discussion.Id);
                parameters.Add("@DiscussionText", discussion.DiscussionText);
                parameters.Add("@MovieId", discussion.MovieId);
                parameters.Add("@UserId", discussion.UserId);
                parameters.Add("@Created", discussion.Created);

                await connection.ExecuteAsync(StoredProcedureName.DiscussionInsert, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteDiscussion(Guid DiscussionId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", DiscussionId);

                await connection.ExecuteAsync(StoredProcedureName.DiscussionDelete, parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<DiscussionViewModel>> GetDiscussion(Guid MovieId, int skipcount, int size)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MovieId", MovieId);
                parameters.Add("@skipcount", skipcount);
                parameters.Add("@size", size);

                return await connection.QueryAsync<DiscussionViewModel>(StoredProcedureName.GetDiscussionForMovie, parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<DiscussionViewModel> GetDiscussionById(Guid DiscussionId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", DiscussionId);

                return await connection.QueryFirstOrDefaultAsync<DiscussionViewModel>(StoredProcedureName.GetDiscussionById, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> GetDiscussionCountByMovieId(Guid MovieId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MovieId", MovieId);

                return await connection.QueryFirstOrDefaultAsync<int>(StoredProcedureName.DiscussionCount, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
