using BeSpokedBikesAPI.Interfaces;
using BeSpokedBikesAPI.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BeSpokedBikesAPI.DataAccess
{
    public class BeSpokeBikesRepo : IBeSpokeBikesRepo
    {
        private readonly BeSpokedBikesSQLContainer _context;

        public BeSpokeBikesRepo(BeSpokedBikesSQLContainer context)
        {
            _context = context;
        }

        #region GetMethods
        public async Task<List<Customers>> GetAllCustomers()
        {
            return await _context.Customer.ToListAsync();
        }

        public async Task<List<Products>> GetAllProducts()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<List<GetSalesResponse>> GetAllSales()
        {
            return await _context.Sale
                .Include(s => s.Product)
                .Include(s => s.Salesperson)
                .Include(s => s.Customer)
                .Select(s => new GetSalesResponse
                {
                    SaleId = s.SaleId,
                    ProductName = s.Product.Name,
                    CustomerName = s.Customer.FirstName + " " + s.Customer.LastName,
                    SaleDate = s.SaleDate,
                    SalePrice = s.Product.SalePrice,
                    DiscountedSalePrice = s.DiscountedSalePrice,
                    SalesPerson = s.Salesperson.FirstName + " " + s.Salesperson.LastName,
                    SalesPersonCommission = s.Product.SalePrice * (s.Product.CommissionPercentage / 100)
                }).ToListAsync();
        }

        public async Task<List<SalesPersons>> GetAllSalesPersons()
        {
            return await _context.SalesPerson.ToListAsync();
        }

        #endregion

        #region UpdateMethods
        public async Task<UpdateEntityResponse> UpdateProduct(UpdateProductRequest product)
        {
            UpdateEntityResponse response = new UpdateEntityResponse();

            var productExists = _context.Product.Any(p => p.Name == product.Name && p.ProductId != product.ProductId);

            if (productExists)
            {
                response.Message = "Product Name already exists";
            }
            else
            {
                var productToUpdate = await _context.Product.FindAsync(product.ProductId);

                if (productToUpdate != null)
                {
                    productToUpdate.Name = product.Name;
                    productToUpdate.Manufacturer = product.Manufacturer;
                    productToUpdate.Style = product.Style;
                    productToUpdate.PurchasePrice = product.PurchasePrice;
                    productToUpdate.SalePrice = product.SalePrice;
                    productToUpdate.QuantityOnHand = product.QuantityOnHand;
                    productToUpdate.CommissionPercentage = product.CommissionPercentage;

                    await _context.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.Message = $"Product {product.ProductId} updated successfully";
                }
                else
                {
                    response.Message = "Product Name doesn't exist";
                }

            }

            return response;
        }

        public async Task<UpdateEntityResponse> UpdateSalesPerson(UpdateSalesPersonRequest salesPerson)
        {
            UpdateEntityResponse response = new UpdateEntityResponse();

            var salesPersonExists = _context.SalesPerson.Any(p => p.FirstName == salesPerson.FirstName && p.LastName == salesPerson.LastName && p.SalesPersonId !=salesPerson.SalesPersonId);

            if (salesPersonExists)
            {
                response.Message = "Sales person Name already exists";
            }
            else
            {

                var dataToUpdate = await _context.SalesPerson.FindAsync(salesPerson.SalesPersonId);

                if (dataToUpdate != null)
                {
                    dataToUpdate.FirstName = salesPerson.FirstName;
                    dataToUpdate.LastName = salesPerson.LastName;
                    dataToUpdate.Address = salesPerson.Address;
                    dataToUpdate.Phone = salesPerson.Phone;
                    dataToUpdate.StartDate = salesPerson.StartDate;
                    if (salesPerson.TerminationDate != null)
                        dataToUpdate.TerminationDate = salesPerson.TerminationDate;
                    dataToUpdate.Manager = salesPerson.Manager;

                    await _context.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.Message = $"SalesPerson details {salesPerson.SalesPersonId} updated successfully";
                }
                else
                {
                    response.Message = "SalesPerson doesn't exist";
                }
            }
            return response;
        }
        #endregion

        #region AddNewMethods
        public async Task<UpdateEntityResponse> AddNewSale(AddNewSaleRequest request)
        {
            UpdateEntityResponse response = new UpdateEntityResponse();
            Sales newSale = new Sales();

            // Fetch the product details
            var product = await _context.Product.FindAsync(request.ProductId);
            var customer = await _context.Customer.FindAsync(request.CustomerId);
            var salesPerson = await _context.SalesPerson.FindAsync(request.SalespersonId);

            if (product == null || salesPerson == null || customer == null)
            {
               response.Message = "Product/customer/SalesPerson one of these does not exist.";
            }
            // Validate SaleDate against SalesPerson.TerminationDate
            else if (salesPerson.TerminationDate.HasValue && newSale.SaleDate > salesPerson.TerminationDate.Value)
            {
                response.Message = "Sale date exceeds the salesperson's termination date.";
            }
            else
            {
                // Check for applicable discount
                var discount = await _context.Discount
                    .Where(d => d.ProductId == request.ProductId &&
                                request.SaleDate >= d.BeginDate &&
                                request.SaleDate <= d.EndDate)
                    .FirstOrDefaultAsync();

                if (discount != null)
                {
                    // Calculate the discounted sale price
                    newSale.DiscountedSalePrice = product.SalePrice - (product.SalePrice * (discount.DiscountPercentage / 100));
                }
                else
                {
                    // No discount applies; use the regular sale price
                    newSale.DiscountedSalePrice = product.SalePrice;
                }

                // Add the sale to the database
                newSale.SaleDate = request.SaleDate;
                newSale.SalespersonId = request.SalespersonId;
                newSale.CustomerId = request.CustomerId;
                newSale.ProductId = request.ProductId;

                _context.Sale.Add(newSale);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.Message = "Sale added successfully";
            }
            return response;
        }

        public async Task<List<GetQuarterlyBonusResponse>> GetQuarterlyBonus(int year, int quarter)
        {
            //Determine the start and end dates of the specified quarter
            var startDate = new DateTime(year, (quarter - 1) * 3 + 1, 1);
            var endDate = startDate.AddMonths(3).AddDays(-1);


            //get sales of all SalesPersons and also double check termination date in case of bad data
            var sales = await _context.Sale
                .Include(s => s.Product) //to get commissionPercentage
                .Include(s => s.Salesperson)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .Where(s => s.Salesperson.TerminationDate == null || s.SaleDate <= s.Salesperson.TerminationDate) // Exclude sales after termination
                .ToListAsync();

            var bonusResponse = sales
                .GroupBy(s => new { s.Salesperson.FirstName, s.Salesperson.LastName })
                .Select(group => new GetQuarterlyBonusResponse
                {
                    SalesPersonName = $"{group.Key.FirstName} {group.Key.LastName}",
                    CommissionEarned = Math.Round(group.Sum(s => (decimal)(s.DiscountedSalePrice * (s.Product.CommissionPercentage / 100))),2)
                })
                .ToList();

            return bonusResponse;
        }
        #endregion

    }
}
