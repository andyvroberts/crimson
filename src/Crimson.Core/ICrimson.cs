namespace Crimson.Core
{
    /// <summary>
    /// The Crimson class library has only one entry point.
    /// Data can be grouped by either Postcode or Outcode, and this 
    /// option is set in the DI container.
    /// </summary>
    public interface ICrimson
    {
        Task RunAsync();

        void Run(string scanValue);
    }
}