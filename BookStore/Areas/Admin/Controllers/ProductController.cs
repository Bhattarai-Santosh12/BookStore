using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using BookStore.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products= _unitOfWork.Product.GetAll().ToList();
          
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            // IEnumerable<SelectListItem> CategoryList = 
            //ViewBag.CategoryList = CategoryList;

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
           
        }

        //[HttpPost]
        //public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if(file != null)
        //        {
        //            string fileName= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string productPath=Path.Combine(wwwRootPath, @"Images\Product");
        //            if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
        //            {
        //                var oldImagePath= 
        //                    Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
        //                if (System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }
        //            using (var fileStream =new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //            productVM.Product.ImageUrl = @"/Images/Product/" + fileName;
        //        }
        //        if(productVM.Product.Id == 0)
        //        {
        //            _unitOfWork.Product.Add(productVM.Product);
        //            TempData["success"] = "Product created successfully";
        //        }
        //        else
        //        {
        //            _unitOfWork.Product.Update(productVM.Product);
        //            TempData["success"] = "Product Updated successfully";
        //        } 

        //            _unitOfWork.Product.Update(productVM.Product);

        //        _unitOfWork.save();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        // Re-populate CategoryList in case of validation failure
        //        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });
        //        return View(productVM);
        //    }
        //}
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    // Ensure the directory exists
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"/Images/Product/" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }

                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            else
            {
                // Re-populate CategoryList in case of validation failure
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }




        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Product obj)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == obj.Id);
            if(product == null)
            {
                return NotFound();
            }
           
                _unitOfWork.Product.Remove(product);
                _unitOfWork.save();
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index");
          
        }

    }
}
