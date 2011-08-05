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
        } // WrongSyntax ()
        //------------------

        static void PrintFraudAssessmentDetails (FraudAssessment fraudAssessment)
        {
            Console.WriteLine("...Fraud Assessment Id: " + fraudAssessment.Id);
            Console.WriteLine("...Fraud Assessment Score: " + fraudAssessment.Score);
            Console.WriteLine("...Fraud Assessment Result: " + fraudAssessment.Result);
            Console.WriteLine("...Fraud Assessment Deciding Factor: " + fraudAssessment.DecidingFactor);
            Console.WriteLine("...Fraud Assessment Updated At: " + fraudAssessment.UpdatedAt);
            Console.WriteLine("......Profile Id: " + fraudAssessment.Profile.Id);
            Console.WriteLine("......Profile Name: " + fraudAssessment.Profile.Name);
            Console.WriteLine("......Profile Updated At: " + fraudAssessment.Profile.UpdatedAt);
            Console.WriteLine("......Case Id: " + fraudAssessment.Case.Id);
            Console.WriteLine("......Case Resolution: " + fraudAssessment.Case.Resolution);
            Console.WriteLine("......Case Status: " + fraudAssessment.Case.Status);
            Console.WriteLine("......Case Update At: " + fraudAssessment.Case.UpdatedAt);
        } // PrintFraudAssessmentDetails ()
        //----------------------------------

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

                Console.WriteLine("About to Create an Assessment Session...");
                AssessmentSession assessmentSession = client.CreateAssessmentSession();
                Console.WriteLine("...done. Assessement Session ID: " + assessmentSession.Id);

                //--------- Generic Event --------------------------------
                Console.WriteLine("About to Create a Generic Event...");
                Event l_event = new Event(Event.GenericEvent);

                l_event.AddData("BookingCode", "1234");
                // this is an example on not specifying whether the time is local or utc
                DateTime startDate = new DateTime(2001, 6, 30, 1, 5, 0); 
                l_event.AddData("StartDate", startDate);

                DateTime reservationDate = new DateTime(2001, 3, 21, 8, 0, 0, DateTimeKind.Local);
                l_event.AddData("ReservationDate", reservationDate);
                Console.WriteLine("...done");

                Console.WriteLine("About to Send Event over to Fraudpointer Server...");
                Event l_eventReturned = client.AppendEventToAssessmentSession(assessmentSession, l_event);
                Console.WriteLine("...done. Event");
                //--------------------------------------------------------

                //-------- Failed Payment Event --------------------------
                Console.WriteLine("About to Create a Failed Payment Event...");
                l_event = new Event(Event.FailedPayment);

                DateTime attemptDate = new DateTime(2011, 3, 22, 10, 0, 0, DateTimeKind.Local);
                l_event.AddData("E_TRAVEL_SA_PURCHASE_DATE", attemptDate);

                l_event.AddData("BILLING_ADDRESS_STREET_NAME", "Othonos");
                Console.WriteLine("...done");

                Console.WriteLine("About to Send Event over to Fraudpointer Server...");
                l_eventReturned = client.AppendEventToAssessmentSession(assessmentSession, l_event);
                Console.WriteLine("...done. Event");
                //---------------------------------------------------------

                //--------- Ask for Fraud Assessment ----------------------
                Console.WriteLine("About to ask for Final Fraud Assessment");
                FraudAssessment fa = client.CreateFraudAssessment(assessmentSession, false);
                Console.WriteLine("...done");
                PrintFraudAssessmentDetails(fa);
                //---------------------------------------------------------

                //---- For demo purposed I will now ask for the previously created Fraud Assessment ----
                Console.WriteLine("About to ask for the previously created Fraud Assessment");
                FraudAssessment fa_got = client.GetFraudAssessment(assessmentSession, fa.Id);
                Console.WriteLine("...done");
                PrintFraudAssessmentDetails(fa_got);
                //--------------------------------------------------------------------------------------
                
            } // try
            catch (Fraudpointer.API.ClientException ex)
            {
                // something went really wrong. Let us find out what:
                Console.Error.WriteLine(ex.Message);
                // and the inners of it:
                Console.Error.WriteLine("Inner: {0}", ex.InnerException.Message);

            } // catch  

        } // Main 
        //---------

    } // class
} // namespace
