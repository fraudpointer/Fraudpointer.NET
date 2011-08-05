using System;
using Fraudpointer.API.Models;

namespace Fraudpointer.API
{    
    /// <summary>
    /// Fraudpointer.API.IClient is the interface that each C# client should implement in order to be used as a client proxy to access
    /// FraudPointer Service. <br/><b>Important Note:</b> Fraudpointer.NET client DLL comes with a ready to use implementation of this
    /// service. You need to use Fraudpointer.API.ClientFactory to instantiate this implementation and start using FraudPointer Service.
    /// </summary>
    /// <remarks>
    /// As a user of the FraudPointer Client Library you should declare a variable of IClient type and then use
    /// ClientFactory::Construct() to get an instance of a class that implements this interface.
    /// 
    /// \anchor sample_code Below, you can see a sample client that uses IClient to communicate with FraudPointer Server:
    /// 
    /// <code>
    /// static void Main(string[] args)
    /// {
    ///    // Instantiate the client object that implements the IClient interface
    ///    //
    ///    IClient client = ClientFactory.Construct("https://production.fraudpointer.com/api",
    ///                                         "8397355f81234567890abcdef1234567890abcdef1234567890abcdef1234567");
    /// 
    ///    // 1st STEP: Create Assessment Session. This goes to the FraudPointer Server and gets back an 
    ///    //           Assessment Session object
    ///    //
    ///    AssessmentSession assessmentSession = client.CreateAssessmentSession();
    ///    Console.WriteLine("Assessement Session ID: " + assessmentSession.Id);
    ///    
    ///    // 2nd STEP: Create an Event object of "Generic" Type. 
    ///    //
    ///    Event l_event = new Event(Event.GenericEvent);
    ///
    ///    // 3rd STEP: Add data to the created Event
    ///    //
    ///    l_event.AddData("BookingCode", "B2YB32");
    ///    l_event.AddData("StartDate", "20110322");
    ///    l_event.AddData("ReservationDate", "20110321");            
    ///
    ///    // 4th STEP: Send Event to FraudPointer Server to store it in the database
    ///    //           alongside with the already stored AssessmentSession
    ///    //
    ///    client.AppendEventToAssessmentSession(assessmentSession, l_event);
    /// 
    ///    // Repeat step 3 and 4. Note that you can create as many Events as you like for the 
    ///    //                      same Assessment Session
    ///    // 
    ///    l_event = new Event(Event.FailedPayment);
    ///
    ///    l_event.AddData("AttemptDate", "20110322");
    ///    l_event.AddData("BILLING_ADDRESS_STREE_NAME", "Othonos");
    ///
    ///    client.AppendEventToAssessmentSession(assessmentSession, l_event);
    ///
    ///    // 5th STEP: Ask FraudPointer Server to carry out a Fraud Assessment. This is the real value
    ///    //           of the Service because it can tell you real time about the fraud risk of the
    ///    //           transaction that you are trying to handle.
    ///    // 
    ///    FraudAssessment fa = fpapic.CreateFraudAssessment(assessmentSession, false);
    ///    Console.WriteLine("FraudAssessment: ID:" + fa.Id + ", Score: " + fa.Score + ", fa: " + fa);
    ///    Console.ReadLine();
    ///
    /// }
    /// </code>
    /// 
    /// </remarks>
    public interface IClient
    {
        /// <summary>
        /// Communicates with FraudPointer Server and gets back a ready to use Models.AssessmentSession object.
        /// </summary>
        /// <remarks>
        /// It should be used as soon as you want to start using the FraudPointer service. Without a Models::AssessmentSession object
        /// you cannot do anything.
        /// </remarks>
        /// <returns>An object of type Models.AssessmentSession as returned by FraudPointer Server. Object should contain the Models.AssessmentSession.Id that uniquely identifies
        /// this Models.AssessmentSession. You should use this Models.AssessmentSession.Id in whichever place the Models.AssessmentSession Id is required.
        /// 
        /// Method always returns a valid object, unless an exception is thrown.</returns>
        /// <exception cref="API.ClientException">It may throw an API::ClientException if an error occurs</exception>
        AssessmentSession CreateAssessmentSession();

