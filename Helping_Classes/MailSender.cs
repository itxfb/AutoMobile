﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.Helping_Classes
{
    public class MailSender
    {
        public static bool SendForgotPasswordEmail(string email, string encId, string BaseUrl = "")
        {
            GeneralPurpose gp = new GeneralPurpose();
            try
            {
                string MailBody = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'>" +
                "</div> </nav><center><div><h1 class='text-center'>Password Reset!</h1>" +
                "<p class='text-center'> Simply click the button showing below to reset your password (Link will expire after date change): </p><br>" +
                //"<button style='background-color: rgb(0,174,239);'>" +
                    "<a style='padding:8px; border-radius:5px; background-color: black; text-decoration:none; color:yellow; font-weight:bold;' href='" + BaseUrl + "Auth/ResetPassword?encId=" + encId + "&t=" + gp.DateTimeNow().Ticks + "' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a>" +
                //"</button>" +
                "<p style='color:red;'>Link will not work in spam. Please move this mail into your inbox.</p>" +
                //"<p onclick='alert('test')'>Please use the following link if <b>Reset</b> button did not work</p>" +
                //"<p style='color:blue'>" + BaseUrl + "Auth/ResetPassword?email=" + StringCipher.Base64Encode(email) + "&time=" + StringCipher.Base64Encode(GeneralPurpose.GetDateTime().ToString("MM/dd/yyyy")) + "</p>" +
                "</div></center>" +
                "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script></body></html>";



                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-ff43f744db29071a74f3106541e3b925");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "nodlays.co.uk", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "no.replay.nodlays@gmail.com");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Test Project | Password Reset");
                request.AddParameter("html", MailBody);
                request.Method = Method.POST;
                string response = client.Execute(request).Content.ToString();
                if (response.ToLower().Contains("queued"))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        public static bool SendOfferEmail(string email, int encId, string BaseUrl = "")
        {
            GeneralPurpose gp = new GeneralPurpose();
            try
            {
                string MailBody = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'>" +
                "</div> </nav><center><div><h1 class='text-center'>View Offer Accepted User Detail</h1>" +
                "<p class='text-center'> Simply click the button showing below to view your Offer details (Link will expire after date change): </p><br>" +
                //"<button style='background-color: rgb(0,174,239);'>" +
                    "<a style='padding:8px; border-radius:5px; background-color: black; text-decoration:none; color:yellow; font-weight:bold;' href='" + BaseUrl + "/Admin/ViewOfferDetails?userDetailId=" + encId + "&t=" + gp.DateTimeNow().Ticks + "' style='text-decoration:none;font-size:15px;color:white;'>View Offer Detail</a>" +
                //"</button>" +
                "<p style='color:red;'>Link will not work in spam. Please move this mail into your inbox.</p>" +
                //"<p onclick='alert('test')'>Please use the following link if <b>Reset</b> button did not work</p>" +
                //"<p style='color:blue'>" + BaseUrl + "Auth/ResetPassword?email=" + StringCipher.Base64Encode(email) + "&time=" + StringCipher.Base64Encode(GeneralPurpose.GetDateTime().ToString("MM/dd/yyyy")) + "</p>" +
                "</div></center>" +
                "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script></body></html>";



                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-ff43f744db29071a74f3106541e3b925");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "nodlays.co.uk", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "no.replay.nodlays@gmail.com");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Test Project | View  Car Offer Detail");
                request.AddParameter("html", MailBody);
                request.Method = Method.POST;
                string response = client.Execute(request).Content.ToString();
                if (response.ToLower().Contains("queued"))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


    }
}