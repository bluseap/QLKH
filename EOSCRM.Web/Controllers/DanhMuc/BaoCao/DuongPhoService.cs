using Microsoft.Extensions.Configuration;
using POWACO.Dapper.DanhMuc.BaoCao.DuongPho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOSCRM.Web.Controllers.DanhMuc.BaoCao
{
    public class DuongPhoService
    {        
        IDuongPhoRepository _iduongphoRepository;        

        public DuongPhoService()
        {          
            _iduongphoRepository = new DuongPhoRepository();
        }

        public List<DuongPhoVm> GetAll()
        {
            return _iduongphoRepository.GetAll();
        }

    }
}