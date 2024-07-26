using AutoMapper;
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

    public CustomerDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    public async Task<ApiResponse<CustomerDetailResponse>> Handle(CreateCustomerDetailCommand request, CancellationToken cancellationToken)
    {
        var detail = mapper.Map<CustomerDetail>(request.Request);
        detail.CustomerId = request.CustomerDetailId;

        var existingDetail = await unitOfWork.CustomerDetailRepository.GetById(request.CustomerDetailId);
        if (existingDetail != null)
        {
            return new ApiResponse<CustomerDetailResponse>(false)
            {
                Message = "Customer details already exist."
            };
        }

        await unitOfWork.CustomerDetailRepository.Insert(detail);
        await unitOfWork.Complete();

        var response = mapper.Map<CustomerDetailResponse>(detail);
        return new ApiResponse<CustomerDetailResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateCustomerDetailCommand request, CancellationToken cancellationToken)
    {
        var detail = await unitOfWork.CustomerDetailRepository.GetById(request.CustomerDetailId);
        if (detail == null)
        {
            return new ApiResponse()
            {
                Message = "Customer details not found."
            };
        }

        mapper.Map(request.Request, detail);
        unitOfWork.CustomerDetailRepository.Update(detail);
        await unitOfWork.Complete();
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteCustomerDetailCommand request, CancellationToken cancellationToken)
    {
        var detail = await unitOfWork.CustomerDetailRepository.GetById(request.CustomerDetailId);
        if (detail != null)
        {
            await unitOfWork.CustomerDetailRepository.Delete(detail.CustomerId);
            await unitOfWork.Complete();
        }
        return new ApiResponse();
    }
}