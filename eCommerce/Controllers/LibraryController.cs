﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class LibraryController : Controller
    {
        private readonly GameContext _context;

        public LibraryController(GameContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<VideoGame> allGames = await VideoGameDb.GetAllGames(_context);
            return View(allGames);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(VideoGame game)
        {
            if (ModelState.IsValid)
            {
                await VideoGameDb.Add(game, _context);
                return RedirectToAction("Index");
            }

            // Return view with model including error messages
            return View(game);
        }
    }
}