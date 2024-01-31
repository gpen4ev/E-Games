using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Data.Data;
using E_Games.Data.Data.Models;
using E_Games.Web.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace E_Games.Services.E_Games.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        public GameService(ApplicationDbContext context, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public async Task<List<PlatformPopularityDto>> GetTopPlatformsAsync() => await _context.Products
                .GroupBy(p => p.Platform)
                .Select(group => new PlatformPopularityDto
                {
                    PlatformName = group.Key.ToString(),
                    Count = group.Count()
                })
                .OrderByDescending(dto => dto.Count)
                .Take(3)
                .ToListAsync();

        public async Task<List<SearchGameDto>> SearchGamesAsync(string term, int limit, int offset)
        {
            if (limit <= 0 || offset < 0)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest,
                    "Invalid parameters: Limit must be greater than zero and Offset cannot be negative.");
            }

            var queryResult = await _context.Products
                .Where(p => p.Name!.Contains(term))
                .Skip(offset)
                .Take(limit)
                .Select(p => new SearchGameDto
                {
                    Name = p.Name,
                })
                .ToListAsync();

            return queryResult;
        }

        public async Task<FullProductInfoDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound);
            }

            return _mapper.Map<FullProductInfoDto>(product);
        }

        public async Task<CreateProductDto> CreateProductAsync(CreateProductDto model)
        {
            if (model.LogoFile != null)
            {
                var logoUrl = await _cloudinaryService.UploadImageAsync(model.LogoFile);
                if (!string.IsNullOrEmpty(logoUrl))
                {
                    model.Logo = logoUrl;
                }
            }

            if (model.BackgroundImageFile != null)
            {
                var backgroundUrl = await _cloudinaryService.UploadImageAsync(model.BackgroundImageFile);
                if (!string.IsNullOrEmpty(backgroundUrl))
                {
                    model.Background = backgroundUrl;
                }
            }

            var product = _mapper.Map<Product>(model);
            if (product == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound);
            }

            _context.Products.Add(product!);
            await _context.SaveChangesAsync();

            return _mapper.Map<CreateProductDto>(product);
        }

        public async Task<UpdateProductDto> UpdateProductAsync(UpdateProductDto model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (product == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound);
            }

            if (model.LogoFile != null)
            {
                var logoUrl = await _cloudinaryService.UploadImageAsync(model.LogoFile);
                if (!string.IsNullOrEmpty(logoUrl))
                {
                    model.Logo = logoUrl;
                }
            }

            if (model.BackgroundImageFile != null)
            {
                var backgroundUrl = await _cloudinaryService.UploadImageAsync(model.BackgroundImageFile);
                if (!string.IsNullOrEmpty(backgroundUrl))
                {
                    model.Background = backgroundUrl;
                }
            }

            _mapper.Map(model, product);

            _context.Products.Update(product!);
            await _context.SaveChangesAsync();

            return _mapper.Map<UpdateProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "Product not found");
            }

            _context.Products.Remove(product!);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
