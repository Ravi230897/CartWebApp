﻿using Cart.DataAccess.Data;
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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
             return View(objCompanyList);
        }

        // for creating action method 
        // by default get method
        public IActionResult Upsert(int? id)
        {
           
           

            if (id == null || id == 0)
            {
                //Create
                return View(new Company());
            }
            else
            {
                //update 
                Company companyObj = _unitOfWork.Company.Get(u=>u.Id==id);
                return View(companyObj);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)

        {
            if (ModelState.IsValid)
            {
     
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }


                _unitOfWork.Save();
                TempData["Success"] = "Company Create  Successfully";
                return RedirectToAction("Index");
            }
            return View(CompanyObj);
        }

      

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.GetAll().FirstOrDefault(f => f.Id.Equals(id.Value));
            //_unitOfWork.Company.Get(u =>u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save(); 

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
