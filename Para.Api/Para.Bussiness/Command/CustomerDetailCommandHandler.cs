using AutoMapper;
using FluentValidation;
using MediatR;
using Para.Base.Response;
using Para.Bussiness.Cqrs;
using Para.Data.Domain;
using Para.Data.UnitOfWork;
using Para.Schema;

namespace Para.Bussiness.Command;

public class CustomerDetailCommandHandler :
    IRequestHandler<CreateCustomerDetailCommand, ApiResponse<CustomerDetailResponse>>,
    IRequestHandler<UpdateCustomerDetailCommand, ApiResponse>,
    IRequestHandler<DeleteCustomerDetailCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IValidator<CustomerRequest> validator;

    public CustomerDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CustomerRequest> validator)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.validator = validator;
    }

    public async Task<ApiResponse<CustomerDetailResponse>> Handle(CreateCustomerDetailCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CustomerDetailRequest, Customer>(request.Request);
        mapped.CustomerNumber = new Random().Next(1000000, 9999999);
        await unitOfWork.CustomerRepository.Insert(mapped);
        await unitOfWork.Complete();

        var response = mapper.Map<CustomerDetailResponse>(mapped);
        return new ApiResponse<CustomerDetailResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateCustomerDetailCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CustomerDetailRequest, Customer>(request.Request);
        mapped.Id = request.CustomerDetailId;
        unitOfWork.CustomerRepository.Update(mapped);
        await unitOfWork.Complete();
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteCustomerDetailCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.CustomerRepository.Delete(request.CustomerDetailId);
        await unitOfWork.Complete();
        return new ApiResponse();
    }
}