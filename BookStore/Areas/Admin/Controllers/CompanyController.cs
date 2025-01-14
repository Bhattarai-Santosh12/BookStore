using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models.Models;
using BookStore.Models.ViewModel;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> company = _unitOfWork.Company.GetAll().ToList();

            return View(company);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                var companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                if (companyObj == null)
                {
                    TempData["error"] = "Company not found.";
                    return RedirectToAction("Index");
                }
                return View(companyObj);
            }
        }


        [HttpPost]
        public IActionResult Upsert(Company companyobj)
        {
            if (ModelState.IsValid)
            {
                if (companyobj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyobj);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(companyobj);
                    TempData["success"] = "Product Updated successfully";
                }
                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyobj);
            }

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> company = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = company });
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var companyToDelete = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {
                _unitOfWork.Company.Remove(companyToDelete);
                _unitOfWork.save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }
    }
}