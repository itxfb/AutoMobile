using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarSystem.Models;
using CarSystem.BL;
using CarSystem.Helping_Classes;
//using CountryData.Standard;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Reflection;

namespace CarSystem.Controllers
{
    public class AjaxController : Controller
    {
        private GeneralPurpose gp = new GeneralPurpose();
        private DatabaseEntities de = new DatabaseEntities();

        #region User Starting

        [HttpPost]
        public ActionResult GetUserDataTableList(string name = "", string email = ""/*, int gender = -1*/)  //  change   2/18/2022
        {
            #region for custom search

            List<User> ulist = new UserBL().GetActiveUsersList(de).Where(x => x.Role == 2).ToList();

            if (name != "")
            {
                ulist = ulist.Where(x => x.Name.ToLower().Contains(name.Trim().ToLower())).ToList();
            }
            if (email != "")
            {
                ulist = ulist.Where(x => x.Email.ToLower().Contains(email.Trim().ToLower())).ToList();
            }
            //if (gender != -1)
            //{
            //    ulist = ulist.Where(x => x.Gender == gender).ToList();        change   2/18/2022
            //}

            #endregion custom search

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (!String.IsNullOrEmpty(sortColumnName))
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Contact != null && x.Contact.Trim().ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.Name != null && x.Name.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.Email != null && x.Email.ToLower().Contains(searchValue.Trim().ToLower())
                                    ).ToList();
            }
            int totalrowsafterfilterinig = ulist.Count();
            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            //string gen = "";                                      change   2/18/2022

