using System;
using Fraudpointer.API.Models;

namespace Fraudpointer.API
{   
    class WindowsConsoleClientExample
    {
        static void Main(string[] args)
        {
            try
            {                
                var client = ClientFactory.Construct("https://production.fraudpointer.com/api", "KEY");
                AssessmentSession assessmentSession = client.CreateAssessmentSession();
                Console.WriteLine("Assessement Session ID: " + assessmentSession.Id);

                Event l_event = new Event(Event.GenericEvent);

                l_event.AddData("BookingCode", "1234");
                // this is an example on not specifying whether the time is local or utc
                DateTime startDate = new DateTime(2001, 6, 30, 1, 5, 0); 
                l_event.AddData("StartDate", startDate);

                DateTime reservationDate = new DateTime(2001, 3, 21, 8, 0, 0, DateTimeKind.Local);
                l_event.AddData("ReservationDate", reservationDate);

                client.AppendEventToAssessmentSession(assessmentSession, l_event);

                l_event = new Event(Event.FailedPayment);

                DateTime attemptDate = new DateTime(2011, 3, 22, 10, 0, 0, DateTimeKind.Local);
                l_event.AddData("E_TRAVEL_SA_PURCHASE_DATE", attemptDate);

                l_event.AddData("BILLING_ADDRESS_STREE_NAME", "Othonos");

                client.AppendEventToAssessmentSession(assessmentSession, l_event);

                FraudAssessment fa = client.CreateFraudAssessment(assessmentSession, false);
                Console.WriteLine("FraudAssessment: ID:" + fa.Id + ", Score: " + fa.Score + ", fa: " + fa);
                Console.ReadLine();
                
            } // try
            catch (Fraudpointer.API.ClientException ex)
            {
                // something went really wrong. Let us find out what:
                Console.Error.WriteLine(ex.Message);
                // and the inners of it:
                Console.Error.WriteLine("Inner: {0}", ex.InnerException.Message);

            } // catch  

        } // Main 
    } // class
} // namespace
