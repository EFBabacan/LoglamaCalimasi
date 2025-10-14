using PostaGuvercini.Logging; // [Sensitive] attribute'u için gerekli

namespace Logging.WebApiTest
{
    public class UserRegistrationModel
    {
        public string Username { get; set; }
        public string Password { get; set; } // İsminden dolayı maskelenecek

        [Sensitive] // Attribute ile işaretlendiği için maskelenecek
        public string TCKimlikNo { get; set; }
    }
}