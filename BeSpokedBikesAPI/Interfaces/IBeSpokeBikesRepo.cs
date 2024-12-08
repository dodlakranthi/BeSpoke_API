using BeSpokedBikesAPI.Models;

namespace BeSpokedBikesAPI.Interfaces
{
    public interface IBeSpokeBikesRepo
    {
        Task<List<Products>> GetAllProducts();
        Task<UpdateEntityResponse> UpdateProduct(UpdateProductRequest product);
        Task<List<SalesPersons>> GetAllSalesPersons();
        Task<UpdateEntityResponse> UpdateSalesPerson(UpdateSalesPersonRequest product);
        Task<List<Customers>> GetAllCustomers();
        Task<List<GetSalesResponse>> GetAllSales();
        Task<UpdateEntityResponse> AddNewSale(AddNewSaleRequest request);
        Task<List<GetQuarterlyBonusResponse>> GetQuarterlyBonus(int year, int quarter);

    }
}
