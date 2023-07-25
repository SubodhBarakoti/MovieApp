using Common.Constants;
using Dapper;
using DataAccessLayer.Persistance.Dapper;
using DataAccessLayer.Repositories.Interface;
using Entities;
using System.Data;

namespace DataAccessLayer.Repository.DapperRepository
{
    public class DapperRatingRepository : IRatingRepository
    {
        public readonly DapperContext _dapperContext;
        public DapperRatingRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task AddRating(Rating rating)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", rating.Id);
                parameters.Add("@rating", rating.rating);
                parameters.Add("@RatedBy", rating.RatedBy);
                parameters.Add("@MovieId", rating.MovieId);
                parameters.Add("@RatedDate", rating.RatedDate);

                await connection.ExecuteAsync(StoredProcedureName.RatingInsert, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> HasRated(Guid MovieId, string UserId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MovieId", MovieId);
                parameters.Add("@UserId", UserId);

                return await connection.QueryFirstOrDefaultAsync<bool>(StoredProcedureName.HasRated, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> RatingCountByMovie(Guid MovieId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MovieId", MovieId);

                return await connection.QueryFirstOrDefaultAsync<int>(StoredProcedureName.RatingCountByMovie, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateRating(Rating rating)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", rating.Id);
                parameters.Add("@rating", rating.rating);
                parameters.Add("@RatedDate", rating.RatedDate);

                await connection.ExecuteAsync(StoredProcedureName.RatingUpdate, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
