﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using QuanLyDienThoaiEntity.Models;


namespace QuanLyDienThoai.DAL
{
    class DetailDAL
    {
        QLYCUOCDTEntities db = new QLYCUOCDTEntities();
        DETAIL detail = new DETAIL();
        public void setDetail(int id)
        {
            this.detail.ID = id;
        }
        public void setDetail(int id, string id_sim, DateTime start, DateTime stop)
        {
            this.detail.ID = id;
            this.detail.ID_SIM = id_sim;
            this.detail.TIME_START = start;
            this.detail.TIME_STOP = stop;
        }
        public void setDetail(string id_sim, DateTime start, DateTime stop)
        {
            this.detail.ID_SIM = id_sim;
            this.detail.TIME_START = start;
            this.detail.TIME_STOP = stop;
        }
        public void setDetail(string id_sim, DateTime start, DateTime stop, int minutea7, int minutea23, int fare)
        {
            this.detail.ID_SIM = id_sim;
            this.detail.TIME_START = start;
            this.detail.TIME_STOP = stop;
            this.detail.MINUTE_AFTER7 = minutea7;
            this.detail.MINUTE_AFTER23 = minutea23;
            this.detail.FARE = fare;
        }
        public IEnumerable<DETAIL> GetAll()
        {
            List<DETAIL> detail = db.DETAILs.ToList();
            return detail;
        }
        public void Create()
        {
            var numeric_value = 1;

            while (db.DETAILs.Any(c => c.ID == numeric_value))
            {
                numeric_value++;
            }
            detail.ID = numeric_value;


            db.DETAILs.Add(detail);
            db.SaveChanges();

            db.Entry(detail).State = EntityState.Detached;
        }
        public void Delete()
        {
            var delete_detail = db.DETAILs.First(p => p.ID == detail.ID);

            db.DETAILs.Remove(delete_detail);
            db.SaveChanges();

            db.Entry(detail).State = EntityState.Detached;
        }

        public void Update()
        {
            var edited_detail = db.DETAILs.First(p => p.ID == detail.ID);

            edited_detail.ID_SIM = detail.ID_SIM;
            edited_detail.TIME_START = detail.TIME_START;
            edited_detail.TIME_STOP = detail.TIME_START;
            edited_detail.MINUTE_AFTER7 = detail.MINUTE_AFTER7;
            edited_detail.MINUTE_AFTER23 = detail.MINUTE_AFTER23;
            edited_detail.FARE = detail.FARE;

            db.SaveChanges();

            db.Entry(detail).State = EntityState.Detached;
        }

        public IEnumerable<DETAIL> SearchByName(string name)
        {
            if (db.DETAILs.Any(c => c.SIM.CUSTOMER.NAME.Contains(name)))
            {
                List<DETAIL> result = db.DETAILs.Where(c => c.SIM.CUSTOMER.NAME.Contains(name)).ToList();
                return result;
            }
            return null;
        }

        public int GetFare(string sim_id,DateTime date_export, DateTime date_cut)
        {
            var results = from detail in db.DETAILs
                          where date_export <= detail.TIME_START && date_cut >= detail.TIME_STOP && detail.ID_SIM == sim_id
                          group detail by detail.ID_SIM into g
                          select new
                          {
                              SimID = g.Key,
                              TotalFare = g.Sum(x => x.FARE)
                          };
            return Convert.ToInt32(results.Select(a => a.TotalFare).SingleOrDefault());
        }
        public int GetTotalUseMins(string sim_id, DateTime date_export, DateTime date_cut)
        {
            var results = from detail in db.DETAILs
                          where date_export <= detail.TIME_START && date_cut >= detail.TIME_STOP && detail.ID_SIM == sim_id
                          group detail by detail.ID_SIM into g
                          select new
                          {
                              SimID = g.Key,
                              TotalMin = g.Sum(x => x.MINUTE_AFTER7) + g.Sum(x => x.MINUTE_AFTER23)
                          };
            return Convert.ToInt32(results.Select(a => a.TotalMin).SingleOrDefault());
        }
    }
}
