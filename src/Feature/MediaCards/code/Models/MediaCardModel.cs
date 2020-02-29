﻿using System.Collections.Generic;

namespace Feature.MediaCards.Models
{
    public class MediaCardModel
    {
        public string TeamName { get; set; }
        public string FirstParticipantName { get; set; }
        public string SecondParticipantName { get; set; }
        public string ThirdParticipantName { get; set; }
        public List<string> TwitterProfiles { get; set; }
        public List<string> LinkedInProfiles { get; set; }

    }
}