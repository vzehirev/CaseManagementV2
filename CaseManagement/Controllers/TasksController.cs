﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.Models;
using CaseManagement.Services.Tasks;
using CaseManagement.ViewModels;
using CaseManagement.ViewModels.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ITasksService tasksService;
        private readonly UserManager<ApplicationUser> userManager;

        public TasksController(ITasksService tasksService, UserManager<ApplicationUser> userManager)
        {
            this.tasksService = tasksService;
            this.userManager = userManager;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskInputModel inputModel, int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            inputModel.CaseId = id;
            await this.tasksService.CreateTaskAsync(inputModel, userId);

            return RedirectToAction("ViewUpdate", "Cases", new { id });
        }
        public IActionResult ViewUpdate(int id)
        {
            var outputModel = this.tasksService.GetTaskById(id);

            return View(outputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ViewUpdateTaskModel inputModel)
        {
            var caseId = await this.tasksService.UpdateTaskAsync(inputModel);

            return RedirectToAction("ViewUpdate", "Cases", new { id = caseId });
        }
    }
}