        /// <summary>
        /// Appends a Models.Event to a Models.AssessmentSession object. 
        /// </summary>
        /// <remarks>This method also communicates with
        /// the FraudPointer Server, which will store all Models.Event data alongside with the Models.AssessmentSession
        /// given.
        /// 
        /// You can call this method as many times as you want (as long as you pass a new Models.Event on each call).
        /// It is actually recommended to do so. In other words, it is recommended to create various Events, of 
        /// various types and send them to FraudPointer to be stored against a Models.AssessmentSession.
        /// </remarks>
        /// 
        /// <param name="assessmentSession">Should be constructed with a call to CreateAssessmentSession()</param>
        /// <param name="paramEvent">Should be constructed with a call to new Models.Event(event_type)</param>
        /// <returns>An object of type Models.Event as returned by the FraudPointer Server.
        /// 
        /// Method always returns a valid object, unless an exception is thrown.</returns>
        /// <exception cref="API.ClientException">It may throw a ClientException if an error occurs</exception>
        Event AppendEventToAssessmentSession(AssessmentSession assessmentSession, Event paramEvent);

        /// <summary>
        /// This method should be called in order to evaluate the fraud level of a Models.AssessmentSession.
        /// </summary>
        /// <remarks>
        /// This method should be called after having sent to FraudPointer Server one or more Events. It is used
        /// to evaluate the fraud risk of the Models.AssessmentSession. Note that you can ask multiple interim 
        /// Fraud Assessments, but you can only ask for one final (non-interim) Fraud Assessment. Interim Fraud Assessments
        /// can be requested in a cycle of interactions with the FraudPointer Server such as the following:
        ///  - create assessment session
        ///  - create event
        ///  - add data to event
        ///  - add data to event
        ///  - ...
        ///  - add data to event
        ///  - append event to assessment session
        ///  - create another event
        ///  - add data to new event
        ///  - add data to new event
        ///  - ...
        ///  - add data to new event
        ///  - append new event to assessment session
        ///  - create interim fraud assessment 
        ///  - create one more event
        ///  - add data to last created event
        ///  - add data to last created event
        ///  - ...
        ///  - add data to last created event
        ///  - append last created event to assessment session
        ///  - create interim fraud assessment 
        ///  - ....
        ///  - create final (non-interim) fraud assessment
        /// The last and only final (non-interim) fraud assessment will also create a CASE in FraudPointer Application.
        /// </remarks>
        /// <param name="assessmentSession">A valid Models.AssessmentSession, previously created with CreateAssessmentSession()</param>
        /// <param name="interim">A boolean value true or false. If true, then an interim Models.FraudAssessment will be created. 
        /// If false, a final (non-interim) Models.FraudAssessment will be created</param>
        /// <returns>A Models.FraudAssessment valid object filled in with information returned by FraudPointer Server. It contains the Fraud
        /// Assessment Result.</returns>
        /// <exception cref="API.ClientException">It may throw a ClientException if an error occurs</exception>
        FraudAssessment CreateFraudAssessment(AssessmentSession assessmentSession, bool interim);

        /// <summary>
        /// Gets the results of a previously assesed session.
        /// </summary>
        /// <remarks>
        /// Use this method to retrieve a previously created Fraud assesment.
        /// 
        /// When you create a fraud assesment with <see cref="CreateFraudAssessment"/> you receive
        /// the result of the assesment and its Id. Using the session id and the assesment id you can retrieve that result
        /// with this method.
        /// </remarks>
        /// <param name="assessmentSession">A valid Models.AssessmentSession, previously created with <see cref="CreateAssessmentSession"/></param>
        /// <param name="assesmentId">A valid assesment id previously obtained with <see cref="CreateFraudAssessment"/></param>
        FraudAssessment GetFraudAssessment(AssessmentSession assessmentSession, string assesmentId);

        /// <summary>
        /// Use this method to generate a hash of a credit card number. Use the generated hash to send the encrypted credit card number
        /// to FraudPointer Server instead of the credit card number itself.
        /// </summary>
        /// <remarks>
        /// FraudPointer Server tries to identify the existence of the same credit card number in various transactions. These
        /// transactions either take place during the same session or take place in different sessions, but at the same time, or 
        /// took place in a session in the past. However, FraudPointer Server does not want to store the credit card numbers in clear format 
        /// and it does not need to do that in order to accomplish its goal. Hence, you need to encrypt them using the method provided here.
        /// Note that hash is one-way encryption method and FraudPointer Server cannot derive the credit card number from the hash.
        /// 
        /// This method does not communicate with the FraudPointer Server to generate the hash. Works locally.
        /// </remarks>
        /// <param name="creditCardNumber">The credit card number that you want to get its hash value.</param>
        /// <returns>The hash of the credit card number.</returns>
        /// <exception cref="API.ClientException">It may throw a ClientException if an error occurs</exception>
        string CreditCardHash(string creditCardNumber);

    }
}
