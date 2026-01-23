using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface IProductService
{
    Task<ResponseDto?> GetAllProductsAsync();
    Task<ResponseDto?> GetProductByIdAsync(int id);
    Task<ResponseDto?> CreateProductAsync(ProductDTO productDTO);
    Task<ResponseDto?> UpdateProductAsync(ProductDTO productDTO);
    Task<ResponseDto?> DeleteProductAsync(int id);
}
