using Microsoft.AspNetCore.Mvc;
using Objects;
using State;
using State.Src;

namespace Core.API.View.ViewExtensions
{
    public static class ViewExtensions
    {
        public static ActionResult<AffectionViewModel> ToView(this OperationResult result)
        {
            if (result.Message == null)
            {
                return new OkObjectResult(new AffectionViewModel(result.Id));
            }

            return new BadRequestObjectResult(new ErrorViewResponse(result.ErrorCode, result.Message));
        }

        public static ActionResult<TModel> ToView<TModel>(this FindResult<TModel> findResult)
        {
            if (findResult.Data != null)
            {
                return new OkObjectResult(findResult);
            }

            return new NotFoundObjectResult(new ErrorViewResponse(findResult.ErrorCode, findResult.ErrorMessage));
        }
    }
}
