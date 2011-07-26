using System;
using Fraudpointer.API.Models;
using Fraudpointer.API.Responses;
using NUnit.Framework;
using Fraudpointer.API.Clients;

namespace Fraudpointer.API.Tests.Unit.Models
{
    /// <summary>
    /// With this tests we try to make sure that the Model works with
    /// the JSON returned by the fraudpointer server
    /// </summary>
    [TestFixture]
    public class FromJsonToModel
    {
        [Test]
        public void Deserialize_CompleteCase_Ok()
        {
            const string @case =
            @"{
           ""resolution"": ""Accept"",
           ""updated_at"": ""2011-07-25T16:44:04+03:00"",
           ""id"": 984306690,
           ""status"": ""Closed""
       }";
            var c = @case.DeserializeJson<Case>();
            Assert.IsNotNull(c);
            Assert.AreEqual("Accept",c.Resolution);
            Assert.AreEqual("984306690",c.Id);
            Assert.AreEqual("Closed",c.Status);
            Assert.AreEqual(new DateTime(2011, 07, 25, 16, 44, 04, DateTimeKind.Local), c.UpdatedAt);
        }
        [Test]
        public void Deserialize_FraudAssesment_Ok()
        {
            const string assessment = @"{
   ""fraud_assessment"": {
       ""deciding_factor"": ""Profile thresholds"",
       ""result"": ""Review"",
       ""profile"": {
           ""name"": ""Default"",
           ""updated_at"": ""2011-07-25T16:44:04+03:00"",
           ""id"": 366801811
       },
       ""updated_at"": ""2011-07-25T16:44:04+03:00"",
       ""id"": 620510104,
       ""case"": {
           ""resolution"": null,
           ""updated_at"": ""2011-07-25T16:44:04+03:00"",
           ""id"": 984306690,
           ""status"": ""Open""
       },
       ""score"": -30
   }
}";

            // This is how deserialization is done in the HttpWrapper class
            var obj = assessment.DeserializeJson<ResponseFraudAssessment>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.FraudAssessment);
            var fa = obj.FraudAssessment;
            Assert.AreEqual("Profile thresholds", fa.DecidingFactor);
            Assert.AreEqual("Review",fa.Result);
            Assert.AreEqual("620510104",fa.Id);
            Assert.AreEqual(new DateTime(2011, 07, 25, 16, 44, 04, DateTimeKind.Local),fa.UpdatedAt);
            Assert.AreEqual("-30", fa.Score);
            //Profile
            Assert.IsNotNull(fa.Profile);
            var profile = fa.Profile;
            Assert.AreEqual("Default",fa.Profile.Name);
            Assert.AreEqual(new DateTime(2011, 07, 25, 16, 44, 04, DateTimeKind.Local), profile.UpdatedAt);
            Assert.AreEqual("366801811", profile.Id);
            // Case
            Assert.IsNotNull(obj.FraudAssessment.Case);
            Assert.IsNull(obj.FraudAssessment.Case.Resolution);
            Assert.AreEqual("984306690", obj.FraudAssessment.Case.Id);
            Assert.AreEqual("Open",obj.FraudAssessment.Case.Status);
            Assert.AreEqual(new DateTime(2011,07,25,16,44,04,DateTimeKind.Local),obj.FraudAssessment.Case.UpdatedAt);
        }
    }
}
