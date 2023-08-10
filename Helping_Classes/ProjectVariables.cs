using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.Helping_Classes
{
    public class ProjectVariables
    {
        public static string EncKey = "Nodlays";

        //public static string OldCopartClientApi = "https://c-auth-qa4.copart.com/employee/oauth/token";
        //public static string OldTokenCopart = "Basic YjJiLXJvdW5kbGFrZS1hdXRvOjMwZjY1ZGM0ZGNiYTQ0Yzk5MjUwZDlhNDU3ZGE3YTc5";
        //public static string ProQuoteApi = "https://b2b-stg.copart.com/v1/proquote";  //Development

        public static string CopartClientApi = "https://auth.copart.com/employee/oauth/token?";
        public static string ClientId = "b2b-roundlake-auto";
        public static string ClientSecretKey = "9dd703edd722488eb68ed826105f2a6e";
        public static string ProQuoteApi = "https://b2b.copart.com/v1/proquote";   // Production


        // Client Twilio Credentials

        //public static string AccountSid = "ACf257f1bde3675c84adbc5ef3a4dcf1de";
        //public static string AuthToken = "f78e1bf3d5ff9570b5a9f4bacbc764d7";

        public static string AccountSid = "AC6273061cc7e73484a7c3efcbd304c7a9";
        public static string AuthToken = "58c3b955bbcfe9801c2ba5f7c9fb59b6";





        //End Client Twilio Credentials

    }
}