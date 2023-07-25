using Common.Constants;
using Common.ViewModels;
using Dapper;
using DataAccessLayer.Persistance.Dapper;
using DataAccessLayer.Repositories.Interface;
using Entities;
using System.Data;

namespace DataAccessLayer.Repository.DapperRepository
{
    public class DapperGenreRepository : IGenreRepository
    {
        public readonly DapperContext _dapperContext;

        public DapperGenreRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task AddGenre(Genres genre)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", genre.Id);
                parameters.Add("@Name", genre.Name);
                parameters.Add("@Added_By", genre.Added_By);
                parameters.Add("@Created_Date", genre.Created_Date);

                await connection.ExecuteAsync(StoredProcedureName.GenreInsert, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteGenre(Guid id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id);

                await connection.ExecuteAsync(StoredProcedureName.GenreDelete, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllGenres()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<GenreViewModel>(StoredProcedureName.GetAllGenres, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<GenreViewModel> GetGenreById(Guid id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", id);

                return await connection.QueryFirstOrDefaultAsync<GenreViewModel>(StoredProcedureName.GetGenreById, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateGenre(Genres genre)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@Id", genre.Id);
                parameters.Add("@Name", genre.Name);

                await connection.ExecuteAsync(StoredProcedureName.GenreUpdate, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
