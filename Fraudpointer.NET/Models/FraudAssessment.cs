using System;
using Newtonsoft.Json;

namespace Fraudpointer.API.Models
{    
    /// <summary>
    /// This is an object that is returned by FraudPointer Server when requesting a Fraud Assessment.
    /// </summary>
    /// <remarks>
    /// Models.FraudAssessment is returned back to you when you ask for a Fraud Assessment. The API to ask for a
    /// Fraud Assessment is API.IClient.CreateFraudAssessment().
    /// 
    /// The most important properties of the object that you need to take into account are:
    /// - FraudAssessment.Result: Gives the Assessment Result. Can take one of the values:
    ///  - "Accept"
    ///  - "Review"
    ///  - "Reject"
    /// - FraudAssessment.Score: An integer value that gives the Assessment Score
    /// - DecidingFactor: Gives a hint about why the Assessment gave such a FraudAssessment.Result. Takes the values:
    ///  - "Profile thresholds": This means that the FraudAssessment.Score fell into a range that, according to Profile thresholds, 
    ///    returned a result equal to the value that exists in the FraudAssessment.Result property.
    ///  - or the name of the Deciding Rule.
    /// - Profile: The name of the Profile that has been used to carry out the assessment.
    ///
    /// It is up to your business to decide what to do with this FraudAssessment.Result. For example, if the FraudAssessment.Result is "Reject", 
    /// you can stop from carrying out the actual transaction with your bank.
    /// </remarks>
    public class FraudAssessment
    {
        /// <summary>
        /// Unique <c>Id</c> as returend by the FraudPointer Server
        /// </summary>
        [JsonProperty(PropertyName="id")]
        public string Id { get; set; }

        /// <summary>
        /// The Score of the Fraud Assessment. 
        /// </summary>
        /// <remarks>
        /// The Score of the Fraud Assessment has significance only if the Deciding Factor is "Profile thresholds".
        /// If the Deciding Factor has the name of a Rule, then do not take into account Score.
        /// </remarks>
        [JsonProperty(PropertyName="score")]
        public string Score { get; set; }

        /// <summary>
        /// bool to indicate interim or final (non-interim) Fraud Assessment.
        /// </summary>
        /// <remarks>
        /// Indicates whether the Fraud Assessment is final (non-interim) or not (interim). <c>true</c> means that
        /// the Fraud Assessment is interim (non-final) whereas <c>false</c> means that Fraud Assessment is 
        /// final (non-interim). Note that final Fraud Assessments create a Case in Fraud Pointer Application.
        /// </remarks>
        [JsonProperty(PropertyName="interim")]
        public bool Interim { get; set; }

        /// <summary>
        /// An indicator about how the FraudAssessment.Result has been calculated.
        /// </summary>
        /// <remarks>
        /// The Deciding Factor takes two values:
        /// 
        ///  - "Profile thresholds"
        ///  - Or the name of the Deciding Rule that matched.
        /// 
        /// If the Deciding Factor has the value "Profile thresholds", then you can see the FraudAssessment.Score that the 
        /// Fraud Assessment calculated. This FraudAssessment.Score normally falls into one of the ranges that are defined in the
        /// Profile level that has been used to carry out the Fraud Assessment. According to this range, the
        /// Result is set. 
        /// 
        /// If the Deciding Factor has a value different from "Profile thresholds", then it has the value of
        /// the Rule that matched. That Rule is a Deciding Rule and its FraudAssessment.Result was set as Result of the Fraud Assessment
        /// at hand.
        /// </remarks>
        [JsonProperty(PropertyName="deciding_factor")]
        public string DecidingFactor { get; set; }

        /// <summary>
        /// The Result of the Fraud Assessment.
        /// </summary>
        /// <remarks>
        /// Takes the values:
        /// 
        ///  - "Accept"
        ///  - "Reject"
        ///  - "Review"
        /// 
        /// You need to evaluate the Result of the Fraud Assessment and take necessary actions.        
        /// </remarks>
        [JsonProperty(PropertyName="result")]
        public string Result { get; set; }


        /// <summary>
        /// The Profile used to carry out the Fraud Assessment.
        /// </summary>
        /// <remarks>
        /// The Fraud Assessment uses a set of Fraud Assessment Rules that belong to one specific Profile. The Profile is
        /// selected at run-time using the Profile Selection Rules. This property returns a Profile object 
        /// that has been selected in order to carry out the Fraud Assessment at hand.
        /// </remarks>
        [JsonProperty(PropertyName="profile")]
        public Profile Profile { get; set; }

        /// <summary>
        /// The case generated with the assesment.
        /// </summary>
        /// <remarks>
        /// When a final assesment is created a case is created too. This field will contain the case information
        /// when you create a final assesment or you get a previous assesment.
        /// </remarks>
        [JsonProperty(PropertyName="case")]
        public Case Case { get; set; }

        /// <summary>
        /// Last time this FraudAssesment was updated.
        /// </summary>
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

// public class FraudAssessment
    //--------------------------------

} // namespace Fraudpointer.API.Models
//--------------------------------------

