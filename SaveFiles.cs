namespace PalettePicker
{
    /// <summary>
    /// This is a class for public profile of the palette
    /// </summary>

    internal class PublicProfile
    {
        /// <summary>
        /// Unique identifier for the palette (UUID/GUID).
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Name of the palette.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Description of the palette.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Author or creator of the palette.
        /// </summary>
        public string? Author { get; set; }
        /// <summary>
        /// Indicates if the palette is public (true) or private (false).
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// Date and time when the palette was created.
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Date and time when the palette was last modified.
        /// </summary>
        public DateTime Modified { get; set; }
        /// <summary>
        /// Indicates if the palette is designed for dark mode (true) or light mode (false).
        /// </summary>
        public bool IsDark { get; set; }
        /// <summary>
        /// The base color of the palette (hex code).
        /// </summary>
        public string? BaseColor { get; set; }
        /// <summary>
        /// Number of times the palette has been viewed.
        /// </summary>
        public int Views { get; set; }
        /// <summary>
        /// Number of times the palette has been favorited.
        /// </summary>
        public int Favorites { get; set; }
        /// <summary>
        /// Number of times the palette has been downloaded.
        /// </summary>
        public int Downloads { get; set; }
        /// <summary>
        /// Dictionary of color roles and their hex values (e.g., "p1": "#FF0000").
        /// </summary>
        public Dictionary<string, string>? Colors { get; set; }
        /// <summary>
        /// List of tags associated with the palette.
        /// </summary>
        public List<string>? Tags { get; set; }
    }

    /// <summary>
    /// This is a class for the private profile of the palette
    /// </summary>

    internal class PrivateProfile
    {
        /// <summary>
        /// Unique identifier for the palette (UUID/GUID).
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// File path to the palette on disk.
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// Indicates if the palette is pinned by the user.
        /// </summary>
        public bool Pinned { get; set; }
        /// <summary>
        /// List of collection names this palette belongs to.
        /// </summary>
        public List<string>? Collections { get; set; }
    }

    /// <summary>
    /// This is a class for user profiles
    /// </summary>

    internal class UserProfile
    {
        /// <summary>
        /// Unique identifier for the user (UUID/GUID).
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Name of the user.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Description or bio of the user.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// The password hash of the user.
        /// </summary>
        public string? PasswordHash { get; set; }
        /// <summary>
        /// List of palette IDs owned by the user.
        /// </summary>
        public List<string>? Palettes { get; set; }
        /// <summary>
        /// Number of times the user's palettes have been viewed.
        /// </summary>
        public int Views { get; set; }
        /// <summary>
        /// Number of followers the user has.
        /// </summary>
        public int Followers{ get; set; }
        /// <summary>
        /// Number of users this user is following.
        /// </summary>
        public int Following { get; set; }
    }
}
