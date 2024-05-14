using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Services
{
    public class ProductService : IProductServices
    {
        private  IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _repository = productRepository;
            _mapper = mapper;
            
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var productEntity = await _repository.GetProductsAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(productEntity);
        }

        public async Task<ProductDTO> GetById(int id)
        {
            var productEntity = await _repository.GetByIdAsync(id);

            return _mapper.Map<ProductDTO>(productEntity);

        }

        public async Task<ProductDTO> GetProductCategory(int? id)
        {
            var productEntity = await _repository.GetProductCategoryAsync(id);

            return _mapper.Map<ProductDTO>(productEntity);
        }

        public async Task Add(ProductDTO productDTO)
        {
            var productEntity = _mapper.Map<Product>(productDTO);

            await _repository.CreateAsync(productEntity);
        }

        public async Task Update(ProductDTO productDTO)
        {
            var productEntity = _mapper.Map<Product>(productDTO);

            await _repository.UpdateAsync(productEntity);
        }

        public async Task Remove(int? id)
        {
            var productEntity = _repository.GetByIdAsync(id).Result;

            await _repository.RemoveAsync(productEntity);
        }
    }
}