            List<UserDTO> udto = new List<UserDTO>();
            foreach (User item in ulist)
            {

                //if (item.Gender == 2)
                //    gen = "Female";                               change   2/18/2022
                //else
                //    gen = "Male";

                UserDTO obj = new UserDTO()
                {
                    Id = item.Id,
                    EncId = StringCipher.EncryptId(item.Id),
                    Name = item.Name,
                    Email = item.Email,
                    Contact = item.Contact,
                    //Gender = gen                                       change   2 / 18 / 2022
                    Gender = "1"
                };

                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetOfferDetailDataTableList(string VehicleAddress = "", string City = "",string ZipCode="",int IsRejected=-1)  //  change   2/18/2022
        {
            var LoggedInUser = gp.ValidateLoggedinUser();
            #region for custom search
            List<UserCarDetail> ulist = new UserBL().GetActiveUserCarDetailsList(de).Where(x => x.IsActive == 1).OrderByDescending(a => a.Id).ToList();

            if(LoggedInUser.Role==2)
            {
                ulist = ulist.Where(x => x.UserId == LoggedInUser.Id).ToList();
            }

            if (IsRejected!=-1 && IsRejected==0)
            {
                ulist = ulist.Where(x => x.IsActive == 1 && x.IsRejected == 0).OrderByDescending(a => a.Id).ToList();

            }
            else if(IsRejected != -1 && IsRejected == 1)
            {
                ulist = ulist.Where(x => x.IsActive == 1 && x.IsRejected == 1).OrderByDescending(a => a.Id).ToList();

            }


            if (!string.IsNullOrEmpty(VehicleAddress))
            {
                ulist = ulist.Where(x =>x.VehicleAddress.Trim().ToLower().Contains(VehicleAddress.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(City))
            {
                ulist = ulist.Where(x => x.City.ToLower().Contains(City.Trim().ToLower())).ToList();
            }
            //if (gender != -1)
            //{
            //    ulist = ulist.Where(x => x.Gender == gender).ToList();        change   2/18/2022
            //}

            #endregion custom search

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (!String.IsNullOrEmpty(sortColumnName))
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.FirstName.Trim().ToLower().Contains(searchValue.Trim().ToLower())||x.VehicleAddress.Trim().ToLower().Contains(searchValue.Trim().ToLower())||x.UserContact.Trim().ToLower().Contains(searchValue.Trim().ToLower())||x.City.Trim().ToLower().Contains(searchValue.Trim().ToLower())||x.State.Trim().ToLower().Contains(searchValue.Trim().ToLower())||x.ZipCode.Trim().ToLower().Contains(searchValue.Trim().ToLower())
                                 
                                    ).ToList();
            }
            int totalrowsafterfilterinig = ulist.Count();
            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            //string gen = "";                                      change   2/18/2022

            //List<UserDTO> udto = new List<UserDTO>();
            //foreach (User item in ulist)
            //{

            //    //if (item.Gender == 2)
            //    //    gen = "Female";                               change   2/18/2022
            //    //else
            //    //    gen = "Male";

            //    UserDTO obj = new UserDTO()
            //    {
            //        Id = item.Id,
            //        EncId = StringCipher.EncryptId(item.Id),
            //        Name = item.Name,
            //        Email = item.Email,
            //        Contact = item.Contact,
            //        //Gender = gen                                       change   2 / 18 / 2022
            //        Gender = "1"
            //    };

            //    udto.Add(obj);
            //}

            return Json(new { data = ulist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetRejectedOfferDetailDataTableList(string VehicleAddress = "", string City = "", string ZipCode = "")  //  change   2/18/2022
        {
            #region for custom search

            List<UserCarDetail> ulist = new UserBL().GetActiveUserCarDetailsList(de).Where(x => x.IsActive == 1 && x.IsRejected == 1).OrderByDescending(a => a.Id).ToList();

            if (!string.IsNullOrEmpty(VehicleAddress))
            {
                ulist = ulist.Where(x => x.VehicleAddress.Trim().ToLower().Contains(VehicleAddress.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(City))
            {
                ulist = ulist.Where(x => x.City.ToLower().Contains(City.Trim().ToLower())).ToList();
            }
            //if (gender != -1)
            //{
            //    ulist = ulist.Where(x => x.Gender == gender).ToList();        change   2/18/2022
            //}

            #endregion custom search

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (!String.IsNullOrEmpty(sortColumnName))
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.FirstName.Trim().ToLower().Contains(searchValue.Trim().ToLower())

                                    ).ToList();
            }
            int totalrowsafterfilterinig = ulist.Count();
            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            //string gen = "";                                      change   2/18/2022

            //List<UserDTO> udto = new List<UserDTO>();
            //foreach (User item in ulist)
            //{

            //    //if (item.Gender == 2)
            //    //    gen = "Female";                               change   2/18/2022
            //    //else
            //    //    gen = "Male";

            //    UserDTO obj = new UserDTO()
            //    {
            //        Id = item.Id,
            //        EncId = StringCipher.EncryptId(item.Id),
            //        Name = item.Name,
            //        Email = item.Email,
            //        Contact = item.Contact,
            //        //Gender = gen                                       change   2 / 18 / 2022
            //        Gender = "1"
            //    };

            //    udto.Add(obj);
            //}

            return Json(new { data = ulist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCarImagesDataTableList(int userDetailId=-1,string name = "", string email = ""/*, int gender = -1*/)  //  change   2/18/2022
        {
            #region for custom search

            List<CarImage> ulist = new UserBL().GetCarImagesList(de).Where(x => x.IsActive == 1 && x.UserDetailId== userDetailId).ToList();

           

            #endregion custom search

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (!String.IsNullOrEmpty(sortColumnName))
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.ImagePath.Trim().ToLower().Contains(searchValue.Trim().ToLower())

                                    ).ToList();
            }
            int totalrowsafterfilterinig = ulist.Count();
            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            //string gen = "";                                      change   2/18/2022

            //List<UserDTO> udto = new List<UserDTO>();
            //foreach (User item in ulist)
            //{

            //    //if (item.Gender == 2)
            //    //    gen = "Female";                               change   2/18/2022
            //    //else
            //    //    gen = "Male";

            //    UserDTO obj = new UserDTO()
            //    {
            //        Id = item.Id,
            //        EncId = StringCipher.EncryptId(item.Id),
            //        Name = item.Name,
            //        Email = item.Email,
            //        Contact = item.Contact,
            //        //Gender = gen                                       change   2 / 18 / 2022
            //        Gender = "1"
            //    };

            //    udto.Add(obj);
            //}

            return Json(new { data = ulist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOfferDetails(int userDetailId = -1)  //  change   2/18/2022
        {
            List<CarImage> ulist = new UserBL().GetCarImagesList(de).Where(x => x.IsActive == 1 && x.UserDetailId == userDetailId).ToList();

            return Json(ulist);

        }

        [HttpPost]
        public ActionResult CheckCityByZipCode(string CityName = "")  //  change   2/18/2022
        {
            var GetCityData = new CarDetailBL().GetActiveCityByName(CityName, de);

            if (GetCityData != null)
            {

                return Json(true);
            }
            else
            {
                return Json(false);
            }

            return Json("");

        }

        [HttpPost]
        public ActionResult CheckUserExist(string email = "",string password="")  //  change   2/18/2022
        {
            var decryptPw=StringCipher.Decrypt("7eXv10VDazR1et8ZmaW5KghPuuU1Og0X+3WDa4kig92oO6UTaI/abQyNOUiXDkGn3rGIjD2lfJBn93u4mbRRtV/417YaFC6qg5zg0NCX6nd0BAicjmfK3Fbp7J9rARLi");
            User user = new UserBL().GetActiveUsersList(de).Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && StringCipher.Decrypt(x.Password).Equals(password)).FirstOrDefault();

            
            if (user != null)
            {

                return Json(true);
            }
            else
            {
                return Json(false);
            }

            return Json("");

        }

        [HttpPost]
        public ActionResult GetUserById(int id)
        {
            //string gender = "";                                        change   2/18/2022
            User user = new UserBL().GetActiveUserById(id, de);
            if (user == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            //if (user.Gender == 2)
            //    gender = "Female";                                     change   2/18/2022
            //else
            //    gender = "Male";

            UserDTO obj = new UserDTO()
            {
                Id = user.Id,
                EncId = StringCipher.EncryptId(user.Id),
                Name = user.Name,
                Email = user.Email,
                Contact = user.Contact,
                //Gender = gender,                                        change   2/18/2022
                Gender = "1",
                Password = StringCipher.Decrypt(user.Password)
                //ProfilePath = u.ProfilePath,
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #endregion User Ending

        #region Car Detail Starting

        [HttpPost]
        public ActionResult GetCarDetailDataTableList(string Year = "", string Model = "", string damagetype = "",int id=-1)
        {

            #region for custom search

            List<CarDetail> cdlist = new CarDetailBL().GetActiveCarDetailList(de).ToList();
            //if(id!=-1)
            //{
            //    cdlist = cdlist.Where(x => x.UserCarDetailId == id && x.IsActive==1).ToList();
            //}

            if (Year != "")
            {
                cdlist = cdlist.Where(x => x.Year.ToLower()==Year.ToLower()).ToList();
            }
            if (Model != "")
            {
                cdlist = cdlist.Where(x => x.Model.ToLower().Contains(Model.ToLower())).ToList();
            }
            if (damagetype != "")
            {
                cdlist = cdlist.Where(x => x.DamageSeverity.ToLower().Contains(damagetype.ToLower())).ToList();
            }

            #endregion custom search



            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            if (!String.IsNullOrEmpty(sortColumnName))
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        cdlist = cdlist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        cdlist = cdlist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = cdlist.Count();
            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                cdlist = cdlist.Where(x => x.Year != null && x.Make.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.Make != null && x.Make.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.Model != null && x.Model.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.postalCode != null && x.postalCode.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.odometerReadings != null && x.odometerReadings.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.Doesitdrive != null && x.Doesitdrive.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.doesStart != null && x.doesStart.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.Title != null && x.Title.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.driveablestatus != null && x.driveablestatus.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.powerTrain != null && x.powerTrain.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.HasKey != null && x.HasKey.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.FrontEndDamage != null && x.FrontEndDamage.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.SalePrice != null && x.SalePrice.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.RepairCost != null && x.RepairCost.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.LeftSideDamage != null && x.RightSideDamage.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.RightSideDamage != null && x.RightSideDamage.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.FloodOrFireDamage != null && x.FloodOrFireDamage.ToLower().Contains(searchValue.Trim().ToLower()) ||
                                    x.AirbagDeployed != null && x.AirbagDeployed.ToLower().Contains(searchValue.Trim().ToLower())||
                                    x.DamageSeverity != null && x.DamageSeverity.ToLower().Contains(searchValue.Trim().ToLower())||
                                    x.RepairCost != null && x.RepairCost.ToLower().Contains(searchValue.Trim().ToLower())
                                    ).ToList();
            }
            int totalrowsafterfilterinig = cdlist.Count();
            // pagination
            cdlist = cdlist.Skip(start).Take(length).ToList();


            List<CarDetail> cd_Dto = new List<CarDetail>();
            foreach (CarDetail item in cdlist)
            {

                CarDetail obj = new CarDetail()
                {
                    Id = item.Id,
                    Year = item.Year,
                    Make = item.Make,
                    Model = item.Model,
                    postalCode = item.postalCode,
                    odometerReadings = item.odometerReadings,
                    doesStart = item.doesStart,
                    Doesitdrive = item.Doesitdrive,
                    transRemove = item.transRemove,
                    Title = item.Title,
                    vin = item.vin,
                    HasKey = item.HasKey,
                    powerTrain = item.powerTrain,
                    SalePrice = item.SalePrice,
                    RepairCost = item.RepairCost,
                    FrontEndDamage = item.FrontEndDamage,
                    RearEndDamage = item.RearEndDamage,
                    LeftSideDamage = item.LeftSideDamage,
                    RightSideDamage= item.RightSideDamage,
                    FloodOrFireDamage = item.FloodOrFireDamage,
                    AirbagDeployed = item.AirbagDeployed,
                    AnyPartIsRemoved = item.AnyPartIsRemoved,
                    Usercity = item.Usercity,
                    Userstate = item.Userstate,
                    driveablestatus = item.driveablestatus,
                    DamageSeverity= item.DamageSeverity
                };

                cd_Dto.Add(obj);
            }

            return Json(new { data = cd_Dto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult GetCarDetailById(int id)
        //{
        //    CarDetail cd = new CarDetailBL().GetActiveCarDetailById(id, de);
        //    if (cd == null)
        //    {
        //        return Json(0, JsonRequestBehavior.AllowGet);
        //    }


        //    //CarDetailDTO obj = new CarDetailDTO()
        //    CarDetail obj = new CarDetail()
        //    {
        //        Id = cd.Id,
        //        Year = cd.Year,
        //        Make = cd.Make,
        //        Model = cd.Model,
        //        LotRunCondition = cd.LotRunCondition,
        //        DamageTypeDescription = cd.DamageTypeDescription,
        //        CopartFacilityName = cd.CopartFacilityName,
        //        SaleTitleState = cd.SaleTitleState,
        //        SaleTitleType = cd.SaleTitleType,
        //        DamageType = cd.DamageType,
        //        LotColor = cd.LotColor,
        //        HasKey = cd.HasKey,
        //        OdometerReading = cd.OdometerReading,
        //        SalePrice = cd.SalePrice,
        //        RepairCost = cd.RepairCost
        //    };
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        #endregion Car Detail Ending


        #region Partail Views => form1
        [HttpPost]

        public ActionResult GetLotList(string Year = "", string Make = "", string Model = "")                             //int subjectId
        {

            if (!string.IsNullOrEmpty(Year) && string.IsNullOrEmpty(Make) && string.IsNullOrEmpty(Model))
            {
                //ViewBag.lotYearVal = Year;

                List<string> CDlist = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Year.ToLower() == Year.ToLower()).Select(x => x.Make).Distinct().ToList();

                return Json(CDlist, JsonRequestBehavior.AllowGet);
            }

            else if (!string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Make) && string.IsNullOrEmpty(Model))
            {
                //ViewBag.lotMakeVal = Make;

                List<string> CDlist = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Make.ToLower() == Year.ToLower() && x.Make.ToLower() == Make.ToLower()).Select(x => x.Model).Distinct().ToList();

                return Json(CDlist, JsonRequestBehavior.AllowGet);
            }

            else if (!string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Make) && !string.IsNullOrEmpty(Model))
            {
                ViewBag.lotModelVal = Model;

                List<string> CDlist = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Year.ToLower() == Year.ToLower() && x.Make.ToLower() == Make.ToLower() && x.Model.ToLower() == Model.ToLower()).Select(x => x.Model).Distinct().ToList();

                return Json(CDlist, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


        #endregion  end Partail Views => form1

        [HttpPost] //old code Ajax
        public ActionResult SubmitFullForm(string Year = "", string Make = "", string Model = "",
                                            int ZipCode = -1, string BodyStyleOfCar = "",
                                           string DriveWheel = "", string EngineSize = "",
                                           string Transmission = "", string FuelType = "",
                                           string OdometerReading = "", string[] PrimaryDamage = null,
                                           string SalePrice = "", string TitleType = "", string Countrycode = "", string Repaircost = "", string Vinssnumber = "", string Claimno = "")
        {
            //if(Countrycode=="1")
            //{
            //    Countrycode = "US";
            //}
            //var helper = new CountryHelper();
            //var regions = helper.GetRegionByCountryCode(Countrycode);

            //if(regions!=null)
            //{
            //    string countrycodesuccess = "your code matched with USA ISO Code";

            //    //var countryregions = regions.Select(x => x.Name).ToList();
            //}






            if (string.IsNullOrEmpty(Year))
            {
                return Json("Year is Required", JsonRequestBehavior.AllowGet);
            }
            List<CarDetail> cda = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Model == "SANTA FE").ToList();
            if (!string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Make) && !string.IsNullOrEmpty(Model))
            {

                CarDetail getObj = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Year.ToLower() == Year.ToLower()
                                    && x.Make.ToLower() == Make.ToLower() && x.Model.ToLower() == Model.ToLower()).FirstOrDefault();

                int count1 = 0;

                if (getObj == null)
                {

                    if (count1 == 0)
                    {
                        string niss = "NISS";
                        Make = niss;
                        List<string> lista = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Make.ToLower() == Make.ToLower())
                            .Select(x => x.Year).Distinct().ToList();
                        List<int> myLotYearIntList = lista.Select(s => int.Parse(s)).ToList();

                        var number = Convert.ToInt32(Year);

                        // find closest to number

                        int closestYear = myLotYearIntList.OrderBy(item => Math.Abs(number - item)).First();

                        string year = Convert.ToString(closestYear);

                        CarDetail getObj1 = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Year.ToLower() == year.ToLower()
                                        && x.Make.ToLower() == Make.ToLower() && x.Model.ToLower() == Model.ToLower()).FirstOrDefault();
                        if (getObj != null)
                        {
                            string name1 = getObj1.Year + " " + getObj1.Make + " " + getObj1.Model;

                            double GetSalePrice1 = Math.Round((Convert.ToDouble(getObj1.SalePrice) / 100) * 65, 2);

                            CarDetailDisplayNameAndPriceDTO cdDTO1 = new CarDetailDisplayNameAndPriceDTO()
                            {
                                Name = name1,
                                GetSalePrice = GetSalePrice1
                            };

                            return Json(cdDTO1, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            count1++;
                        }


                        if (count1 > 0)
                        {
                            List<string> modelList1 = (List<string>)new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Make.ToLower() == Make.ToLower()
                            && x.Year == year).Select(x => x.Model).Distinct().ToList();

                            for (int w = 0; w <= modelList1.Count - 1; w++)
                            {
                                string model = modelList1[w];

                                CarDetail getObj2 = new CarDetailBL().GetActiveCarDetailList(de).Where(x => x.Year.ToLower() == year.ToLower()
                                            && x.Make.ToLower() == Make.ToLower() && x.Model.ToLower() == model.ToLower() &&
                                            x.SalePrice == SalePrice).FirstOrDefault();

                                if (getObj2 != null)
                                {
                                    string name2 = getObj2.Year + " " + getObj2.Make + " " + getObj2.Model;

                                    double GetSalePrice2 = Math.Round((Convert.ToDouble(getObj1.SalePrice) / 100) * 65, 2);

                                    CarDetailDisplayNameAndPriceDTO cdDTO2 = new CarDetailDisplayNameAndPriceDTO()
                                    {
                                        Name = name2,
                                        GetSalePrice = GetSalePrice2
                                    };

                                    return Json(cdDTO2, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    continue;
                                }
                            }


                        }


                    }


                    CarDetailDisplayNameAndPriceDTO cdDTO3 = new CarDetailDisplayNameAndPriceDTO()
                    {
                        Name = "Not Found",
                        GetSalePrice = 0.00
                    };
                    return Json(cdDTO3, JsonRequestBehavior.AllowGet);


                    //CarDetailDisplayNameAndPriceDTO cdDTO1 = new CarDetailDisplayNameAndPriceDTO()
                    //{
                    //   Name = "Not Found",
                    //   GetSalePrice = 0.00
                    //};
                    //return Json(cdDTO1, JsonRequestBehavior.AllowGet);
                }

                string name = getObj.Year + " " + getObj.Make + " " + getObj.Model;

                double GetSalePrice = Math.Round((Convert.ToDouble(getObj.SalePrice) / 100) * 65, 2);

                CarDetailDisplayNameAndPriceDTO cdDTO = new CarDetailDisplayNameAndPriceDTO()
                {
                    Name = name,
                    GetSalePrice = GetSalePrice
                };

                return Json(cdDTO, JsonRequestBehavior.AllowGet);


            }

            CarDetailDisplayNameAndPriceDTO cdDTO4 = new CarDetailDisplayNameAndPriceDTO()
            {
                Name = "Not Found",
                GetSalePrice = 0.00
            };
            return Json(cdDTO4, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ValidateEmail(string email, int id = -1)
        {
            return Json(gp.ValidateEmail(email, id), JsonRequestBehavior.AllowGet);
        }



        //public ActionResult CheckCountryCode(string Countrycode = "")                             // check US countrycode Validity
        //{
        //    if (Countrycode == "1")
        //    {
        //        Countrycode = "US";
        //    }
        //    var helper = new CountryHelper();
        //    string countrycodesuccess = "";

        //    if (Countrycode == "US")
        //    {
        //        var regions = helper.GetRegionByCountryCode(Countrycode);
        //        if (regions != null)
        //        {
        //            countrycodesuccess = "your code matched with USA ISO Code";

        //            return Json(countrycodesuccess, JsonRequestBehavior.AllowGet);
        //            //var countryregions = regions.Select(x => x.Name).ToList();
        //        }
        //    }



        //    return Json(countrycodesuccess, JsonRequestBehavior.AllowGet);
        //}



        //APi Implementation of token and Auto car price Genrating
        public ActionResult GenerateOfferPrice(string Year = "", string Make = "",
            string Model = "", string postalCode = "", string odometerReadings = "",
            string transRemove = "", string Title = "", string vin = "",
            string driveablestatus = "", string doesStart = "", string powerTrain = "",
            string FrontEndDamage = "", string RearEndDamage = "", string LeftSideDamage = "",
            string RightSideDamage = "", string FloodOrFireDamage = "", string AirbagDeployed = "" )
        {
            try
            {
                List<double> getPrices= new List<double>();
                List<string> JsonFormate = new List<string>();
                List<string> Json = new List<string>();
                var getAllSecondaryLists = "";
                var DamageSeverity = "M";
                var Burn = "N";
                #region token genrate
                var client = new RestClient("https://c-auth-qa4.copart.com/employee/oauth/token");
                var request = new RestRequest(Method.POST);

                request.AddHeader("authorization", "Basic YjJiLXJvdW5kbGFrZS1hdXRvOjMwZjY1ZGM0ZGNiYTQ0Yzk5MjUwZDlhNDU3ZGE3YTc5");
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


                if (token != null)
                {
                    //var clientss = new RestClient("https://b2b-stg.copart.com/v1/proquote");
                    //clientss.Timeout = -1;
                    //var requestss = new RestRequest(Method.POST);
                    //requestss.AddHeader("Content-Type", "application/json");
                    //requestss.AddHeader("Authorization", "Bearer " + token);
                    //requestss.AddHeader("Cookie", "copartgauth=5ad246159e1f6e8cbf1476da59d9eec8; incap_ses_960_2016155=0n2vLIQB1Rhu+U2bkZtSDTjaumIAAAAARarAEBs3Kz9ucv8pKhvtzg==; visid_incap_2016155=lmYilV2bRQS1T4iTp4gBxEnesmIAAAAAQUIPAAAAAAA+WiIRfIKIoF9mMK/QsGvA");

                    //var reader = new StreamReader(Server.MapPath("~/Content/Frontassets/basicjsonAttr.json"));
                    //var body = reader.ReadToEnd();

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
                        doesStart = "D";
                    }

                    if (doesStart == "Y")
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

                    string repairCosts = 1.ToString();
                    if (!string.IsNullOrEmpty(FrontEndDamage) && FrontEndDamage != "NO")
                    {
                        transRemove = FrontEndDamage;
                    }
                    else
                    {
                        if (transRemove == "ST")
                        {
                            transRemove = "ST";
                            doesStart = "N";
                        }
                    }
                    if (transRemove.ToLower() == "fr")
                    {
                        DamageSeverity = "H";
                    }
                    else
                    {
                        DamageSeverity = "M";
                    }
                    if (!string.IsNullOrEmpty(FloodOrFireDamage) && FloodOrFireDamage != "NO")
                    {
                        var Burns = "B";
                        var Flood = "WA";
                        getAllSecondaryLists = Flood;
                        Burn = Burns;
                    }
                    else
                    {
                        getAllSecondaryLists = "MN";
                    }
                    if (string.IsNullOrEmpty(transRemove))
                    {
                        transRemove = getAllSecondaryLists;
                        getAllSecondaryLists = "";
                    }
                    ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title);
                    JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                    if (JsonFormate[1] != "N/A")
                    {
                        getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                        Json.Add(JsonFormate[0]);
                    }
                    else
                    {
                        return RedirectToAction("ResultPage", "Home", new { price = "", msg = "There's no offer for you available right now! ", jsonFormat = Json });
                    }

                    #region SecondaryRelatedAPIs

                    if (!string.IsNullOrEmpty(RearEndDamage) && RearEndDamage != "NO")
                    {
                        getAllSecondaryLists = RearEndDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            return RedirectToAction("ResultPage", "Home", new { price = "There's no offer for you available right now! ", jsonFormat = Json });
                        }
                    }
                    if (!string.IsNullOrEmpty(LeftSideDamage) && LeftSideDamage != "NO")
                    {
                        getAllSecondaryLists = LeftSideDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            return RedirectToAction("ResultPage", "Home", new { price = "There's no offer for you available right now! ", jsonFormat = Json });
                        }
                    }
                    if (!string.IsNullOrEmpty(RightSideDamage) && RightSideDamage != "NO")
                    {
                        getAllSecondaryLists = RightSideDamage;
                        ResponseOffer = getReponse(token, postal, Burn, getAllSecondaryLists, DamageSeverity, AirbagDeployed, vin, Year,
                        Make, Model, vehicleTypes, odometerReading, hasKeyss, powerTrain, transRemove, doesStart, driveablestatus,
                        Title);
                        JsonFormate = JsonConvert.DeserializeObject<List<string>>(ResponseOffer);

                        if (JsonFormate[1] != "N/A")
                        {
                            getPrices.Add(Convert.ToDouble(JsonFormate[1]));
                            Json.Add(JsonFormate[0]);
                        }
                        else
                        {
                            return RedirectToAction("ResultPage", "Home", new { price = "There's no offer for you available right now! ", jsonFormat = Json });
                        }
                    }
                    #endregion

                    //body = body.Replace("_vin", vin.TrimEnd());
                    //body = body.Replace("_postalCode_", postal);
                    //body = body.Replace("_primaryPointOfImpact_", transRemove);
                    //body = body.Replace("_secondaryPointOfImpact_", getAllSecondaryLists);
                    //body = body.Replace("_year_", years);
                    //body = body.Replace("_makeDescription_", makeCodes);
                    //body = body.Replace("_makeCode_", makeCodes);
                    //body = body.Replace("_vehicleType_", vehicleTypes);
                    //body = body.Replace("_model_", models);
                    //body = body.Replace("_odometerReading_", odometerReading);
                    //body = body.Replace("_odometerBrand_", "Actual");
                    //body = body.Replace("_hasKeys_", hasKeyss);
                    //body = body.Replace("_acv_", 20000.ToString());
                    //body = body.Replace("_repairCost_", repairCosts);
                    //body = body.Replace("_titleCategory_", titleCategorys);
                    //body = body.Replace("_lossType_", Burn);
                    //body = body.Replace("_drivable_", doesStart);
                    //body = body.Replace("_drivabilityRating_", driveablestatus);
                    //body = body.Replace("_powerTrain_", powerTrain);
                    //body = body.Replace("_damageSeverity_", DamageSeverity);
                    //body = body.Replace("_airbagsDeployed_", AirbagDeployed);


                    //requestss.AddParameter("application/json", body, ParameterType.RequestBody);
                    //IRestResponse responses = clientss.Execute(requestss);


                    //var offer = ResponseOffer.Split('"');
                    //if (offer.Count() >= 1)
                    //{
                    //    ResponseOffer = offer[19];
                    //    if (ResponseOffer == "text/javascript")
                    //    {
                    //        ResponseOffer = "N/A";
                    //    }
                    //    else
                    //    {
                    //        var offerprice = Convert.ToDouble(ResponseOffer) * 0.67; // 67% of it
                    //        var finaloffer = "$" + Convert.ToInt32(offerprice).ToString();
                    //        return RedirectToAction("ResultPage", "Home", new { price = finaloffer });
                    //    }
                    //}
                    if(getPrices.Count() > 0)
                    {
                        double savePrice = 0;
                        foreach(var price in getPrices)
                        {
                            savePrice = savePrice + price;
                        }
                        int getPrice = getPrices.Count();
                        var getSavePrice = savePrice / getPrice;
                        var FinalOffer = getSavePrice * 0.67;
                        return RedirectToAction("ResultPage", "Home", new { price = "$"+System.Math.Round(FinalOffer, 2), jsonFormat = Json });
                    }
                }

                return RedirectToAction("ResultPage", "Home", new { price = "",
                    msg = "There's no offer for you available right now! ", jsonFormat = Json });

            }
            catch (Exception e)
            {
                return RedirectToAction("ResultPage", "Home", new { price = "",
                    msg = "Kindly provide a valid data !" });
            }
        }

        public string getReponse(string token, string postal = "", string Burn = "",
             string getAllSecondaryLists = "", string DamageSeverity = "", 
             string AirbagDeployed = "", string vin = "", string Year = "", 
             string Make = "", string Model = "", string vehicleTypes = "",
             string odometerReading = "", string hasKeyss = "", string powerTrain = "",
             string transRemove = "",  string doesStart = "", 
             string driveablestatus = "", string Title = ""
              
            )
        {
            var clientss = new RestClient("https://b2b-stg.copart.com/v1/proquote");
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

            body = body.Replace("_vin", vin.TrimEnd());
            body = body.Replace("_year_", Year);
            body = body.Replace("_makeCode_", Make);
            body = body.Replace("_makeDescription_", Make);
            body = body.Replace("_model_", Model);
            body = body.Replace("_vehicleType_", vehicleTypes);

            body = body.Replace("_odometerReading_", odometerReading);
            body = body.Replace("_odometerBrand_", "Actual");


            body = body.Replace("_hasKeys_", hasKeyss);
            body = body.Replace("_powerTrain_", powerTrain);

            body = body.Replace("_acv_", 20000.ToString());
            body = body.Replace("_repairCost_", "1");
            
            
            body = body.Replace("_drivable_", doesStart);
            body = body.Replace("_drivabilityRating_", driveablestatus);
            body = body.Replace("_titleCategory_", Title);


            requestss.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse responses = clientss.Execute(requestss);
            var ResponseOffer = responses.Content;

            var offer = ResponseOffer.Split('"');
            if (offer.Count() >= 1)
            {
                ResponseOffer = offer[19];
                if (ResponseOffer == "text/javascript")
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

    }

    public class JsonFormat
    {
        public string Json { get; set; }
        public string Price { get; set; }
    }
}
