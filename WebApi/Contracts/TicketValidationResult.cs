namespace ReservationSystem.Contracts
{
    /// <summary>
    /// Returns the result of a ticket validation
    /// </summary>
    public class TicketValidationResult
    {
        /// <summary>
        /// true wheter the ticket is valid
        /// </summary>
        public bool ValidationResult { get; set; }
    }
}