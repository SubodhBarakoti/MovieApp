namespace Common.Constants
{
    public static class StoredProcedureName
    {
        //Genres Stored Procedure
        public readonly static string GenreInsert = "SP_InsertGenre";
        public readonly static string GenreDelete = "SP_DeleteGenre";
        public readonly static string GenreUpdate = "SP_UpdateGenre";
        public readonly static string GetAllGenres = "SP_GetAllGenres";
        public readonly static string GetGenreById = "SP_GetGenreById";
        public readonly static string GenreExists = "SP_GenreExists";

        //Discussion Stored Procedure
        public readonly static string DiscussionInsert = "SP_InsertDiscussion";
        public readonly static string DiscussionDelete = "SP_DeleteDiscussion";
        public readonly static string GetDiscussionForMovie = "SP_GetDiscussionsForMovie";
        public readonly static string DiscussionCount = "SP_GetDiscussionCount";
        public readonly static string DiscussionUpdate = "SP_UpdateDiscussion";
        public readonly static string GetDiscussionById = "SP_GetDiscussionById";
        public readonly static string DiscussionExists = "SP_DiscussionExists";


        //Movie Stored Procedure
        public readonly static string MovieInsert = "SP_InsertMovie";
        public readonly static string MovieDelete = "SP_DeleteMovie";
        public readonly static string GetAllMovies = "SP_GetAllMovies";
        public readonly static string GetAverageRating = "SP_GetAverageRating";
        public readonly static string GetMovieById = "SP_GetMovieById";
        public readonly static string GetViewMovieById = "SP_GetViewMovieById";
        public readonly static string GetMovieCount = "SP_MovieCount";
        public readonly static string MovieUpdate = "SP_UpdateMovie";
        public readonly static string MovieExists = "SP_MovieExists";
        public readonly static string UpdateMovieImage = "SP_UpdateMovieImage";


        //Rating Stored Procedure
        public readonly static string RatingInsert = "SP_InsertRating";
        public readonly static string HasRated = "SP_HasRated";
        public readonly static string RatingCountByMovie = "SP_RatingCountByMovie";
        public readonly static string RatingDelete = "SP_DeleteRating";
        public readonly static string RatingUpdate = "SP_UpdateRating";
        public readonly static string RatingExists = "SP_RatingExists";

    }
}
