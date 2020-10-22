using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POWACO.Dapper.DanhMuc.BaoCao.DuongPho
{
    public interface IDuongPhoRepository
    {
        List<DuongPhoVm> GetAll();
        //Customer FindById(int Id);
        //bool AddCustomer(Customer customer);
        //bool UpdateCustomer(Customer customer);
        //bool DeleteCustomer(int Id);
    }
}
