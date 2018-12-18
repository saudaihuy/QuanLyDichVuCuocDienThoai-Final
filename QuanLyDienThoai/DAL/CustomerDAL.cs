﻿using QuanLyDienThoai.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyDienThoaiEntity.Models;


namespace QuanLyDienThoai.DAL
{
    public class CustomerDAL
    {
        QLYCUOCDTEntities db = new QLYCUOCDTEntities();
        CUSTOMER customer = new CUSTOMER();
        public void setCustomer(string id)
        {
            this.customer.ID_CUSTOMER = id;
        }
        public void setCustomer(string name, int identity, string job, string position, string address)
        {
            this.customer.NAME = name;
            this.customer.IDENTIFY = identity;
            this.customer.JOB = job;
            this.customer.POSITION = position;
            this.customer.ADDRESS = address;
        }
        public void setCustomer(string id,string name, int identity, string job, string position, string address)
        {
            this.customer.ID_CUSTOMER = id;
            this.customer.NAME = name;
            this.customer.IDENTIFY = identity;
            this.customer.JOB = job;
            this.customer.POSITION = position;
            this.customer.ADDRESS = address;
        }
        public IEnumerable<CUSTOMER> GetAll()
        {
            List<CUSTOMER> customer = db.CUSTOMERs.ToList();
            return customer;
        }
        public void Create()
        {
            var numeric_value = 1;
            var id_str = "KH0";

            while (db.CUSTOMERs.Any(c => c.ID_CUSTOMER == id_str + numeric_value.ToString()))
            {
                numeric_value++;
                if (numeric_value > 9)
                    id_str = "KH";
            }            
            customer.ID_CUSTOMER = id_str+numeric_value.ToString();
            

            db.CUSTOMERs.Add(customer);
            db.SaveChanges();
        }
        
        public void Delete()
        {
            var delete_customer = db.CUSTOMERs.First(p => p.ID_CUSTOMER == customer.ID_CUSTOMER);
            IList<CONTRACT> delete_contracts = db.CONTRACTs.Where(p => p.SIM.ID_CUSTOMER == customer.ID_CUSTOMER).ToList();
            IList<SIM> delete_sims = db.SIMs.Where(p => p.ID_CUSTOMER == customer.ID_CUSTOMER).ToList();
            IList<BILL> delete_bills = db.BILLs.Where(p => p.SIM.ID_CUSTOMER == customer.ID_CUSTOMER).ToList();
            IList<DETAIL> delete_details = db.DETAILs.Where(p => p.SIM.ID_CUSTOMER == customer.ID_CUSTOMER).ToList();
            IList<ACCOUNT> delete_accounts = db.ACCOUNTs.Where(p => p.ID_CUSTOMER == customer.ID_CUSTOMER).ToList();

            db.ACCOUNTs.RemoveRange(delete_accounts);
            db.DETAILs.RemoveRange(delete_details);
            db.BILLs.RemoveRange(delete_bills);
            db.CONTRACTs.RemoveRange(delete_contracts);
            db.SIMs.RemoveRange(delete_sims);
            db.CUSTOMERs.Remove(delete_customer);
            db.SaveChanges();
            db.Entry(customer).State = EntityState.Detached;
        }
        

        public void Update()
        {
            var edited_customer = db.CUSTOMERs.Where(p => p.ID_CUSTOMER == customer.ID_CUSTOMER).Select(p => p).FirstOrDefault();            
            edited_customer.NAME = customer.NAME;
            edited_customer.IDENTIFY = customer.IDENTIFY;
            edited_customer.JOB = customer.JOB;
            edited_customer.POSITION = customer.POSITION;
            edited_customer.ADDRESS = customer.ADDRESS;            
            db.SaveChangesAsync();

            db.Entry(customer).State = EntityState.Detached;
        }
        public bool checkExistCustomer()
        {
            return db.CUSTOMERs.Any(c =>c.ID_CUSTOMER!= customer.ID_CUSTOMER &&( c.IDENTIFY == customer.IDENTIFY || c.NAME == customer.NAME && c.ADDRESS == customer.ADDRESS && c.IDENTIFY == customer.IDENTIFY));
        }
        public IEnumerable<CUSTOMER> SearchByName(string name)
        {
            if (db.CUSTOMERs.Any(c => c.NAME.Contains(name)))
            {
                List<CUSTOMER> result = db.CUSTOMERs.Where(c => c.NAME.Contains(name)).ToList();
                return result;
            }
            return null;
        }

        public List<CUSTOMER> SearchById_Customer(string id_customer)
        {
            if (db.CUSTOMERs.Any(c => c.ID_CUSTOMER.Contains(id_customer)))
            {
                List<CUSTOMER> result = db.CUSTOMERs.Where(c => c.ID_CUSTOMER.Contains(id_customer)).ToList();
                return result;
            }
            return null;
        }

        public string getName_in_Customer(string id_customer)
        {
            return (from h in db.CUSTOMERs where h.ID_CUSTOMER.Equals(id_customer) select h.NAME).FirstOrDefault();
        }

        public void AdvSearch()
        {

        }
    }
}
