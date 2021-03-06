using System;
using System.Collections.Generic;

namespace Soccers.Common.Models
{
    public class TournamentResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public DateTime EndDate { get; set; }

        public DateTime EndDateLocal => EndDate.ToLocalTime();

        public bool IsActive { get; set; }

        public string LogoPath { get; set; }

        public string LogoFullPath => string.IsNullOrEmpty(LogoPath)
            ? "https://SoccerWeb0.azurewebsites.net//images/noimage.png"
            : $"https://zulusoccer.blob.core.windows.net/tournaments/{LogoPath}";

        public string LogosFullPath => string.IsNullOrEmpty(LogoPath)
            ? "noimage" 
            : string.Format("http://soccergame.ddns.net:8085/{0}", LogoPath.Substring(1));

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

        public List<GroupResponse> Groups { get; set; }
    }
}
