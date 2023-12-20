using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Results;

public partial class ResultService
{
    private delegate ValueTask<Result> ReturnResultFunction();

    private async ValueTask<Result> TryCatch(ReturnResultFunction returnResultFunction)
    {
        try
        {
            return await returnResultFunction();
        }
        catch (NullResultException nullResultException)
        {
            throw CreateAndLogValidationException(nullResultException);
        }
        catch(InvalidResultException invalidResultException)
        {
            throw CreateAndLogValidationException(invalidResultException);
        }
    }

    private ResultValidationException CreateAndLogValidationException(Xeption exception)
    {
        var resultValidationException = 
            new ResultValidationException(exception);

        this.loggingBroker.LogError(resultValidationException);

        return resultValidationException;
    }
}