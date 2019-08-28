using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace NA.Common.Extentions
{
    public static class TransactionScopeExtention
    {
        public static TransactionScope BeginTransactionScope(Action<TransactionOptions> options = null)
        {
            // option default
            var option = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead
            };
            if (options != null)
            {
                options.Invoke(option);
            }
            return new TransactionScope(TransactionScopeOption.Required, option, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
