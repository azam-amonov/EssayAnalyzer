using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes
{
    public partial class EssayAnalysisService
    {
        private delegate ValueTask<string> ReturnEssayAnalysisAsync();

        private async ValueTask<string> TryCatch(ReturnEssayAnalysisAsync returnEssayAnalysisAsync)
        {
            try
            {
                return await returnEssayAnalysisAsync();
            }
            catch (NullEssayAnalysisException nullEssayAnalysisException)
            {
                throw CreateAndLogValidationException(nullEssayAnalysisException);
            }
        }
        private EssayAnalysisServiceValidationException CreateAndLogValidationException(Xeption exceptionn)
        {
            var essayAnalysisServiceValidationException =
                new EssayAnalysisServiceValidationException(exceptionn);

            this.loggingBroker.LogError(essayAnalysisServiceValidationException);
            return essayAnalysisServiceValidationException;
        }
    }
}