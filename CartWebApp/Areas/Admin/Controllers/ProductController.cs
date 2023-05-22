using Cart.DataAccess.Data;
using Cart.DataAccess.Repository.IRepository;
using Cart.Models;
using Cart.Models.ViewModels;
using Cart.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace CartWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
             return View(objProductList);
        }

        // for creating action method 
        // by default get method
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = 

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                product = new Product()
        };
            if (id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //update 
                productVM.product = _unitOfWork.Product.Get(u=>u.Id==id);
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)

        {
            if (ModelState.IsValid)
            {
                string wwwRootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootpath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.product.ImageUrl))
                    {
                        var oldImagePath =
                            Path.Combine(wwwRootpath, productVM.product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }


                    using (var fileStream = new FileStream(Path.Combine(productPath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.product.ImageUrl = @"\images/product\" + FileName;
                }
                if (productVM.product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.product);
                }


                _unitOfWork.Save();
                TempData["Success"] = "Product Create  Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        //[HttpPost]
        //public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if(file!=null)
        //        {
        //            string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string productPath = Path.Combine(wwwRootPath, @"images\product");

        //            if (!string.IsNullOrEmpty(productVM.product.ImageUrl))
        //            {
        //                //delete the old image
        //                var oldImagePath = 
        //                    Path.Combine(wwwRootPath, productVM.product.ImageUrl.TrimStart('\\'));

        //                if (System.IO.File.Exists(oldImagePath)) 
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }

        //            using (var fileStream = new FileStream(Path.Combine(productPath, FileName),FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }

        //            productVM.product.ImageUrl = @"\images/product\" + FileName;

        //        }

        //        if (productVM.product.Id == 0)
        //        {
        //            _unitOfWork.Product.Add(productVM.product);
        //        }
        //        else
        //        {
        //            _unitOfWork.Product.Update(productVM.product);
        //        }


        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Create  Successfully";
        //        return RedirectToAction("Index");
        //    }

        //    else
        //    {
        //        //productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        //{
        //        //    Text = u.Name,
        //        //    Value = u.Id.ToString()
        //        //});

        //        return View(productVM);
        //    }

        //}

        // for editing action method 
        // by default get method
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    //Product? productFromDb1 = _db.Catego.FirstOrDefault(u=>u.Id==id);
        //    //Product? productFromDb2 = _db.Catego.Where(u => u.Id == id).FirstOrDefault();

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product Updated  Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        // for deleting action method 
        // by default get method
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Product Deleted  Successfully";
        //    return RedirectToAction("Index");


        //}

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.GetAll().FirstOrDefault(f => f.Id.Equals(id.Value));
            //_unitOfWork.Product.Get(u =>u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            var oldImagePath =
                            Path.Combine(_webHostEnvironment.WebRootPath, 
                            productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save(); 

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
