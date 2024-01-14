//using System.ComponentModel.DataAnnotations;

namespace GscToolTest.GoogleSearchConsole
{
    public class GoogleSettings
    {
        public GoogleSettings()
        {
            ApplicationName = string.Empty;
        }

        public string ApplicationName { get; set; }

        //[RegularExpression(@"[A-Z{2,15}]", ErrorMessage = "DefaultFeedLabel must be between 2-15 upper case characters.")]
        //public string DefaultFeedLabel { get; set; }

        //[RegularExpression(@"[A-Z{2,15}]", ErrorMessage = "SupplementalFeedLabel must be between 2-15 upper case characters.")]
        //public string SupplementalFeedLabel { get; set; }

        //[RegularExpression(@"[A-Z]{2}", ErrorMessage = "CountryCode must be 2 upper case characters.")]
        //public string CountryCode { get; set; }

        //[RegularExpression(@"[a-z]{2}", ErrorMessage = "LanguageCode must be 2 lower case characters.")]
        //public string LanguageCode { get; set; }
    }
}
