using Autofac;
using FirstDemo.Application;
using FirstDemo.Domain.Exceptions;
using FirstDemo.Infrastructure;
using FirstDemo.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstDemo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ILifetimeScope scope,
            ILogger<CourseController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        [Authorize(Policy = "CourseViewRequirementPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = _scope.Resolve<CourseCreateModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.CreateCourseAsync();

					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Course created successfuly",
						Type = ResponseTypes.Success
					});

					return RedirectToAction("Index");
                }
				catch (DuplicateTitleException de)
				{
					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = de.Message,
						Type = ResponseTypes.Danger
					});
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Server Error");

					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "There was a problem in creating course",
						Type = ResponseTypes.Danger
					});
				}
			}

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetCourses(CourseListModel model)
        {
            var dataTablesModel = new DataTablesAjaxRequestUtilityCopy(Request);
            model.Resolve(_scope);

            var data = await model.GetPagedCoursesAsync(dataTablesModel);
            return Json(data);
        }

        public async Task<JsonResult> GetCourseEnrollments()
        {
            CourseEnrollmentListModel model = new();
            model.Resolve(_scope);
            model.SearchItem = new CourseEnrollmentSearch
            {
                CourseName = "C#",
                StudentName = "Jalaluddin",
                EnrollmentDateFrom = new DateTime(2020, 1, 1),
                EnrollmentDateTo = new DateTime(2030, 1, 1)
            };

            var data = await model.GetPagedCourseEnrollmentsAsync(1, 10, "CourseName");
            return Json(data);
        }

        [Authorize(Policy = "CourseUpdatePolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var model = _scope.Resolve<CourseUpdateModel>();
            await model.LoadAsync(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CourseUpdatePolicy")]
        public async Task<IActionResult> Update(CourseUpdateModel model)
        {
            model.Resolve(_scope);

            if (ModelState.IsValid)
            {
                try
                {
                    await model.UpdateCourseAsync();
                    return RedirectToAction("Index");
                }
                catch(DuplicateTitleException de)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = de.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Server Error");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in updating course",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = _scope.Resolve<CourseListModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    await model.DeleteCourseAsync(id); TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "Course deleted successfuly",
						Type = ResponseTypes.Success
					});

					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Server Error");

					TempData.Put("ResponseMessage", new ResponseModel
					{
						Message = "There was a problem in deleting course",
						Type = ResponseTypes.Danger
					});
				}
			}

            return RedirectToAction("Index");
        }
    }
}
