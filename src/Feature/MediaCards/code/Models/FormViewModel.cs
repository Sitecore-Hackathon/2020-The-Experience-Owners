using System.Collections.Generic;

namespace Feature.MediaCards.Models
{
    public class FormViewModel
    {
        public string FormEntryId { get; set; }

        public string FieldName { get; set; }

        public string Value { get; set; }
    }

    public class ParticipantDetails
    {
        public string TeamName { get; set; }

        public string FirstParticipantName { get; set; }

        public string SecondParticipantName { get; set; }

        public string ThirdParticipantName { get; set; }

        public string ContactEmail { get; set; }

        public List<string> TwitterProfiles { get; set; }

        public List<string> LinkedInProfiles { get; set; }
    }
}