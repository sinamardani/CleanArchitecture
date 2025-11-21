namespace Domain.Commons.Constants;

public abstract class Policies
{
    /// <summary>
    /// Default policy
    /// </summary>
    public const string CanPurge = nameof(CanPurge);
    /// <summary>
    /// Combining two roles : Admin and User
    /// </summary>
    public const string AdminOrUser = nameof(AdminOrUser);
}