﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyDienThoaiEntity.Models;


namespace QuanLyDienThoai.DAL
{
    public class ContractDAL
    {
        QLYCUOCDTEntities db = new QLYCUOCDTEntities();
        CONTRACT contract = new CONTRACT();
        public void setCONTRACT(String sim_id,DateTime date,int? fee, bool status)
        {
            this.contract.ID_SIM = sim_id;
            this.contract.FEE = fee;
            this.contract.DATEREGISTER = date;
            this.contract.STATUS = status;
        }
        public void setCONTRACT(string id)
        {
            this.contract.ID_CONTRACT = id;
        }

        public void setCONTRACT_bySimID(string sim_id)
        {
            this.contract.ID_SIM = sim_id;
        }

        public void setCONTRACT(string id, string sim_id,DateTime date, int? fee, bool status)
        {
            this.contract.ID_CONTRACT = id;
            this.contract.ID_SIM = sim_id;
            this.contract.FEE = fee;
            this.contract.DATEREGISTER = date;
            this.contract.STATUS = status;
        }
        public IEnumerable<CONTRACT> GetAll()
        {
            List<CONTRACT> contract = db.CONTRACTs.ToList();
            return contract;
        }
        public void Create()
        {
            var numeric_value = 1;
            var id_str = "C0";

            while (db.CONTRACTs.Any(c => c.ID_CONTRACT == id_str + numeric_value.ToString()))
            {
                numeric_value++;
                if (numeric_value > 9)
                    id_str = "C";
            }
            contract.ID_CONTRACT = id_str + numeric_value.ToString();
            db.CONTRACTs.Add(contract);
            db.SaveChanges();
            db.Entry(contract).State = EntityState.Detached;
        }

        public void Delete()
        {
            var delete_contract = db.CONTRACTs.First(p => p.ID_CONTRACT == contract.ID_CONTRACT);
            var sim_id = delete_contract.ID_SIM;
            var customer_id = delete_contract.SIM.ID_CUSTOMER;
            var contract_count = db.CONTRACTs.Count(p => p.SIM.ID_CUSTOMER == customer_id);
            if (contract_count < 2)
            {
                IList<SIM> delete_sims = db.SIMs.Where(p => p.ID_CUSTOMER == customer_id).ToList();
                IList<CUSTOMER> delete_customers = db.CUSTOMERs.Where(p => p.ID_CUSTOMER == customer_id).ToList();
                IList<BILL> delete_bills = db.BILLs.Where(p => p.ID_SIM == sim_id).ToList();
                IList<DETAIL> delete_details = db.DETAILs.Where(p => p.ID_SIM == sim_id).ToList();
                IList<ACCOUNT> delete_accounts = db.ACCOUNTs.Where(p => p.ID_CUSTOMER == customer_id).ToList();

                db.ACCOUNTs.RemoveRange(delete_accounts);
                db.DETAILs.RemoveRange(delete_details);
                db.BILLs.RemoveRange(delete_bills);
                db.SIMs.RemoveRange(delete_sims);
                db.CUSTOMERs.RemoveRange(delete_customers);
            }
            
            db.CONTRACTs.Remove(delete_contract);
            db.SaveChanges();
            db.Entry(contract).State = EntityState.Detached;
        }
               
        public void Update()
        {
            var edited_contract = db.CONTRACTs.First(p => p.ID_CONTRACT == contract.ID_CONTRACT);
            edited_contract.ID_SIM = contract.ID_SIM;
            edited_contract.FEE = contract.FEE;
            edited_contract.DATEREGISTER = contract.DATEREGISTER;
            edited_contract.STATUS = contract.STATUS;
            db.SaveChanges();
            db.Entry(contract).State = EntityState.Detached;
        }
        public List<CONTRACT> SearchBy_CustomerName(string name)
        {
            if (db.CONTRACTs.Any(c => c.SIM.CUSTOMER.NAME.Contains(name)))
            {
                List<CONTRACT> result = db.CONTRACTs.Where(c => c.SIM.CUSTOMER.NAME.Contains(name)).ToList();
                return result;
            }
            return null;
        }
        public void cancelContract_bySimID()
        {
            var cancel_contract = db.CONTRACTs.First(p => p.ID_SIM == contract.ID_SIM);

            cancel_contract.STATUS = false;
            db.SaveChanges();

            db.Entry(cancel_contract).State = EntityState.Detached;
        }

    }
}
