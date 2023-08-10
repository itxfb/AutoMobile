//using CountryData.Standard;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarSystem.Models;
using CarSystem.BL;
using CarSystem.Helping_Classes;
using System.Web;

using System.Web.Mvc;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Principal;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Clients;

namespace CarSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private readonly DatabaseEntities de = new DatabaseEntities();
        private readonly GeneralPurpose gp = new GeneralPurpose();
        public ActionResult Index()
        {
            List<State> statesList = new CarDetailBL().GetActiveStatesList(de).Take(48).ToList();

            return View(statesList);
        }

        public ActionResult Offer()
        {

            return View();
        }


        public ActionResult ThankyouPage(int offerstatus = -1)
        {
            ViewBag.offerstatus = offerstatus;
            return View();
        }


        public ActionResult OfferDetailPage()
        {

            return View();
        }

        public ActionResult terms_conditions()
        {

            return View();
        }

        
        public ActionResult privacy_policy()
        {

            return View();
        }

        public ActionResult accessibility()
        {

            return View();
        }
        

        public ActionResult WeBuyCar(/*int id=-1*/ string StateName = "")
        {
            //For getting customizing base url
            var getUrl = Request.Url.ToString();
            var getStateName = getUrl.Split('/').Last();
            if (getStateName.Contains("?"))
            {
                getStateName = getStateName.Split('?').Last();
            }
            if (string.IsNullOrEmpty(getStateName) || getStateName == "Index")
            {
                getStateName = getUrl.Split('/').Reverse().Take(2).Last();
            }

            //For getting customizing base url

            var GetStateData = new CarDetailBL().GetActiveStateByStateName(getStateName, de);



            List<City> CityList = new CarDetailBL().GetActiveCitiesListByStateId(GetStateData.Id, de);

            //var GetStateData = new CarDetailBL().GetActiveStateById(id,de);
            if (GetStateData != null)
            {
                string[] StateData = null;
                if (GetStateData.StateName.Contains("-"))
                {
                    StateData = GetStateData.StateName.Split('-');

                }
                else
                {
                    StateData = GetStateData.StateName.Split(' ');

                }
                ViewBag.StateName = StateData[0];
                ViewBag.StateAbrevation = StateData[1];
            }


            return View(CityList);
        }

        public ActionResult CarByCityDetail(string ids = "")
        {
            int id = Convert.ToInt32(StringCipher.DecryptId(ids));
            //string ids = Request.QueryString["id"];
            //For getting customizing base url
            var getUrl = Request.Url.ToString();
            var GetNewUrl = getUrl.Split('?');
            getUrl = GetNewUrl[0];

            var getCityName = getUrl.Split('/').Last();
            if (getCityName.Contains("?"))
            {
                getCityName = getCityName.Split('?').Last();
            }
            if (string.IsNullOrEmpty(getCityName) || getCityName == "Index")
            {
                getCityName = getUrl.Split('/').Reverse().Take(2).Last();
            }

            //For getting customizing base url

            var GetCityData = new CarDetailBL().GetActiveCityById(id, de);
            //var GetCityData = new CarDetailBL().GetActiveCityByName(getCityName, de);

            if (GetCityData != null)
            {

                ViewBag.CityData = GetCityData;
            }

            var GetStateData = new CarDetailBL().GetActiveStateById(GetCityData.StateId.Value, de);
            if (GetStateData != null)
            {
                string[] StateData = GetStateData.StateName.Split('-');
                ViewBag.StateName = StateData[0];
                ViewBag.StateAbrevation = StateData[1];
            }

            return View();
        }
        //public ActionResult WeBuyCarByState()
        //{
        //    List<State> statesList = new CarDetailBL().GetActiveStatesList(de);

        //    return View(statesList);
        //}
        public ActionResult About()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            List<State> statesList = new CarDetailBL().GetActiveStatesList(de).Take(48).ToList();

            return View(statesList);
        }

        [HttpPost]
        public ActionResult PostUserDetails(UserCarDetail _userDetail, HttpPostedFileBase[] Images = null, string SignedImageUrl=null,string email="",string password = "",int Islogin=-1 )
        {

            //Register User

            var UserId = 0;
            
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                if(Islogin!=1)
                {
                    User usr = new User()
                    {
                        Name = _userDetail.FirstName + " " + _userDetail.LastName,
                        Contact = _userDetail.UserContact,
                        Email = email.Trim(),
                        Password = StringCipher.Encrypt(password),
                        //Gender = _user.Gender,                              //   2/18/2022
                        Gender = 1,                                           //   2/18/2022
                        Role = 2,
                        IsActive = 1,
                        CreatedAt = gp.DateTimeNow()
                    };

                    UserId = new UserBL().AddNewUser(usr, de);


                }

                else
                {
                    //User user = new UserBL().GetActiveUsersList(de).Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && StringCipher.Decrypt(x.Password).Equals(password)).FirstOrDefault();
                    var LoginUser = gp.AddCookiesIdentity(email,password);
                    User user = gp.ValidateLoggedinUser();
                    


                    if (user != null)
                    {
                        UserId= user.Id;
                        
                    }
                    //return RedirectToAction("PostLogin", "Auth", new { Email = email, Password = password, Islogin=1 });
                }

            }

            


            //Register User

            // save signature image 
            var SignaturePath = "";
            var test = "";
            if (SignedImageUrl != "")
            {
                byte[] imageBytes = Convert.FromBase64String(SignedImageUrl.Replace("data:image/png;base64,", String.Empty));
                Image image;
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    var path = "~/Content/BarnDiagram";
                    if (!System.IO.File.Exists(path))
                    {
                        Directory.CreateDirectory(Server.MapPath("~") + "/Content/BarnDiagram/");

                    }
                    image = Image.FromStream(ms);
                    string fileName = "BarnDiagram-" + DateTime.Now.ToString("yymmddfff") + ".png";
                    test = "/Content/BarnDiagram/" + fileName;
                    SignaturePath = Path.Combine(Server.MapPath("~/Content/BarnDiagram/"), fileName);
                    // file is uploaded
                    image.Save(SignaturePath, System.Drawing.Imaging.ImageFormat.Png);
                    // releasing resources
                    image.Dispose();
                }
            }


            //end save signature image 


            UserCarDetail u = new UserCarDetail()
            {
                FirstName = _userDetail.FirstName,
                MidName = _userDetail.MidName,
                LastName = _userDetail.LastName,
                VehicleAddress = _userDetail.VehicleAddress,
                City = _userDetail.City,
                IsRejected = 0,
                //Gender = _user.Gender,                              //   2/18/2022
                State = _userDetail.State,                                           //   2/18/2022
                ZipCode = _userDetail.ZipCode,
                UserContact = _userDetail.UserContact,
                IsActive = 1,
                CreatedAt = gp.DateTimeNow(),
                UserSignature= test,
                UserId=UserId
                
                //UserSignature= SignaturePath
            };



            var chkUser = new UserBL().AddUserDetail(u, de);

            
            int ids = -1;
            if (chkUser)
            {
                var GetUserDetailsLastRecord = new UserBL().GetActiveLastRecordUserDetail(de);

                var carimageUpload = UploadImages(GetUserDetailsLastRecord.Id, Images);
                string BaseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/");

                MailSender.SendOfferEmail("hassan@nodlays.com", GetUserDetailsLastRecord.Id, BaseUrl);
                //MailSender.SendOfferEmail("elixre@gmail.com", GetUserDetailsLastRecord.Id, BaseUrl);

                // update car detail adding userDetailId

                CarDetail GetCarDetail = new CarDetailBL().GetActiveCarDetailLastRecord(de);

                if(GetCarDetail != null)
                {
                    GetCarDetail.UserCarDetailId = GetUserDetailsLastRecord.Id;
                    var updateCarDetails = new CarDetailBL().UpdateCarDetails(GetCarDetail, de);

                    // sending offer message 



                    string accountSid = ProjectVariables.AccountSid;
                    string authToken = ProjectVariables.AuthToken;

                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Car offer for the vehicle associated with VIN number:" + " " + GetCarDetail.vin + "\n" + "Offered Price:" + " " + GetCarDetail.SalePrice + "\n" + "Zip Code:" + " " + GetCarDetail.Userzipcode + "\n" + "Customer Name: \n" + _userDetail.FirstName + " " + _userDetail.LastName + "\n " + "Phone Number:\n" + _userDetail.UserContact + "\n" + "has been accepted. Congratulations on your successful bid!",
                        from: new Twilio.Types.PhoneNumber("+18554655815"), // Client twilio account
                        to: new Twilio.Types.PhoneNumber("+13122968428")
                    //from: new Twilio.Types.PhoneNumber("+13157400691"), //babar twilio account
                    //to: new Twilio.Types.PhoneNumber("+923214914514")
                    );


                    // sending offer message 

                }



                //end update car detail adding userDetailId





                return RedirectToAction("ThankyouPage", "Home", new { offerstatus = 0 });

            }


            return RedirectToAction("ResultPage", "Home", new { msg = "Your Details are Not submit please Submit try again !", color = "red" });

        }


        [HttpPost]
        public ActionResult PostOfferRejectedDetails(UserCarDetail _userDetail, HttpPostedFileBase[] Images = null, string SignedImageUrl = null, string email = "", string password = "", int Islogin = -1)
        {

            //Register User

            var UserId = 0;

            if (email != null && password != null)
            {
                if (Islogin != 1)
                {
                    User usr = new User()
                    {
                        Name = _userDetail.FirstName + " " + _userDetail.LastName,
                        Contact = _userDetail.UserContact,
                        Email = email.Trim(),
                        Password = StringCipher.Encrypt(password),
                        //Gender = _user.Gender,                              //   2/18/2022
                        Gender = 1,                                           //   2/18/2022
                        Role = 2,
                        IsActive = 1,
                        CreatedAt = gp.DateTimeNow()
                    };

                    UserId = new UserBL().AddNewUser(usr, de);


                }

                else
                {
                    //User user = new UserBL().GetActiveUsersList(de).Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && StringCipher.Decrypt(x.Password).Equals(password)).FirstOrDefault();
                    var LoginUser = gp.AddCookiesIdentity(email, password);
                    User user = gp.ValidateLoggedinUser();



                    if (user != null)
                    {
                        UserId = user.Id;

                    }
                    //return RedirectToAction("PostLogin", "Auth", new { Email = email, Password = password, Islogin=1 });
                }

            }




            //Register User

            // save signature image 
            var SignaturePath = "";
            var test = "";
            if (SignedImageUrl != "")
            {
                byte[] imageBytes = Convert.FromBase64String(SignedImageUrl.Replace("data:image/png;base64,", String.Empty));
                Image image;
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    var path = "~/Content/BarnDiagram";
                    if (!System.IO.File.Exists(path))
                    {
                        Directory.CreateDirectory(Server.MapPath("~") + "/Content/BarnDiagram/");

                    }
                    image = Image.FromStream(ms);
                    string fileName = "BarnDiagram-" + DateTime.Now.ToString("yymmddfff") + ".png";
                    test = "/Content/BarnDiagram/" + fileName;
                    SignaturePath = Path.Combine(Server.MapPath("~/Content/BarnDiagram/"), fileName);
                    // file is uploaded
                    image.Save(SignaturePath, System.Drawing.Imaging.ImageFormat.Png);
                    // releasing resources
                    image.Dispose();
                }
            }




            //end save signature image 

            UserCarDetail u = new UserCarDetail()
            {
                FirstName = _userDetail.FirstName,
                MidName = _userDetail.MidName,
                LastName = _userDetail.LastName,
                VehicleAddress = _userDetail.VehicleAddress,
                City = _userDetail.City,
                IsRejected = 1,
                State = _userDetail.State,                                           //   2/18/2022
                ZipCode = _userDetail.ZipCode,
                UserContact = _userDetail.UserContact,
                IsActive = 1,
                CreatedAt = gp.DateTimeNow(),
                UserSignature= test,
                UserId= UserId
            };

            bool chkUser = new UserBL().AddUserDetail(u, de);

            int ids = -1;
            if (chkUser)
            {
                var GetUserDetailsLastRecord = new UserBL().GetActiveLastRecordUserDetail(de);

                var carimageUpload = UploadImages(GetUserDetailsLastRecord.Id, Images);
                string BaseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/");

                //MailSender.SendOfferEmail("hassan@nodlays.com", GetUserDetailsLastRecord.Id, BaseUrl);

                // update car detail adding userDetailId

                CarDetail GetCarDetail = new CarDetailBL().GetActiveCarDetailLastRecord(de);

                if (GetCarDetail != null)
                {
                    GetCarDetail.UserCarDetailId = GetUserDetailsLastRecord.Id;
                    var updateCarDetails = new CarDetailBL().UpdateCarDetails(GetCarDetail, de);



                    // sending reject offer message 



                    string accountSid = ProjectVariables.AccountSid;
                    string authToken = ProjectVariables.AuthToken;

                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: " Car offer for the vehicle associated with VIN number:" + " " + GetCarDetail.vin + " " + "has been Rejected,lets see your bid!",

                        from: new Twilio.Types.PhoneNumber("+18554655815"),
                        to: new Twilio.Types.PhoneNumber("+13122968428")
                    );


                    //end sending  message 
                }


                // update car detail adding userDetailId



                return RedirectToAction("ThankyouPage", "Home", new { offerstatus = 1 });

            }
            return RedirectToAction("ResultPage", "Home", new { msg = "Your Details are Not submit please Submit try again !", color = "red" });

        }

        public bool UploadImages(int userDetailId, HttpPostedFileBase[] imageFiles)
        {
            if (imageFiles != null && imageFiles.Length > 0)
            {
                foreach (var imageFile in imageFiles)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var fileName = DateTime.Now.Ticks + "_" + Path.GetFileName(imageFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/assets/images"), fileName);
                        imageFile.SaveAs(path);

                        CarImage img = new CarImage()
                        {
                            ImagePath = path,
                            CreatedAt = DateTime.Now,
                            IsActive = 1,
                            UserDetailId = userDetailId,

                        };
                        img.ImagePath = "/Content/assets/images/" + fileName;
                        bool chkUser = new UserBL().AddCarDetailImages(img, de);
                        if (!chkUser)
                        {
                            return false;

                        }

                    }
                }
            }
            return true;
        }
        public ActionResult Contact()
        {
            return View();
        }

        //public ActionResult ResultPage(string price = "", string msg = "", List<string> jsonFormat = null)
        //{
        //    ViewBag.Price = price;
        //    ViewBag.msg = msg;
        //    if(jsonFormat != null)
        //    {
        //        ViewBag.jsonFormat = jsonFormat;
        //    }
        //    return View();
        //}

        public ActionResult Contactss()
        {
            return View();
        }

        public ActionResult ResultPage(string Year = "", string Make = "",
            string Model = "", string postalCode = "", string odometerReadings = "",
            string Doesitdrive = "",
            string transRemove = "", string Title = "", string vin = "",
            string driveablestatus = "", string doesStart = "", string powerTrain = "",
            string FrontEndDamage = "", string RearEndDamage = "", string LeftSideDamage = "",
            string RightSideDamage = "", string FloodOrFireDamage = "", string AirbagDeployed = "",
            string AnyPartIsRemoved = "", string Usercity = "", string Userstate = "", string Userzipcode = "",string DamageSeverity="",
            string RepairCost = "")
        {
            try
            {
                List<double> getPrices = new List<double>();
                List<string> JsonFormate = new List<string>();
                List<string> Json = new List<string>();
                var prices = "";
                var msg = "";
                var getAllSecondaryLists = "";
                //var DamageSeverity = "";
                var Burn = "";
                #region token genrate

                //oldApicall
                //var client = new RestClient("https://c-auth-qa4.copart.com/employee/oauth/token"); 
                //request.AddHeader("authorization", "Basic YjJiLXJvdW5kbGFrZS1hdXRvOjMwZjY1ZGM0ZGNiYTQ0Yzk5MjUwZDlhNDU3ZGE3YTc5");

                var client = new RestClient(ProjectVariables.CopartClientApi);
                var request = new RestRequest(Method.POST);
                var AuthorizationHeaders = AuthorizationHeader(ProjectVariables.ClientId, ProjectVariables.ClientSecretKey);
                request.AddHeader("Authorization", string.Format(AuthorizationHeaders));

                string credentials = "grant_type=client_credentials";
                request.AddParameter("application/x-www-form-urlencoded", credentials, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var tokenresponse = JsonConvert.DeserializeObject(response.Content);
                var token = response.Content;
                var x = token.Split('"');
                if (x.Count() >= 1)
                {
                    token = x[3];
                }
                string ResponseOffer = "You are ran out of system time please refresh your system and try again ";

                #endregion

               // token = null; //for nodlays server
                if (token != null)
                {
                    string titleCategorys = Title;
                    string postal = postalCode.ToString();
                    string years = Year;
                    string makeCodes = Make;
                    string vehicleTypes = "A";
                    string models = Model;
                    string odometerReading = odometerReadings;
                    string hasKeyss = "Y";

                    if (driveablestatus == "D")
                    {
                        driveablestatus = "Y";
                        doesStart = "D";
                    }

                    if (doesStart == "Y" /*|| doesStart == "D"*/)
                    {
                        doesStart = "S";
                        //doesStart = "D";
                        hasKeyss = "Y";
                    }
                    else if (doesStart == "D")
                    {
                        hasKeyss = "Y";

                    }
                    else
                    {
                        hasKeyss = "N";
                    }

                    if (odometerReadings == "")
                    {
                        odometerReadings = "50000";
                    }

                    if (!string.IsNullOrEmpty(Doesitdrive))
                    {
                        transRemove = Doesitdrive;
                    }
                    if (transRemove != "ST" && transRemove != "MN")
                    {
                        transRemove = "MC";
                    }

                    if (!string.IsNullOrEmpty(FloodOrFireDamage) && FloodOrFireDamage != "NO")
                    {
                        var Burns = "B";
                        var Flood = "WA";
                        getAllSecondaryLists = Flood;
                        Burn = Burns;
                    }


                    ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title, RepairCost);
                    JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                    if (JsonFormate[1] != "N/A")
                    {
                        getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                        Json.Add(JsonFormate[0]);
                    }
                    else
                    {
                        prices = "";
                        msg = "There's no offer for you available right now!";
                    }

                    #region SecondaryRelatedAPIs
                    if (!string.IsNullOrEmpty(FrontEndDamage) && FrontEndDamage != "NO")
                    {
                        getAllSecondaryLists = FrontEndDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title, RepairCost);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            prices = "";
                            msg = "There's no offer for you available right now!";
                        }
                    }
                    if (!string.IsNullOrEmpty(AnyPartIsRemoved) && AnyPartIsRemoved != "NO")
                    {
                        getAllSecondaryLists = AnyPartIsRemoved;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title, RepairCost);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            prices = "";
                            msg = "There's no offer for you available right now!";
                        }
                    }
                    if (!string.IsNullOrEmpty(RearEndDamage) && RearEndDamage != "NO")
                    {
                        getAllSecondaryLists = RearEndDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title, RepairCost);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            prices = "";
                            msg = "There's no offer for you available right now!";
                        }
                    }
                    if (!string.IsNullOrEmpty(LeftSideDamage) && LeftSideDamage != "NO")
                    {
                        getAllSecondaryLists = LeftSideDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title, RepairCost);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            prices = "";
                            msg = "There's no offer for you available right now!";
                        }
                    }
                    if (!string.IsNullOrEmpty(RightSideDamage) && RightSideDamage != "NO")
                    {
                        getAllSecondaryLists = RightSideDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title, RepairCost);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            prices = "";
                            msg = "There's no offer for you available right now!";
                        }
                    }
                    #endregion


                    if (getPrices.Count() > 0)
                    {
                        double savePrice = 0;
                        foreach (var price in getPrices)
                        {
                            savePrice = savePrice + price;
                        }
                        int getPrice = getPrices.Count();
                        var getSavePrice = savePrice / getPrice;
                        //change price by client 
                        getSavePrice = getSavePrice - 165;
                        var FinalOffer = getSavePrice * 0.67;
                        //prices = /*"$" +*/ System.Math.Round(FinalOffer).ToString();
                        prices = String.Format("{0:0.00}",FinalOffer);
                    }
                }

                ViewBag.msg = msg;
                var priceArray = prices.Split('$');
                ViewBag.Price = prices;
                 //ViewBag.Price = "1000"; //for nodlays server
                ViewBag.Json = Json;

                //babar

                ViewBag.usercity = Usercity;
                ViewBag.userstate = Userstate;
                ViewBag.userzipcode = Userzipcode;

                //dto transfer 
                CarDetail dto=new CarDetail();
                dto.Title = Title;
                dto.Year = Year;
                dto.Model = Model;
                dto.postalCode = postalCode;
                dto.odometerReadings = odometerReadings;
                dto.doesStart = doesStart;
                dto.driveablestatus=driveablestatus;
                dto.DamageSeverity = DamageSeverity;
                dto.AnyPartIsRemoved = AnyPartIsRemoved;
                dto.AirbagDeployed= AirbagDeployed;
                dto.FloodOrFireDamage= FloodOrFireDamage;
                dto.FrontEndDamage = FrontEndDamage;
                dto.LeftSideDamage = LeftSideDamage;
                dto.RightSideDamage= RightSideDamage;
                dto.transRemove = transRemove;
                dto.Usercity = Usercity;
                dto.Userstate = Userstate;
                dto.Userzipcode = postalCode;
                dto.RearEndDamage = RearEndDamage;
                dto.RepairCost = RepairCost;
                dto.Make=Make;
                dto.vin=vin;
                dto.powerTrain= powerTrain;
                dto.CreatedAt = DateTime.Now;
                dto.IsActive = 1;
                dto.SalePrice = prices;
                //dto.SalePrice = "1000";

                var AddCarDetails = new CarDetailBL().AddCarDetail(dto,de);

                return View();

            }
            catch (Exception e)
            {

                ViewBag.usercity = Usercity;
                ViewBag.userstate = Userstate;
                ViewBag.userzipcode = Userzipcode;
                return View("ResultPage");

                // return RedirectToAction("ResultPage", "Home", new
                //{
                //    price = "",
                //    msg = "Kindly provide a valid data !"
                //});
            }
        }



        private string AuthorizationHeader(string CliendId, string SecretKey)
        {
            var AuthorizationHeader = System.Text.Encoding.UTF8.GetBytes($"{CliendId}:{SecretKey}");
            var encoded = Convert.ToBase64String(AuthorizationHeader);
            return $"Basic {encoded}";
        }
        public string getReponse(string token, string postal = "", string Burn = "",
             string getAllSecondaryLists = "", string DamageSeverity = "",
             string AirbagDeployed = "", string vin = "", string Year = "",
             string Make = "", string Model = "", string vehicleTypes = "",
             string odometerReading = "", string hasKeyss = "", string powerTrain = "",
             string transRemove = "", string doesStart = "",
             string driveablestatus = "", string Title = "", string repairCost = ""
            )
        {

            try
            {
                var clientss = new RestClient(ProjectVariables.ProQuoteApi);
                clientss.Timeout = -1;
                var requestss = new RestRequest(Method.POST);
                requestss.AddHeader("Content-Type", "application/json");
                requestss.AddHeader("Authorization", "Bearer " + token);
                requestss.AddHeader("Cookie", "copartgauth=5ad246159e1f6e8cbf1476da59d9eec8; incap_ses_960_2016155=0n2vLIQB1Rhu+U2bkZtSDTjaumIAAAAARarAEBs3Kz9ucv8pKhvtzg==; visid_incap_2016155=lmYilV2bRQS1T4iTp4gBxEnesmIAAAAAQUIPAAAAAAA+WiIRfIKIoF9mMK/QsGvA");

                var reader = new StreamReader(Server.MapPath("~/Content/Frontassets/basicjsonAttr.json"));
                var body = reader.ReadToEnd();



                body = body.Replace("_postalCode_", postal);
                body = body.Replace("_lossType_", Burn);

                body = body.Replace("_primaryPointOfImpact_", transRemove);
                body = body.Replace("_secondaryPointOfImpact_", getAllSecondaryLists);
                body = body.Replace("_damageSeverity_", DamageSeverity);
                body = body.Replace("_airbagsDeployed_", AirbagDeployed);

                body = body.Replace("_vin_", vin.TrimEnd());
                body = body.Replace("_year_", Year);
                body = body.Replace("_makeCode_", Make);
                body = body.Replace("_makeDescription_", Make);
                body = body.Replace("_model_", Model);
                body = body.Replace("_vehicleType_", vehicleTypes);

                body = body.Replace("_odometerReading_", odometerReading);
                body = body.Replace("_odometerBrand_", "Actual");


                body = body.Replace("_hasKeys_", hasKeyss);
                body = body.Replace("_powerTrain_", powerTrain);

                //body = body.Replace("_repairCost_", repairCost);


                body = body.Replace("_drivable_", driveablestatus); // Not  In PostManApi
                body = body.Replace("_drivabilityRating_", doesStart);
                body = body.Replace("_titleCategory_", Title);

                //"acv": "_acv_", remove from json


                requestss.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse responses = clientss.Execute(requestss);
                var ResponseOffer = responses.Content;


                // for create a log of response 

                var path = Path.Combine(Server.MapPath("~/Content/assets/logs.txt"));
                var dt = DateTime.Now.ToString("g");

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLineAsync("DateTime: " + dt + "---------------- Vin Number: " + vin.TrimEnd() + " ------------------------ Response Offer: " + ResponseOffer);

                    writer.Close();
                }

                // end create a log of response 



                var offer = ResponseOffer.Split('"');
                if (offer.Count() >= 1)
                {
                    ResponseOffer = offer[19];
                    if (ResponseOffer == "text/javascript" || ResponseOffer == "margin:0px;height:100%")
                    {
                        ResponseOffer = "N/A";
                    }
                }
                List<string> JsonFormate = new List<string>();
                JsonFormate.Add(body);
                JsonFormate.Add(ResponseOffer);
                var y = JsonConvert.SerializeObject(JsonFormate, Formatting.Indented);
                //return ResponseOffer;
                return y;
            }
            catch(Exception ex)
            {
                var path = Path.Combine(Server.MapPath("~/Content/assets/errorsLog.txt"));
                var dt = DateTime.Now.ToString("g");

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLineAsync("DateTime: " + dt + "---------------- Vin Number: " + vin.TrimEnd() + " ------------------------ Error: " + ex.InnerException.Message.ToString());
                    writer.Close();
                }
                return "N/A";
            }
        }

    }
}



