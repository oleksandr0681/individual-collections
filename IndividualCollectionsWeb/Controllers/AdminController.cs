using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using IndividualCollectionsWeb.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IndividualCollectionsWeb.Controllers
{
    [Authorize(Roles = "administrator")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> applicationUsers = UserManager.Users;
            List<EditUserViewModel> editUsers = new List<EditUserViewModel>();
            ApplicationUser adminUser = UserManager.FindById(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                foreach (var item in applicationUsers)
                {
                    EditUserViewModel editUser =
                        new EditUserViewModel
                        {
                            Id = item.Id,
                            Email = item.Email,
                            Blocked = item.Blocked
                        };
                    editUsers.Add(editUser);
                }
                for (int i = 0; i < editUsers.Count; i++)
                {
                    editUsers[i].IsAdministrator = UserManager.IsInRole(editUsers[i].Id, "administrator");
                }
                return View(editUsers);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    IQueryable<Collection> collections = db.Collections.Where(c => c.ApplicationUserId == user.Id);
                    for (int i = 0; i < collections.ToList().Count; i++)
                    {
                        db.Collections.Remove(collections.ToList()[i]);
                    }
                    db.SaveChanges();
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Error", new HandleErrorInfo(new Exception(), "Admin", "Delete"));
                    }
                }
                else
                {
                    return View("Error", new HandleErrorInfo(new Exception(), "Admin", "Delete"));
                }
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Block(string id)
        {
            ApplicationUser adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Blocked = true;
                    IdentityResult validateResult = await UserManager.UserValidator.ValidateAsync(user);
                    if (validateResult.Succeeded)
                    {
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }
                else
                {
                    return View("Error", new HandleErrorInfo(new Exception(), "Admin", "Block"));
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Unblock(string id)
        {
            ApplicationUser adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Blocked = false;
                    IdentityResult validateResult = await UserManager.UserValidator.ValidateAsync(user);
                    if (validateResult.Succeeded)
                    {
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }
                else
                {
                    return View("Error", new HandleErrorInfo(new Exception(), "Admin", "Unblock"));
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        public async Task<ActionResult> EditUser(string id)
        {
            ApplicationUser adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                EditUserViewModel editUser = new EditUserViewModel();
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    editUser.Id = user.Id;
                    editUser.Email = user.Email;
                    editUser.Blocked = user.Blocked;
                    editUser.IsAdministrator = UserManager.IsInRole(id, "administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
                return View(editUser);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(EditUserViewModel editUser)
        {
            ApplicationUser adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                if (editUser != null)
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(editUser.Id);
                    if (user != null)
                    {
                        user.Blocked = editUser.Blocked;
                        if (editUser.IsAdministrator)
                        {
                            UserManager.AddToRole(user.Id, "administrator");
                        }
                        else
                        {
                            UserManager.RemoveFromRole(user.Id, "administrator");
                        }
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь не найден");
                    }
                }
                return View(editUser);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        public ActionResult UserIndex(string id)
        {
            ApplicationUser adminUser = UserManager.FindById(User.Identity.GetUserId());
            if (adminUser != null && adminUser.DefaultPasswordChanged)
            {
                IEnumerable<Collection> collections = null;
                ApplicationUser user = UserManager.FindById(id);
                if (user != null)
                {
                    collections = db.Collections.Include(c => c.Theme).Where(c => c.ApplicationUserId == user.Id);
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
                return View(collections.ToList());
            }
            else
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private void AddErrors(IdentityResult resultErrors)
        {
            foreach (var item in resultErrors.Errors)
            {
                ModelState.AddModelError("", item);
            }
        }
    }
}