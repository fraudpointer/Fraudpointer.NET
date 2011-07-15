using System;

namespace Fraudpointer.API
{
    /// <summary>
    /// Thrown by all IClient methods in case an error takes place and method cannot return its results normally.
    /// </summary>
    /// <remarks>
    /// You should always put your IClient methods into <c>try/catch</c> blocks and make sure you catch this exception. 
    /// If you catch this exception, the message borne will be adequate to explain to you what has gone wrong. Especially the
    /// message borne by the <c>InnerException</c> of the <c>Exception</c> caught.
    /// 
    /// Here is how you can wrap your code (example of a Windows Console Application) arround <c>try/catch</c> block
    /// and on error get information about what has gone wrong:
    /// <code>
    /// try
    /// {
    ///    ... write your FraudPointer code here ....
    ///    
    /// } // try
    /// catch (Fraudpointer.API.ClientException ex)
    /// {
    /// 
    ///    // something went really wrong. Let us find out what:
    ///    Console.Error.WriteLine(ex.Message);
    ///    // and the inners of it:
    ///    Console.Error.WriteLine("Inner: {0}", ex.InnerException.Message);
    ///
    /// } // catch  
    /// </code>     
    /// </remarks>
    public class ClientException : Exception
    {
        public ClientException(string message, Exception innerException)            
            :base(message, innerException)
        {            
        } // ClientException constructor
        
    } // class ClientException
} // namespace
