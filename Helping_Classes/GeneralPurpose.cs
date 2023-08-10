using CarSystem.BL;
using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CarSystem.Helping_Classes
{
    public class GeneralPurpose
    {

        DatabaseEntities de = new DatabaseEntities();

        public User ValidateLoggedinUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal; // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
            User loggedInUser = new UserBL().GetActiveUserById(Convert.ToInt32(userId), de);

            return loggedInUser;
        }

        public bool ValidateEmail(string email = "", int id = -1)
        {
            int emailCount = 0;

            if (id != -1)
            {
                emailCount = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == email.ToLower() && x.Id != id).Count();
            }
            else
            {
                emailCount = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == email.ToLower()).Count();
            }

            if (emailCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DateTime DateTimeNow()
        {
            return DateTime.Now;
        }


        public bool AddCookiesIdentity(string email = "", string Password = "")
        {
            User user = new UserBL().GetActiveUsersList(de).Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && StringCipher.Decrypt(x.Password).Equals(Password)).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Role", user.Role.ToString()),

            }, "ApplicationCookie");

            var claimsPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = claimsPrincipal;
            var httpContext = HttpContext.Current;
            var authManager = httpContext.GetOwinContext().Authentication;

            authManager.SignIn(identity);

            if (user.Role == 1 || user.Role == 2)
            {
                return true;
            }

            else
            {
                return false;
            }



        }
    }
}