using HelpDesk.Data.Specification;
using HelpDesk.Entity;
using System;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Specification
{
    public class RequestFileByNameAndForignKeyOrTempKeySpecification : Specification<RequestFile>
    {
        private readonly string fileName;
        private readonly long? requestId;
        private readonly Guid? tempRequestKey;
        public RequestFileByNameAndForignKeyOrTempKeySpecification(string fileName, 
            long? requestId, Guid? tempRequestKey)
        {
            this.fileName = fileName;
            this.requestId = requestId;
            this.tempRequestKey = tempRequestKey;
        }

        public override Expression<Func<RequestFile, bool>> IsSatisfied()
        {
            return t => (t.RequestId != null && t.RequestId == requestId || 
                t.TempRequestKey != null && t.TempRequestKey == tempRequestKey) && 
                t.Name.ToUpper() == fileName.ToUpper();
        }
    }
}
