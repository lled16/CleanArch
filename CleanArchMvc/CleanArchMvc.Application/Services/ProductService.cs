using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Application.Products.Queries;
using MediatR;

namespace CleanArchMvc.Application.Services
{
    public class ProductService : IProductServices
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, IMediator mediator)
        {

            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var productsQuery = new GetProductQuery();

            if(productsQuery is null)
                throw new ApplicationException($"Entity could not be loaded");

            var result = await _mediator.Send(productsQuery);

            return _mapper.Map<IEnumerable<ProductDTO>>(result);
        }

        public async Task<ProductDTO> GetById(int? id)
        {
            var productsByIdQuery = new GetProductByIdQuery(id.Value);

            if (productsByIdQuery is null)
                throw new ApplicationException($"Entity could not be loaded");

            var result = await _mediator.Send(GetById(id));

            return _mapper.Map<ProductDTO>(result);

        }

        public async Task<ProductDTO> GetProductCategory(int? id)
        {
            var productsByIdQuery = new GetProductByIdQuery(id.Value);

            if (productsByIdQuery is null)
                throw new ApplicationException($"Entity could not be loaded");

            var result = await _mediator.Send(GetById(id));

            return _mapper.Map<ProductDTO>(result);
        }

        public async Task Add(ProductDTO productDTO)
        {
            ProductCreateCommand productCreateCommand = _mapper.Map<ProductCreateCommand>(productDTO);

            var result = _mediator.Send(productCreateCommand);

        }

        public async Task Update(ProductDTO productDTO)
        {
            ProductUpdateCommand productCreateCommand = _mapper.Map<ProductUpdateCommand>(productDTO);

            var result = _mediator.Send(productCreateCommand);
        }

        public async Task Remove(int? id)
        {
            var productRemoveCommand = new ProductRemoveCommand(id.Value);

            if (productRemoveCommand is null)
                throw new ApplicationException($"Entity could not be loaded");

            await _mediator.Send(productRemoveCommand);
        }
    }
}
