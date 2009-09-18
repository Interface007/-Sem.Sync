namespace Sem.Azure.Storage
{
    /// <summary>
    /// Contains regular expressions for checking whether container and table names conform
    /// to the rules of the storage REST protocols.
    /// </summary>
    public static class RegularExpressionStrings
    {
        /// <summary>
        /// Container or queue names that match against this regular expression are valid.
        /// </summary>
        public const string ValidContainerNameRegex = @"^([a-z]|\d){1}([a-z]|-|\d){1,61}([a-z]|\d){1}$";

        /// <summary>
        /// Table names that match against this regular expression are valid.
        /// </summary>
        public const string ValidTableNameRegex = @"^([a-z]|[A-Z]){1}([a-z]|[A-Z]|\d){2,62}$";
    }
}