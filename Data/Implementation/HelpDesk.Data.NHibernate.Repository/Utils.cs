using NHibernate;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;

namespace HelpDesk.Data.NHibernate.Repository
{
    public static class Utils
    {
        public static string ToSqlString(this System.Linq.IQueryable queryable)
        {
            ISession session = NHibernateSessionManager.Instance.GetSession();
            var sessionImp = (ISessionImplementor)session;
            var nhLinqExpression = new NhLinqExpression(queryable.Expression, sessionImp.Factory);
            var translatorFactory = new ASTQueryTranslatorFactory();
            var translators = translatorFactory.CreateQueryTranslators(nhLinqExpression, null, false, sessionImp.EnabledFilters, sessionImp.Factory);

            return translators[0].SQLString;
        }
    }
}
