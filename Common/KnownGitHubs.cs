namespace Common
{
    public static class KnownGitHubs
    {
        public const int AppId = 59669;

        public const string ImgBotLogin = "ImgBotApp";

        public const string ImgBotEmail = "ImgBotHelp@gmail.com";

        public const string CommitMessageTitle = "[ImgBot] Optimize images";

        public const string Username = "x-access-token";

        public const string BranchName = "imgbot";

        /// <remarks>
        /// {0} = installation_id
        /// </remarks>
        public const string AccessTokensUrlFormat = "https://api.github.com/app/installations/{0}/access_tokens";
    }
}
