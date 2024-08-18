﻿using Autofac;
using AutoMapper;
using FirstDemo.Application.Features.Training.Services;
using FirstDemo.Domain.Entities;
using FirstDemo.Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace FirstDemo.Api.RequestHandlers
{
    public class ViewCourseRequestHandler : DataTables
    {
        private ICourseManagementService? _courseService;
        private IMapper _mapper;

        public CourseSearch SearchItem { get; set; }

        public ViewCourseRequestHandler()
        {

        }

        public ViewCourseRequestHandler(ICourseManagementService coursService, IMapper mapper)
        {
            _courseService = coursService;
            _mapper = mapper;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _courseService = scope.Resolve<ICourseManagementService>();
            _mapper = scope.Resolve<IMapper>();
        }

        internal async Task<IList<Course>>? GetCoursesAsync()
        {
            return await _courseService?.GetCoursesAsync();
        }

        internal void DeleteCourse(Guid id)
        {
            _courseService?.DeleteCourseAsync(id);
        }

        //internal Course GetCourse(string name)
        //{
        //    return _courseService.GetCourse(name);
        //}

        internal async Task<Course>? GetCourseAsync(Guid id)
        {
            return await _courseService?.GetCourseAsync(id);
        }

        internal async Task<object?> GetPagedCourses()
        {

            var data = await _courseService?.GetPagedCoursesAsync(
                PageIndex,
                PageSize,
                SearchItem.Title,
                SearchItem.CourseFeesFrom,
                SearchItem.CourseFeesTo,
                FormatSortExpression("Title", "Description", "Fees"));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Title,
                                record.Description,
                                record.Fees.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
