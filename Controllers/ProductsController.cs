using Microsoft.AspNetCore.Mvc;
using PracticeForRevision.Infrastructure.Interface;
using PracticeForRevision.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeForRevision.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IPaymentService _paymentService;
        private readonly IExportService _exportService;
        private readonly ILoggingService _loggingService;
        private const int PageSize = 8; // Number of products per page

        public ProductsController(IProductRepository productRepository, IPaymentService paymentService, IExportService exportService, ILoggingService loggingService)
        {
            _productRepository = productRepository;
            _paymentService = paymentService;
            _exportService = exportService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var products = await _productRepository.GetProducts();
                var totalProducts = products.Count();

                var productsDto = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    PhotoPath = p.PhotoPath
                    // Map other properties as needed
                }).ToList();

                var viewModel = new ProductsListViewModel
                {
                    Products = productsDto.Skip((page - 1) * PageSize).Take(PageSize),
                    PaginationInfo = new PaginationInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalProducts,
                        Url = "/Products/Index"
                    }
                };

                await _loggingService.LogActionAsync("Products/Index", $"Viewing products page {page}");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Index");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await _loggingService.LogActionAsync("Products/Create", "Viewing create product form");
                return View();
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Create");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductDto pdt, IFormFile photo)
        {
            try
            {
                if (photo != null)
                {
                    var filePath = Path.Combine("wwwroot/imgs", photo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    pdt.PhotoPath = photo.FileName;
                }

                var product = new Product
                {
                    Name = pdt.Name,
                    Description = pdt.Description,
                    PhotoPath = pdt.PhotoPath,
                    Price = pdt.Price
                };

                await _productRepository.CreateProduct(product);
                await _loggingService.LogActionAsync("Products/Create", $"Created product: {product.Name}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Create");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    PhotoPath = product.PhotoPath,
                    Price = product.Price
                };

                await _loggingService.LogActionAsync("Products/Edit", $"Editing product: {productDto.Name}");
                return View(productDto);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Edit");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            try
            {
                if (id != productDto.Id)
                {
                    return BadRequest();
                }

                var product = await _productRepository.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.PhotoPath = productDto.PhotoPath;
                product.Price = productDto.Price;

                await _productRepository.UpdateProduct(product);
                await _loggingService.LogActionAsync("Products/Edit", $"Product edited: {product.Name}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Edit");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpGet("ProductDetails/{id}")]
        public async Task<IActionResult> ProductDetails(int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    PhotoPath = product.PhotoPath,
                    Price = product.Price
                };

                await _loggingService.LogActionAsync("Products/ProductDetails", $"Viewing details of product: {productDto.Name}");
                return View(productDto);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/ProductDetails");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productRepository.DeleteProduct(id);
                await _loggingService.LogActionAsync("Products/DeleteConfirmed", $"Deleted product with ID: {id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/DeleteConfirmed");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(decimal amount)
        {
            try
            {
                var paymentUrl = await _paymentService.CreatePayPalPaymentAsync(amount.ToString("F2")); // Convert amount to string with 2 decimal places
                await _loggingService.LogActionAsync("Products/ProcessPayment", $"Initiated payment process for amount: {amount.ToString("F2")}");
                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/ProcessPayment");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        [HttpGet]
        public async Task<IActionResult> Success(string paymentId, string token, string payerId)
        {
            try
            {
                await _paymentService.StorePaymentDetailsAsync(paymentId, token, payerId);
                await _loggingService.LogActionAsync("Products/Success", "Successfully stored payment details");
                return View("Success"); // Assuming you have a Success.cshtml view
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Success");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }

        public async Task<IActionResult> Export()
        {
            try
            {
                var fileBytes = await _exportService.ExportDataToExcelAsync();
                var fileName = $"Export_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"; // Add timestamp to file name

                await _loggingService.LogActionAsync("Products/Export", "Exported data to Excel");
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex, "Products/Export");
                throw; // Re-throw the exception to preserve the original behavior
            }
        }
    }
}
