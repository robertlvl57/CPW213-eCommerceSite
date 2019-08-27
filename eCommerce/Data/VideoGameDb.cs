﻿using eCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Data
{
    /// <summary>
    /// DB Helper class for VideoGames
    /// </summary>
    public static class VideoGameDb
    {
        /// <summary>
        /// Returns 1 page worth of products. Products are sorted alphabetically by Title
        /// </summary>
        /// <param name="context">The db context</param>
        /// <param name="pageNum">The page number for the products</param>
        /// <param name="pageSize">The number of products per page</param>
        /// <returns></returns>
        public static async Task<List<VideoGame>> GetGamesByPage(GameContext context, int pageNum, int pageSize)
        {
            // Make sure to call skip BEFORE take
            // Make sure oderby comes first
            List<VideoGame> games = await context.VideoGames.OrderBy(vg => vg.Title).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

            return games;
        }

        /// <summary>
        /// Returns the total number of pages needed to have <paramref name="pageSize"/> amount of products per page
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<int> GetTotalPages(GameContext context, int pageSize)
        {
            int totalNumGames = await context.VideoGames.CountAsync();

            // Partial number of pages
            double pages = (double)totalNumGames / pageSize;

            return (int)Math.Ceiling(pages);
        }

        /// <summary>
        /// Adds a VideoGame to the data store and sets the ID value
        /// </summary>
        /// <param name="g">The game to be added</param>
        /// <param name="context">The DB context to use</param>
        /// <returns>VideoGame with ID populated</returns>
        public static async Task<VideoGame> Add(VideoGame g, GameContext context)
        {
            await context.AddAsync(g);
            await context.SaveChangesAsync();
            return g;
        }

        /// <summary>
        /// Retrieves all games sorted in alphabetical order by title
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<List<VideoGame>> GetAllGames(GameContext context)
        {
            // LINQ Query syntax
            //List<VideoGame> games = await (from vidGame in context.VideoGames
            //                         orderby vidGame.Title ascending
            //                         select vidGame).ToListAsync();

            // LINQ Method Syntax
            List<VideoGame> games = await context.VideoGames
                                                 .OrderBy(g => g.Title)
                                                 .ToListAsync();

            return games;
        }

        public static async Task<VideoGame> UpdateGame(VideoGame g, GameContext context)
        {
            context.Update(g);
            await context.SaveChangesAsync();
            return g;
        }

        public static async Task DeleteById(int id, GameContext context)
        {
            // Create video game object, with the id of the game we want to remove from the database
            VideoGame g = new VideoGame()
            {
                Id = id
            };

            context.Entry(g).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a game with a specified. If no game is found null is returned
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<VideoGame> GetGameById(int id, GameContext context)
        {
            VideoGame g = await (from game in context.VideoGames
                           where game.Id == id
                           select game).SingleOrDefaultAsync();

            return g;
        }

        /// <summary>
        /// Searches for games that mathc the criteria and
        /// returns all games that match
        /// </summary>
        /// <param name="context"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static async Task<List<VideoGame>> Search(GameContext context, SearchCriteria criteria)
        {
            // SELECT * FROM VideoGames
            // This does NOT query the database
            IQueryable<VideoGame> allGames = from g in context.VideoGames
                                             select g;

            if (criteria.MinPrice.HasValue)
            {
                // Add to WHERE clause
                // Price >= criteria.MinPrice
                allGames = from g in allGames
                           where g.Price >= criteria.MinPrice
                           select g;
            }

            if (criteria.MaxPrice.HasValue)
            {
                allGames = from g in allGames
                           where g.Price <= criteria.MaxPrice
                           select g;
            }

            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                // WHERE LEFT(Title) = criteria.Title
                allGames = from g in allGames
                           where g.Title.StartsWith(criteria.Title)
                           select g;
            }

            if (!string.IsNullOrWhiteSpace(criteria.Rating))
            {
                // WHERE Rating = criteria.Rating
                allGames = from g in allGames
                           where g.Rating == criteria.Rating
                           select g;
            }

            // Send final query to database to return results
            // EF does not send the query to the DB until it has to
            return await allGames.ToListAsync();
        }
    }
}
