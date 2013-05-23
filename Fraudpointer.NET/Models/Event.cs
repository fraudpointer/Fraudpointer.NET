using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;


namespace Fraudpointer.API.Models
{
    /// <summary>
    /// Event object groups a set of Data under an Event Type. This whole set, is then associated to a
    /// Models.AssessmentSession by calling API.IClient.AppendEventToAssessmentSession().
    /// </summary>
    /// <remarks>
    /// When you are using the FraudPointer Service, you need to collect data and sent it over to 
    /// FraudPointer Server. In order to do that you instantiate a Models.Event object and then you
    /// call Event.AddData() method. Then you call API.IClient.AppendEventToAssessmentSession() and
    /// Event packaged data are sent over.
    /// 
    /// It is important, though, to instantiate the appropriate Event Type. The following event types are 
    /// supported:
    /// - Event.GenericEvent
    /// - Event.CheckoutEvent
    /// - Event.FailedPaymentEvent
    /// - Event.PurchaseEvent
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class Event
    {
        /// <summary>
        /// Use this constant to instantiate an Event that is not one of the other types. 
        /// </summary>
        /// <remarks>
        /// Usually, you group your data into a more specific Type. However, if the data that you want
        /// to group cannot be categorized below one of the specific Types, you can use the
        /// GenericEvent Type.
        /// </remarks>
        public const string GenericEvent = "GenericEvent";

        /// <summary>
        /// Use this constant to instantiate an Event that is related to Checkout data.
        /// </summary>
        /// <remarks>
        /// You can create a CheckoutEvent when you get data from a check out form. You essentially want
        /// to store in FraudPointer server the fact that your customer is trying to check out together with
        /// relevant check out data. Example of Data that you might want to store below the CheckoutEvent Type might be:
        /// - Firstname
        /// - Lastname        
        /// - Customer e-mail
        /// - Customer address
        /// - Product to Purchase
        /// - Price
        /// - e.t.c.
        /// </remarks>
        public const string CheckoutEvent = "CheckoutEvent";

        /// <summary>
        /// Use this constant to instantiate an Event to mark a Failed Payment.
        /// </summary>
        /// <remarks>
        /// When you decide to proceed with charging your customer, with whatever data you have at your hand,
        /// your attempt to charge its credit card might fail. In that case, record this failed payment by 
        /// instantiating a FailedPayment Event Type and sending it over to FraudPointer, without necessarily
        /// attaching any data to it. Only the FailedPayment Event is enough to mark the fact that the payment
        /// carried out during this session failed.
        /// 
        /// You can, and you probably should, instantiate this event many times. One for each failed payment attempt
        /// during the same assessment session.
        /// </remarks>
        public const string FailedPayment = "FailedPaymentEvent";


        /// <summary>
        /// Use this constant to instantiate an Event to mark a Successful Payment.
        /// </summary>
        /// <remarks>
        /// When you decide to proceed with charging your customer, with whatever data you have at your hand,
        /// your attempt to charge its credit card might succeed. In that case, record this successful payment by 
        /// instantiating a Purchase Event Type and sending it over to FraudPointer, without necessarily
        /// attaching any data to it. Only the PurchaseEvent Event is enough to mark the fact that the payment
        /// carried out during this session has succeeded.
        /// 
        /// </remarks>
        public const string PurchaseEvent = "PurchaseEvent";

		/// <summary>
		/// Reserved
        /// </summary>
        /// <remarks>
		/// Reserved
        /// </remarks>
        public const string TrackEvent = "TrackEvent";
		
        /// <summary>
        /// Instantiate a Models.Event by calling <c>new</c> on this Constructor
        /// </summary>
        /// <remarks>
        /// Use
        /// <code>
        /// Event anEvent = new Event(....)
        /// </code>
        /// to instantiate a new Event before start filling it with data. The argument should be one of the 
        /// valid event types.
        /// </remarks>
        /// <param name="type">One of:
        /// - Event.CheckoutEvent
        /// - Event.FailedPayment
        /// - Event.GenericEvent
        /// - Event.PurchaseEvent
        /// </param>
        public Event(string type)
        {
            Type = type;
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        ///  Event Type to send to the server for event creation
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Returns the Data appended to the particular Models.Event
        /// </summary>
        /// <remarks>
        /// If you want to get read access to the Data that you have appended to a Models.Event you can use this
        /// readonly property.
        /// 
        /// Note that if you want to append data to the Models.Event use one of the methods Models.Event.AddData
        /// </remarks>
        [JsonProperty(PropertyName = "data")]
        public Dictionary<String, String> Data { get; private set; }

        /// <summary>
        /// Adds one datum to the particular Models.Event. The datum is actually a value of an Attribute and 
        /// value given is a <c>string</c> instance or literal.
        /// </summary>
        /// <remarks>
        /// Use this method if you want to add a datum of type <c>string</c> in your Event at hand.
        /// 
        /// Here is an example:
        /// <code>
        /// Event anEvent = new Event(Event.CheckoutEvent);
        /// anEvent.AddData("BILLING_COUNTRY", "FR");
        /// anEvent.AddData("BILLING_CITY", "Paris");
        /// </code>
        /// </remarks>
        /// <param name="key">KEY value of the Attribute that you want to use. The Attributes are either System
        /// or Custom Account Attributes. The System Attributes can be found here: 
        /// \link system_attributes System Attributes\endlink. The Custom Account Attributes
        /// should be given to you by the person who has created them in the FraudPointer Application.
        /// </param>
        /// <param name="value">Value of the Attribute. A <c>string</c> instance or literal should be given.</param>
        /// <returns>The current Event instance, in order to continue adding data</returns>
        public Event AddData(string key, string value)
        {
            Data.Add(key, value);
            return this;
        }

        /// <summary>
        /// Adds one datum to the particular Models.Event. The datum is actually a value of an Attribute and 
        /// value given is an <c>int</c> instance or literal.
        /// </summary>
        /// <remarks>
        /// Use this method if you want to add a datum of type <c>int</c> in your Event at hand.
        /// 
        /// Here is an example:
        /// <code>
        /// Event anEvent = new Event(Event.CheckoutEvent);
        /// anEvent.AddData("NUMBER_OF_PURCHASED_GOODS", 4);
        /// </code>
        /// </remarks>
        /// <param name="key">KEY value of the Attribute that you want to use. The Attributes are either System
        /// or Custom Account Attributes. The System Attributes can be found here: 
        /// \link system_attributes System Attributes\endlink. The Custom Account Attributes
        /// should be given to you by the person who has created them in the FraudPointer Application.
        /// </param>
        /// <param name="value">Value of the Attribute. An <c>int</c> instance or literal should be given.</param>
        /// <returns>The current Event instance, in order to continue adding data</returns>
        public Event AddData(string key, int value)
        {
            return AddData(key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds one datum to the particular Models.Event. The datum is actually a value of an Attribute and 
        /// value given is a <c>bool</c> instance or literal.
        /// </summary>
        /// <remarks>
        /// Use this method if you want to add a datum of type <c>bool</c> in your Event at hand.
        /// 
        /// Here is an example:
        /// <code>
        /// Event anEvent = new Event(Event.CheckoutEvent);
        /// anEvent.AddData("TWO_WAY_TRIP", false);
        /// </code>
        /// </remarks>
        /// <param name="key">KEY value of the Attribute that you want to use. The Attributes are either System
        /// or Custom Account Attributes. The System Attributes can be found here: 
        /// \link system_attributes System Attributes\endlink. The Custom Account Attributes
        /// should be given to you by the person who has created them in the FraudPointer Application.
        /// </param>
        /// <param name="value">Value of the Attribute. A <c>bool</c> instance or literal should be given.</param>
        /// <returns>The current Event instance, in order to continue adding data</returns>
        public Event AddData(string key, bool value)
        {
            return AddData(key, value ? "true" : "false");
        }

        /// <summary>
        /// Adds one datum to the particular Models.Event. The datum is actually a value of an Attribute and 
        /// value given is a <c>decimal</c> instance or literal.
        /// </summary>
        /// <remarks>
        /// Use this method if you want to add a datum of type <c>decimal</c> in your Event at hand.
        /// 
        /// Here is an example:
        /// <code>
        /// Event anEvent = new Event(Event.CheckoutEvent);
        /// anEvent.AddData("PURCHASE_AMOUNT", 32.56);
        /// </code>
        /// </remarks>
        /// <param name="key">KEY value of the Attribute that you want to use. The Attributes are either System
        /// or Custom Account Attributes. The System Attributes can be found here: 
        /// \link system_attributes System Attributes\endlink. The Custom Account Attributes
        /// should be given to you by the person who has created them in the FraudPointer Application.
        /// </param>
        /// <param name="value">Value of the Attribute. A <c>decimal</c> instance or literal should be given.</param>
        /// <returns>The current Event instance, in order to continue adding items</returns>
        public Event AddData(string key, decimal value)
        {
            return AddData(key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds one datum to the particular Models.Event. The datum is actually a value of an Attribute and 
        /// value given is a <c>DateTime</c> instance.
        /// </summary>
        /// <remarks>
        /// Use this method if you want to add a datum of type <c>DateTime</c> in your Event at hand.
        /// 
        /// Here is an example:
        /// <code>
        /// Event anEvent = new Event(Event.CheckoutEvent);
        /// anEvent.AddData("DATE_OF_ARRIVAL", new DateTime(2011, 5, 27, 23, 15, 30, DateTimeKind.Local));
        /// </code>
        /// Internally, the <c>DateTime</c> is converted to a string representation that follows the 
        /// expanded version of the standard ISO 8601. Hence, the example given above will be converted to
        /// <c>"2011-05-27T23:15:30+03:00"</c> if the local time zone is 3 hours ahead of UTC.
        /// Note that if you have not specified the <c>DateTimeKind</c> when constructing the <c>DateTime</c> object
        /// method <c>AddData</c> will assume <c>DateTimeKind.Local</c>
        /// </remarks>
        /// <param name="key">KEY value of the Attribute that you want to use. The Attributes are either System
        /// or Custom Account Attributes. The System Attributes can be found here: 
        /// \link system_attributes System Attributes\endlink. The Custom Account Attributes
        /// should be given to you by the person who has created them in the FraudPointer Application.
        /// </param>
        /// <param name="value">Value of the Attribute. A <c>DateTime</c> instance should be given.</param>
        /// <returns>The current Event instance, in order to continue adding items</returns>
        public Event AddData(string key, DateTime value)
        {
            DateTime dateToAdd;
            if (value.Kind == DateTimeKind.Unspecified)
            {                
                dateToAdd = DateTime.SpecifyKind(value, DateTimeKind.Local);
            }
            else
            {
                dateToAdd = value;
            }
            return AddData(key, dateToAdd.ToString("yyyy-MM-ddTHH:mm:ssK"));
        }
    }
}
