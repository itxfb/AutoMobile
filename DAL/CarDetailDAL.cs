using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.DAL
{
    public class CarDetailDAL
    {
        public List<CarDetail> GetActiveCarDetailList(DatabaseEntities de)
        {
            try
            {
                return de.CarDetails.Where(x => x.IsActive == 1).ToList();
            }
            catch (Exception ex) 
            {
                throw;
            }
            
        }

        public List<State> GetActiveStatesList(DatabaseEntities de)
        {
            try
            {
                return de.States.Where(x => x.IsActive == 1).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        
        public List<City> GetActiveCitiesListByStateId(int id,DatabaseEntities de)
        {
            try
            {
                return de.Cities.Where(x => x.IsActive == 1 &&x.StateId==id).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public CarDetail GetActiveCarDetailById(int id, DatabaseEntities de)
        {
            return de.CarDetails.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);
        } 
        public CarDetail GetActiveCarDetailByUserDetailId(int userDetailId, DatabaseEntities de)
        {
            return de.CarDetails.Where(x => x.UserCarDetailId == userDetailId).FirstOrDefault(x => x.IsActive == 1);
        } 
        public CarDetail GetActiveCarDetailLastRecord(DatabaseEntities de)
        {
            return de.CarDetails.Where(x =>x.IsActive==1).OrderByDescending(x=>x.Id).FirstOrDefault();
        }
        public State GetActiveStateById(int id, DatabaseEntities de)
        {
            return de.States.Where(x => x.Id == id).FirstOrDefault();
        }

        public State GetActiveStateByStateName(String Sname, DatabaseEntities de)
        {
            return de.States.Where(x => x.StateName.ToLower().Contains(Sname.ToLower())).FirstOrDefault();
        }
        public City GetActiveCityById(int id, DatabaseEntities de)
        {
            return de.Cities.Where(x => x.Id == id).FirstOrDefault();
        }
        public City GetActiveCityByName(string CityName, DatabaseEntities de)
        {
            return de.Cities.Where(x => x.CityName.ToLower().Contains(CityName.ToLower())).FirstOrDefault();
        }

        public bool AddCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            try
            {
                de.CarDetails.Add(carDetail);
                de.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            try
            {
                de.Entry(carDetail).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCarOfferDetail(UserCarDetail carDetail, DatabaseEntities de)
        {
            try
            {
                de.Entry(carDetail).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        

        //public bool DeleteCarDetail(int id, DatabaseEntities de)
        //{
        //    try
        //    {
        //        de.CarDetails.Remove(de.CarDetails.Where(x => x.Id == id).FirstOrDefault());
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