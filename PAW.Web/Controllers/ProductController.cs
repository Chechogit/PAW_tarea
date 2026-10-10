using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;
using System.Diagnostics;

namespace PAW.Web.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            IExchangeRateService exchangeRateService,
            ILogger<ProductController> logger)
            : base(exchangeRateService)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET: /Product
        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductViewModel
            {
                Products = await _productService.GetProductsAsync(),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Product/Create  -> muestra el formulario vacío
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductDTO());
        }

        // POST: /Product/Create  -> recibe el formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDTO product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                var created = await _productService.CreateProductAsync(product);
                TempData["Success"] = $"Product \"{created.Name}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(product);
            }
        }

        // GET: /Product/Edit/5  -> muestra el formulario con los datos actuales
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product is null)
                return NotFound();

            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDTO product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                var created = await _productService.CreateProductAsync(product);
                TempData["Success"] = $"Product \"{created.Name}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product is null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductDTO product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                await _productService.UpdateProductAsync(product);
                TempData["Success"] = $"Product \"{product.Name}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating product {Id}", product.ProductId);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(product);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
