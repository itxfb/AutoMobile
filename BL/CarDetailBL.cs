using CarSystem.DAL;
using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.BL
{
    public class CarDetailBL
    {
        public List<CarDetail> GetActiveCarDetailList(DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCarDetailList(de);
        }

        public List<State> GetActiveStatesList(DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveStatesList(de);
        }

        public List<City> GetActiveCitiesListByStateId(int id,DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCitiesListByStateId(id,de);
        }

        public State GetActiveStateById(int id, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveStateById(id, de);
        }
        public State GetActiveStateByStateName(string SName, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveStateByStateName(SName, de);
        }

        public City GetActiveCityById(int id, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCityById(id, de);
        }
        public City GetActiveCityByName(string CityName, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCityByName(CityName, de);
        }


        public CarDetail GetActiveCarDetailById(int id, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCarDetailById(id, de);
        }

        public CarDetail GetActiveCarDetailByUserDetailId(int UserDetailId, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCarDetailByUserDetailId(UserDetailId, de);
        }

        public CarDetail GetActiveCarDetailLastRecord(DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCarDetailLastRecord(de);
        }

        public bool AddCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            if (carDetail==null)
            {
                return false;
            }
            else
            {
                return new CarDetailDAL().AddCarDetail(carDetail, de);
            }
        }
        //public bool UpdateCarDetail(CarDetail carDetail, DatabaseEntities de)
        //{
        //    if (String.IsNullOrEmpty(carDetail.Year) || String.IsNullOrEmpty(carDetail.DamageType) || String.IsNullOrEmpty(carDetail.DamageTypeDescription)
        //        || string.IsNullOrEmpty(carDetail.Model) || string.IsNullOrEmpty(carDetail.LotRunCondition) || string.IsNullOrEmpty(carDetail.SalePrice)
        //        || string.IsNullOrEmpty(carDetail.OdometerReading))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return new CarDetailDAL().UpdateCarDetail(carDetail, de);
        //    }
        //}

        public bool UpdateCarOfferDetail(UserCarDetail carOfferDetail, DatabaseEntities de)
        {
            //if (carDetail.Id!=null)
            //{
            //    return false;
            //}
            //else
            //{
                return new CarDetailDAL().UpdateCarOfferDetail(carOfferDetail, de);
            //}
        }
        public bool UpdateCarDetails(CarDetail carDetail, DatabaseEntities de)
        {
            //if (carDetail.Id!=null)
            //{
            //    return false;
            //}
            //else
            //{
            return new CarDetailDAL().UpdateCarDetail(carDetail, de);
            //}
        }

        //public bool DeleteCarDetail(int id, DatabaseEntities de)
        //{
        //    return new CarDetailDAL().DeleteCarDetail(id, de);
        //}
    }
}