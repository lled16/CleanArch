﻿using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.Interfaces;
using MediatR;

namespace CleanArchMvc.Application.Products.Handlers
{
    public class ProductUpsateCommandHandler : IRequestHandler<ProductUpdateCommand, Product>
    {
        private readonly IProductRepository _repository;

        public ProductUpsateCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<Product> Handle(ProductUpdateCommand request, CancellationToken cancellation)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new ArgumentException($"Error could not be found.");
            }
            else
            {
                product.Update(request.Name, request.Description, request.Price, request.Stock, request.Image, request.CategoryId);
                return await _repository.UpdateAsync(product);
            }
        }
    }
}
