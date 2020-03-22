namespace ReservationSystem.Contracts
{
    /// <summary>
    /// The Login Response
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// JWT Bearer Token which is valid for one hour
        /// </summary>
        public string JWTWebToken { get; set; }
    }
}