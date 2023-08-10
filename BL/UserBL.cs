using CarSystem.DAL;
using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CarSystem.BL
{
    public class UserBL
    {
        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUsersList(de);
        }
        public List<UserCarDetail> GetActiveUserCarDetailsList(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserCarDetailsList(de);
        }
        //public List<UserCarDetail> GetActiveUserRejectedCarDetailsList(DatabaseEntities de)
        //{
        //    return new UserDAL().GetActiveUserRejectedCarDetailsList(de);
        //}
        public List<CarImage> GetCarImagesList(DatabaseEntities de)
        {
            return new UserDAL().GetCarImagesList(de);
        }


        public User GetActiveUserById(int id, DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserById(id, de);
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUser(user, de);
            }
        }
        public int AddNewUser(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                return 0;
            }
            else
            {
                return new UserDAL().AddNewUser(user, de);
            }
        }

        public bool UpdateUser(User user, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            else
            {
                return new UserDAL().UpdateUser(user, de);
            }
        }

        public bool AddUserDetail(UserCarDetail userdetail, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(userdetail.FirstName) || String.IsNullOrEmpty(userdetail.LastName) || String.IsNullOrEmpty(userdetail.City))
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUserDetail(userdetail, de);
            }
        }

        public bool AddCarDetailImages(CarImage userdetail, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(userdetail.ImagePath))
            {
                return false;
            }
            else
            {
                return new UserDAL().AddCarDetailImages(userdetail, de);
            }
        }
        public UserCarDetail GetActiveOfferDetailById(int id,DatabaseEntities de)
        {
            return new UserDAL().GetActiveOfferDetailById(id,de);
        }

        public UserCarDetail GetActiveLastRecordUserDetail(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUserDetail(de);
        }
        //public bool DeleteUser(int id, DatabaseEntities de)
        //{
        //    return new UserDAL().DeleteUser(id, de);
        //}
    }
}