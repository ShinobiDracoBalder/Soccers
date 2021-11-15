namespace Soccers.Common.Models
{
    public class TeamResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LogoPath { get; set; }

        public string LogoFullPath => string.IsNullOrEmpty(LogoPath)
            ? "https://SoccerWeb0.azurewebsites.net//images/noimage.png"
            : $"https://zulusoccer.blob.core.windows.net/teams/{LogoPath}";

        public string LogosFullPath => string.IsNullOrEmpty(LogoPath)
            ? "noimage": string.Format("http://soccergame.ddns.net:8085/{0}", LogoPath.Substring(1));

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(LogoPath))
                {
                    return "noimage";
                }

                return string.Format(
                    "http://soccergame.ddns.net:8085/{0}",
                    LogoPath.Substring(1));
            }
        }

        public string ShortName => Name.Length > 12 ? $"{Name.Substring(0, 12)}..." : Name;
    }
}