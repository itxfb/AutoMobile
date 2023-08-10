using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.DAL
{
    public class UserDAL
    {
        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            var test= de.Users.Where(x => x.IsActive == 1).ToList();
            return de.Users.Where(x => x.IsActive == 1).ToList();
        }
        
        public User GetActiveUserById(int id, DatabaseEntities de)
        {
            try
            {
                return de.Users.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<UserCarDetail> GetActiveUserCarDetailsList(DatabaseEntities de)
        {
            return de.UserCarDetails.Where(x => x.IsActive == 1).ToList();
        }

        //public List<UserCarDetail> GetActiveUserRejectedCarDetailsList(DatabaseEntities de)
        //{
        //    return de.UserCarDetails.Where(x => x.IsActive == 1 && x.IsRejected == 1).ToList();
        //}


        public List<CarImage> GetCarImagesList(DatabaseEntities de)
        {
            return de.CarImages.Where(x => x.IsActive == 1).ToList();
        }
        public UserCarDetail GetActiveUserDetail(DatabaseEntities de)
        {
            try
            {
                //var test= de.UserCarDetails.Where(x => x.IsActive == 1 && x.IsRejected == 0).OrderByDescending(x=>x.Id).FirstOrDefault();
                return de.UserCarDetails.Where(x => x.IsActive==1).OrderByDescending(a=>a.Id).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public UserCarDetail GetActiveOfferDetailById(int id,DatabaseEntities de)
        {
            try
            {
                return de.UserCarDetails.Where(x => x.IsActive == 1 &&x.Id==id).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            try
            {
                var getUserId= de.Users.Add(user);
                var getId=de.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int AddNewUser(User user, DatabaseEntities de)
        {
            try
            {
                var getUserId = de.Users.Add(user);
                var getId = de.SaveChanges();

                return getUserId.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool AddUserDetail(UserCarDetail userDetail, DatabaseEntities de)
        {
            try
            {
                de.UserCarDetails.Add(userDetail);
                de.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddCarDetailImages(CarImage userDetail, DatabaseEntities de)
        {
            try
            {
                de.CarImages.Add(userDetail);

                
                de.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Entry(user).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public bool DeleteUser(int id, DatabaseEntities de)
        //{
        //    try
        //    {
        //        de.Users.Remove(de.Users.Where(x => x.Id == id).FirstOrDefault());
        //        de.SaveChanges();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}