using Feature.MediaCards.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Sitecore.ExperienceForms;

namespace Feature.MediaCards.Controllers
{
    public class MediaCardController : Controller
    {
        protected string StoredProcedureName
        {
            get
            {
                return "[sitecore_forms_storage].[RegistrationFormData_Retrieve]";
            }
        }
        // GET: MediaCard
        public ActionResult GetMediaCards()
        {
            try
            {
                List<FormViewModel> formData = new List<FormViewModel>();
                var rendering = RenderingContext.Current.Rendering;
                string formId = rendering.Parameters["Form ID"].Replace("{", string.Empty).Replace("}", string.Empty);
                DateTime startDate = Sitecore.DateUtil.ParseDateTime(rendering.Parameters["Start Date"], DateTime.Now);
                DateTime endDate = Sitecore.DateUtil.ParseDateTime(rendering.Parameters["End Date"], DateTime.Now);
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["experienceforms"].ToString());
                SqlDataReader sqlDataReader;
                using (var cmd = new SqlCommand(StoredProcedureName, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FormDefinitionId", formId ?? DBNull.Value.ToString());
                    cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                    sqlConnection.Open();
                    sqlDataReader = cmd.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        var data = new FormViewModel
                        {
                            FormEntryId = sqlDataReader.GetString(sqlDataReader.GetOrdinal("FormEntryId")),
                            FieldName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("FieldName")),
                            Value = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Value"))
                        };
                        formData.Add(data);
                    }

                    sqlConnection.Close();
                }

                var result = formData.GroupBy(x => x.FormEntryId);
                List<ParticipantDetails> participantDetails = new List<ParticipantDetails>();
                foreach (var group in result)
                {

                    ParticipantDetails data = new ParticipantDetails()
                    {
                        FirstParticipantName = group.Where(x => x.FieldName.Equals("FirstParticipantName")).Select(x => x.Value).FirstOrDefault(),
                        SecondParticipantName = group.Where(x => x.FieldName.Equals("SecondParticipantName")).Select(x => x.Value).FirstOrDefault(),
                        ThirdParticipantName = group.Where(x => x.FieldName.Equals("ThirdParticipantName")).Select(x => x.Value).FirstOrDefault(),
                        ContactEmail = group.Where(x => x.FieldName.Equals("ContactEmail")).Select(x => x.Value).FirstOrDefault(),
                        TwitterProfiles = group.Where(x => x.FieldName.Equals("TwitterProfiles")).Select(x => x.Value).FirstOrDefault().Split(',').ToList(),
                        LinkedInProfiles = group.Where(x => x.FieldName.Equals("LinkedInProfiles")).Select(x => x.Value).FirstOrDefault().Split(',').ToList(),
                        TeamName = group.Where(x => x.FieldName.Equals("TeamName")).Select(x => x.Value).FirstOrDefault()
                    };

                    participantDetails.Add(data);
                }
                return View(participantDetails);
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error("Error Occurred:", exception, this);
                return null;
            }
        }
    }
}