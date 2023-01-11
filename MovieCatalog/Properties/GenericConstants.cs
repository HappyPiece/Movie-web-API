using MovieCatalog.DAL.Models;
using System.Globalization;

namespace MovieCatalog.Properties
{
    public static class GenericConstants
    {
        // minimum eight characters, at least one letter and one number
        public const string PasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
        // number of milliseconds that defines how often expired tokens will be purged from database
        public const int TokenPurgeFrequency = 1000*60*5;

        // auth-related >>>
        public const string MissingAuthHeader = "Authentication header required";
        public const string InvalidToken = "Invalid authentication token";
        // <<< auth-related

        // user-related >>>
        public const string InvalidCredentials = "Invalid username or password";
        public const string UsernameTaken = "This username is already taken";
        public const string EmailTaken = "There is already an account with such email";
        public const string InappropriatePassword = "Your password must contain minimum eight characters, at least one letter and one number";

        public const string DefaultAvatar = "https://assets.stickpng.com/thumbs/5845e614fb0b0755fa99d7e8.png";
        public const string DefaultNickname = "UnknownDoge";
        public static DateTime DefaultBirthday = DateTime.Parse("2005-11-02").ToUniversalTime();
        public static Gender DefaultGender = Gender.ApacheAttackHelicopter;

        public const string ProfileEdited = "Profile successfully edited";
        // <<< user-related

        // movie-related >>>
        public const string NoSuchPage = "Requested page not found";
        public const string NoSuchMovie = "Requested movie not found";
        public const string MovieAlreadyInFavorites = "This movie is already in your favorites";
        public const string MovieAddedToFavorites = "Movie has been added to your favorites";
        public const string UnableToRemoveFromFavorites = "Unable to remove movie from favorites since it's present there";
        public const string MovieRemovedFromFavorites = "Movie has been removed from your favorites";
        // <<< movie-related

        // review-related >>>
        public const string NoSuchReview = "Requested review not found";
        public const string NotYourReview = "Not enough rights to access this review";
        public const string ReviewAdded = "Review successfully added";
        public const string ReviewEdited = "Review successfully edited";
        public const string ReviewDeleted = "Review successfully deleted";
        // <<< review-related

        // generic >>>
        public const string InternalError = "Error while processing request";
        // <<< generic
    }
}
