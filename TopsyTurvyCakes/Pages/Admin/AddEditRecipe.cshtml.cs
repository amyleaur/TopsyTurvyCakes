using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace TopsyTurvyCakes.Pages.Admin
{
    [Authorize]
    public class AddEditRecipeModel : PageModel
    {
        private IRecipesService recipesService;

        [FromRoute]

        public long? Id { get; set; }
        public bool IsNewRecipe
        {
            get { return Id == null; }
        }

        [BindProperty]

        public Recipe Recipe { get; set; } //expose the Recipe class we'll use

        [BindProperty]
        public IFormFile ImageFile{ get; set; }

        public async Task OnGetAsync()
        {
            //var recipesService = new RecipesService();
            Recipe = recipesService.Find(Id.GetValueOrDefault()) ?? new Recipe();
        }

        public async Task <IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            //Recipe.Id = Id.GetValueOrDefault();
            var CurrentRecipe = recipesService.Find(Id.GetValueOrDefault()) ?? new Recipe();
            CurrentRecipe.Name = Recipe.Name;
            CurrentRecipe.Description = Recipe.Description;
            CurrentRecipe.Ingredients = Recipe.Ingredients;
            CurrentRecipe.Directions = Recipe.Directions;


            if (ImageFile != null)
            {
                CurrentRecipe.SetImage(ImageFile);
            }

            await recipesService.SaveAsync(CurrentRecipe);
            return RedirectToPage("/Recipe", new { name = CurrentRecipe.Name });
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }



        public AddEditRecipeModel (IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }
    }
}