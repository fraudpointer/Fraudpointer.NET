using System;
using Fraudpointer.API.Models;

namespace Fraudpointer.API
{   
    class WindowsConsoleClientExample
    {
        static void WrongSyntax()
        {
            Console.Error.WriteLine("You didn't use the correct arguments to call this program.");
            Console.Error.WriteLine("You need two: ");
            Console.Error.WriteLine("   1st has to be the base url for the Fraudpointer API. Try: https://production.fraudpointer.com/api");
            Console.Error.WriteLine("   2nd has to be your API Key. You will find it in your Fraudpointer Application Account details.");
            Console.Error.WriteLine("   You can optionally pass the web request timeout as 3rd parameter (given in milliseconds)");
        }

        static void Main(string[] args)
        {
            // parse run-time arguments
            if ( args.Length < 2 || args.Length > 3)
            {
                WrongSyntax();
                return;
            }
            String baseUrl = args[0];
            String apiKey = args[1];
            int webRequestTimeout = 5000;
            if ( args.Length == 3)
            {
                try
                {
                    webRequestTimeout = int.Parse(args[2]);
                    if (webRequestTimeout<0)
                    {
                        WrongSyntax();
                        return;
                    }                        
                }
                catch (Exception ex)
                {
                    WrongSyntax();
                    return;
                }                
            }
            //-----------------------------------

            try
            {
                var client = ClientFactory.Construct(baseUrl, apiKey, webRequestTimeout);
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

                l_event.AddData("BILLING_ADDRESS_STREET_NAME", "Othonos");

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
